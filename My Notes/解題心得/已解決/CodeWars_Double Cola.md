# CodeWars:Double Cola:20200915:C#

[Reference](https://www.codewars.com/kata/551dd1f424b7a4cdae0001f0)



## Question

Sheldon, Leonard, Penny, Rajesh and Howard are in the queue for a "Double Cola" drink vending machine; there are no other people in the queue. The first one in the queue (Sheldon) buys a can, drinks it and doubles! The resulting two Sheldons go to the end of the queue. Then the next in the queue (Leonard) buys a can, drinks it and gets to the end of the queue as two Leonards, and so on.

For example, Penny drinks the third can of cola and the queue will look like this:

```
Rajesh, Howard, Sheldon, Sheldon, Leonard, Leonard, Penny, Penny
```

Write a program that will return the name of the person who will drink the `n`-th cola.

### Input:

The input data consist of an array which contains at least 1 name, and single integer `n` which may go as high as the biggest number your language of choice supports (if there's such limit, of course).

### Output / Examples:

Return the single line — the name of the person who drinks the n-th can of cola. The cans are numbered starting from 1.

```csharp
string[] names = new string[] { "Sheldon", "Leonard", "Penny", "Rajesh", "Howard" };
Line.WhoIsNext(names, 1) == "Sheldon"
Line.WhoIsNext(names, 52) == "Penny"
Line.WhoIsNext(names, 7230702951) == "Leonard"
```

##### courtesy of CodeForces: https://codeforces.com/problemset/problem/82/A



## My Solution

看完這題目首先想到的是如此兇殘的遊戲肯定會玩死人...





```C#
        public static string WhoIsNext(string[] names, long n)
        {
            List<string> nameList = new List<string>(names);
            while(true)
            {
                if (n <= nameList.Count && n<=Int32.MaxValue)
                    return nameList[Convert.ToInt32(n) - 1];
                nameList.AddRange(new string[] {nameList[0],nameList[0]});
                nameList = nameList.Skip(1).ToList();
                n--;
            }
        }
```

初見最直接的想法，看到測試項目有一個`Line.WhoIsNext(names, 7230702951) == "Leonard"`就知道肯定過不了關(理論上花很多時間應該還是算得出來)，只好果斷重寫。

```C#
    public class Line
    {
        public static string WhoIsNext(string[] names, long n)
        {
            int playerCount = names.Length;
            for (int level = 1; ; level++)
                if (n <= playerCount * LevelCount(level))
                    return names[(int)Math.Ceiling((double)(n - playerCount * LevelCount(level - 1)) / Math.Pow(2, level - 1)) - 1];
        }

        public static long LevelCount(int level) => Convert.ToInt64(Enumerable.Range(0, level).Select(num => Math.Pow(2, num)).Sum());
    }
```

改變思路，不要超長陣列，不要一次一次計算，直接算出n是第幾個人再回傳，寫得比想像中的還簡潔這點我很滿意，中途常常腦筋轉不過來(數學老師的鍋)



## Better Solutions



### Solution 1

```C#
using System;

public class Line
    {
        public static string WhoIsNext(string[] names , long n)
        { 
            long x = 5;
            long i = 1;
  
            while (n > x)
            {
                n -= x;
                x *= 2;
                i *= 2;
            }
            
            return (names[(n - 1)/i]);
        }
    }
```

無話可說，優秀，簡潔明瞭，要雞蛋裡挑骨頭的話就是 `long x = 5`抓測試程式給的names都是5個人的破綻



### Solution 2

```C#
using System;

public class Line
{
    public static string WhoIsNext(string[] names , long n)
    {
        var l = names.Length;
        return n <= l ? names[n - 1] : WhoIsNext(names, (n - l + 1) / 2);
    }
}
```

相當精彩的遞迴，佩服。