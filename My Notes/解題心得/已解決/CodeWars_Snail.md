# CodeWars:Snail:20200924:C#

[Reference](https://www.codewars.com/kata/521c2db8ddc89b9b7a0000c1)



## Question

## Snail Sort

Given an `n x n` array, return the array elements arranged from outermost elements to the middle element, traveling clockwise.

```
array = [[1,2,3],
         [4,5,6],
         [7,8,9]]
snail(array) #=> [1,2,3,6,9,8,7,4,5]
```

For better understanding, please follow the numbers of the next array consecutively:

```
array = [[1,2,3],
         [8,9,4],
         [7,6,5]]
snail(array) #=> [1,2,3,4,5,6,7,8,9]
```

This image will illustrate things more clearly:

![img](http://www.haan.lu/files/2513/8347/2456/snail.png)

NOTE: The idea is not sort the elements from the lowest value to the highest; the idea is to traverse the 2-d array in a clockwise snailshell pattern.

NOTE 2: The 0x0 (empty matrix) is represented as en empty array inside an array `[[]]`.

## My Solution

4 kyu C#人氣第一的題目，題目錯字en empty(X) => an empty(O)

題目沒有說明輸入是否都是正方形或許有矩形，先以矩形也能解答的方向去想好了

假如是w*h的矩形，分解動作如下

第一步從起點往右走:`[0][0]`=>`[0][1]`=>...=>`[0][w]`

第二步是從右上往下走:`[0][w]`=>`[1][w]`=>...=>`[h][w]`

接著往左:`[h][w]`=>`[h][w-1]`=>...=>`[h][0]`

接著往上(第一列已經被走過了):`[h][0]`=>`[h-1][0]`=>...=>`[1][0]`

依此類推。

因為無法確定輸入矩形的大小，所以勢必得用迴圈進行重複運算，接著就是界定每次回圈的內容了

看完題目後直覺想到的方案有二

1. 每次執行一圈
2. 分成右下左上右下左上這樣執行

採用方案一進行撰寫

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public class SnailSolution
{
        public static int[] Snail(int[][] array)
        {
            List<int> result = new List<int>();
            double maxCount = Math.Ceiling((double)Math.Min(array.Length, array.First().Length) / 2);
            for (int i = 0; i < maxCount; i++)
                result.AddRange(snailGo(i, array));
            if (array.Length == array.First().Length && array.Length % 2 == 1)
                result.Add(array[array.Length / 2][array.Length / 2]);
            return result.ToArray();
        }
        private static List<int> snailGo(int count, int[][] array)
        {
            List<int> result = new List<int>();
            int height = array.Length;
            int width = array.First().Length;
            //right
            for (int j = count; j < width - 1 - count; j++)
                result.Add(array[count][j]);
            //down
            for (int i = count; i < height - 1 - count; i++)
                result.Add(array[i][width - 1 - count]);
            //left
            for (int j = width - 1 - count; j > count; j--)
                result.Add(array[height - 1 - count][j]);
            //up
            for (int i = height - 1 - count; i > count; i--)
                result.Add(array[i][count]);
            return result;

        }
}
```

為了方便閱讀就沒有特別簡化寫法，原先沒有考慮到最中間的數字會被忽略，測試失敗一次，修正後就通過了，算是非常順利。



雖然懶得再寫一次，不過還是記錄一下方案二的構思(其實我比較喜歡這種類似接龍的作法，但是這題感覺寫起來會變得很長所以作罷)

```C#
    public static int[] Snail(int[][] array)
    {
        List<int> result = new List<int>();
        snailRignt(array,0,result);
        return result.ToArray();
    }
	private static List<int> snailRignt(int[][] array,int count,List<int> result)
	{
		bool end;
		//...
		return end?result:snailDown(array,count++, result);
	}
	private static List<int> snailDown(int[][] array,int count,List<int> result)
	{
		bool end;
		//...
		return end?result:snailLeft(array,count++, result);
	}
	private static List<int> snailLeft(int[][] array,int count,List<int> result)
	{
		bool end;
		//...
		return end?result:snailUp(array,count++, result);
	}
	private static List<int> snailUp(int[][] array,int count,List<int> result)
	{
		bool end;
		//...
		return end?result:snailRignt(array,count++, result);
	}
```

## Better Solutions



### Solution 1

```C#
public class SnailSolution
{
    public static int[] Snail(int[][] array)
    {
        int l = array[0].Length;
        int[] sorted = new int[l * l];
        Snail(array, -1, 0, 1, 0, l, 0, sorted);
        return sorted;
    }

    public static void Snail(int[][] array, int x, int y, int dx, int dy, int l, int i, int[] sorted)
    {
        if (l == 0)
            return;
        for (int j = 0; j < l; j++)
        {
            x += dx;
            y += dy;
            sorted[i++] = array[y][x];
        }
        Snail(array, x, y, -dy, dx, dy == 0 ? l - 1 : l, i, sorted);
    }
}
```

熱門解法，沒有using任何內容真的厲害，不過我有點看不太懂就是了。