# CodeWars:Sum Strings as Numbers:202008XX:C#

[Reference](https://www.codewars.com/kata/5324945e2ece5e1f32000370/csharp)



## Question

Given the string representations of two integers, return the string representation of the sum of those integers.

For example:

```javascript
sumStrings('1','2') // => '3'
```

A string representation of an integer will contain no characters besides the ten numerals "0" to "9".

## My Solution

```C#
using System;
using System.Collections.Generic;
using System.Linq;
public static class Kata
{
        public static string sumStrings(string a, string b)
        {
            a = pureNumString(a);
            b = pureNumString(b);
            string biggerOne = a.Length >= b.Length ? a : b;
            string smallerOne = a.Length < b.Length ? a : b;
            int difLength = biggerOne.Length - smallerOne.Length;
            for (int i = 0; i < difLength; i++)
                smallerOne = "0" + smallerOne;
            smallerOne = "0" + smallerOne;
            biggerOne = "0" + biggerOne;
            var biggerOneList = biggerOne.Select(s => s).ToList();
            var smallerOneList = smallerOne.Select(s => s).ToList();
            List<int> sumList = new List<int>();
            string returnValue = "";
            for (int i = biggerOneList.Count() - 1; i > 0; i--)
                sumList.Add(Convert.ToInt32(biggerOneList[i].ToString()) + Convert.ToInt32(smallerOneList[i].ToString()));
            for (int i = 0; i < sumList.Count(); i++)
            {
                if (sumList[i] >= 10)
                {
                    sumList[i] -= 10;
                    try { sumList[i + 1] += 1; }
                    catch { sumList.Add(1); }
                }
            }
            foreach (int i in sumList)
                returnValue = $"{i.ToString()}{returnValue}";
            return returnValue;
        }
        private static string pureNumString(string str)
        {
            string returnValue = "";
            var ps = str.SkipWhile(c => c == '0');
            foreach (char c in ps)
                returnValue += c.ToString();
            return returnValue;
        }
}
```



第一眼看到題目敘述只想到兂麼會有這麼簡單的Kyu4題目，興沖沖的把輸入Convert.toInt32之後相加，然後就跳了一堆Out of range

發現測試用數據每個都大的誇張後，只能回頭扎扎實實的將每個位置的數字相加在算上進位(有考慮過一次計算多個數字但想不出具體實現方法)，將兩個字串變成相同長度並預先留下進位用的位置是這題思路的重點。



## Better Solutions

### Solution 1

```C#
using System;
using System.Numerics;

public static class Kata
{
    public static string sumStrings(string a, string b)
    {
      BigInteger aInt;
      BigInteger bInt;
      
      BigInteger.TryParse(a, out aInt);
      BigInteger.TryParse(b, out bInt);
      
      return (aInt + bInt).ToString();
    }
}
```

呃...`BigInteger`是什麼東西啊?也太好用了吧，我懷疑題目作者會認為你作弊

[MSDN:BigInteger](https://docs.microsoft.com/zh-tw/dotnet/api/system.numerics.biginteger?view=netcore-3.1)



### Solution 2

```C#
using System;
public static class Kata
{
    public static string sumStrings(string a, string b)
    {
     return (Convert.ToInt32(a)+Convert.ToInt32(b)).ToString();
    }
}
```



?????唉不是，這樣寫是怎麼過關的??搞不懂

查了下評論發現題目有改版過，以前似乎不會用超大數據測試



### Solution 3

```C#
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Kata
{
    public static string sumStrings(string a, string b)
    {
        var res = "";
        var c = 0;
        var aChars = new Stack<char>(a.ToCharArray());
        var bChars = new Stack<char>(b.ToCharArray());
        while (aChars.Count + bChars.Count + c > 0) {
            c += (aChars.Count > 0 ? (aChars.Pop() - '0') : 0) +
                (bChars.Count > 0 ? (bChars.Pop() - '0') : 0);
            res = c % 10 + res;
            c /= 10; 
        }
        return new Regex("^0").Replace(res, "");
    }
}
```

我只服這位，我覺得前兩個都在偷吃步

[MSDN:Stack](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.generic.stack-1?view=netcore-3.1)

[MSDN:Regex](https://docs.microsoft.com/zh-tw/dotnet/api/system.text.regularexpressions.regex?view=netcore-3.1)