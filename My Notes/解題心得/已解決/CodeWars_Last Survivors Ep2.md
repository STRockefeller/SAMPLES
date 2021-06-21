# CodeWars:Last Survivors Ep.2:20210621:Python

[Reference](https://www.codewars.com/kata/60a1aac7d5a5fc0046c89651/python)



## Question

Substitute two equal letters by the next letter of the alphabet (two letters convert to one):

```
"aa" => "b", "bb" => "c", .. "zz" => "a".
```

The equal letters do *not* have to be adjacent.
Repeat this operation until there are no possible substitutions left.
Return a string.

Example:

```
let str = "zzzab"
    str = "azab"
    str = "bzb"
    str = "cz"
return "cz"
```

#### Notes

- The order of letters in the result is not important.
- The letters `"zz"` transform into `"a"`.
- There will only be lowercase letters.

If you like this kata, check out another one: [Last Survivor Ep.3](https://www.codewars.com/kata/60a2d7f50eee95000d34f414)

## My Solution

6 kyu 系列題的第二題，這次使用的是也很不熟悉的python

題目依然很簡單，不過使用了不熟悉的工具就感受到自己的linq依賴症狀有多嚴重

```python
def next_char(c):
    if c == 'z':
        return 'a'
    else:
        return chr(ord(c) + 1)
def find_duplicate(arr):
    tempArr=[]
    for c in arr:
        tempArr=arr.copy()
        tempArr.remove(c)
        if c in tempArr:
            return [True,c]
    return[False,None]
def replace(arr,c):
    arr.remove(c)
    arr.remove(c)
    arr.append(next_char(c))
    return arr
def last_survivors(string):
    arr = []
    for c in string:
        arr += c
    while True:
        res=find_duplicate(arr)
        if res[0]:
            arr=replace(arr,res[1])
        else:
            break
    return "".join(arr)
```

落落長...



## Better Solutions



### Solution 1

```python
def last_survivors(string):
    ans = list(string); abc = list(map(chr, range(97, 123))) # all letters
    abc.append('a') # append the first letter at last z - a - b ...
    
    for f in range(len(string)):
        for i in ans:
            if ans.count(i) >= 2:
                index = abc.index(i)
                ans.remove(i); ans.remove(i)
                ans.append(abc[index + 1])
            
    return "".join(ans)   
```

原來加上`;`可以把兩行程式碼寫在一起

這個人的寫法就是比較linq的感覺

`for f in range(len(string)):`其中f去紀錄index有點像`for(int i=0;i<string.Length;i++)`這樣

`for i in ans:`抓出輸入字串的每一個字元

`if ans.count(i) >= 2:`判斷是否重複

接著一樣移除重複的項目兩次再`append`合併後的字元



### Solution 2

```python
from re import sub
def last_survivors(s):
    while len(set(s)) != len(s):
        s = sub(r'(.)(.*)\1', lambda x: chr((ord(x.group(1))-96)%26+97) + x.group(2),s)
    return s
```

