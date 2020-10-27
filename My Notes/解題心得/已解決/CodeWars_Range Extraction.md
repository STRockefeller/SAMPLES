# CodeWars:CodeWars_Range Extraction:20201027:C#

[Reference](https://www.codewars.com/kata/51ba717bb08c1cd60f00002f/train/csharp)



## Question

A format for expressing an ordered list of integers is to use a comma separated list of either

- individual integers
- or a range of integers denoted by the starting integer separated from the end integer in the range by a dash, '-'. The range includes all integers in the interval including both endpoints. It is not considered a range unless it spans at least 3 numbers. For example "12,13,15-17"

Complete the solution so that it takes a list of integers in increasing order and returns a correctly formatted string in the range format.

**Example:**

```javascript
solution([-6, -3, -2, -1, 0, 1, 3, 4, 5, 7, 8, 9, 10, 11, 14, 15, 17, 18, 19, 20]);
// returns "-6,-3-1,3-5,7-11,14,15,17-20"
```

*Courtesy of rosettacode.org*

## My Solution

4 Kyu 新題目

看起來不複雜，雖然題目沒講明，但依Example推斷輸入很可能都是升冪排列，就先依此下去解題吧

跑迴圈，建一個List去存已讀取項目，當不符合`num[n+1]==num[n]+1`時輸出

```C#
using System.Collections.Generic;
using System.Linq;   
    public class RangeExtraction
    {
        public static string Extract(int[] args)
        {
            List<int> tempNum = new List<int>();
            List<string> res = new List<string>();
            foreach (int num in args)
            {
                if (tempNum.Count == 0 || num == tempNum.Max() + 1)
                {
                    tempNum.Add(num);
                    if (num != args.Max())
                        continue;
                }
                if (tempNum.Count < 3)
                {
                    res.AddRange(tempNum.Select(num => num.ToString()));
                    tempNum.Clear();
                    tempNum.Add(num);
                    if (num == args.Max() && !res.Contains(num.ToString()))
                        res.Add(num.ToString());
                }
                else
                {
                    res.Add($"{tempNum.Min()}-{tempNum.Max()}");
                    if (num == args.Max() && !tempNum.Contains(num))
                        res.Add(num.ToString());
                    tempNum.Clear();
                    tempNum.Add(num);
                }
            }
            return string.Join(',', res);
        }
    }
```

過關，CodeWars測試通關時間`**Completed in 37.8660ms**`，嘗試壓縮這個時間

把`args.Max()`提出來就不用一直找最大了。(比較過`args.Last()`、`args.Max()`以及`args[args.Length-1]`後者最快)

```C#
using System.Collections.Generic;
using System.Linq;   
    public class RangeExtraction
    {
        public static string Extract(int[] args)
        {
            int max = args[args.Length-1];
            List<int> tempNum = new List<int>();
            List<string> res = new List<string>();
            foreach (int num in args)
            {
                if (tempNum.Count == 0 || num == tempNum.Max() + 1)
                {
                    tempNum.Add(num);
                    if (num != max)
                        continue;
                }
                if (tempNum.Count < 3)
                {
                    res.AddRange(tempNum.Select(num => num.ToString()));
                    tempNum.Clear();
                    tempNum.Add(num);
                    if (num == max && !res.Contains(num.ToString()))
                        res.Add(num.ToString());
                }
                else
                {
                    res.Add($"{tempNum.Min()}-{tempNum.Max()}");
                    if (num == max && !tempNum.Contains(num))
                        res.Add(num.ToString());
                    tempNum.Clear();
                    tempNum.Add(num);
                }
            }
            return string.Join(',', res);
        }
    }
```

`**Completed in 32.0560ms**`



## Better Solutions



### Solution 1

```C#
using System;
using System.Collections.Generic;
using System.Linq;

public class RangeExtraction
{
    public int Value, Count;
    public int NextValue => Value + Count;

    public RangeExtraction(int value)
    {
        Value = value;
        Count = 1;
    }

    public override string ToString()
        => Count == 1 ? $"{Value}" :
           Count == 2 ? $"{Value},{Value + 1}" :
                        $"{Value}-{NextValue-1}";

    public static string Extract(int[] args)
    {
        var list = new List<RangeExtraction>();
        
        foreach (var n in args)
            if (list.LastOrDefault()?.NextValue == n) list.Last().Count++;
            else list.Add(new RangeExtraction(n));

        return string.Join(",", list);
    }
}
```

