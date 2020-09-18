# CodeWars:Strip Comments:20200918:C#

[Reference](https://www.codewars.com/kata/51c8e37cee245da6b40000bd/csharp)



## Question

Complete the solution so that it strips all text that follows any of a set of comment markers passed in. Any whitespace at the end of the line should also be stripped out.

**Example:**

Given an input string of:

```
apples, pears # and bananas
grapes
bananas !apples
```

The output expected would be:

```
apples, pears
grapes
bananas
```

The code would be called like so:

```csharp
string stripped = StripCommentsSolution.StripComments("apples, pears # and bananas\ngrapes\nbananas !apples", new [] { "#", "!" })
// result should == "apples, pears\ngrapes\nbananas"
```

## My Solution

```C#
        public static string StripComments(string text, string[] commentSymbols)
        {
            List<string> lines = text.Split("\n").ToList();
            for (int index = 0; index < lines.Count; index++)
            {
                foreach (string commentSymbol in commentSymbols)
                    lines[index] = new string(lines[index].Replace(commentSymbol, "#").ToCharArray().TakeWhile(c => c != '#').ToArray()).TrimEnd(' ');
            }
            return string.Join("\n", lines);
        }
```

4 kyu 的題目，比想像中的簡單(目前做過的4 kyu中最簡單的)，考慮比較久的部分是因為`commentSymbols`作為string而非char，一直在考慮如何從字串中找到她的位置，最後是將其以字元'*'取代，再做處理。

其實這個解答雖然通關了但並不完美，因為字串中如果有非註解字元或不應該被註解掉的'*'，答案就會錯了。

## Better Solutions

```C#
using System;
using System.Linq;
using System.Text.RegularExpressions;
public class StripCommentsSolution
{
    public static string StripComments(string text, string[] commentSymbols)
    {
        string[] lines = text.Split(new [] { "\n" }, StringSplitOptions.None);
        lines = lines.Select(x => x.Split(commentSymbols, StringSplitOptions.None).First().TrimEnd()).ToArray();
        return string.Join("\n", lines);
    }
}
```

直接拿`commentSymbols`作為split的分隔字，很不錯的做法。