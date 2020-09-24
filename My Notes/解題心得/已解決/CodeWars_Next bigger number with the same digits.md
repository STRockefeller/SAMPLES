# CodeWars:Next bigger number with the same digits:20200924:C#

[Reference](https://www.codewars.com/kata/55983863da40caa2c900004e)



## Question

Create a function that takes a positive integer and returns the next bigger number that can be formed by rearranging its digits. For example:

```
12 ==> 21
513 ==> 531
2017 ==> 2071
nextBigger(num: 12)   // returns 21
nextBigger(num: 513)  // returns 531
nextBigger(num: 2017) // returns 2071
```

If the digits can't be rearranged to form a bigger number, return `-1` (or `nil` in Swift):

```
9 ==> -1
111 ==> -1
531 ==> -1
nextBigger(num: 9)   // returns nil
nextBigger(num: 111) // returns nil
nextBigger(num: 531) // returns nil
```

## My Solution

從今天的解答開始(包含前一個snail)，把開始動手前的思路記下來(如果題目不是一看就解開的話)，養成先想清楚再動手的習慣

不考慮暴力解法

先看失敗例，除了不能交換或交換也一樣的9、111以外再來就是降冪排列的531因為本身就是最大數所以無解

所以推測以上三種情形以外應該都有解

升冪排列的數一定是所有排列中的最小數，任意更換排列都會比原來大

以下思考幾個例子

1234=>1243 換個位數和十位數

2134=>2143 換個位數和十位數

2143=>2314 個位數移到百位數

2486=>2648 個位數移到百位數

稍微找到規律了，為了讓變化最小，所以變化的位置從右邊往左邊找，如果從最右邊數n個數不是降冪排列的話，那一定能在這n個數重新排列得到答案

首要改變的數字之後盡可能升冪排列已得到最小數，例如2143=>因為43沒有更大的排列所以考慮到百位數=>百位數比1大的在143裡面取3剩下的14升冪排列得到2314。

```C#
        public static long NextBiggerNumber(long n)
        {
            List<long> inputList = new List<long>();
            while (n > 0)
            {
                inputList.Add(n % 10);
                n /= 10;
            }
            int changeFromIndex = 0;
            for (int i = 1; i < inputList.Count; i++)
                if (inputList[i] < inputList[i - 1])
                {
                    changeFromIndex = i;
                    break;
                }
            if (changeFromIndex == 0)
                return -1;
            List<long> changeList = inputList.Take(changeFromIndex + 1).ToList();
            int changeNumIndex = changeList.IndexOf(changeList.Where(num => num > changeList[changeFromIndex]).Min());
            changeList[changeFromIndex] = changeList[changeNumIndex] + changeList[changeFromIndex] - (changeList[changeNumIndex] = changeList[changeFromIndex]);
            List<long>  resultChangeList = changeList.Take(changeFromIndex).OrderByDescending(num => num).ToList();
            resultChangeList.Add(changeList[changeFromIndex]);
            resultChangeList.AddRange(inputList.Skip(changeFromIndex + 1));
            long result = 0;
            long scale = 1;
            foreach(long num in resultChangeList)
            {
                result += num * scale;
                scale *= 10;
            }
            return result;
        }
```

確定好解題方向後，就照著一步步實現，雖然篇幅比想像中大了不少但是測試一次就通過算是非常順利

以下分解動作

因為`long`資料不好做分析和處理，所以將其變為`List<long>`來處理，也考慮過`string`，不過做比較時還是要變回數字應該不會比較方便。

```C#
            List<long> inputList = new List<long>();
            while (n > 0)
            {
                inputList.Add(n % 10);
                n /= 10;
            }
```

從個位數開始往後找非降冪排列的位置(順便排除無法計算的輸入)

```C#
            int changeFromIndex = 0;
            for (int i = 1; i < inputList.Count; i++)
                if (inputList[i] < inputList[i - 1])
                {
                    changeFromIndex = i;
                    break;
                }
            if (changeFromIndex == 0)
                return -1;
```

取出要處理的部分

```C#
            List<long> changeList = inputList.Take(changeFromIndex + 1).ToList();
```

將重點位置的數字與正確值交換

```C#
            int changeNumIndex = changeList.IndexOf(changeList.Where(num => num > changeList[changeFromIndex]).Min());
            changeList[changeFromIndex] = changeList[changeNumIndex] + changeList[changeFromIndex] - (changeList[changeNumIndex] = changeList[changeFromIndex]);
```

剩下的部分升冪排列，完成後把所有的片段拼湊起來

```C#
            List<long>  resultChangeList = changeList.Take(changeFromIndex).OrderByDescending(num => num).ToList();
            resultChangeList.Add(changeList[changeFromIndex]);
            resultChangeList.AddRange(inputList.Skip(changeFromIndex + 1));
```

最後再把`List<long>`變回`long`

```C#
            long result = 0;
            long scale = 1;
            foreach(long num in resultChangeList)
            {
                result += num * scale;
                scale *= 10;
            }
            return result;
```



## Better Solutions



### Solution 1

```C#
using System;
using System.Linq;
using System.Collections.Generic;

public class Kata
{
     public static long NextBiggerNumber(long n)
    {        
       String str = GetNumbers(n);
        for (long i = n+1; i <= long.Parse(str); i++)
        {
            if(GetNumbers(n)==GetNumbers(i))
            {
                return i;
            }
        }
        return -1;        
    }
    public static string GetNumbers(long number)
    {
      return string.Join("", number.ToString().ToCharArray().OrderByDescending(x => x));
    }
}
```

盯了五分鐘才看懂，這算是暴力解法吧

方法`GetNumbers`會把數字排列後返傳字串

所以迴圈的範圍就是n到n的降冪排列(所有排列的最大值)

迴圈中符合排列後的值相同者回傳，又因i從n一直往上加所以回傳值一定比n大

簡潔方面是這個方法完勝，但是效率方面還是我寫的方式比較好，傳入的n越大這個方法的迴圈可能執行次數會明顯變大(接近n)，而我的迴圈最多執行log(10)n次。這個做法給個9999999999999999999就差不多要timeout了吧。



### Solution 2

```C#
using System.Linq;
public class Kata
{
    public static long NextBiggerNumber(long n)
    {
      if (n<10) return -1;
      string s=n+"";
      for (int i=s.Length-2;i>=0;i--){
        if (s.Substring(i)!=string.Concat(s.Substring(i).OrderByDescending(x=>x))){
          var t=string.Concat(s.Substring(i).OrderBy(x=>x));
          var c=t.First(x=>x>s[i]);
          return long.Parse(s.Substring(0,i)+c+string.Concat(t.Where((x,y)=>y!=t.IndexOf(c))));
        }
      }
      return -1;
    }
}
```

我比較喜歡的做法，不過下方有留言表示這個做法很糟

> ###### [Jesus_Like](https://www.codewars.com/users/Jesus_Like)(5 kyu)[2 years ago](https://www.codewars.com/kata/55983863da40caa2c900004e/solutions/csharp/all/best_practice#5bc463ef0ca593a29b00028e)
>
> Are you serious? Sorting array with each iteration is just terrible. I hope I'll never see that again.

好吧，基礎太差看不懂為啥不好。