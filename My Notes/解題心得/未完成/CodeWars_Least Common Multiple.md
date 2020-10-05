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



## Better Solutions

