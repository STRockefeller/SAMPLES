# CodeWars:Least Common Multiple:20201005:C#

[Reference](https://www.codewars.com/kata/5259acb16021e9d8a60010af/train/csharp)



## Question

Write a function that calculates the *least common multiple* of its arguments; each argument is assumed to be a non-negative integer. In the case that there are no arguments (or the provided array in compiled languages is empty), return `1`.

## My Solution

求最小公倍數，把List中的最小值變大，重複這個動作直到整個List都是同樣的值為止

```C#
        public static int Lcm(List<int> nums)
        {
            List<int> oriNums = new List<int> (nums);
            int minIdex = nums.IndexOf(nums.Min());
            int maxIdex = nums.IndexOf(nums.Max());
            while (nums[minIdex]!=nums[maxIdex])
            {
                while (nums[minIdex] < nums[maxIdex])
                    nums[minIdex] += oriNums[minIdex];
                minIdex = nums.IndexOf(nums.Min());
                maxIdex = nums.IndexOf(nums.Max());
            }
            return nums[0];
        }
```

um...進階測試time out



---

兩數的情況下就能使用公式解了，嘗試把List值兩兩抓出來解

```C#
        public static int Lcm(List<int> nums)
        {
            for(int i=0;i<nums.Count-1;i++)
            {
                nums[i + 1] = lcm2(nums[i], nums[i + 1]);
            }
            return nums[nums.Count-1];
        }
        static int gcd2(int a, int b) => a % b == 0 ? b : gcd2(b, a % b);

        static int lcm2(int a, int b) => a * b / gcd2(a, b);
```

進階測試跳index out of range 以及 divide by zero

針對錯誤輸入修改了一下後

![](https://i.imgur.com/AmWJ0jO.png)

???題目不是說`each argument is assumed to be a non-negative integer`怎會給出負值的答案

---

突然又想到一個解法，找List裡面最大的數，不斷的往上乘直到找到最小公倍數為止，實作如下

```C#
        public static int Lcm(List<int> nums)
        {
            if (nums.Any(num => num == 0)) { return 0; }
            if (nums.Count < 1) { return 1; }
            int maxNum = nums.Max();
            for (int i = 1; ; i++)
                if (nums.All(n => (maxNum * i) % n == 0))
                    return maxNum * i;
        }
```

然後就過關了，到最後我還是不知道上一個解法錯在哪裡

## Better Solutions



### Solution 1

```C#
using System.Linq;
using System.Collections.Generic;

public static class Kata
{
    public static int Lcm(List<int> nums)
    {
        if (nums.Count == 0) return 1;
        if (nums.Contains(0)) return 0;

        var sum = nums.Max();
        while (nums.Count(i => sum % i == 0) != nums.Count)
            sum += nums.Max();

        return sum;
    }
}
```

和我最後一個解法差不多



### Solution 2

```C#
using System.Linq;
using System.Collections.Generic;
public static class Kata
{
  private static int Gcf(int a, int b) => b == 0 ? a : Gcf(b, a%b);
  public static int Lcm(IEnumerable<int> nums) => nums.Any() ? nums.Aggregate((g, x) => g*x/Gcf(g,x)) : 1;
}
```

[MSDN:Aggregate](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.aggregate?view=netcore-3.1)