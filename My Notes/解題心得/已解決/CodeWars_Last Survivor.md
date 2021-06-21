# CodeWars:Last Survivor:20210621:PowerShell

[Reference](https://www.codewars.com/kata/609eee71109f860006c377d1/solutions/powershell)



## Question

You are given a string of letters and an array of numbers.
The numbers indicate positions of letters that must be removed, in order, starting from the beginning of the array.
After each removal the size of the string decreases (there is no empty space).
Return the only letter left.

Example:

```
let str = "zbk", arr = [0, 1]
    str = "bk", arr = [1]
    str = "b", arr = []
    return 'b'
```

#### Notes

- The given string will never be empty.
- The length of the array is always one less than the length of the string.
- All numbers are valid.
- There can be duplicate letters and numbers.

If you like this kata, check out the next one: [Last Survivors Ep.2](https://www.codewars.com/kata/60a1aac7d5a5fc0046c89651)

## My Solution

7 kyu 系列題的第一題，可能會挑戰看看全系列，嘗試使用PowerShell解題

題目很簡單，但是因為對PowerShell不熟悉所以稍微踩了坑，因此還是記錄一下

解答:

```powershell
function Last-Survivor([string]$letters, [int[]]$numbers)
{
    $lettersList = New-Object System.Collections.Generic.List[char]
    $lettersList.AddRange($letters.ToCharArray())
    foreach($num in $numbers)
    {
        $lettersList.RemoveAt($num)
    }
    return $lettersList[0]
}
```



---

踩坑紀錄

1.使用Array做運算，因為Array是fixed size 所以使用`removeAt()`方法就會報錯

```powershell
以 "1" 引數呼叫 "RemoveAt" 時發生例外狀況: "集合屬於固定大小。"
位於 線路:6 字元:9
+         $lettersArray.RemoveAt($num)
+         ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : NotSpecified: (:) [], MethodInvocationException
    + FullyQualifiedErrorId : NotSupportedException
```

所以為啥要給Array `RemoveAt()`這種方法??



2.使用List做運算，在List建構式中丟入Array，(這我還是不曉得為甚麼不行)

```powershell
function say($sth){return $sth}
function showList ([System.Collections.Generic.List[System.Object]] $toshow){foreach($l in $toshow){say($l)}}
function Last-Survivor([string]$letters, [int[]]$numbers)
{
    $lettersList = [System.Collections.Generic.List[System.Object]]($letters.ToCharArray());
    foreach($num in $numbers)
    {
        say("enter the loop and the index to remove is $num")
        say("the letters are")
        showList($lettersList)
        $lettersList.RemoveAt($num)
        say("after removed, the letters are")
        showList($lettersList)
        say("---")
    }
    
    $res=$lettersList[0]
  return $res
}
```

```powershell
enter the loop and the index to remove is 0
the letters are
z
b
k
after removed, the letters are
---
enter the loop and the index to remove is 1
the letters are
以 "1" 引數呼叫 "RemoveAt" 時發生例外狀況: "索引超出範圍。必須為非負數且小於集合的大小。
參數名稱: index"
位於 線路:13 字元:9
+         $lettersList.RemoveAt($num)
+         ~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : NotSpecified: (:) [], MethodInvocationException
    + FullyQualifiedErrorId : ArgumentOutOfRangeException
 
after removed, the letters are
---
```

一開始展開確認List裡面的內容都正確，執行`RemoveAt()`方法後發現全部的內容都被一起清空了。第二次執行就自然沒東西可以刪除。

## Better Solutions

基本上我的解法完全無法展現PowerShell的特色，與其說是用PowerShell解題，不如說是解完題目後套上PowerShell的皮。

不得不說可以直接看到別人同種程式語言的解題方式實在是code wars最吸引我的一點，多參考別人的作法多學學吧。

### Solution 1

```powershell
function Last-Survivor([string]$letters, [int[]]$numbers)
{
    while ($numbers.Length -gt 0) 
    {
        $i, $numbers = $numbers
        $a = $letters.Substring(0,$i)
        $b = $letters.Substring($i+1)
        $letters = -join($a, $b) 
    }
    
    return $letters[0]
}
```

[StackOverFlow](https://stackoverflow.com/questions/24754822/powershell-remove-item-0-from-an-array)

[MSDN](https://docs.microsoft.com/en-gb/powershell/module/microsoft.powershell.core/about/about_assignment_operators?view=powershell-5.1#assigning-multiple-variables)

[MSDN Blog](https://devblogs.microsoft.com/powershell/powershell-tip-how-to-shift-arrays/)

`$i, $numbers = $numbers`是multiple assignment的做法(我有另外撰寫一篇筆記記錄它)，簡單的說就是`$i=$numbers[0]`然後`$numbers=$numbers[1..($numbers.Count()-1)]`

`$a = $letters.Substring(0,$i)`從index=0開始取長度`$i`的substring

`$b = $letters.Substring($i+1)`從`$i+1`開始往後取的substring

例如以範例的`str = "zbk", arr = [0, 1]` 來說

第一次

`$i`==0

`$a`=="" (空字串)

`$b`=="bk"

第二次

`$i`==1

`$a`=="b"

`$b`==""



### Solution 2

```powershell
function Last-Survivor([string]$letters, [int[]]$numbers) {
  $chars = [System.Collections.ArrayList]::new($letters.ToCharArray())
  foreach ($i in $numbers) {
    $chars.RemoveAt($i)
  }
  return $chars -Join ""
}
```

`ArrayList`結構可以正常使用`RemoveAt()`方法

### Solution 3

```powershell
function Last-Survivor([string]$letters, [int[]]$numbers)
{
  $numbers | ForEach-Object {
    $letters = "$($letters.Substring(0, $PSItem))$($letters.Substring($PSItem + 1))"
  }
  return $letters
}
```

基本上和第一個差不多，不過它使用了pipeline看起來比較有powerShell的風格
