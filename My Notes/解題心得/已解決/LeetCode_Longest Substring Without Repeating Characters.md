# LeetCode:3:20201014:C#

Reference

Longest Substring Without Repeating Characters

## Question

Given a string `s`, find the length of the **longest substring** without repeating characters.

 

**Example 1:**

```
Input: s = "abcabcbb"
Output: 3
Explanation: The answer is "abc", with the length of 3.
```

**Example 2:**

```
Input: s = "bbbbb"
Output: 1
Explanation: The answer is "b", with the length of 1.
```

**Example 3:**

```
Input: s = "pwwkew"
Output: 3
Explanation: The answer is "wke", with the length of 3.
Notice that the answer must be a substring, "pwke" is a subsequence and not a substring.
```

**Example 4:**

```
Input: s = ""
Output: 0
```

 

**Constraints:**

- `0 <= s.length <= 5 * 104`
- `s` consists of English letters, digits, symbols and spaces.

## My Solution

從第一個字元開始往後抓，抓到有重複的就輸出長度，並把前面skip掉再繼續

例如第一個範例`s = "abcabcbb"`

a-->ab-->abc

-->abca，重複了輸出長度(4-1)，把重複字元之前的skip掉(a)，變成bca繼續

-->bcab，重複了輸出長度(4-1)，把重複字元之前的skip掉(b)，變成cab繼續...

感覺應該效率不佳，但還是先實作看看

```C#
    public class Solution
    {
        private int length = 0;
        public int LengthOfLongestSubstring(string s)
        {
            if (s == new string(s.Distinct().ToArray()))
                return s.Length>length?s.Length:length;
            List<char> substring = new List<char>();
            foreach (char c in s)
            {
                if (!substring.Contains(c))
                    substring.Add(c);
                else
                {
                    length = substring.Count > length ? substring.Count : length;
                    return LengthOfLongestSubstring(new string(s.SkipWhile(cc=>cc!=c).Skip(1).ToArray()));
                }
            }
            length = substring.Count > length ? substring.Count : length;
            return length;
        }
    }
```

結果是timeout，不太意外，再想想

---

一樣使用第一個範例`s = "abcabcbb"`

這次不刪字串遞迴了

a-->ab-->abc-->abca，a重複，找上一個a算長度-->abcab，b重複，找上一個b算長度...

再更進階點

1. a `把a(位置0)記錄起來`
2. ab `把b(位置1)記錄起來`
3. abc `把c(位置2)記錄起來`
4. abca `把a(位置3)記錄起來，發現之前記錄過a(位置0)了，輸出長度，並覆蓋之前的紀錄，只留下a(位置3)`
5. ...

實現如下

```C#
   public class Solution
    {
        public int LengthOfLongestSubstring(string s)
        {
            int res = 0;
            int lastIndex;
            Dictionary<char, int> dic = new Dictionary<char, int>();
            for(int i=0;i<s.Length;i++)
            {
                if(dic.ContainsKey(s[i]))
                {
                    dic.TryGetValue(s[i], out lastIndex);
                    res = Math.Max(res, i - lastIndex);
                    dic.Remove(s[i]);
                    dic.Add(s[i], i);
                }
                else
                    dic.Add(s[i],i);
            }
            return res;
        }
   }
```

新的問題來了，這樣子最長substring在頭尾的情況無法計算(因為只找了重複字元間的字段)

例如輸入"asdfg"會回傳0

來分析一下這種情況我們想要的輸出

以"hellow"為例，最長的應該是"hel"和"low"

從後方往前找不重複的字段，以及從前方往後找，多建兩個list來計算，反正空間不值錢，實現如下(另外res的計算有錯abba會回傳3，順便改了)

```C#
    public class Solution
    {
        public int LengthOfLongestSubstring(string s)
        {
            int res = 0;
            int lastIndex = 0;
            Dictionary<char, int> dic = new Dictionary<char, int>();
            for(int i=0;i<s.Length;i++)
            {
                if(dic.ContainsKey(s[i]))
                {
                    int tempLast;
                    dic.TryGetValue(s[i], out tempLast);
                    lastIndex = Math.Max(lastIndex, tempLast);
                    res = Math.Max(res, i - lastIndex);
                    dic.Remove(s[i]);
                    dic.Add(s[i], i);
                    lastIndex = i;
                }
                else
                    dic.Add(s[i],i);
            }

            List<char> fromEnd = new List<char>();
            int fromEndLength = 0;
            for(int i=s.Length-1;i>=0&&!fromEnd.Contains(s[i]);i--)
            {
                fromEnd.Add(s[i]);
                fromEndLength++;
            }
            if(fromEndLength>s.Length/2)
                return Math.Max(res,fromEndLength);
            List<char> fromFirst = new List<char>();
            int fromFirstLength = 0;
            for (int i = 0; i<s.Length && !fromFirst.Contains(s[i]); i++)
            {
                fromFirst.Add(s[i]);
                fromFirstLength++;
            }
            int[] length = new int[3] { res, fromEndLength, fromFirstLength };
            return length.Max();
        }
    }
```

"ohomm" wrong

這個輸入應該要找到"hom"，但是依現在的邏輯，res會找到'o'和'o'之間的長度2，從前面則是"oh"的長度2，後面"m"的長度1

全錯...

---

又陸續改了很多次才成功

```C#
    public class Solution
    {
        public int LengthOfLongestSubstring(string s)
        {
            int res = 0;
            int lastIndex = 0;
            int tempRes = 0;
            Dictionary<char, int> dic = new Dictionary<char, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (dic.ContainsKey(s[i]))
                {
                    int tempLastIndex;
                    dic.TryGetValue(s[i], out tempLastIndex);
                    if (tempLastIndex < lastIndex)
                        tempRes++;
                    else
                    {
                        lastIndex = Math.Max(lastIndex, tempLastIndex);
                        tempRes = i - lastIndex;
                    }
                    res = Math.Max(res, tempRes);
                    dic.Remove(s[i]);
                    dic.Add(s[i], i);
                    lastIndex = Math.Max(tempLastIndex, lastIndex);
                }
                else
                {
                    dic.Add(s[i], i);
                    tempRes++;
                    res = Math.Max(res, tempRes);
                }
            }
            return res;
        }
    }
```

![](https://i.imgur.com/l5PtyPm.png)

成績不太理想，記憶體竟然用了26M，是因為Dictionary很佔空間嗎?



## Better Solutions



### Solution 1

```python
class Solution:
    def lengthOfLongestSubstring(self, s):
        dicts = {}
        maxlength = start = 0
        for i,value in enumerate(s):
            if value in dicts:
                sums = dicts[value] + 1
                if sums > start:
                    start = sums
            num = i - start + 1
            if num > maxlength:
                maxlength = num
            dicts[value] = i
        return maxlength
```



### Solution 2

```C
int lengthOfLongestSubstring(char* s) {
    int count=1;
    int max_count=1;
    int j_start=0;
    
    if(!s[0])
        return 0;
    else if(!s[1])
        return max_count;
    
    for(int i=1; s[i]; i++)
    {
        for(int j=j_start; j<i; j++)
        {
            if(s[i] != s[j])
                count++;
            else
                j_start = j+1;         
        }
        if(max_count < count)
            max_count = count;
        count = 1;
    }
    return max_count;
}
```

號稱8ms解，我的十分之一

