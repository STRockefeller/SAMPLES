# LeetCode:Decoded String at Index:20201221:C#

[Reference](https://leetcode.com/explore/challenge/card/december-leetcoding-challenge/571/week-3-december-15th-december-21st/3572/)



## Question

An encoded string `S` is given. To find and write the *decoded* string to a tape, the encoded string is read **one character at a time** and the following steps are taken:

- If the character read is a letter, that letter is written onto the tape.
- If the character read is a digit (say `d`), the entire current tape is repeatedly written `d-1` more times in total.

Now for some encoded string `S`, and an index `K`, find and return the `K`-th letter (1 indexed) in the decoded string.

 

**Example 1:**

```
Input: S = "leet2code3", K = 10
Output: "o"
Explanation: 
The decoded string is "leetleetcodeleetleetcodeleetleetcode".
The 10th letter in the string is "o".
```

**Example 2:**

```
Input: S = "ha22", K = 5
Output: "h"
Explanation: 
The decoded string is "hahahaha".  The 5th letter is "h".
```

**Example 3:**

```
Input: S = "a2345678999999999999999", K = 1
Output: "a"
Explanation: 
The decoded string is "a" repeated 8301530446056247680 times.  The 1st letter is "a".
```

 

**Constraints:**

- `2 <= S.length <= 100`
- `S` will only contain lowercase letters and digits `2` through `9`.
- `S` starts with a letter.
- `1 <= K <= 10^9`
- It's guaranteed that `K` is less than or equal to the length of the decoded string.
- The decoded string is guaranteed to have less than `2^63` letters.

## My Solution

最直觀的作法是先decode再找index，但是若遇到像example 3這種類型的題目，目標的index在最前端，若將整串字串都解碼完成再找顯得相當沒有效率。

這樣看來邊decode邊找目標反而是最佳解。感覺相當適合遞迴解法。

```C#
public class Solution {
    public string DecodeAtIndex(string S, int K)
    {
        if (S.Length >= K && S.Substring(0, K).All(c => !isDigit(c)))
            return new string(new char[1] { S[K - 1] });
        string toCopy = new string(S.TakeWhile(c => !isDigit(c)).ToArray());
        int count = S.First(c => isDigit(c)) - '0';
        string rest = new string(S.Skip(toCopy.Length + 1).ToArray());
        return DecodeAtIndex(copyString(toCopy, count) + rest, K);
    }
    private bool isDigit(char c) => c >= '0' && c <= '9';
    private string copyString(string str, int count)
    {
        string res = "";
        for (int i = 0; i < count; i++)
            res += str;
        return res;
    }
}
```

進階測試未通過，超時。

測試條件:**"y959q969u3hb22odq595" 222280369**



## Better Solutions

