# CodeWars:Josephus Permutation:20200928:C#

[Reference](https://www.codewars.com/kata/5550d638a99ddb113e0000a2)



## Question

This problem takes its name by arguably the most important event in the life of the ancient historian Josephus: according to his tale, he and his 40 soldiers were trapped in a cave by the Romans during a siege.

Refusing to surrender to the enemy, they instead opted for mass suicide, with a twist: **they formed a circle and proceeded to kill one man every three, until one last man was left (and that it was supposed to kill himself to end the act)**.

Well, Josephus and another man were the last two and, as we now know every detail of the story, you may have correctly guessed that they didn't exactly follow through the original idea.

You are now to create a function that returns a Josephus permutation, taking as parameters the initial *array/list of items* to be permuted as if they were in a circle and counted out every *k* places until none remained.

**Tips and notes:** it helps to start counting from 1 up to n, instead of the usual range 0..n-1; k will always be >=1.

For example, with n=7 and k=3 `josephus(7,3)` should act this way.

```
[1,2,3,4,5,6,7] - initial sequence
[1,2,4,5,6,7] => 3 is counted out and goes into the result [3]
[1,2,4,5,7] => 6 is counted out and goes into the result [3,6]
[1,4,5,7] => 2 is counted out and goes into the result [3,6,2]
[1,4,5] => 7 is counted out and goes into the result [3,6,2,7]
[1,4] => 5 is counted out and goes into the result [3,6,2,7,5]
[4] => 1 is counted out and goes into the result [3,6,2,7,5,1]
[] => 4 is counted out and goes into the result [3,6,2,7,5,1,4]
```

So our final result is:

```
josephus([1,2,3,4,5,6,7],3)==[3,6,2,7,5,1,4]
```

For more info, browse the [Josephus Permutation](http://en.wikipedia.org/wiki/Josephus_problem) page on wikipedia; related kata: [Josephus Survivor](http://www.codewars.com/kata/josephus-survivor).

## My Solution

題目很有趣，雖然故事的主角給人感覺很差

最直觀的解法就是數n個數然後移除原陣列的值，增加回傳陣列的值，重複執行

移除的動作如果用remove的話，考慮到陣列中可能有重複的值，所以可能會出錯

在msdn查到直接根據index移除的方法 [MSDN:RemoveAt](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.generic.list-1.removeat?view=netcore-3.1)

```C#
using System;
using System.Collections.Generic;
public class Josephus
{
        public static List<object> JosephusPermutation(List<object> items, int k)
        {
            if (items.Count == 0)
                return new List<object>();
            List<object> result = new List<object>();
            subJosephusPermutation(items, ref result, k, 0);
            return result;
        }
        private static void subJosephusPermutation(List<object> items,ref List<object> result, int k, int index)
        {
            int oriK = k;
            while (k > items.Count - index)
                k -= items.Count;
            result.Add(items[index + k-1]);
            items.RemoveAt(index + k-1);
            index = index + k > items.Count ?0: index + k - 1;
            if (items.Count > 1)
                subJosephusPermutation(items, ref result, oriK, index);
            else if (items.Count == 1)
                result.Add(items[0]);
        }
}
```

測試時失敗兩次，一次是因為k在`subJosephusPermutation`被我改得面目全非然後又傳到下一次遞迴作為參數，故新增一個變數紀錄原來的k值；另一次是傳入List為空時會變成無窮遞迴導致time out，新增一段若輸入為空則回傳空值。

## Better Solutions



### Solution 1

```C#
using System;
using System.Collections.Generic;
public class Josephus
{
   public static List<object> JosephusPermutation(List<object> items, int k)
   {
       List<object> res = new List<object>();
       int pos = 0;
       while (items.Count > 0)
       {
           pos = (pos + k - 1) % items.Count;
           res.Add(items[pos]);
           items.RemoveAt(pos);
       }
       return res;
   }
}
```

同樣的思路，更簡潔的寫法

