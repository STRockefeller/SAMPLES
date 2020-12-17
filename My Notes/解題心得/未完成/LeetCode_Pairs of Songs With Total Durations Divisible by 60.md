# LeetCode:Pairs of Songs With Total Durations Divisible by 60:20201210:C#

[Reference](https://leetcode.com/explore/challenge/card/december-leetcoding-challenge/570/week-2-december-8th-december-14th/3559/)



## Question

You are given a list of songs where the ith song has a duration of `time[i]` seconds.

Return *the number of pairs of songs for which their total duration in seconds is divisible by* `60`. Formally, we want the number of indices `i`, `j` such that `i < j` with `(time[i] + time[j]) % 60 == 0`.

 

**Example 1:**

```
Input: time = [30,20,150,100,40]
Output: 3
Explanation: Three pairs have a total duration divisible by 60:
(time[0] = 30, time[2] = 150): total duration 180
(time[1] = 20, time[3] = 100): total duration 120
(time[1] = 20, time[4] = 40): total duration 60
```

**Example 2:**

```
Input: time = [60,60,60]
Output: 3
Explanation: All three pairs have a total duration of 120, which is divisible by 60.
```

 

**Constraints:**

- `1 <= time.length <= 6 * 10^4`
- `1 <= time[i] <= 500`

## My Solution

LeetCode 2020 12月 第二週 活動題目(?)

試試身手，看題目其實並不複雜，重點應該是如何避免寫成O(n^2)

第一版

```C#
public class Solution {
    public int NumPairsDivisibleBy60(int[] time) {
        int res=0;
        int[] timeModulo = new int[time.Length];
        for(int i = 0; i<time.Length ; i++)
            timeModulo[i] = time[i]%60;
        int[] backet = new int[60];
        foreach(int t in timeModulo)
            backet[t]++;
        for(int i=1 ; i<30 ; i++)
            if(backet[i]!=0)
                res+=backet[i]*backet[60-i];
        res += take2(backet[0]) + take2(backet[30]);
        return res;
    }
    private int take2(int num) => num >= 2 ? factorial(num) / 2 * (factorial(num - 2)) : 0;
    private int factorial(int n)
    {
        int res = 1;
        foreach (int num in Enumerable.Range(1, n))
            res *= num;
        return res;
    }
}
```

進階測試，失敗。

## Better Solutions

