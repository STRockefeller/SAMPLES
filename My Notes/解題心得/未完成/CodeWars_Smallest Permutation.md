# CodeWars:Smallest Permutation:20210604:C#

[Reference](https://www.codewars.com/kata/5fefee21b64cc2000dbfa875)



## Question

Given a number, find the permutation with the smallest absolute value (no leading zeros).

```python
-20 => -20
-32 => -23
0 => 0
10 => 10
29394 => 23499
```

The input will always be an integer.

## My Solution

怠惰了好一陣子沒解題了，先來個簡單的練手。

```C#
        public int MinPermutation(int n)
        {
            if (n == 0) { return 0; }
            List<char> charList = new List<char>();
            charList.AddRange(n.ToString().ToCharArray());
            //'-' detection
            bool isNegative = charList.Remove('-');
            //'0' detection
            int zeros = charList.RemoveAll(c => c == '0');
            charList = charList.OrderBy(c => c).ToList();
            if (zeros != 0)
                for (int i = 0; i < zeros; i++)
                    charList.Insert(1, '0');
            return isNegative ? -Convert.ToInt32(new string(charList.ToArray())) : Convert.ToInt32(new string(charList.ToArray()));
        }
```

基本上沒有特別困難的地方，只要注意一下負值和leading zero的情況即可。

## Better Solutions



Solution1

```C#
namespace Solution 
{
    using System;
    using System.Linq;
  
    public class Kata
    {
        public int MinPermutation(int n)
        {
            var f = n < 0 ? -1 : 1;
            var s = new string($"{Math.Abs(n)}".OrderBy(c=>c).ToArray());
            var z = s.Count(c=>c=='0');
            s = s.Replace("0",string.Empty);
            if (s.Length == 0) return 0;
            s = $"{s[0]}{new string('0',z)}{s.Substring(1)}";
            return int.Parse(s) * f;
        }
    }
}
```

用變數(1/-1)表正負最後再乘回去是個不錯的做法，至少比我的`?:`寫法簡潔的多。
