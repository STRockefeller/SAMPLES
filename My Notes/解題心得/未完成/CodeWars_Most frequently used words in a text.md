# CodeWars:Most frequently used words in a text:20201126:C#

[Reference](https://www.codewars.com/kata/51e056fe544cf36c410000fb)



## Question

Write a function that, given a string of text (possibly with punctuation and line-breaks), returns an array of the top-3 most occurring words, in descending order of the number of occurrences.

### Assumptions:

- A word is a string of letters (A to Z) optionally containing one or more apostrophes (') in ASCII. (No need to handle fancy punctuation.)
- Matches should be case-insensitive, and the words in the result should be lowercased.
- Ties may be broken arbitrarily.
- If a text contains fewer than three unique words, then either the top-2 or top-1 words should be returned, or an empty array if a text contains no words.

### Examples:

```
top_3_words("In a village of La Mancha, the name of which I have no desire to call to
mind, there lived not long since one of those gentlemen that keep a lance
in the lance-rack, an old buckler, a lean hack, and a greyhound for
coursing. An olla of rather more beef than mutton, a salad on most
nights, scraps on Saturdays, lentils on Fridays, and a pigeon or so extra
on Sundays, made away with three-quarters of his income.")
# => ["a", "of", "on"]

top_3_words("e e e e DDD ddd DdD: ddd ddd aa aA Aa, bb cc cC e e e")
# => ["e", "ddd", "aa"]

top_3_words("  //wont won't won't")
# => ["won't", "wont"]
```

### Bonus points (not really, but just for fun):

1. Avoid creating an array whose memory footprint is roughly as big as the input text.
2. Avoid sorting the entire array of unique words.

## My Solution

C#測試程式

```C#
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
public class SolutionTest
{

    [Test]
    public void SampleTests()
    {
        Assert.AreEqual(new List<string> { "e", "d", "a" }, TopWords.Top3("a a a  b  c c  d d d d  e e e e e"));
        Assert.AreEqual(new List<string> { "e", "ddd", "aa" }, TopWords.Top3("e e e e DDD ddd DdD: ddd ddd aa aA Aa, bb cc cC e e e"));
        Assert.AreEqual(new List<string> { "won't", "wont" }, TopWords.Top3("  //wont won't won't "));
        Assert.AreEqual(new List<string> { "e" }, TopWords.Top3("  , e   .. "));
        Assert.AreEqual(new List<string> { }, TopWords.Top3("  ...  "));
        Assert.AreEqual(new List<string> { }, TopWords.Top3("  '  "));
        Assert.AreEqual(new List<string> { }, TopWords.Top3("  '''  "));
        Assert.AreEqual(new List<string> { "a", "of", "on" }, TopWords.Top3(
            string.Join("\n", new string[]{"In a village of La Mancha, the name of which I have no desire to call to",
                  "mind, there lived not long since one of those gentlemen that keep a lance",
                  "in the lance-rack, an old buckler, a lean hack, and a greyhound for",
                  "coursing. An olla of rather more beef than mutton, a salad on most",
                  "nights, scraps on Saturdays, lentils on Fridays, and a pigeon or so extra",
                  "on Sundays, made away with three-quarters of his income." })));
    }
}
```



先不考慮兩個加分項目，最直接想到的方法就是將字串split後座處理排除掉標點符號，然後計算出現次數做比較排序。

```C#
using System;
using System.Collections.Generic;
public class TopWords
{
        public static List<string> Top3(string s)
        {
            List<string> strArr = s.Split(' ').ToList();
            strArr.RemoveAll(str => str == "");
            strArr = strArr.Select(str => cleanLetter(str.ToLower())).ToList();
            strArr.RemoveAll(str => str == "");
            var q = strArr.GroupBy(str => str).OrderByDescending(g => g.Count()).ToList();
            if (q.Count() >= 3)
                return new List<string>() { q[0].FirstOrDefault(), q[1].FirstOrDefault(), q[2].FirstOrDefault() };
            switch (q.Count())
            {
                case 0:
                    return new List<string>();
                case 1:
                    return new List<string>() { q[0].FirstOrDefault() };
                case 2:
                    return new List<string>() { q[0].FirstOrDefault(), q[1].FirstOrDefault() };
                default:
                    return new List<string>() { q[0].FirstOrDefault(), q[1].FirstOrDefault(), q[2].FirstOrDefault() };
            }
            string cleanLetter(string str)
            {
                List<char> charArr = new List<char>(str);
                charArr.RemoveAll(c => (c < 'a' || c > 'z') && c != '\'');
                return charArr.All(c => c == '\'') ? "" : new string(charArr.ToArray());
            }
        }
}
```

第一階段的測試可以順利通過，第二階段的Random Test失敗。



## Better Solutions

