# CodeWars:Next smaller number with the same digits:20200925:C#

[Reference](https://www.codewars.com/kata/5659c6d896bc135c4c00021e)



## Question

Write a function that takes a positive integer and returns the next smaller positive integer containing the same digits.

For example:

```csharp
nextSmaller(21) == 12
nextSmaller(531) == 513
nextSmaller(2071) == 2017
```

Return -1 (for `Haskell`: return `Nothing`, for `Rust`: return `None`), when there is no smaller number that contains the same digits. Also return -1 when the next smaller number with the same digits would require the leading digit to be zero.

```csharp
nextSmaller(9) == -1
nextSmaller(111) == -1
nextSmaller(135) == -1
nextSmaller(1027) == -1 // 0721 is out since we don't write numbers with leading zeros
```

- some tests will include very large numbers.
- test data only employs positive integers.

*The function you write for this challenge is the inverse of this kata: "[Next bigger number with the same digits](http://www.codewars.com/kata/next-bigger-number-with-the-same-digits)."*

## My Solution

昨天才解過類似問題，看這次能不能想出更好的解法

先回想並整理一下前提:

1. 數字最大值是其降冪排列；最小值是其升冪排列，以這題的情況，如果輸入是升冪排列則無解
2. 為了讓變化影響盡可能小，我們一樣從最右方(個位數)找起，找**非升冪排列**的數
3. 這次不用`List<long>`來處理了，昨天那題證實了string比較方便(因為char的OrderBy和數字的結果是一樣的)
4. 這次還要考慮排除 leading zero



```C#
        public static long NextSmaller(long n)
        {
            if (n < 10)
                return -1;
            string s = n.ToString();
            for (int i = s.Length - 2; i >= 0; i--)
            {
                if (s.Substring(i) != string.Concat(s.Substring(i).OrderBy(x => x)))
                {
                    string tail = string.Concat(s.Substring(i).OrderByDescending(x => x));
                    char center = tail.First(x => x < s[i]);
                    return !(i == 0 && center == '0') ? long.Parse(s.Substring(0, i) + center + string.Concat(tail.Where((x, y) => y != tail.IndexOf(center)))) : -1;
                }
            }
            return -1;
        }
```



## Better Solutions



從缺