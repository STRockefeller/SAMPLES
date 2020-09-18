# CodeWars:Sum of Digits / Digital Root:20200916:C#

[Reference](https://www.codewars.com/kata/541c8630095125aba6000c00/csharp)



## Question

[Digital root](https://en.wikipedia.org/wiki/Digital_root) is the *recursive sum of all the digits in a number.*

Given `n`, take the sum of the digits of `n`. If that value has more than one digit, continue reducing in this way until a single-digit number is produced. This is only applicable to the natural numbers.

### Examples

```
    16  -->  1 + 6 = 7
   942  -->  9 + 4 + 2 = 15  -->  1 + 5 = 6
132189  -->  1 + 3 + 2 + 1 + 8 + 9 = 24  -->  2 + 4 = 6
493193  -->  4 + 9 + 3 + 1 + 9 + 3 = 29  -->  2 + 9 = 11  -->  1 + 1 = 2
```

## My Solution

```C#
using System;
using System.Linq;
public class Number
{
        public int DigitalRoot(long n)=> Convert.ToInt32(subDigitalRoot(n.ToString()));
        private string subDigitalRoot(string num)=>num.Length == 1?num: subDigitalRoot(num.Select(c => Convert.ToInt32(c.ToString())).Sum().ToString());
}
```

兩行，還算不錯吧應該

看到題目最直接想到的寫法，將輸入轉為字串做拆解，轉為數值做加總，再次轉為字串遞迴。

## Better Solutions

### Solution 1

```C#
public class Number
{
  public int DigitalRoot(long n)
  {
     return (int)(1 + (n - 1) % 9);
  }
}
```

四十多個人寫出這個解法，數學老師我對不起你

[Wikipedia:Digital Root](https://en.wikipedia.org/wiki/Digital_root#Congruence_formula)

### Solution 2

```C#
public class Number
{
    public int DigitalRoot(long n)
    {
        if (n < 10) return (int)n;
        long r = 0;
        while (n > 0)
        {
            r += n % 10;
            n /= 10;
        }
        return DigitalRoot(r);
    }
}
```

簡單易懂的寫法，只用一個函式遞迴