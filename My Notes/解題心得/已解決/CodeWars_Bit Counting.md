# CodeWars:Bit Counting:20200916:C#

[Reference](https://www.codewars.com/kata/526571aae218b8ee490006f4/csharp)



## Question

Write a function that takes an integer as input, and returns the number of bits that are equal to one in the binary representation of that number. You can guarantee that input is non-negative.

*Example*: The binary representation of `1234` is `10011010010`, so the function should return `5` in this case

## My Solution

```C#
using System;
using System.Linq;

public class Kata
{
  public static int CountBits(int n) => n == 0 ? 0 : Convert.ToString(n, 2).GroupBy(c => c).Where(g => g.Contains('1')).Single().Count();
}
```

一行解完成，比較可惜的是輸入0會跳Exception(應該是因為`Where(g => g.Contains('1'))`抓不到東西)，偷吃步把輸入0的情況另外寫。

## Better Solutions

### Solution 1

```C#
using System;
using System.Linq;

public class Kata
{
  public static int CountBits(int n)
  {
    return Convert.ToString(n, 2).Count(x => x == '1');
  }
}
```

這才是我想要寫的，竟然忘記可以在Count裡面加條件。



### Solution 2

```C#
using System;

public class Kata
{
  public static int CountBits(int n)
  {
    int result = 0;
    while(n > 0)
    {
      result += n & 1;
      n >>= 1;
    }
    return result;
  }
}
```

愣了一下才看懂，因為我完全沒想到還有這種解法。

將輸入和1做位元&運算之後輸入右移。



如範例的1234運作起來應該如下

result=0;

  10011010010

&00000000001

=00000000000

result+=0

 1001101001

&000000001

=000000001

result+=1

...



這兩個運算子雖然知道但幾乎沒機會使用(或者說我不會用)

[MSDN:&](https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/boolean-logical-operators#logical-and-operator-)

[MSDN:>>](https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/operators/bitwise-and-shift-operators#right-shift-operator-)