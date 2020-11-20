# CodeWars:Friend or Foe?:20201117:Python

[Reference](https://www.codewars.com/kata/55b42574ff091733d900002f)



## Question

Make a program that filters a list of strings and returns a list with only your friends name in it.

If a name has exactly 4 letters in it, you can be sure that it has to be a friend of yours! Otherwise, you can be sure he's not...

Ex: Input = ["Ryan", "Kieran", "Jason", "Yous"], Output = ["Ryan", "Yous"]

i.e.

```haskell
friend ["Ryan", "Kieran", "Mark"] `shouldBe` ["Ryan", "Mark"]
```

Note: keep the original order of the names in the output.

## My Solution

7 kyu 的題目， 題目本身很簡單沒啥好說的，僅是想記錄下python簡化寫法

```python
def friend(x):
    res = []
    for name in x:
        if len(name)==4:
            res.append(name)
    return res
```



## Better Solutions



### Solution 1

```python
def friend(x):
    return [f for f in x if len(f) == 4]
```

Python菜雞，看到沒看過的寫法都記下來

字面上不難理解，每當`f in x`符合`len(f) == 4`條件時回傳`f`

有點像yield return的概念

```C#
IEnumerable<string> friend(List<string> x)
{
    foreach (string f in x)
        if (f.Length == 4)
           yield return f;
}
```

至於中括弧的用法暫時看不出來，等學會了再回頭補