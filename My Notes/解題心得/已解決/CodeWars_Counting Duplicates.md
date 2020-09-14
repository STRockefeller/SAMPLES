# CodeWars:Counting Duplicates:20200914:C#

[Reference](https://www.codewars.com/kata/54bf1c2cd5b56cc47f0007a1/csharp)



## Question

### Count the number of Duplicates

Write a function that will return the count of **distinct case-insensitive** alphabetic characters and numeric digits that occur more than once in the input string. The input string can be assumed to contain only alphabets (both uppercase and lowercase) and numeric digits.

### Example

"abcde" -> 0 `# no characters repeats more than once`
"aabbcde" -> 2 `# 'a' and 'b'`
"aabBcde" -> 2 `# 'a' occurs twice and 'b' twice (`b` and `B`)`
"indivisibility" -> 1 `# 'i' occurs six times`
"Indivisibilities" -> 2 `# 'i' occurs seven times and 's' occurs twice`
"aA11" -> 2 `# 'a' and '1'`
"ABBA" -> 2 `# 'A' and 'B' each occur twice`

## My Solution

```C#
using System;
using System.Linq;
public class Kata
{
        public static int DuplicateCount(string str)
        {
            int counts = 0;
            str = str.ToLower();
            while (str.Length >= 1)
            {
                if (str.Skip(1).Any(ch => ch == str[0]))
                    counts++;
                str = str.Replace(str[0].ToString(), "");
            }
            return counts;
        }
}
```

相當簡單的題目，解題思路是重複把第一個字元抓出來看後面有沒有一樣的字元有就counts++判斷完後將字串中所有該字元刪除直到字串被刪光光。

## Better Solutions

### Solution 1

```C#
using System.Linq;

public class Kata
{
  public static int DuplicateCount(string str)
  {
    return str.ToLower().GroupBy(c => c).Where(g => g.Count() > 1).Count();
  }
}
```

一行解，用了LinQ裡面我比較不熟悉的`GroupBy`

試著解析一下

`str.ToLower().GroupBy(c => c)`將字串中的內容依字元分組(ex:"aabaccd"-->{'a','a','a'},{'b'},{'c','c'},{'d'})

這段的型別是`IEnumerable<IGrouping<char,char>>`

接著從這個`IEnumerable`裡面找數量多於1(有重複)的Group，最後輸出符合條件的Group數量。



[MSDN:GroupBy](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.groupby?view=netcore-3.1)

[ITHelp:GroupBy](https://ithelp.ithome.com.tw/articles/10196181)



### Solution 2

```C#
using System;
using System.Linq;

public class Kata
{
  public static int DuplicateCount(string str)
  {
    return str.ToLower().GroupBy(c => c).Count(c => c.Count() > 1);
  }
}
```



同樣是一行解，做法和上面的差不多

值得一提的是，Count的使用方式，習慣性每次使用Count都是Count()竟然不知道這個方法還可以塞參數進去。



[MSDN:Count](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.count?view=netcore-3.1)