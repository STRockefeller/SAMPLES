# Shift Arrays

參考[MSDN Blog](https://devblogs.microsoft.com/powershell/powershell-tip-how-to-shift-arrays/)



主要是討論需要將array中的內容物按順序取出的情況



## 將第一個物件複製後，把Array每個元素左移



作法

```powershell
$first = $oldArray[0]

$oldArray = $oldArray[1 .. ($oldArray.count-1)]
```



測試

```powershell
function printArray([object[]] $arr){foreach($items in $arr){$items}}
$oldArray = 1..10
"original array is"
printArray($oldArray)
$first = $oldArray[0]
$oldArray=$oldArray[1..($oldArray.Count-1)]
"first = $first"
"oldArray is:"
printArray($oldArray)
---------------------------------------------------------------------------------------------------------
original array is
1
2
3
4
5
6
7
8
9
10
first = 1
oldArray is:
2
3
4
5
6
7
8
9
10
```



## 使用 PowerShell’s multiple assignment feature



簡化的寫法，可以直接把Array 拆成 第一項和其他

```powershell
$var1,$var2=$array
```

變數分別代表:

`$var1`=`$array[0]`

`$var2`=`$array[1 .. ($array.count-1)]`



```powershell
function printArray([object[]] $arr){foreach($items in $arr){$items}}
$oldArray = 1..10
$first,$rest=$oldArray
"first is $first"
"rest is "
printArray($rest)
--------------------------------------------------------------------------------------
first is 1
rest is 
2
3
4
5
6
7
8
9
10
```



### 如果第一個物件是不需要的，可以使用`$null`來捨棄



```powershell
function printArray([object[]] $arr){foreach($items in $arr){$items}}
$oldArray = 1..10
$null,$rest=$oldArray
"rest is "
printArray($rest)
rest is 
2
3
4
5
6
7
8
9
10
```

