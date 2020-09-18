# CodeWars:Rot13:20200916:C#

[Reference](https://www.codewars.com/kata/530e15517bc88ac656000716/csharp)



## Question

ROT13 is a simple letter substitution cipher that replaces a letter with the letter 13 letters after it in the alphabet. ROT13 is an example of the Caesar cipher.

Create a function that takes a string and returns the string ciphered with Rot13. If there are numbers or special characters included in the string, they should be returned as they are. Only letters from the latin/english alphabet should be shifted, like in the original Rot13 "implementation".

## My Solution

關於題目

[Wikipedia:Rot13](https://en.wikipedia.org/wiki/ROT13)

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public class Kata
{
        public static string Rot13(string message)
        {
            var returnValue = message.Select(c =>
            {
                if (increase(Convert.ToInt32(c)))
                    return Convert.ToChar(Convert.ToInt32(c) + 13);
                if (decrease(Convert.ToInt32(c)))
                    return Convert.ToChar(Convert.ToInt32(c) - 13);
                return c;
            }).ToArray();
            return new string(returnValue);
        }
        private static bool increase(int num) => (num >= 65 && num <= 77) || (num >= 97 && num <= 109) ? true : false;
        private static bool decrease(int num) => (num >= 78 && num <= 90) || (num >= 110 && num <= 122) ? true : false;
}
```

嘗試一行解?中途就放棄了，要判斷的條件太多寫成一行也會長的誇張，還是分段把邏輯寫清楚好了

建兩個方法判斷是否要加或減13都不符合救回傳原來的字元

## Better Solutions

### Solution 1

```C#
using System;
public class Kata
{
  public static string Rot13(string message)
  {
    string result = "";
            foreach (var s in message)
            {
                if ((s >= 'a' && s <= 'm') || (s >= 'A' && s <= 'M'))
                    result += Convert.ToChar((s + 13)).ToString();
                else if ((s >= 'n' && s <= 'z') || (s >= 'N' && s <= 'Z'))
                    result += Convert.ToChar((s - 13)).ToString();
                else result += s;
            }
            return result;
  }
}
```

差不多的思路但更為簡潔

### Solution 2

```C#
using System;
using System.Linq;
public class Kata
{
  public static string Rot13(string message)
  {
     return String.Join("", message.Select(x => char.IsLetter(x) ? (x >= 65 && x <= 77) || (x >= 97 && x <= 109) ? (char)(x + 13) : (char)(x - 13) : x));
  }
}
```

出現了，是一行解

`IsLetter`判斷是否為unicode字元，竟然還有這招

[MSDN:IsLetter](https://docs.microsoft.com/zh-tw/dotnet/api/system.char.isletter?view=netcore-3.1)