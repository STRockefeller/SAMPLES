# CodeWars:How Many Numbers? II:20200929:C#

[Reference](https://www.codewars.com/kata/55f5efd21ad2b48895000040)



## Question

We want to find the numbers higher or equal than 1000 that the sum of every four consecutives digits cannot be higher than a certain given value. If the number is `num = d1d2d3d4d5d6`, and the maximum sum of 4 contiguous digits is `maxSum`, then:

```python
d1 + d2 + d3 + d4 <= maxSum
d2 + d3 + d4 + d5 <= maxSum
d3 + d4 + d5 + d6 <= maxSum
```

For that purpose, we need to create a function, `max_sumDig()`, that receives `nMax`, as the max value of the interval to study (the range (1000, nMax) ), and a certain value, `maxSum`, the maximum sum that every four consecutive digits should be less or equal to. The function should output the following list with the data detailed bellow:

```
[(1), (2), (3)]
```

(1) - the amount of numbers that satisfy the constraint presented above

(2) - the closest number to the mean of the results, if there are more than one, the smallest number should be chosen.

(3) - the total sum of all the found numbers

Let's see a case with all the details:

```
max_sumDig(2000, 3) -------> [11, 1110, 12555]

(1) -There are 11 found numbers: 1000, 1001, 1002, 1010, 1011, 1020, 1100, 1101, 1110, 1200 and 2000

(2) - The mean of all the found numbers is:
      (1000 + 1001 + 1002 + 1010 + 1011 + 1020 + 1100 + 1101 + 1110 + 1200 + 2000) /11 = 1141.36363,  
      so 1110 is the number that is closest to that mean value.

(3) - 12555 is the sum of all the found numbers
      1000 + 1001 + 1002 + 1010 + 1011 + 1020 + 1100 + 1101 + 1110 + 1200 + 2000 = 12555

Finally, let's see another cases
```

max_sumDig(2000, 4) -----> [21, 1120, 23665]

max_sumDig(2000, 7) -----> [85, 1200, 99986]

max_sumDig(3000, 7) -----> [141, 1600, 220756]

\```

Happy coding!!

## My Solution

系列第二題，難度5 kyu

輸入已經確定大於四位數，再歷遍所有可能顯然效率不佳

不過還是先來歷歷看

```C#
        public static long[] MaxSumDig(long nmax, int maxsm)
        {
            var result = Enumerable.Range(1000, (int)nmax - 999).Where(num => checkMaxSumDig(num, maxsm));
            float average = (float)result.Sum() / result.Count();
            float minDist = Math.Abs(result.FirstOrDefault() - average);
            int target = result.FirstOrDefault();
            foreach (int num in result)
                if (Math.Abs(num - average) < minDist)
                {
                    target = num;
                    minDist = Math.Abs(num - average);
                }
            return new long[3] { result.Count(), target, result.Sum() };
        }

        private static bool checkMaxSumDig(int num, int maxsm)
        {
            string strNum = num.ToString();
            for (int i = 0; i < strNum.Length - 3; i++)
                if (strNum[i] - '0' + strNum[i + 1] - '0' + strNum[i + 2] - '0' + strNum[i + 3] - '0' > maxsm)
                    return false;
            return true;
        }
```

因為第一段測試的數據都很小所以僥倖試試看這樣寫能不能過，第一段測試順利通過，不意外的，最後在進階測試跳例外`System.OverflowException : Arithmetic operation resulted in an overflow.` 看來強行把`long`輸入轉成`int`處理並不是個好方法，不過我想超過`Int32.Max`的輸入值依這個邏輯跑到time out的可能性很大就是了。

---

換個解法

好吧，我想不出來

把先前的解法型別的問題解決後再跑跑看

```C#
        public static long[] MaxSumDig(long nmax, int maxsm)
        {
            List<long> result = new List<long>();
            for (long i = 1000; i < nmax; i++)
                if (checkMaxSumDig(i, maxsm))
                    result.Add(i);

            float average = (float)result.Sum() / result.Count();
            float minDist = Math.Abs(result.FirstOrDefault() - average);
            long target = result.FirstOrDefault();
            foreach (int num in result)
                if (Math.Abs(num - average) < minDist)
                {
                    target = num;
                    minDist = Math.Abs(num - average);
                }
            return new long[3] { result.Count(), target, result.Sum() };
        }

        private static bool checkMaxSumDig(long num, int maxsm)
        {
            string strNum = num.ToString();
            for (int i = 0; i < strNum.Length - 3; i++)
                if (strNum[i] - '0' + strNum[i + 1] - '0' + strNum[i + 2] - '0' + strNum[i + 3] - '0' > maxsm)
                    return false;
            return true;
        }
```

然後就過關了= =

雖然沒有time out 但也執行了512ms，不是很滿意。



## Better Solutions

### Solution 1

```C#
using System;
using System.Linq;
using System.Collections.Generic;

class MaxSumDigits {

    public static long[] MaxSumDig(long n_max, int max_sm) 
    {
        List<long> res = new List<long>();
        
        for (long i = 1000; i < n_max + 1; i++)
        {
            bool is_good = true;
            var n = i.ToString().Select( e => e - 48);
            for (int j = 0; j < n.Count() - 3; j++)
                if (n.Skip(j).Take(4).Sum() > max_sm)
                    is_good = false;
            if (is_good == true)
                res.Add(i);
        }
        
        long num = res.Count, tot = res.Sum();
        return new long[] { num, res.OrderBy(e => Math.Abs(e - tot/(float)num)).ThenBy(e => e).First(), tot };
    }
    
}
```





## Weird Solutions



### Solution 1

```C#
using System;
using System.Linq;
using System.Collections.Generic;

class MaxSumDigits {

    public static long[] MaxSumDig(long nmax, int maxsm) 
    {
        var sumList =  new List<long>();
        
        for(var i = 1000; i <= nmax; i++)
        {
            if((i.ToString().Sum(x => x - '0')) <= maxsm)
            {
              sumList.Add(i);
            }
        }
        
        var avg = sumList.Average();
        
        return new long[] {sumList.Count(), sumList.OrderBy(i => Math.Abs(avg - i)).First(), sumList.Sum()};
    }
}
```

花了不少時間寫出題目之後看到這種解答放在最上方，感覺真的不太好，所以特別加了一個weird solution給他。

去留言看看有沒有人能說明一下`if((i.ToString().Sum(x => x - '0')) <= maxsm){sumList.Add(i);}`這明顯不符合題意的寫法是怎麼過的。
