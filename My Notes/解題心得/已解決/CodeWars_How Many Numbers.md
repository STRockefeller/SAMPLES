# CodeWars:How Many Numbers?:20200929:C#

[Reference](https://www.codewars.com/kata/55d8aa568dec9fb9e200004a)



## Question

Create a function `sel_number()`, that will select numbers that fulfill the following constraints:

1) The numbers should have 2 digits at least.

2) They should have their respective digits in increasing order from left to right. Examples: 789, 479, 12678, have these feature. But 617, 89927 are not of this type. In general, if `d1, d2, d3....` are the digits of a certain number `i` Example: `( i = d1d2d3d4d5) so, d1 < d2 < d3 < d4 < d5`

3) They cannot have digits that occurs twice or more. Example: 8991 should be discarded.

4) The difference between neighbouring pairs of digits cannot exceed certain value. Example: If the difference between contiguous digits cannot excced 2, so 1345, 23568 and 234578 pass this test. Other numbers like 1456, 389, 157 don't belong to that group because in the first number(1456), the difference between second and first digit 4 - 1 > 2; in the next one(389), we have 8 - 3 > 2; and see by yourself why 157 should be discarded. In general, taking the example above of `i = d1d2d3d4d5`:

```
d2 - d1 <= d;

d3 - d2 <= d;

d4 - d3 <= d;

d5 - d4 <= d;
```

The function should accept two arguments n and d; n is the upper limit of the range to work with(all the numbers should be less or equal than n), and d is maximum difference between every pair of its contiguous digits. It's clear that 1 <= d <= 8.

Here we have some cases:

```
sel_number(0,1) = 0 # n = 0, empty range
sel_number(3, 1) = 0 # n = 3, numbers should be higher or equal than 12
sel_number(13, 1) = 1 # only 12 fulfill the requirements
sel_number(20, 2) = 2 # 12 and 13 are the numbers
sel_number(30, 2) = 4 # 12, 13, 23 and 24 are the selected ones
sel_number(44, 2) = 6 # 12, 13, 23, 24, 34 and 35 are valid ones
sel_number(50, 3) = 12 # 12, 13, 14, 23, 24, 25, 34, 35, 36, 45, 46 and 47 are valid
```

Compare the last example with this one:

```
sel_number(47, 3) = 12 # 12, 13, 14, 23, 24, 25, 34, 35, 36, 45, 46 and 47 are valid 
```

(because the instructions says the value of may be included if it fulfills the above constraints of course)

Happy coding!!

## My Solution

系列第一題 6 kyu難度 

直觀解法有二

1. 歷遍範圍內的所有整數，判斷符合條件者的個數
2. 符合條件的數值由小排到大取範圍內的個數



採用方案一(因為比較簡單)

```C#
using System.Linq;
public class HowManyNumbers 
{
        public static int SelNumber(int n, int d)=>n<12?0:Enumerable.Range(12, n - 12).Where(num => checkSelNumber(num, d)).Count();

        private static bool checkSelNumber(int num,int d)
        {
            string strNum = num.ToString();
            bool l1 = strNum == new string(strNum.OrderBy(c => c).ToArray());
            bool l2 = !(strNum.GroupBy(c => c).Any(g => g.Count() > 1));
            bool l3 = true;
            for(int i=0;i<strNum.Length-1;i++)
                if (strNum[i + 1] - strNum[i] > d)
                    l3 = false;
            return l1 && l2 && l3;
        }
}
```



## Better Solutions



### Solution 1

```C#
using System.Linq;
public class HowManyNumbers 
{
    public static int SelNumber(int n, int d)
    {
        return n<12?0:Enumerable.Range(12,n-12+1).Where(i=>i.ToString().Reverse().Select(x=>x-'0').Zip(i.ToString().Reverse().Select(x=>x-'0').Skip(1).Concat(new[]{0}),(x,y)=>x-y).Take(i.ToString().Length-1).All(x=>x>0&&x<=d)).Count();
    }
}
```

[MSDN:Enumerable.Zip](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.zip?view=netcore-3.1)