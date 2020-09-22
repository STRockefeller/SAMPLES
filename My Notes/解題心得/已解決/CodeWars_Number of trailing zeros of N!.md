# CodeWars:Number of trailing zeros of N!:20200922:C#

[Reference](https://www.codewars.com/kata/52f787eb172a8b4ae1000a34/csharp)



## Question

Write a program that will calculate the number of trailing zeros in a factorial of a given number.

```
N! = 1 * 2 * 3 * ... * N
```

Be careful `1000!` has 2568 digits...

For more info, see: http://mathworld.wolfram.com/Factorial.html

### Examples

```python
zeros(6) = 1
# 6! = 1 * 2 * 3 * 4 * 5 * 6 = 720 --> 1 trailing zero

zeros(12) = 2
# 12! = 479001600 --> 2 trailing zeros
```

*Hint: You're not meant to calculate the factorial. Find another way to find the number of zeros.*

## My Solution

```C#
using System;

public static class Kata 
{
        public static int TrailingZeros(int n)
        {
            int result = 0;
            for(int f=1;Math.Pow(5,f)<=n;f++)
                result += n / (int)Math.Pow(5, f);
            return result;
        }
}
```

5 kyu 非常簡單的題目，因為最後那段提示`*Hint: You're not meant to calculate the factorial. Find another way to find the number of zeros.*`又變得更簡單了，後方0的數目來自factorial中10的個數再將其拆成2和5，又因為2的數量一定比5多所以只要考慮5的數量就好，推斷每5個數就會多一個0，比如5=>1，因為包含2、4(2*2)、5共一組2和5，每25個也會多一個0因為多了1個5，125、625...以此類推，雖然是個簡單的題目但是也看到了不少不錯的解答方式，所以還是紀錄一下。

## Better Solutions

### Solution 1

```C#
using System;

public static class Kata 
{
  public static int TrailingZeros(int n)
  { 
    int fives = 0;
    for (int i = 5; i <= n; i *= 5)
        fives += n/i;
    
    return fives;
  }
}
```

基本上跟我寫的一樣，不過用i*=5簡潔許多。



### Solution 2

```C#
using System;
using System.Linq;

public static class Kata 
{
  public static int TrailingZeros(int n) => Enumerable.Range(1, (int)Math.Log(n, 5)).Sum(i => (int)(n / Math.Pow(5, i)));
}
```

不意外的linq一行解。



### Solution 3

```C#
using System;

public static class Kata 
{
  public static int TrailingZeros(int n)
  {
    return (n >= 5) ? (n / 5) + TrailingZeros(n / 5) : 0;
  }
}
```

遞迴一行解，相當漂亮的解法，我給一個clever。