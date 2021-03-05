# Tuple & Value Tuple

[MSDN:Tuple](https://docs.microsoft.com/zh-tw/dotnet/api/system.tuple?view=net-5.0)

[MSDN:ValueTuple](https://docs.microsoft.com/zh-tw/dotnet/api/system.valuetuple?view=net-5.0)

## Tuple



Provides static methods for creating tuple objects.

```csharp
public static class Tuple
```



一個static class ，作用在我看來類似於`struct`，在命名空間`System`底下，`using System`就能直接使用

MSDN Example

```C#
var primes = Tuple.Create(2, 3, 5, 7, 11, 13, 17, 19);
Console.WriteLine("Prime numbers less than 20: " + 
                  "{0}, {1}, {2}, {3}, {4}, {5}, {6}, and {7}",
                  primes.Item1, primes.Item2, primes.Item3, 
                  primes.Item4, primes.Item5, primes.Item6,
                  primes.Item7, primes.Rest.Item1);
// The example displays the following output:
//    Prime numbers less than 20: 2, 3, 5, 7, 11, 13, 17, and 19
```



方法就一個Create

```C#
Tuple<int> t = Tuple.Create<int>(2);
```

Create的泛型可以拿掉

```C#
Tuple<int> t = Tuple.Create(2);
```

但是前面變數的泛型不能拿掉，不會有`Tuple t`這種東西，因為不能為static class建立變數，順帶一提`Tuple<T>`是一般的public class所以沒有這個問題。

調用內容的時候就使用Item1, Item2, ...... 

```C#
Tuple<int> t = Tuple.Create<int>(2);
Console.WriteLine(t.Item1);
```



## Value Tuple

C# 7.0 之後適用的Tuple升級版

Provides static methods for creating value tuples.

```csharp
public struct ValueTuple : IComparable, IComparable<ValueTuple>, IEquatable<ValueTuple>, System.Collections.IStructuralComparable, System.Collections.IStructuralEquatable, System.Runtime.CompilerServices.ITuple
```

一樣在命名空間`System`底下，但是要安裝Nuget套件`System.ValueTuple`才能使用

以下直接放比較

* Tuple

  ```C#
              Tuple<string, string> puniColor = Tuple.Create("blue", "red");
              Console.WriteLine(puniColor.Item2);
              Console.WriteLine(Tuple.Create("blue", "red").Item2);
  ```

* ValueTuple

  ```C#
              (string first,string second) puniColor = (first: "blue", second: "red");
              Console.WriteLine(puniColor.Item2);
              Console.WriteLine(puniColor.second);
              Console.WriteLine(("blue", "red").Item2);
  ```

  

列舉一下差異

|           |                 Tuple                  |                          ValueTuple                          |
| :-------: | :------------------------------------: | :----------------------------------------------------------: |
|   型別    | 參考型別，生成如`Tuple<int,int>`的物件 |                    值型別，如`(int,int)`                     |
|   命名    | 無法命名，依順序自動為Item1、Item2...  | 可以命名，除了原本的Item1、Item2...，還能另外自定義名稱如`(int hr,int min)` |
| namespace |       在System底下，可以直接使用       |           在System底下，需安裝Nuget套件才能使用。            |

