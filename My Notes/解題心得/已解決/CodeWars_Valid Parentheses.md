# CodeWars:Valid Parentheses:20200918:C#

[Reference](https://www.codewars.com/kata/52774a314c2333f0a7000688/csharp)



## Question

Write a function called that takes a string of parentheses, and determines if the order of the parentheses is valid. The function should return `true` if the string is valid, and `false` if it's invalid.

### Examples

```
"()"              =>  true
")(()))"          =>  false
"("               =>  false
"(())((()())())"  =>  true
```

### Constraints

```
0 <= input.length <= 100
```

Along with opening (`(`) and closing (`)`) parenthesis, input may contain any valid ASCII characters. Furthermore, the input string may be empty and/or not contain any parentheses at all. Do **not** treat other forms of brackets as parentheses (e.g. `[]`, `{}`, `<>`).

## My Solution

```C#
        public static bool ValidParentheses(string input)
        {
            List<char> charList = input.ToCharArray().ToList();
            charList.RemoveAll(c => c != '(' && c != ')');
            while (charList.Count > 0)
            {
                int preCont = charList.Count;
                for (int i = 0; i < charList.Count - 1; i++)
                {
                    if (charList[i] == '(' && charList[i + 1] == ')')
                    {
                        charList[i] = '*';
                        charList[i + 1] = '*';
                        charList.RemoveAll(c => c == '*');
                    }
                }
                if (charList.Count == preCont)
                    return false;
            }
            return true;
        }
```

想法是先將輸入中不需要的資訊排除只留下'(' ')'，接著由裡而外將有效括弧刪掉，如果最後全刪完則代表全部括弧都有效

## Better Solutions



### Solution 1

```C#
public class Parentheses
{
    public static bool ValidParentheses(string input)
    {
        int parentheses = 0;
        foreach (char t in input)
        {
            if (t == '(')
            {
                parentheses++;
            }
            else if (t == ')')
            {
                parentheses--;

                if (parentheses < 0)
                {
                    return false;
                }
            }
        }

        return parentheses == 0;
    }
}
```

不一樣的思路，有效括弧一定是先左再右，並且最後左右括弧的數量要相等。少一層迴圈，比起我的解法更加有效率



### Solution 2

```C#
using System;
using System.Linq;
public class Parentheses
{
    public static bool ValidParentheses(string input)
    {
        input = string.Concat(input.Where(x => x == '(' || x == ')'));
        while(input.Contains("()"))
        {
          input = input.Replace("()", "");
        }
        return string.IsNullOrEmpty(input);
    }
}
```

和我的想法比較接近，但更加簡潔

用Contact將`input.Where(x => x == '(' || x == ')')`串接成一個string

[MSDN:string.Contact](https://docs.microsoft.com/zh-tw/dotnet/api/system.string.concat?view=netcore-3.1)

後面Replace的作法剛剛竟然沒想到orz