# LeetCode:Palindrome Partitioning:20201215:C#

[Reference](https://leetcode.com/explore/challenge/card/december-leetcoding-challenge/570/week-2-december-8th-december-14th/3565/)



## Question

Given a string `s`, partition `s` such that every substring of the partition is a **palindrome**. Return all possible palindrome partitioning of `s`.

A **palindrome** string is a string that reads the same backward as forward.

 

**Example 1:**

```
Input: s = "aab"
Output: [["a","a","b"],["aa","b"]]
```

**Example 2:**

```
Input: s = "a"
Output: [["a"]]
```

 

**Constraints:**

- `1 <= s.length <= 16`
- `s` contains only lowercase English letters.

## My Solution

2020 12月 第二週 的最後一題(因為時差的關係LeetCode還是12/14)

題目描述有點簡陋，範例也不夠清楚，只能先試錯看看了解題意。

例如題目若給[a,a,b,b]那我要回{[a,a,b,b],[aa,b,b],[a,a,bb],[aa,bb]}還是{[a,a,b,b],[aa,bb]}就好?

全部的substring湊起來是否必須等於輸入值?



```C#
public class LeetSolution20201215
    {
        public IList<IList<string>> Partition(string s)
        {
            IList<IList<string>> res;
            List<string> allSubstring = findAllSubstring(s);
            List<char> charList = new List<char>(s.ToCharArray());
            List<string> palindrome = allSubstring.Where(str => isPalindrome(str)).ToList();
            //todo
            
        }
        private List<string> findAllSubstring(string str)
        {
            List<string> res = new List<string>();
            for (int length = 1; length < str.Length; length++)
                for (int start = 0; start <= str.Length - length; start++)
                    res.Add(str.Substring(start, length));
            return res;
        }
        private bool isPalindrome(List<char> charList)
        {
            for(int i=0;i<=charList.Count/2;i++)
                if (charList[i] != charList[charList.Count - i - 1])
                    return false;
            return true;
        }
        private bool isPalindrome(string str)
        {
            for (int i = 0; i <= str.Length / 2; i++)
                if (str[i] != str[str.Length - i - 1])
                    return false;
            return true;
        }
    }
```

做到一半察覺到就算把所有迴文的substring找出來也不知道怎麼做組合

換個做法比如:abacade

1. 長度為4,5找不到迴文
2. 長度為3找到aba和aca
3. 這邊先以aca往下看
4. 把字串拆成"ab","aca","de"
5. 除了aca以外的兩組遞迴
6. 回到步驟3這次以aba往下看
7. 接著判斷長度2
8. 以此類推



## Better Solutions

