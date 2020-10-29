# CodeWars:Calculator:20201028:C#

[Reference](https://www.codewars.com/kata/5235c913397cbf2508000048)



## Question

Create a simple calculator that given a string of operators (), +, -, *, / and numbers separated by spaces returns the value of that expression

Example:

```python
Calculator().evaluate("2 / 2 + 3 * 4 - 6") # => 7
```

Remember about the order of operations! Multiplications and divisions have a higher priority and should be performed left-to-right. Additions and subtractions have a lower priority and should also be performed left-to-right.

## My Solution

挑戰 3 kyu ，先求過關其他再說

模擬計算機簡單好懂，僅四則運算含括弧，怎麼實現就沒這麼簡單了

先整理計算順序

1. ()
2. */
3. +-

找個例子來分析

( 10 + 30 ) * 20 / 2

=> 40*20/2 =>800/2 => 400

改輸入然後遞迴應該不錯



找括弧是第一步驟，還必須從最內側的括弧算起=>由左往右找')'再從這個位置往左找'('



計算的部分，由左往右找*或/，找到就做計算，然後遞迴。ex:"40 * 20 / 2"=>"800 / 2"



實現如下

```C#
    public class Evaluator
    {
        public double Evaluate(string expression) => Evaluate(expression.Split(' ').ToList());
        public double Evaluate(List<string> elements)
        {
            if (elements.Count == 1) { return Convert.ToDouble(elements[0]); }
            if (elements.Contains("("))
                elements = findParen(elements);
            else
            {
                int indexMul = elements.Contains("*") ? elements.IndexOf("*") : 9999;
                int indexDev = elements.Contains("/") ? elements.IndexOf("/") : 9999;
                int indexPlu = elements.Contains("+") ? elements.IndexOf("+") : 9999;
                int indexMin = elements.Contains("-") ? elements.IndexOf("-") : 9999;
                //*/
                if (indexDev != 9999 || indexMul != 9999)
                {
                    // *
                    if (indexMul < indexDev)
                    {
                        elements[indexMul] = (Convert.ToDouble(elements[indexMul - 1]) * Convert.ToDouble(elements[indexMul + 1]))
                            .ToString();
                        elements[indexMul - 1] = "del";
                        elements[indexMul + 1] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                    // /
                    else
                    {
                        elements[indexDev] = (Convert.ToDouble(elements[indexDev - 1]) / Convert.ToDouble(elements[indexDev + 1]))
                            .ToString();
                        elements[indexDev - 1] = "del";
                        elements[indexDev + 1] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                }
                //+-
                else
                {
                    // +
                    if (indexPlu < indexMin)
                    {
                        elements[indexPlu] = (Convert.ToDouble(elements[indexPlu - 1]) + Convert.ToDouble(elements[indexPlu + 1]))
                            .ToString();
                        elements[indexPlu - 1] = "del";
                        elements[indexPlu + 1] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                    // /
                    else
                    {
                        elements[indexMin] = (Convert.ToDouble(elements[indexMin - 1]) - Convert.ToDouble(elements[indexMin + 1]))
                            .ToString();
                        elements[indexMin - 1] = "del";
                        elements[indexMin + 1] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                }
            }
            return Evaluate(elements);
        }
        private List<string> findParen(List<string> elements)
        {
            int[] parenIndex = new int[2] { 0, 0 };
            //find the first close paren )
            parenIndex[1] = elements.IndexOf(")");
            //find the other paren (
            for (int i = parenIndex[1]; i >= 0; i--)
            {
                if (elements[i] == "(")
                {
                    parenIndex[0] = i;
                    break;
                }
            }
            string[] front = elements.Take(parenIndex[0]).ToArray();
            double mid = Evaluate(elements.Skip(parenIndex[0] + 1).Take(parenIndex[1] - parenIndex[0] - 1).ToList());
            string[] back = elements.Skip(parenIndex[1] + 1).ToArray();
            List<string> res = new List<string>(front);
            res.Add(mid.ToString());
            res.AddRange(back);
            return res;
        }
    }
```

簡單測試

```C#
        private static bool calculatorTest(string input,double ans)
        {
            Evaluator evaluator = new Evaluator();
            return evaluator.Evaluate(input) == ans;
        }
```

```C#
            Console.WriteLine(calculatorTest("1 + 1", 2));
            Console.WriteLine(calculatorTest("2 * 3 + 1", 7));
            Console.WriteLine(calculatorTest("( 1 + 1 ) / 2 + 1", 2));
            Console.WriteLine(calculatorTest("( ( 1 + 1 ) )", 2));
            Console.WriteLine(calculatorTest("10 / 2 + 12 / 3", 9));
```

通過，丟到CodeWars測試看看

```C#
    [TestCase("1-1", ExpectedResult = 0)]
    [TestCase("1+1", ExpectedResult = 2)]
    [TestCase("1 - 1", ExpectedResult = 0)]
    [TestCase("1* 1", ExpectedResult = 1)]
    [TestCase("1 /1", ExpectedResult = 1)]
```

被騙了，說好的`numbers separated by spaces`呢?

---

認命修改吧，既然數字和運算子並沒有總是以空格隔開，那顯然不能用split簡單的拆解字串了

把運算子和數字做區隔，比較麻煩的是負數和減法運算子符號有衝突

修改第一個方法做字串拆分

```C#
        public double Evaluate(string expression)
        {
            HashSet<string> parens = new HashSet<string> { "(", ")" };
            HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/" };
            expression = expression.Replace(" ", "");
            List<string> elements = new List<string>();
            while (expression.Length > 0)
            {
                string temp = new string(expression.Take(1).ToArray());
                if (parens.Contains(temp))
                {
                    elements.Add(temp);
                    expression = new string(expression.Skip(1).ToArray());
                    continue;
                }
                if (operators.Contains(temp))
                {
                    //num<0
                    if (temp == "-"
                        && (operators.Contains(elements.LastOrDefault())
                        || parens.Contains(elements.LastOrDefault())
                        || elements.Count == 0))
                    {
                        string mNum = "-" + new string(expression.Skip(1).TakeWhile(c => !parens.Contains(c.ToString()) 
                        && !operators.Contains(c.ToString())).ToArray());
                        elements.Add(mNum);
                        expression = new string(expression.Skip(mNum.Length).ToArray());
                    }
                    else
                    {
                        elements.Add(temp);
                        expression = new string(expression.Skip(1).ToArray());
                    }
                    continue;
                }
                string num = new string(expression.TakeWhile(c => !parens.Contains(c.ToString())
                && !operators.Contains(c.ToString())).ToArray());
                elements.Add(num);
                expression = new string(expression.Skip(num.Length).ToArray());
            }
            return Evaluate(elements);
        }
```

`"12* 123/-(-5 + 2)"`跳例外，用Debug檢查

在`"1476", "/", "-", "-3"`的情況下找到`/`符號執行除法目標為`1476/-`所以跳例外

這個情況下`-`應與後方的`-3`結合為一個數字才對。

極端點，還必須能辨認連續`-`號如`-(-(-(-(4))))`

---

修改後的完整程式碼(230行有夠長)

```C#
    public class Evaluator
    {
        public double Evaluate(string expression)
        {
            HashSet<string> parens = new HashSet<string> { "(", ")" };
            HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/" };
            expression = expression.Replace(" ", "");
            List<string> elements = new List<string>();
            while (expression.Length > 0)
            {
                string temp = new string(expression.Take(1).ToArray());
                if (parens.Contains(temp))
                {
                    elements.Add(temp);
                    expression = new string(expression.Skip(1).ToArray());
                    continue;
                }
                if (operators.Contains(temp))
                {
                    //num<0
                    if (temp == "-"
                        && (operators.Contains(elements.LastOrDefault())
                        || parens.Contains(elements.LastOrDefault())
                        || elements.Count == 0))
                    {
                        string mNum = "-" + new string(expression.Skip(1).TakeWhile(c => !parens.Contains(c.ToString())
                        && !operators.Contains(c.ToString())).ToArray());
                        elements.Add(mNum);
                        expression = new string(expression.Skip(mNum.Length).ToArray());
                    }
                    else
                    {
                        elements.Add(temp);
                        expression = new string(expression.Skip(1).ToArray());
                    }
                    continue;
                }
                string num = new string(expression.TakeWhile(c => !parens.Contains(c.ToString())
                && !operators.Contains(c.ToString())).ToArray());
                elements.Add(num);
                expression = new string(expression.Skip(num.Length).ToArray());
            }
            return Evaluate(elements);
        }
        public double Evaluate(List<string> elements)
        {
            if (elements.Count == 1) { return Convert.ToDouble(elements[0]); }
            if (elements.Contains("("))
                elements = findParen(elements);
            else
            {
                int indexMul = elements.Contains("*") ? elements.IndexOf("*") : 9999;
                int indexDiv = elements.Contains("/") ? elements.IndexOf("/") : 9999;
                int indexPlu = elements.Contains("+") ? elements.IndexOf("+") : 9999;
                int indexMin = elements.Contains("-") ? elements.IndexOf("-") : 9999;
                //*/
                if (indexDiv != 9999 || indexMul != 9999)
                {
                    // *
                    if (indexMul < indexDiv)
                    {
                        int secondNumIndex = indexMul + 1;
                        bool isNegative = false;
                        double firstNum = Convert.ToDouble(elements[indexMul - 1]);
                        double secondNum = elements[secondNumIndex] != "-" ? Convert.ToDouble(elements[secondNumIndex])
                            : Convert.ToDouble(elements.SkipWhile(s => s != "*").Skip(1).SkipWhile(s =>
                            {
                                if (s == "-")
                                {
                                    isNegative = !isNegative;
                                    return true;
                                }
                                return false;
                            }).First());
                        for (int i = secondNumIndex; i < elements.Count; i++)
                            if (elements[i] == secondNum.ToString())
                            {
                                secondNumIndex = i;
                                break;
                            }
                        secondNum = isNegative ? -secondNum : secondNum;
                        elements[indexMul] = (firstNum * secondNum).ToString();
                        elements[indexMul - 1] = "del";
                        for (int i = indexMul + 1; i <= secondNumIndex; i++)
                            elements[i] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                    // /
                    else
                    {
                        int secondNumIndex = indexDiv + 1;
                        bool isNegative = false;
                        double firstNum = Convert.ToDouble(elements[indexDiv - 1]);
                        double secondNum = elements[secondNumIndex] != "-" ? Convert.ToDouble(elements[secondNumIndex])
                            : Convert.ToDouble(elements.SkipWhile(s => s != "/").Skip(1).SkipWhile(s =>
                            {
                                if (s == "-")
                                {
                                    isNegative = !isNegative;
                                    return true;
                                }
                                return false;
                            }).First());
                        for (int i = secondNumIndex; i < elements.Count; i++)
                            if (elements[i] == secondNum.ToString())
                            {
                                secondNumIndex = i;
                                break;
                            }
                        secondNum = isNegative ? -secondNum : secondNum;
                        elements[indexDiv] = (firstNum / secondNum).ToString();
                        elements[indexDiv - 1] = "del";
                        for (int i = indexDiv + 1; i <= secondNumIndex; i++)
                            elements[i] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                }
                //+-
                else
                {
                    // +
                    if (indexPlu < indexMin)
                    {
                        int secondNumIndex = indexPlu + 1;
                        bool isNegative = false;
                        double firstNum = Convert.ToDouble(elements[indexPlu - 1]);
                        double secondNum = elements[secondNumIndex] != "-" ? Convert.ToDouble(elements[secondNumIndex])
                            : Convert.ToDouble(elements.SkipWhile(s => s != "+").Skip(1).SkipWhile(s =>
                            {
                                if (s == "-")
                                {
                                    isNegative = !isNegative;
                                    return true;
                                }
                                return false;
                            }).First());
                        for(int i = secondNumIndex;i<elements.Count;i++)
                            if(elements[i]==secondNum.ToString())
                            {
                                secondNumIndex = i;
                                break;
                            }
                        secondNum = isNegative ? -secondNum : secondNum;
                        elements[indexPlu] = (firstNum + secondNum).ToString();
                        elements[indexPlu - 1] = "del";
                        for (int i = indexPlu + 1; i <= secondNumIndex; i++)
                            elements[i] = "del";
                        elements.RemoveAll(str => str == "del");
                    }
                    // -
                    else
                    {
                        if(indexMin==0)
                        {
                            for(int i=1; ;i++)
                            {
                                if(elements[i]!="-")
                                {
                                    elements[i] = (-Convert.ToDouble(elements[i])).ToString();
                                    for (int j = 0; j < i; j++)
                                        elements[j] = "del";
                                    elements.RemoveAll(s => s == "del");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            int secondNumIndex = indexMin + 1;
                            bool isNegative = false;
                            double firstNum = Convert.ToDouble(elements[indexMin - 1]);
                            double secondNum = elements[secondNumIndex] != "-" ? Convert.ToDouble(elements[secondNumIndex])
                                : Convert.ToDouble(elements.SkipWhile(s => s != "-").Skip(1).SkipWhile(s =>
                                {
                                    if (s == "-")
                                    {
                                        isNegative = !isNegative;
                                        return true;
                                    }
                                    return false;
                                }).First());
                            for (int i = secondNumIndex; i < elements.Count; i++)
                                if (elements[i] == secondNum.ToString())
                                {
                                    secondNumIndex = i;
                                    break;
                                }
                            secondNum = isNegative ? -secondNum : secondNum;
                            elements[indexMin] = (firstNum - secondNum).ToString();
                            elements[indexMin - 1] = "del";
                            for (int i = indexMin + 1; i <= secondNumIndex; i++)
                                elements[i] = "del";
                            elements.RemoveAll(str => str == "del");
                        }
                    }
                }
            }
            return Evaluate(elements);
        }
        private List<string> findParen(List<string> elements)
        {
            int[] parenIndex = new int[2] { 0, 0 };
            //find the first close paren )
            parenIndex[1] = elements.IndexOf(")");
            //find the other paren (
            for (int i = parenIndex[1]; i >= 0; i--)
            {
                if (elements[i] == "(")
                {
                    parenIndex[0] = i;
                    break;
                }
            }
            string[] front = elements.Take(parenIndex[0]).ToArray();
            double mid = Evaluate(elements.Skip(parenIndex[0] + 1).Take(parenIndex[1] - parenIndex[0] - 1).ToList());
            string[] back = elements.Skip(parenIndex[1] + 1).ToArray();
            List<string> res = new List<string>(front);
            res.Add(mid.ToString());
            res.AddRange(back);
            return res;
        }
    }
```

通關(37ms)，感動，這題大概寫了兩小時吧，寫得頭昏腦脹我也懶得優化了。

整體來說字串拆分、括弧檢測、計算優先順序等等都寫得算是順利，最難的部份當屬`-`符號的辨認

## Better Solutions



### Solution 1

```C#
using System;
using System.Data;

public class Evaluator
{
  // Shamelessly adapted from https://stackoverflow.com/questions/6052640/in-c-sharp-is-there-an-eval-function
  // (Next-to-top Comment)
  public double Evaluate(string s) => Math.Round(Convert.ToDouble(new DataTable().Compute(s, String.Empty)), 6);
}
```

這也能一行搞定，服

[MSDN:DataTable.Compute](https://docs.microsoft.com/zh-tw/dotnet/api/system.data.datatable.compute?view=netcore-3.1)



### Solution 2

```C#
using System;
using System.Collections.Generic;
using System.Linq;

public class Evaluator
{
    public double Evaluate(string expression)
    {
        expression = ReplaceOperators(expression);
        return IsSimpleExpression(expression)
            ? ParseSimpleExpression(expression)
            : ParseComplexExpression(expression);
    }

    private bool IsSimpleExpression(string expression)
    {
        double res;
        return double.TryParse(expression, out res);
    }

    private double ParseSimpleExpression(string expression)
    {
        return double.Parse(expression);
    }

    private double ParseComplexExpression(string expression)
    {
        List<object> items = GetSplittedExpression(expression);
        items = GetPolishNotatation(items);
        return GetResult(items);
    }

    private double GetResult(List<object> items)
    {
        Stack<double> operands = new Stack<double>();

        foreach (object item in items)
        {
            if (item is double)
            {
                operands.Push((double)item);
            }
            else
            {
                double op2 = operands.Pop();
                double op1 = operands.Pop();

                double res = GetResult(op1, op2, (char)item);
                operands.Push(res);
            }
        }

        return operands.Pop();
    }

    private double GetResult(double op1, double op2, char operation)
    {
        Func<double, double, double> func = GetFunc(operation);
        return func(op1, op2);
    }

    private Func<double, double, double> GetFunc(char operation)
    {
        switch (operation)
        {
            case '+':
                return (a, b) => a + b;

            case '-':
                return (a, b) => a - b;

            case '*':
                return (a, b) => a * b;

            case '/':
                return (a, b) => a / b;

            default:
                return null;
        }
    }

    private List<object> GetPolishNotatation(List<object> items)
    {
        Queue<object> queue = new Queue<object>();
        Stack<char> stack = new Stack<char>();

        foreach (object item in items)
        {
            if (item is double)
            {
                queue.Enqueue(item);
            }
            else if (IsOperator((char)item))
            {
                char c = (char)item;
                while (stack.Count > 0 && (GetPrecendence(stack.Peek()) > GetPrecendence(c)
                        || (GetPrecendence(stack.Peek()) == GetPrecendence(c) && GetAssociativity(c) == "left"))
                       && (stack.Peek() != '('))
                {
                    queue.Enqueue(stack.Pop());
                }

                stack.Push(c);
            }
            else if ((char)item == '(')
            {
                stack.Push('(');
            }
            else if ((char)item == ')')
            {
                while (stack.Peek() != '(')
                {
                    queue.Enqueue(stack.Pop());
                }

                stack.Pop();
            }
        }

        while (stack.Any())
        {
            queue.Enqueue(stack.Pop());
        }

        return queue.ToList();
    }

    private string GetAssociativity(char c)
    {
        switch (c)
        {
            case '+':
            case '-':
            case '*':
            case '/':
                return "left";

            case '^':
                return "right";

            default:
                return string.Empty;
        }
    }

    private int GetPrecendence(char c)
    {
        switch (c)
        {
            case '+':
            case '-':
                return 2;

            case '*':
            case '/':
                return 3;

            case '^':
                return 4;

            default:
                return 0;
        }
    }

    private bool IsOperator(char c)
    {
        return c == '-'
               || c == '+'
               || c == '/'
               || c == '*';

    }

    private List<object> GetSplittedExpression(string expression)
    {
        List<object> objects = new List<object>();
        for (int i = 0; i < expression.Length; i++)
        {
            if (char.IsDigit(expression[i]))
            {
                string number = string.Empty;
                if (i - 2 > 0 && !char.IsDigit(expression[i - 1]) && !char.IsDigit(expression[i - 2]) &&
                    expression[i - 1] == '-')
                {
                    number += "-";
                }

                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    number += expression[i];
                    i++;
                }

                objects.Add(double.Parse(number));
                i--;
            }
            else
            {
                if (i - 1 > 0 && !char.IsDigit(expression[i - 1]) && expression[i] == '-')
                {
                    continue;
                }

                objects.Add(expression[i]);
            }
        }

        return objects;
    }

    private string ReplaceOperators(string expression)
    {
        expression = expression.Replace(" ", string.Empty);
        expression = expression.Replace("--", "+");
        expression = expression.Replace(")-(", ")+-1*(");
        expression = expression.Replace("/-", "/-1/");
        expression = expression.Replace("-(", "(-1*");

        for (int i = 0; i < 10; i++)
        {
            expression = expression.Replace(string.Format("{0}(", i), string.Format("{0}+(", i));
        }


        return expression;
    }
}
```

同樣寫得很長，但有許多值得參考的部分，以下一一分析



#### 判斷是否為運算子

我的寫法是使用HashSet儲存運算子，再用Contains判斷

```C#
HashSet<string> operators = new HashSet<string> { "+", "-", "*", "/" };
```

他是獨立一個方法出來

```C#
    private bool IsOperator(char c)
    {
        return c == '-'
               || c == '+'
               || c == '/'
               || c == '*';
    }
```

這兩種寫法應該沒有明顯的優劣之分，但我個人比較喜歡後者，能用越簡單明瞭的手段達成目的越是高明。



#### 簡化Expression

我想到這麼做，就是硬上

他這邊有把一些情況簡化，讓問題不再複雜，很不錯的做法

```C#
        expression = expression.Replace(" ", string.Empty);
        expression = expression.Replace("--", "+");
        expression = expression.Replace(")-(", ")+-1*(");
        expression = expression.Replace("/-", "/-1/");
        expression = expression.Replace("-(", "(-1*");
```



#### 使用一個委派含括全部四則運算

```C#
   private double GetResult(double op1, double op2, char operation)
    {
        Func<double, double, double> func = GetFunc(operation);
        return func(op1, op2);
    }

    private Func<double, double, double> GetFunc(char operation)
    {
        switch (operation)
        {
            case '+':
                return (a, b) => a + b;

            case '-':
                return (a, b) => a - b;

            case '*':
                return (a, b) => a * b;

            case '/':
                return (a, b) => a / b;

            default:
                return null;
        }
    }
```



#### 量化優先程度

```C#
    private int GetPrecendence(char c)
    {
        switch (c)
        {
            case '+':
            case '-':
                return 2;

            case '*':
            case '/':
                return 3;

            case '^':
                return 4;

            default:
                return 0;
        }
    }
```

比起我那一大堆 if else 乾淨太多了。



#### 計算使用Stack結構

```C#
    private double GetResult(List<object> items)
    {
        Stack<double> operands = new Stack<double>();

        foreach (object item in items)
        {
            if (item is double)
            {
                operands.Push((double)item);
            }
            else
            {
                double op2 = operands.Pop();
                double op1 = operands.Pop();

                double res = GetResult(op1, op2, (char)item);
                operands.Push(res);
            }
        }

        return operands.Pop();
    }
```

