# CodeWars:Sum of Intervals:20200918:C#

[Reference](https://www.codewars.com/kata/52b7ed099cdc285c300001cd/csharp)



## Question

Write a function called `sumIntervals`/`sum_intervals()` that accepts an array of intervals, and returns the sum of all the interval lengths. Overlapping intervals should only be counted once.

### Intervals

Intervals are represented by a pair of integers in the form of an array. The first value of the interval will always be less than the second value. Interval example: `[1, 5]` is an interval from 1 to 5. The length of this interval is 4.

### Overlapping Intervals

List containing overlapping intervals:

```
[
   [1,4],
   [7, 10],
   [3, 5]
]
```

The sum of the lengths of these intervals is 7. Since [1, 4] and [3, 5] overlap, we can treat the interval as [1, 5], which has a length of 4.

### Examples:

```javascript
sumIntervals( [
   [1,2],
   [6, 10],
   [11, 15]
] ); // => 9

sumIntervals( [
   [1,4],
   [7, 10],
   [3, 5]
] ); // => 7

sumIntervals( [
   [1,5],
   [10, 20],
   [1, 6],
   [16, 19],
   [5, 11]
] ); // => 19
```

## My Solution

```C#
using System;
using System.Collections.Generic;
using System.Linq;

public class Intervals
{
        public static int SumIntervals((int, int)[] intervals)
        {
            intervals = intervals.OrderBy(ia => ia.Item1).ToArray();
            int sum = 0;
            for (int i = 0; i < intervals.Length; i++)
            {
                for (int j = i + 1; j < intervals.Length; j++)
                    intervals[j].Item1 = intervals[i].Item2 > intervals[j].Item1 ? intervals[i].Item2 : intervals[j].Item1;
                sum += intervals[i].Item2 - intervals[i].Item1 > 0 ? intervals[i].Item2 - intervals[i].Item1 : 0;
            }
            return sum;
        }
}
```

4 kyu 第一眼看覺得簡單想一下又覺得不簡單再想一下又覺得很簡單的題目(?)，解題過程算是相當順利。

想法是先依左邊數字作排列，然後依序一一將後面數字有重複的地方去除(把比前面出現過的item2還小的item1變成前面出現過的item2)

最後加總(這邊測試失敗一次，因為沒有考慮到將數值改為後面比前面小的情況)

## Better Solutions

### Solution 1

```C#
using System;
using System.Linq;

public class Intervals
{
  public static int SumIntervals((int, int)[] intervals)
  {
    return intervals
      .SelectMany(i => Enumerable.Range(i.Item1, i.Item2 - i.Item1))
      .Distinct()
      .Count();
  }
}
```

一行解，果然沒有linq做不到的事情只是我想不到

1. `SelectMany(i => Enumerable.Range(i.Item1, i.Item2 - i.Item1))`得到全interval涵蓋的數字(不包含item2，因為是取item2-item1個)
2. `Distinct()`刪除重複
3. `Count();`取得個數

拆開來看其實道理滿簡單的。