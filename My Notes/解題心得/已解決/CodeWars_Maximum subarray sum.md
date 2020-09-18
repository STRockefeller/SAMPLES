# CodeWars:Maximum subarray sum:20200917:C#

[Reference](https://www.codewars.com/kata/54521e9ec8e60bc4de000d6c)



## Question

The maximum sum subarray problem consists in finding the maximum sum of a contiguous subsequence in an array or list of integers:

```haskell
maxSequence [-2, 1, -3, 4, -1, 2, 1, -5, 4]
-- should be 6: [4, -1, 2, 1]
```

Easy case is when the list is made up of only positive numbers and the maximum sum is the sum of the whole array. If the list is made up of only negative numbers, return 0 instead.

Empty list is considered to have zero greatest sum. Note that the empty list or array is also a valid sublist/subarray.

## My Solution

### Round1

最初想到的是暴力解法，歷遍所有可能性，不過這樣寫就太遜了

花了點時間思考，想到一個方式

因為他要求得輸出不是陣列所以首先整理數列先把數列中相鄰的正數或負數合併，例如[1,2,-3,2,-4,-1]=>[3,-3,2,-5]

這樣我就會得到一正一負的整齊數列

接著假如我的數列是正數開頭，我將其和其後的負數相加，若為正，則我認定該區塊的最大數為該值以及其後二者的和

例如[3,-1,5,-8,2,-3,9]，因為3+(-1)>0，所以我設定區域最大值為3-1+5=7，然後我會將數列變為[7,-8,2,-3,9]再重複動作

如果前兩數相加為負則將兩數都從數列中刪除。

實現起來如下

```C#
        public static int MaxSequence(int[] arr)
        {
            List<int> sumList = new List<int>();
            int max = 0;
            while (arr.Length >= 2)
            {
                var subA = arr.TakeWhile(i => i >= 0);
                arr = arr.SkipWhile(i => i >= 0).ToArray();
                var subN = arr.TakeWhile(i => i < 0);
                arr = arr.SkipWhile(i => i < 0).ToArray();
                if (subA.Sum() != 0)
                    sumList.Add(subA.Sum());
                sumList.Add(subN.Sum());
            }
            if (arr.Length == 1)
                sumList.Add(arr[0]);
            while (sumList.Count >1)
            {
                if (sumList[0] < 0)
                    sumList = sumList.Skip(1).ToList();
                int subMax = sumList[0];
                if (sumList.Count >= 3)
                    if (sumList[0] + sumList[1] > 0)
                    {
                        subMax = sumList[0] + sumList[1] + sumList[2];
                        sumList = sumList.Skip(3).ToList();
                        sumList=new List<int>( sumList.Prepend(subMax));
                    }
                    else
                        sumList = sumList.Skip(2).ToList();
                else
                    sumList = sumList.Skip(2).ToList();
                max = subMax > max ? subMax : max;
            }
            return max;
        }
```

第一次使用prepend方法就失敗，查MSDN才知道prepend不會改變原本的List內容

[MSDN:prepend](https://docs.microsoft.com/zh-tw/dotnet/api/system.linq.enumerable.prepend?view=netcore-3.1)

結果想了好久的方法竟然過不了Random test

### Round2

重新檢視了一次發先第一個解法還是有破綻，比如[197,-196,10,...]會被合併成11但區域內的最大數應該是197才對，修正完如下

```C#
public static int MaxSequence(int[] arr)
        {
            List<int> sumList = new List<int>();
            int max = 0;
            while (arr.Length >= 2)
            {
                var subA = arr.TakeWhile(i => i >= 0);
                arr = arr.SkipWhile(i => i >= 0).ToArray();
                var subN = arr.TakeWhile(i => i < 0);
                arr = arr.SkipWhile(i => i < 0).ToArray();
                if (subA.Sum() != 0)
                    sumList.Add(subA.Sum());
                sumList.Add(subN.Sum());
            }
            if (arr.Length == 1)
                sumList.Add(arr[0]);
            while (sumList.Count > 1)
            {
                if (sumList[0] < 0)
                    sumList = sumList.Skip(1).ToList();
                int subMax = sumList[0];
                if (sumList.Count >= 3)
                    if (sumList[0] + sumList[1] > 0)
                    {
                        if (sumList[0] + sumList[1] + sumList[2] > subMax)
                        {
                            subMax = sumList[0] + sumList[1] + sumList[2];
                            sumList = sumList.Skip(3).ToList();
                            sumList = new List<int>(sumList.Prepend(subMax));
                        }
                        else
                            sumList = sumList.Skip(2).ToList();
                    }
                    else
                        sumList = sumList.Skip(2).ToList();
                else
                {
                    subMax = sumList.Max();
                    sumList = sumList.Skip(2).ToList();
                }   
                max = subMax > max ? subMax : max;
            }
            return max;
        }
```

還是過不了Random test

### Round3

又想到破綻了，[30,-18,17,-2,30]最大值應該是全加總57，但是我的程式判斷[30,-18,17]加總<30所以會把數列再整理成[17,-2,30]最後回傳45，另外我將max的預設值宣告為0並沒有考慮到數列全部小於0的情形(合併的動作同理)。看來解法從邏輯上就不正確了，重新來過吧。

數列從頭開始往後加總，若數列加總為負則捨棄，過程中記錄最大值。

```C#
        public static int MaxSequence(int[] arr)
        {
            if (arr.Length < 1)
                return 0;
            int max = arr.Max();
            int sum = 0;
            foreach(int i in arr)
            {
                sum = sum < 0 ? i : sum + i;
                max = sum > max ? sum : max;
            }
            return max;
        }
```

過關了，意外的不複雜



## Better Solutions

codewars最上面的那個解答"clever"57的窮舉，看到直接嘴角失守，很有趣，不過太長了所以揭過不提

### Solution 1

```C#
public static class Kata
{
    public static int MaxSequence(int[] arr)
    {
        int max = 0, res = 0, sum = 0;
        foreach(var item in arr)
        {
            sum += item;
            max = sum > max ? max : sum;
            res = res > sum - max ? res : sum - max;
        }
        return res;
    }
}
```

相似的作法，下方留言有人解釋

#### Max SubSequence Sum

| item    | 0    | 1    | 2    | 3    | 4    | 5    | 6    | 7    | 8    | Resume                    |
| ------- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ---- | ------------------------- |
|         | -2   | -2   | -2   | -2   | -2   | -2   | -2   | -2   | -2   |                           |
|         |      | 1    | 1    | 1    | 1    | 1    | 1    | 1    | 1    |                           |
|         |      |      | -3   | -3   | -3   | -3   | -3   | -3   | -3   | **要去掉的最大左侧序列**  |
|         |      |      |      |      |      |      |      |      |      |                           |
|         |      |      |      | 4    | 4    | 4    | 4    | 4    | 4    | **最佳起始位置 itme = 3** |
|         |      |      |      |      | -1   | -1   | -1   | -1   | -1   |                           |
|         |      |      |      |      |      | 2    | 2    | 2    | 2    |                           |
|         |      |      |      |      |      |      | 1    | 1    | 1    | **最佳结束位置 itme = 6** |
|         |      |      |      |      |      |      |      |      |      |                           |
|         |      |      |      |      |      |      |      | -5   | -5   |                           |
|         |      |      |      |      |      |      |      |      | 4    |                           |
|         |      |      |      |      |      |      |      |      |      |                           |
| Sum     | -2   | -1   | -4   | 0    | -1   | 1    | 2    | -3   | 1    |                           |
| Max     | -2   | -2   | -4   | -4   | -4   | -4   | -4   | -4   | -4   |                           |
| Sum-Max | 0    | 1    | 0    | 4    | 3    | 5    | 6    | 1    | 5    |                           |
| Res     | 0    | 1    | 1    | 4    | 4    | 5    | 6    | 6    | 6    |                           |

> **Sum** 从左边第一个向右的所有序列组合
> **Max** 存储要去掉的左侧最长序列组合
> **Sum-Max** 去掉 Max 后, 剩余部分的所有序列组合
> **Res** Sum-Max 序列和的最大值



### Solution 2

```C#
using System.Linq;
public static class Kata
{
  public static int MaxSequence(int[] arr) 
  { 
    var s = arr.ToList();
    return arr.Length<1?0:s.SelectMany((x,i)=>Enumerable.Range(0,arr.Length+1-i).Select(y=>s.GetRange(i,y))).Max(x=>x.Sum());
  }
}
```

兩行解，linq沒有做不到只有我想不到