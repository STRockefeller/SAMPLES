# Codewars:Is my friend cheating?:20200923:C#

[Reference](https://www.codewars.com/kata/5547cc7dcad755e480000004)



## Question

- A friend of mine takes a sequence of numbers from 1 to n (where n > 0).
- Within that sequence, he chooses two numbers, a and b.
- He says that the product of a and b should be equal to the sum of all numbers in the sequence, excluding a and b.
- Given a number n, could you tell me the numbers he excluded from the sequence?

The function takes the parameter: `n` (n is always strictly greater than 0) and returns an array or a string (depending on the language) of the form:

```
[(a, b), ...] or [[a, b], ...] or {{a, b}, ...} or or [{a, b}, ...]
```

with **all** `(a, b)` which are the possible removed numbers in the sequence `1 to n`.

`[(a, b), ...] or [[a, b], ...] or {{a, b}, ...} or ...`will be sorted in increasing order of the "a".

It happens that there are several possible (a, b). The function returns an empty array (or an empty string) if no possible numbers are found which will prove that my friend has not told the truth! (Go: in this case return `nil`).

(See examples of returns for each language in "RUN SAMPLE TESTS")

### Examples:

```
removNb(26) should return [(15, 21), (21, 15)]
```

or

```
removNb(26) should return { {15, 21}, {21, 15} }
```

or

```
removeNb(26) should return [[15, 21], [21, 15]]
```

or

```
removNb(26) should return [ {15, 21}, {21, 15} ]
```

or

```
removNb(26) should return "15 21, 21 15"
```

or

```
in C:
removNb(26) should return  **an array of pairs {{15, 21}{21, 15}}**
tested by way of strings.
```

## My Solution

### Solution 1

```C#
        public static List<long[]> removNb(long n)
        {
            List<long[]> result = new List<long[]>();
            long iniSum = (n + 1) * n / 2;
            for (int i = 1; i <= n; i++)
                for (int j = i + 1; j <= n; j++)
                    if ((iniSum - i - j) % i == 0 && (iniSum - i - j) / i == j)
                    {
                        result.Add(new long[2] { i, j });
                        result.Add(new long[2] { j, i });
                    }
            return result;
        }
```

看題目就覺得應該是用數學去解，但是花了點時間還是沒想出解法，所以先用暴力解法試試=>進階測試會timeout



### Solution 2

```C#
        public static List<long[]> removNb(long n)
        {
            List<long[]> result = new List<long[]>();
            long iniSum = (n + 1) * n / 2;
            for (int i = 1; i <= n; i++)
                if ((iniSum - i) % (1 + i) == 0 && (iniSum - i) / (1 + i) <= n)
                    result.Add(new long[2] { i, (iniSum - i) / (1 + i) });
            return result;
        }
```

靜下心來拿起紙筆發現意外的沒有想像中的複雜。

total=1到n的總和

a=符合條件的第一個數

b=符合條件的第二個數

滿足: total-a-b=ab

total為已知數

在一層迴圈的情況下假設a也是已知數

則

total-a=b+ab

b=(total-a)/(1+a)

最後在考慮b必須是整數且介於1到n之間，就得到解答了。



## Better Solutions

大家的解答基本上都大同小異，這題大概就指難在解出公式吧

```C#
using System.Collections.Generic;
using System.Linq;

public class RemovedNumbers {
  public static List<long[]> removNb(long n) {
    return Enumerable.Range(1, (int)n + 1).Select(x => new { X =x, Y = ((n + 1) * n / 2 - x )/(x + 1d)}).Where(x => x.Y % 1 == 0 && x.Y > 0 && x.Y <= n).Select(x=> new long[]{x.X, (long)x.Y}).ToList();
  
  }
}
```

