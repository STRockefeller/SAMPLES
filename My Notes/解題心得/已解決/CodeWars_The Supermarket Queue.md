# CodeWars:The Supermarket Queue:20200914:C#

[Reference](https://www.codewars.com/kata/57b06f90e298a7b53d000a86)



## Question

There is a queue for the self-checkout tills at the supermarket. Your task is write a function to calculate the total time required for all the customers to check out!

#### input

- customers: an array of positive integers representing the queue. Each integer represents a customer, and its value is the amount of time they require to check out.
- n: a positive integer, the number of checkout tills.

#### output

The function should return an integer, the total time required.

------

### Important

**Please look at the examples and clarifications below, to ensure you understand the task correctly :)**

------

#### Examples

```javascript
queueTime([5,3,4], 1)
// should return 12
// because when there is 1 till, the total time is just the sum of the times

queueTime([10,2,3,3], 2)
// should return 10
// because here n=2 and the 2nd, 3rd, and 4th people in the 
// queue finish before the 1st person has finished.

queueTime([2,3,10], 2)
// should return 12
```

#### Clarifications

- There is only ONE queue serving many tills, and
- The order of the queue NEVER changes, and
- The front person in the queue (i.e. the first element in the array/list) proceeds to a till as soon as it becomes free.

N.B. You should assume that all the test input will be valid, as specified above.

P.S. The situation in this kata can be likened to the more-computer-science-related idea of a thread pool, with relation to running multiple processes at the same time: https://en.wikipedia.org/wiki/Thread_pool



## My Solution



```C#
using System.Collections.Generic;
using System.Linq;

public class Kata
{
        public static long QueueTime(int[] customers, int n)
        {
            List<int> tills = new List<int>();
            tills.AddRange(Enumerable.Repeat(0, n));
            for (int i = 0; i < customers.Length; i++)
            {
                if (i < n)
                {
                    if (tills[i] == 0)
                        tills[i] = customers[i];
                }
                else
                    for (int j = 0; j < tills.Count; j++)
                        if (tills[j] == tills.Min())
                        {
                            tills[j] += customers[i];
                            break;
                        }
            }
            return tills.Max();
        }
}
```

### 思路

建一個`List<int>`去紀錄每個收銀台的作業時間

針對每個顧客做處理，前n個顧客放到對應的收銀檯前面，之後的顧客進入目前處理所需時間最短的收銀台，並把時間加上去。



## Better Solutions



### Solution 1

```C#
using System.Collections.Generic;
using System.Linq;
public class Kata
{
    public static long QueueTime(int[] customers, int n)
    {
      var registers = new List<int>(Enumerable.Repeat(0, n));
      
      foreach(int cust in customers){
        registers[registers.IndexOf(registers.Min())] += cust;
      }
      return registers.Max();
    }
}
```

幾乎一樣的思路，但簡潔很多。



list初值設定用一行搞定

```C#
            List<int> tills = new List<int>();
            tills.AddRange(Enumerable.Repeat(0, n));
```

變成

```C#
      var registers = new List<int>(Enumerable.Repeat(0, n));
```

第一次知道List可以利用建構式設定好初始值。



後面一樣是針對Customers做處理，但我因為不曉得`IndexOf`的用法所以一直使用for loop而不是foreach

[MSDN:IndexOf](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.generic.list-1.indexof?view=netcore-3.1)

(string 以及 Array 也都有名為IndexOf類似作用的方法)

```C#
registers[registers.IndexOf(registers.Min())] += cust;
```

`registers.Min()`得到List中最小的值。

`registers.IndexOf(registers.Min())`得到該值的Index，根據MSDN的定義，就算有同value的項目也只取第一個。



### Solution 2

```C#
using System;
using System.Linq;
using System.Collections.Generic;

public class Kata
{
    public static long QueueTime(int[] customers, int n)
    {
      int[] till = new int[n];         
      for(int i=0;i<customers.Length;i++){       
        till[Array.IndexOf(till, till.Min())]+=customers[i];
      }
     return till.Max();
    }
}
```

基本和上面的解法差不多，但是使用array處理。



