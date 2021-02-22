# Asynchronous programming with Task



[TOC]

Refer to MSDN https://docs.microsoft.com/zh-tw/dotnet/api/system.threading.tasks.task?view=netcore-3.1

## Definition

Namespace : System.Threading.Tasks

```C#
public class Task : IAsyncResult, IDisposable
```

## Asynchronous Programming

### 同步的做法

直接拿MSDN的範例修改

```C#
using System;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        static void Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }

    internal class Juice
    {
    }

    internal class Toast
    {
    }

    internal class Bacon
    {
    }

    internal class Egg
    {
    }

    internal class Coffee
    {
    }
}
```

執行起來如下

![IMG01](https://i.imgur.com/fCFhvnI.png)

> 上述程式碼示範不正確的做法：建構同步程式碼來執行非同步作業。 如內容所指，此程式碼會防止執行緒執行任何其他工作。 在任何工作進行時，它不會遭到中斷。 就像是將麵包放入烤麵包機之後，直盯著烤麵包機。 在吐司彈出之前，您不會理會任何人對您說的話。
>
> 我們將從更新此程式碼開始，讓執行緒在工作執行時不會遭到封鎖。 `await` 關鍵字提供一個非封鎖方式來開始工作，然後在工作完成時繼續執行。 準備早餐程式碼的一個簡單非同步版本看起來像下列程式碼片段：

### 以等候代替封鎖

將main改成如下

```C#
static async Task Main(string[] args)
{
    Coffee cup = PourCoffee();
    Console.WriteLine("coffee is ready");

    Egg eggs = await FryEggsAsync(2);
    Console.WriteLine("eggs are ready");

    Bacon bacon = await FryBaconAsync(3);
    Console.WriteLine("bacon is ready");

    Toast toast = await ToastBreadAsync(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("toast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("oj is ready");
    Console.WriteLine("Breakfast is ready!");
}
```

另外方法改為如下格式

```C#
 private static async Task<Toast> ToastBreadAsync(int slices)
```

執行程式會發現和第一個版本幾乎沒有差別

根據MSDN的說法:

> 此程式碼不會在煎蛋或煎培根時封鎖其他工作。 但此程式碼也不會開始任何其他工作。 您仍會將吐司放入烤麵包機，並在彈出前直盯著它瞧。 但至少，您會回應任何需要您注意的人。 在點了多份早餐的餐廳中，廚師可能會在第一份早餐還在準備時，就開始準備另一份早餐。

### 同時開始

修改方法

將等待動作

```C#
Task.Delay(3000).Wait();
```

改成

```C#
await Task.Delay(3000);
```



修改main

```C#
Egg eggs = await FryEggsAsync(2);
    Console.WriteLine("eggs are ready");
```

改成

```C#
Task<Egg> eggsTask = FryEggsAsync(2);
Egg eggs = await eggsTask;
Console.WriteLine("eggs are ready");
```

並且把等待動作移至結尾處，main如下

```C#
Coffee cup = PourCoffee();
Console.WriteLine("coffee is ready");

Task<Egg> eggsTask = FryEggsAsync(2);
Task<Bacon> baconTask = FryBaconAsync(3);
Task<Toast> toastTask = ToastBreadAsync(2);

Toast toast = await toastTask;
ApplyButter(toast);
ApplyJam(toast);
Console.WriteLine("toast is ready");
Juice oj = PourOJ();
Console.WriteLine("oj is ready");

Egg eggs = await eggsTask;
Console.WriteLine("eggs are ready");
Bacon bacon = await baconTask;
Console.WriteLine("bacon is ready");

Console.WriteLine("Breakfast is ready!");
```

再次執行，可以看到執行時間少了一半多

![IMG02](https://i.imgur.com/1Wdkwd6.png)



main 後方一大堆的await可以透過Task方法簡化

* 使用Task.WhenAll

```C#
            await Task.WhenAll(eggsTask, baconTask, toastTask);
            Console.WriteLine("eggs are ready");
            Console.WriteLine("bacon is ready");
            Console.WriteLine("toast is ready");
            Console.WriteLine("Breakfast is ready!");
```

* 使用Task.WhenAny

```C#
var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
while (breakfastTasks.Count > 0)
{
    Task finishedTask = await Task.WhenAny(breakfastTasks);
    if (finishedTask == eggsTask)
    {
        Console.WriteLine("eggs are ready");
    }
    else if (finishedTask == baconTask)
    {
        Console.WriteLine("bacon is ready");
    }
    else if (finishedTask == toastTask)
    {
        Console.WriteLine("toast is ready");
    }
    breakfastTasks.Remove(finishedTask);
}
```

### 目前為止的程式

```C#
using System;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            DateTime dateTime = DateTime.Now;
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Task<Egg> eggsTask = FryEggsAsync(2);

            Task<Bacon> baconTask = FryBaconAsync(3);

            Task<Toast> toastTask = ToastBreadAsync(2);
            Toast toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            await Task.WhenAll(eggsTask, baconTask, toastTask);
            Console.WriteLine("eggs are ready");
            Console.WriteLine("bacon is ready");
            Console.WriteLine("toast is ready");
            Console.WriteLine("Breakfast is ready!");
            Console.WriteLine($"runtime:{(DateTime.Now - dateTime).TotalMilliseconds}");
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }

    internal class Juice
    {
    }

    internal class Toast
    {
    }

    internal class Bacon
    {
    }

    internal class Egg
    {
    }

    internal class Coffee
    {
    }
}
```

## Task

MSDN的範例寫得很清楚，所以直接照搬

```C#
using System;
using System.Threading;
using System.Threading.Tasks;

class Example
{
    static void Main()
    {
        Action<object> action = (object obj) =>
        {
            Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            Task.CurrentId, obj,
            Thread.CurrentThread.ManagedThreadId);
        };

        // Create a task but do not start it.
        Task t1 = new Task(action, "alpha");

        // Construct a started task
        Task t2 = Task.Factory.StartNew(action, "beta");
        // Block the main thread to demonstrate that t2 is executing
        t2.Wait();

        // Launch t1 
        t1.Start();
        Console.WriteLine("t1 has been launched. (Main Thread={0})",
                          Thread.CurrentThread.ManagedThreadId);
        // Wait for the task to finish.
        t1.Wait();

        // Construct a started task using Task.Run.
        String taskData = "delta";
        Task t3 = Task.Run(() => {
            Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                              Task.CurrentId, taskData,
                               Thread.CurrentThread.ManagedThreadId);
        });
        // Wait for the task to finish.
        t3.Wait();

        // Construct an unstarted task
        Task t4 = new Task(action, "gamma");
        // Run it synchronously
        t4.RunSynchronously();
        // Although the task was run synchronously, it is a good practice
        // to wait for it in the event exceptions were thrown by the task.
        t4.Wait();
    }
}
// The example displays output like the following:
//       Task=1, obj=beta, Thread=3
//       t1 has been launched. (Main Thread=1)
//       Task=2, obj=alpha, Thread=4
//       Task=3, obj=delta, Thread=3
//       Task=4, obj=gamma, Thread=1
```

## 重點整理

* 把方法變為非同步方法

  ```C#
  static void Main()
  ```

  ```C#
  static async Task Main()
  ```

  ```C#
  private string method1()
  ```

  ```C#
  private async Task<string> method1()
  ```

  

* 關於Task.Delay()

  在同步方法內

  ```C#
  Task.Delay(3000).Wait();
  ```

  在非同步方法內

  ```C#
  await Task.Delay(3000);
  ```

  

* 關於Task.Result

  以下面的方法為例

  ```C#
  private static async Task<string> method2() => "data";
  ```

  方法return "data" 但它不是回傳字串，是反傳到Task.Result屬性內

  例如

  ```C#
  string str = method2();
  ```

  是不被compiler接受的

  ```C#
  Task<string> taskStr = method2();
  Task task = method2();
  ```

  如上寫法才可以

  在Debug模式下可以看到`taskStr.Result`和`task.Result`都是"data"

  但是在IDE中task卻無法找到Result這個屬性

  另外，當程式在讀取Result屬性時，似乎會等待Task執行結束。

* 當直接await的時候

  錯誤寫法

  ```C#
  Task<string> task = await method2();
  ```

  正確寫法

  ```C#
  string str = await method2();
  ```

  

* 簡易流程

  ```C#
  private static async Task<string> method2() => "data";
  ```

  ```C#
  //開始執行
  Task<string> task = method();
  //等待執行結果
  await task;
  ```

  

> 20210222補充

剛學了Dart的非同步，對於`await`和`async`有了不太一樣的看法，特此補充

首先將C#的`Task<T>`與Dart的`Future<T>`類比，相當於得到一個未來(或者說不一定已經得到結果的物件)



`async`關鍵字會把方法中的回傳值`T`包裝成`Task<T>`，如以下兩個方法最後都是回傳`Task<string>`物件

```c#
Task<string> helloAsync()=>  new Task<string>(()=>"Hello");// 這裡回傳一個Task<string>物件 
```

```C#
async Task<string> helloAsync()=> "Hello";// 這裡回傳一個string 
```



`await`關鍵字會把非同步的內容變為同步(作法就是等它執行完)，使用上方的例子

```C#
var hello = helloAsync(); // hello 是Task<string>物件，可以立刻取得
```

```C#
var hello = await helloAsync(); // hello 是string，會等待helloAsync()執行完畢
```

