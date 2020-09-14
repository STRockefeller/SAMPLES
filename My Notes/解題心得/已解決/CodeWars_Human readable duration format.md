# CodeWars:Human readable duration format:202008:C#

[Reference](https://www.codewars.com/kata/52742f58faf5485cae000b9a)



## Question

Your task in order to complete this Kata is to write a function which formats a duration, given as a number of seconds, in a human-friendly way.

The function must accept a non-negative integer. If it is zero, it just returns `"now"`. Otherwise, the duration is expressed as a combination of `years`, `days`, `hours`, `minutes` and `seconds`.

It is much easier to understand with an example:

```c
formatDuration (62)    // returns "1 minute and 2 seconds"
formatDuration (3662)  // returns "1 hour, 1 minute and 2 seconds"
```

**For the purpose of this Kata, a year is 365 days and a day is 24 hours.**

Note that spaces are important.

### Detailed rules

The resulting expression is made of components like `4 seconds`, `1 year`, etc. In general, a positive integer and one of the valid units of time, separated by a space. The unit of time is used in plural if the integer is greater than 1.

The components are separated by a comma and a space (`", "`). Except the last component, which is separated by `" and "`, just like it would be written in English.

A more significant units of time will occur before than a least significant one. Therefore, `1 second and 1 year` is not correct, but `1 year and 1 second` is.

Different components have different unit of times. So there is not repeated units like in `5 seconds and 1 second`.

A component will not appear at all if its value happens to be zero. Hence, `1 minute and 0 seconds` is not valid, but it should be just `1 minute`.

A unit of time must be used "as much as possible". It means that the function should not return `61 seconds`, but `1 minute and 1 second` instead. Formally, the duration specified by of a component must not be greater than any valid more significant unit of time.

## My Solution

```C#
using System;
using System.Collections.Generic;

public class HumanTimeFormat{
    public static string formatDuration(int seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            if (seconds == 0)
                return "now";

            return strTimeFormat((int)timeSpan.TotalDays / 365,(int)timeSpan.TotalDays % 365, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
        public static string strTimeFormat(int y, int d, int h, int m, int s)
        {
            List<string> listTimeStr = new List<string>();
            string yy;
            string dd;
            string hh;
            string mm;
            string ss;
            if (y == 1)
                yy = "1 year";
            else
                yy = $"{y} years";
            if (d == 1)
                dd = "1 day";
            else
                dd = $"{d} days";
            if (h == 1)
                hh = "1 hour";
            else
                hh = $"{h} hours";
            if (m == 1)
                mm = "1 minute";
            else
                mm = $"{m} minutes";
            if (s == 1)
                ss = "1 second";
            else
                ss = $"{s} seconds";
            if (y != 0)
                listTimeStr.Add(yy);
            if (d != 0)
                listTimeStr.Add(dd);
            if (h != 0)
                listTimeStr.Add(hh);
            if (m != 0)
                listTimeStr.Add(mm);
            if (s != 0)
                listTimeStr.Add(ss);
            return strSubFormat(listTimeStr);
        }
        public static string strSubFormat(List<string> ls)
        {
            switch (ls.Count)
            {
                case 1:
                    return ls[0];
                case 2:
                    return $"{ls[0]} and {ls[1]}";
                case 3:
                    return $"{ls[0]}, {ls[1]} and {ls[2]}";
                case 4:
                    return $"{ls[0]}, {ls[1]}, {ls[2]} and {ls[3]}";
                case 5:
                    return $"{ls[0]}, {ls[1]}, {ls[2]}, {ls[3]} and {ls[4]}";
                default:
                    return "";
            }
        }
}
```

印象中當初這題解的滿順利的沒有甚麼卡關的情形發生，kyu4 以上難度的題目都會被我寫得很長已經是常態了...

## Better Solutions

### Solution 1

```C#
public class HumanTimeFormat{
  public static string formatDuration(int seconds){
    //Enter Code here
            string s = "";
            int sec = seconds;
            int[] divArr = { 60 * 60 * 24 * 365, 60 * 60 * 24, 60 * 60, 60, 1 };
            string[] nameArr = {"year","day","hour","minute","second"};

            if (seconds == 0)
            {
                s = "now";
            }

            for (int i = 0; i< divArr.Length; i++)
            {
                int k = sec / divArr[i];
                sec = sec % divArr[i];
                if (k != 0)
                {
                    string pref = "";
                    if (s != "")
                    {
                        if (sec == 0)
                        {
                            pref = " and ";    
                        }
                        else
                        {
                            pref = ", ";
                        }
                    }
                    s = s + pref + k.ToString() + " " + nameArr[i];
                    s += k > 1 ? "s" : "";
                }
            }
            return s;
  }
}
```



### Solution 2

```C#
using System;
using System.Linq;

public class HumanTimeFormat{
  public static string formatDuration(int seconds)
  {
    var t = TimeSpan.FromSeconds(seconds);
    Func<int, string, string> f = (n, l) => n == 0 ? "" : $"{n} {l}" + (n == 1 ? "" : "s");
    var h = new[] { f(t.Days / 365, "year"), f(t.Days % 365, "day"), f(t.Hours, "hour"), f(t.Minutes, "minute"), f(t.Seconds, "second") }.Where(p => p.Any());  
    return h.Count() == 0 ? "now" : h.Count() == 1 ? h.Single() : string.Join(", ", h.Take(h.Count() - 1)) + " and " + h.Last();
  }
}
```

最短解，好吧我看不懂。說好的Human readable呢?