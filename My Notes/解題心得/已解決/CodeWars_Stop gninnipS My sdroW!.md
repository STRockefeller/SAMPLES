# CodeWars:Stop gninnipS My sdroW!:20200914:C#

[Reference](https://www.codewars.com/kata/5264d2b162488dc400000001/csharp)



## Question

Write a function that takes in a string of one or more words, and returns the same string, but with all five or more letter words reversed (Just like the name of this Kata). Strings passed in will consist of only letters and spaces. Spaces will be included only when more than one word is present.

Examples: spinWords( "Hey fellow warriors" ) => returns "Hey wollef sroirraw" spinWords( "This is a test") => returns "This is a test" spinWords( "This is another test" )=> returns "This is rehtona test"

## My Solution

```C#
using System.Collections.Generic;
using System.Linq;
using System;

public class Kata
{
        public static string SpinWords(string sentence)
        {
            List<string> strList = sentence.Split(" ").ToList();
            var query = strList.Select(str =>
            {
                char[] arr = str.ToCharArray();
                Array.Reverse(arr);
                return str.Length >= 5 ? new string(arr) : str;
            });
            return String.Join(" ", query);
        }
}
```

簡單的題目，原本想嘗試一行解，但是字串反轉的那個部分一直無法成功濃縮成一行(主要因為`Array.Reverse()`方法回傳void直接改變傳入的參數導致其很難使用在一行內)

## Better Solutions

```C#
using System.Collections.Generic;
using System.Linq;
using System;

public class Kata
{
  public static string SpinWords(string sentence)
  {
    return String.Join(" ", sentence.Split(' ').Select(str => str.Length >= 5 ? new string(str.Reverse().ToArray()) : str));
  }
}
```

別人的一行解，基本差不多，差在他成功在一行內完成反轉字串`new string(str.Reverse().ToArray())`

