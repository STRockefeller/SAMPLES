# CodeWars:The Spider and the Fly:20210226:C#

[Reference](https://www.codewars.com/kata/59cb7e01d751df471e0000a8/csharp)



## Question

### Background

A spider web is defined by

- "rings" numbered out from the centre as `0`, `1`, `2`, `3`, `4`

- "radials" labelled clock-wise from the top as `A`, `B`, `C`, `D`, `E`, `F`, `G`, `H`

Here is a picture to help explain:

![source: imgur.com](https://i.imgur.com/aSAWPl0.png)

### Web Coordinates

As you can see, each point where the rings and the radials intersect can be described by a "web coordinate".

So in this example the spider is at `H3` and the fly is at `E2`

### Kata Task

Our friendly spider is resting and minding his own spidery business at web-coordinate `spider`.

An inattentive fly bumbles into the web at web-coordinate `fly` and gets itself stuck.

Your task is to calculate and return ***\*the shortest path\**** that the spider can take to get to the fly.

### Example

The solution to the scenario described by the picture is `H3-H2-H1-A0-E1-E2`

### Notes

- The centre of the web will always be referred to as `A0`
- Use the `-` character to separate web-coordinates in your "shortest path" result (see the example above)
- The rings intersect the radials at **evenly** spaced distances of **1 unit**
- The spider can only move along the web. He is not a jumping spider!
- Return "the shortest path" means return shortest *total distance* of the path from spider to fly

## My Solution

5 kyu 的題目

一樣先來分析問題

* 題目要找的最短路徑指的是**距離**，而非**經過的節點數**

  * 所以範例的答案是`H3-H2-H1-A0-E1-E2`而非`H3-G3-F3-E3-E2`或者`H3-H2-G2-F2-E2`
  * 所以勢必需要比較每條路徑的距離，以下是快樂數學時間
    * 假設每個`rings`的距離都是1
    * 夾角為360/8 = 45度=>`一個45-67.5-67.5三角形已知等邊為1求第三邊`
    * 答案為2*cos(67.5 degree)約等於0.765
    * 在`0-1-2-3-4`走一個`radials`的距離分別是`0-0.765-1.531-2.295-3.06`

* 接著是找最短路徑的邏輯

  * 因為內圈比外圈小，所以不會有往外圈繞路的情形，兩點的`rings`不同時也不考慮外圈的走法。

  * 兩點在直線的情況下走直線

  * 剩下看角度決定

    * 夾角為45度的情況下

      * `rings`為1處：

        1. 走`rings`兩次為2

        2. 走`radials`一次為0.765 

        * 走`radials`一次比較近，距離為0.765

      * `rings`為2處：

        1. 走`rings`到1處來回為2，加上`rings`為1處的移動距離為0.765，共2.765
        2. 走`radials`一次為1.531

        * 走`radials`一次比較近，距離為1.531

      * `rings`為3處：

        1. 先走`rings`到2處的方法距離為2+1.531 = 3.531
        2. 走`radials``為2.259

        * 走`radials`一次比較近，距離為2.259

      * `rings`為4處：

        1. 先走`rings`到3處的方法距離為2+2.259 = 3.259
        2. 走`radials`為3.06

        * 走`radials`一次比較近，距離為3.06

    * 夾角為90度的情況下

      * `rings`為1處：

        1. 走`rings`兩次為2

        2. 走`radials`兩次為1.531

        * 走`radials`兩次比較近，距離為1.531

      * `rings`為2處：

        1. 走`rings`到1處來回為2，加上`rings`為1處的移動距離為1.531，共3.531
        2. 走`radials`兩次為3.062

        * 走`radials`兩次比較近，距離為3.062

      * `rings`為3處：

        1. 先走`rings`到2處的方法距離為2+3.062 = 5.062
        2. 走`radials`為4.518

        * 走`radials`兩次比較近，距離為4.518

      * `rings`為4處：

        1. 先走`rings`到3處的方法距離為2+4.518 = 6.518
        2. 走`radials`為6.12

        * 走`radials`兩次比較近，距離為6.12

    * 夾角為135度的情況下

      * `rings`為1處：

        1. 走`rings`兩次為2

        2. 走`radials`三次為2.295

        * 走`rings`兩次比較近，距離為2

      * `rings`為2處：

        1. 走`rings`到1處來回為2，加上`rings`為1處的移動距離為2，共4
        2. 走`radials`三次為4.593

        * 走`rings`比較近，距離為4

      * `rings`為3處：

        1. 先走`rings`到2處的方法距離為2+4 = 6
        2. 不算了反正比較遠

        * 走`rings`比較近，距離為6

      * `rings`為4處：

        1. 先走`rings`到3處的方法距離為2+6=8
        2. 不算了反正比較遠

        * 走`rings`比較近，距離為8

    * 夾角為180度的情況下

      * 就直線還繞甚麼路

  * 結論

    * 夾角為45度或90度的情況下，都採用走`radials`的路線
    * 夾角為135度或180度的情況下，都採用走`rings`到中心點再出發的路線

都分析完了好像就沒啥寫程式的必要了

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public class Dinglemouse
    {
        public static string SpiderToFly(string spider, string fly)
        {
            char spiderRadials = spider.First();
            char spiderRings = spider.Last();
            char flyRadials = fly.First();
            char flyRings = fly.Last();
            List<string> path = new List<string>();
            int degree;
            switch (spiderRadials - flyRadials)
            {
                case 1:
                case -1:
                case 7:
                case -7:
                    degree = 45;
                    break;
                case 2:
                case -2:
                case 6:
                case -6:
                    degree = 90;
                    break;
                case 3:
                case -3:
                    degree = 135;
                    break;
                default:
                    degree = 180;
                    break;
            }
            if (spider == "A0" || fly == "A0" || degree == 180)
            {
                if (spider == "A0")
                {
                    path.Add("A0");
                    while (spiderRings != flyRings)
                    {
                        spiderRings++;
                        path.Add(new string(new char[2] { flyRadials, spiderRings }));
                    }
                    return string.Join('-', path);
                }
                if (fly == "A0")
                {
                    path.Add(spider);
                    while (spiderRings != '1')
                    {
                        spiderRings--;
                        path.Add(new string(new char[2] { spiderRadials, spiderRings }));
                    }
                    path.Add("A0");
                    return string.Join('-', path);
                }
                if (spiderRadials != flyRadials)
                {
                    path.Add(spider);
                    while (spiderRings != '1')
                    {
                        spiderRings--;
                        path.Add(new string(new char[2] { spiderRadials, spiderRings }));
                    }
                    path.Add("A0");
                    while (spiderRings != flyRings)
                    {
                        path.Add(new string(new char[2] { flyRadials, spiderRings }));
                        spiderRings++;
                    }
                    path.Add(new string(new char[2] { flyRadials, spiderRings }));
                    return string.Join('-', path);
                }
                path.Add(spider);
                while (spiderRings != flyRings)
                {
                    spiderRings = spiderRings > flyRings ? Convert.ToChar(spiderRings - 1) : Convert.ToChar(spiderRings + 1);
                    path.Add(new string(new char[2] { flyRadials, spiderRings }));
                }
                return string.Join('-', path);
            }
            while (spiderRings > flyRings)
            {
                addSpiderCurrentLocation();
                spiderRings--;
            }
            if (degree >= 135)
            {
                addSpiderCurrentLocation();
                while (spiderRings > '1')
                {
                    spiderRings--;
                    addSpiderCurrentLocation();
                }
                path.Add("A0");
                for (char i = '1'; i <= flyRings; i++)
                    path.Add(flyRadials + i.ToString());
            }
            else if (degree == 45)
            {
                addSpiderCurrentLocation();
                spiderRadials = flyRadials;
                addSpiderCurrentLocation();
                while (spiderRings < flyRings)
                {
                    spiderRings++;
                    addSpiderCurrentLocation();
                }
            }
            else
            {
                addSpiderCurrentLocation();
                if (radialCheck('A', 'G'))
                    spiderRadials = 'H';
                else if (radialCheck('H', 'B'))
                    spiderRadials = 'A';
                else
                    spiderRadials = spiderRadials > flyRadials ?
                        Convert.ToChar(spiderRadials - 1) : Convert.ToChar(spiderRadials + 1);
                addSpiderCurrentLocation();
                spiderRadials = flyRadials;
                addSpiderCurrentLocation();
                while (spiderRings < flyRings)
                {
                    spiderRings++;
                    addSpiderCurrentLocation();
                }
            }

            bool radialCheck(char T1, char T2) =>
                (spiderRadials == T1 && flyRadials == T2) || (spiderRadials == T2 && flyRadials == T1);
            void addSpiderCurrentLocation() => path.Add(new string(new char[2] { spiderRadials, spiderRings }));
            return string.Join('-', path);
        }
    }
```



## Better Solutions



## Solution 1



```C#
using System;
using System.Linq;
public class Dinglemouse
{   
    public static string SpiderToFly(string s, string f)
    {   
      if (s[1] == '0') s = "A0";
      if (s == f) return s;
      
      int radi = (s == "A0" ? f[0] : s[0]) -'A';
      int ring = s[1]-'0';
      int d = Math.Abs(s[0]-f[0]);
      
      if (radi == f[0]-'A' && s[1] < f[1]) ring++;
      else if (s[1] > f[1] || (d > 2 && d < 6)) ring--;
      else radi+= (s[0]-f[0] &7) < 3 ? 7 : 9;
        
      return s + "-" + SpiderToFly($"{(char)('A' + radi%8)}{ring}", f);  
    }
}
```

