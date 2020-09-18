# CodeWars:Where my anagrams at?:20200916:C#

[Reference](https://www.codewars.com/kata/523a86aa4230ebb5420001e1/csharp)



## Question

What is an anagram? Well, two words are anagrams of each other if they both contain the same letters. For example:

```
'abba' & 'baab' == true

'abba' & 'bbaa' == true

'abba' & 'abbba' == false

'abba' & 'abca' == false
```

Write a function that will find all the anagrams of a word from a list. You will be given two inputs a word and an array with words. You should return an array of all the anagrams or an empty array if there are none. For example:

```javascript
anagrams('abba', ['aabb', 'abcd', 'bbaa', 'dada']) => ['aabb', 'bbaa']

anagrams('racer', ['crazer', 'carer', 'racar', 'caers', 'racer']) => ['carer', 'racer']

anagrams('laser', ['lazing', 'lazy',  'lacer']) => []
```

## My Solution

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public static class Kata
{
        public static List<string> Anagrams(string word, List<string> words)
        {
            List<string> returnValue = new List<string>();
            foreach(string perWord in words)
                if (new string(perWord.OrderBy(c => c).ToArray()) == new string(word.OrderBy(c => c).ToArray()))
                    returnValue.Add(perWord);

            return returnValue;
        }
}
```

把字串排序後做比較，相同者輸出

## Better Solutions

### Solution 1

```C#
using System.Text;
using System.Linq;
using System;
using System.Collections.Generic;

public static class Kata
{
    public static List<string> Anagrams(string word, List<string> words)
    {
        var pattern = word.OrderBy(p => p).ToArray();
        return words.Where(item => item.OrderBy(p => p).SequenceEqual(pattern)).ToList();
    }
}
```



[MSDN:SequenceEqual](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.sequenceequal?view=netcore-3.1)

### Solution 2

```C#
using System;
using System.Collections.Generic;
using System.Linq;

public static class Kata
{
  // Sorting is a cheap tactic to make fast algorithms slower
  public static List<string> Anagrams(string word, List<string> words)
  {
    // Hash of the count of each character in word
    Dictionary<char, int> wordCount = word.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count(c => c == c));
    
    // Filter words due to the following predicate
    return words.Where(w => 
      {
        // Hash of the count of each character of a word in words
        Dictionary<char, int> wCount = w.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count(c => c == c));
        
        // Check if the two hashes are equal
        return wCount.Count == wordCount.Count && !wCount.Except(wordCount).Any();
      }).ToList();
  }
}
```

看評論似乎是比OrderBy更快速的解法