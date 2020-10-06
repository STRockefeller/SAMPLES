# CodeWars:Find the unknown digit:20201006:C#

[Reference](https://www.codewars.com/kata/546d15cebed2e10334000ed9)



## Question

To give credit where credit is due: This problem was taken from the ACMICPC-Northwest Regional Programming Contest. Thank you problem writers.

You are helping an archaeologist decipher some runes. He knows that this ancient society used a Base 10 system, and that they never start a number with a leading zero. He's figured out most of the digits as well as a few operators, but he needs your help to figure out the rest.

The professor will give you a simple math expression, of the form

```
[number][op][number]=[number]
```

He has converted all of the runes he knows into digits. The only operators he knows are addition (`+`),subtraction(`-`), and multiplication (`*`), so those are the only ones that will appear. Each number will be in the range from -1000000 to 1000000, and will consist of only the digits 0-9, possibly a leading -, and maybe a few ?s. If there are ?s in an expression, they represent a digit rune that the professor doesn't know (never an operator, and never a leading -). All of the ?s in an expression will represent the same digit (0-9), and it won't be one of the other given digits in the expression. No number will begin with a 0 unless the number itself is 0, therefore 00 would not be a valid number.

Given an expression, figure out the value of the rune represented by the question mark. If more than one digit works, give the lowest one. If no digit works, well, that's bad news for the professor - it means that he's got some of his runes wrong. output -1 in that case.

Complete the method to solve the expression to find the value of the unknown rune. The method takes a string as a paramater repressenting the expression and will return an int value representing the unknown rune or -1 if no such rune exists.

## My Solution

4 kyu 最新的題目(但也沒多新)，挺有趣的感覺不過似乎有點難

題目敘述太多了，這邊整理一下已知資訊

1. 給定的格式固定為`[number][op][number]=[number]`
2. 運算子只有`+` `-` `*` `=` 
3. 數值範圍`-1000000 to 1000000` 為整數且不會有leading zeros
4. 未解讀文字?只會是數字而不是運算子，且所有的?號都代表相同的數字
5. 若有多的?符合條件，取最小值，若無符合條件者回傳-1



延伸想法

1. 因為?只能是0~9所以歷遍所有可能並不會太花時間
2. 因為?不會和任何已出現過的數字相同，所以還能再排除更多可能性
3. 排除leading zero的答案
4. 數值最高可達百萬所以乘法最多可達十億，但未超過Int32的極限(2,147,483,647)，所以還可以用Int32計算
5. 分解輸入內容應該是這題最困難的部分
   1. 等號後方一定是答案值可以先提出來
   2. 前半部看到`+` 或`*`也可以直接作為分割符號使用
   3. `-`由於兩數都可能為負所以要另外判斷



寫完如下

```C#
        public static int solveExpression(string expression)
        {
            List<int> impossibleNums = expression.Where(c => c - '0' >= 0 && c - '0' <= 9).Select(c => c - '0').ToList();
            //[num0][op][num1]=[num2]
            var splitAns = expression.Split('=');
            string[] nums = new string[3];
            nums[2] = splitAns[1];
            //+
            if (expression.Any(c => c == '+'))
            {
                string[] n = splitAns[0].Split('+');
                nums[0] = n[0];
                nums[1] = n[1];
                bool leadingZeroCheck = (nums[0][0] == '?' && nums[0].Count() > 1)
                    || (nums[1][0] == '?' && nums[1].Count() > 1)
                    || (nums[2][0] == '?' && nums[2].Count() > 1);
                for (int i = 0; i <= 9; i++)
                {
                    if (leadingZeroCheck && i == 0) { continue; }
                    if (impossibleNums.Contains(i)) { continue; }
                    int num0 = Int32.Parse(nums[0].Replace("?", i.ToString()));
                    int num1 = Int32.Parse(nums[1].Replace("?", i.ToString()));
                    int num2 = Int32.Parse(nums[2].Replace("?", i.ToString()));
                    if (num2 == num0 + num1) { return i; }
                }
            }
            //*
            else if (expression.Any(c => c == '*'))
            {
                string[] n = splitAns[0].Split('*');
                nums[0] = n[0];
                nums[1] = n[1];
                bool leadingZeroCheck = (nums[0][0] == '?' && nums[0].Count() > 1)
                    || (nums[1][0] == '?' && nums[1].Count() > 1)
                    || (nums[2][0] == '?' && nums[2].Count() > 1);
                for (int i = 0; i <= 9; i++)
                {
                    if (leadingZeroCheck && i == 0) { continue; }
                    if (impossibleNums.Contains(i)) { continue; }
                    int num0 = Int32.Parse(nums[0].Replace("?", i.ToString()));
                    int num1 = Int32.Parse(nums[1].Replace("?", i.ToString()));
                    int num2 = Int32.Parse(nums[2].Replace("?", i.ToString()));
                    if (num2 == num0 * num1) { return i; }
                }
            }
            //-
            else
            {
                int operatorIndex = expression.Take(1).Single() == '-' ?
                    splitAns[0].Skip(1).ToList().IndexOf('-')+1 : splitAns[0].ToList().IndexOf('-');
                List<char> num0List = new List<char>();
                List<char> num1List = new List<char>();
                for (int i = 0; i < splitAns[0].Length; i++)
                {
                    if (i < operatorIndex)
                        num0List.Add(splitAns[0][i]);
                    else if (i > operatorIndex)
                        num1List.Add(splitAns[0][i]);
                }
                nums[0] = new string(num0List.ToArray());
                nums[1] = new string(num1List.ToArray());
                bool leadingZeroCheck = (nums[0][0] == '?' && nums[0].Count() > 1)
                    || (nums[1][0] == '?' && nums[1].Count() > 1)
                    || (nums[2][0] == '?' && nums[2].Count() > 1);
                for (int i = 0; i <= 9; i++)
                {
                    if (leadingZeroCheck && i == 0) { continue; }
                    if (impossibleNums.Contains(i)) { continue; }
                    int num0 = Int32.Parse(nums[0].Replace("?", i.ToString()));
                    int num1 = Int32.Parse(nums[1].Replace("?", i.ToString()));
                    int num2 = Int32.Parse(nums[2].Replace("?", i.ToString()));
                    if (num2 == num0 - num1) { return i; }
                }

            }
            return -1;
        }
```

撰寫和測試都滿順利的，雖然寫的落落長

把重複寫的地方提出來簡化

```C#
        public static int solveExpression(string expression)
        {
            List<int> impossibleNums = expression.Where(c => c - '0' >= 0 && c - '0' <= 9).Select(c => c - '0').ToList();
            //[num0][op][num1]=[num2]
            var splitAns = expression.Split('=');
            string[] nums = new string[3];
            nums[2] = splitAns[1];
            if (expression.Any(c => c == '+'))
            {
                string[] n = splitAns[0].Split('+');
                nums[0] = n[0];
                nums[1] = n[1];
                return findAns(nums, '+');
            }
            else if (expression.Any(c => c == '*'))
            {
                string[] n = splitAns[0].Split('*');
                nums[0] = n[0];
                nums[1] = n[1];
                return findAns(nums, '*');
            }
            else
            {
                int operatorIndex = expression.Take(1).Single() == '-' ?
                    splitAns[0].Skip(1).ToList().IndexOf('-') + 1 : splitAns[0].ToList().IndexOf('-');
                List<char> num0List = new List<char>();
                List<char> num1List = new List<char>();
                for (int i = 0; i < splitAns[0].Length; i++)
                {
                    if (i < operatorIndex)
                        num0List.Add(splitAns[0][i]);
                    else if (i > operatorIndex)
                        num1List.Add(splitAns[0][i]);
                }
                nums[0] = new string(num0List.ToArray());
                nums[1] = new string(num1List.ToArray());
                return findAns(nums, '-');
            }
            int findAns(string[] nums, char op)
            {
                bool leadingZeroCheck = (nums[0][0] == '?' && nums[0].Count() > 1)
                    || (nums[1][0] == '?' && nums[1].Count() > 1)
                    || (nums[2][0] == '?' && nums[2].Count() > 1);
                for (int i = 0; i <= 9; i++)
                {
                    if (leadingZeroCheck && i == 0) { continue; }
                    if (impossibleNums.Contains(i)) { continue; }
                    int num0 = Int32.Parse(nums[0].Replace("?", i.ToString()));
                    int num1 = Int32.Parse(nums[1].Replace("?", i.ToString()));
                    int num2 = Int32.Parse(nums[2].Replace("?", i.ToString()));
                    switch (op)
                    {
                        case '+':
                            if (num2 == num0 + num1) { return i; }
                            break;
                        case '*':
                            if (num2 == num0 * num1) { return i; }
                            break;
                        case '-':
                            if (num2 == num0 - num1) { return i; }
                            break;
                    }
                }
                return -1;
            }
            return -1;
        }
```



## Better Solutions



### Solution 1

```C#
using System.Data;
public class Runes
{
  public static int solveExpression(string expression)
  {
            int missingDigit = -1;
            DataTable dt = new DataTable();
            string TRUSHE4ka = "";
            for (int i = 0; i <= 9; i++)
            {
                if ((expression.Contains("??")) && (i == 0)) { continue; }
                if (expression.Contains(i.ToString())) { continue; }
                TRUSHE4ka = expression.Replace('?', i.ToString().ToCharArray()[0]);
                var v = dt.Compute(TRUSHE4ka, "");
                if ((bool)v)
                {
                    return i;
                }
            }
            return missingDigit;
  }
}
```

似乎是不嚴謹的解答不過還是有可以參考的部分

![](https://i.imgur.com/f022i6D.png)

(文章因為md格式的關係所以變成斜體顯示原文如下)

`solveExpression("1?*1?=1??") returns -1 although it should return 0 since 10*10=100.`



[MSDN:DaTatable](https://docs.microsoft.com/zh-tw/dotnet/api/system.data.datatable?view=netcore-3.1)

[MSDN:DataTable.Compute](https://docs.microsoft.com/zh-tw/dotnet/api/system.data.datatable.compute?view=netcore-3.1)

### Solution 2

```C#
using System;
using System.Data;

public class Runes
{
  public static int solveExpression(string expression)
  {
    string[] sections = expression.Split('=');
    DataTable dt = new DataTable();
    for(int i = 0; i <= 9; i++)
    {
      if(expression.Contains(i.ToString()))
        continue;
      if(dt.Compute(sections[0].Replace('?', i.ToString()[0]), "").ToString() == sections[1].Replace('?', i.ToString()[0]))
        return i;
    }
    return -1;
  }
}
```



不得不說DataTable的Compute方法真的很適合這題