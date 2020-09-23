# CodeWars:Primes in numbers:20200923:C#

[Reference](https://www.codewars.com/kata/54d512e62a5e54c96200019e)



## Question

Given a positive number n > 1 find the prime factor decomposition of n. The result will be a string with the following form :

```
 "(p1**n1)(p2**n2)...(pk**nk)"
```

where `a ** b` means `a` to the power of `b`

with the p(i) in increasing order and n(i) empty if n(i) is 1.

```
Example: n = 86240 should return "(2**5)(5)(7**2)(11)"
```

## My Solution

### Solution 1

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public class PrimeDecomp {
        public static String factors(int lst)
        {
            int[] factorArr = Enumerable.Repeat(0, 1000000).ToArray();
            for(int i=2;i<1000000&&lst!=1;i++)
                while(lst%i==0)
                {
                    factorArr[i]++;
                    lst /= i;
                }
            string result = "";
            for(int i=2;i<1000000;i++)
                if (factorArr[i] > 0)
                    result += $"({i}**{factorArr[i]})";
            return result.Replace("**1", "");
        }
}
```

很快地用了最直覺的作法完成了，1000000的地方原本是寫100但是進階測試過不了，所幸改為一百萬試試，結果成功過關沒有timeout，不過這個做法顯然並不好看，所以先不submit再想想看其他作法。(進階測試完成時間77.1ms)

### Solution 2

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public class PrimeDecomp {
        public static String factors(int lst)
        {
            int root = (int)Math.Ceiling(Math.Sqrt(lst));
            int[] factorArr = Enumerable.Repeat(0, root).ToArray();
            for (int i = 2; i < root && lst != 1; i++)
                while (lst % i == 0)
                {
                    factorArr[i]++;
                    lst /= i;
                }
            string result = "";
            for (int i = 2; i < root; i++)
                if (factorArr[i] > 0)
                    result += $"({i}**{factorArr[i]})";
            if (lst != 1)
                result += $"({lst})";
            return result.Replace("**1", "");
        }
}
```

修改了一下，使用輸入數值的平方根作為迴圈次數上限，考慮到含大質數的情況下(例如17=>(17))，所以將迴圈出來後不等於1的數值加入字串底部。(進階測試執行時間23.4ms，比起上次節省了2/3的時間)

## Better Solutions

### Solution 1

```C#
using System;
using System.Collections.Generic;

public class PrimeDecomp {
  public static String factors(int lst) {
    var primes = new List<string>();
    for (var number = 2; number <= lst; number++)
    {
      var count=0;
      while (lst % number == 0) { count++; lst /= number; }                
      if(count==0) continue;
      primes.Add( String.Format(count > 1 ? "({0}**{1})" : "({0})",number,count));
    }
    return String.Join("", primes);
  }
}
```

排最上方的解答，下面很多人也是類似這樣的寫法，老實說我覺得我寫的比較好，迴圈次數上限等於輸入數值執行的次數一定多很多(甚至比我一開始寫的1000000還多)，而且他沒有寫分解完畢跳出迴圈的機制，所以一定會把這個次數跑完。當然他記錄結果的方法還是值得學習，我的寫法是類似bucket sort的作法需要多一個迴圈去讀資料。

