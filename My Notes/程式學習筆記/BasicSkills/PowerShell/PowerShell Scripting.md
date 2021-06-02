# PowerShell Scripting

`PowerShell Scripting` 之於 `PowerShell`就如同 `Batch file` 之於 `Command line`

滿分造句。



## Execution Policy

在開始之前必須先提一下這個東西

Execution Policy 算是微軟的一種保護機制，windows會限制來路不明的PowerShell Scripting。

在Linux 和 MacOS 上 Execution Policy都被設定為`Unrestricted`，且不能更改

可以透過`Get-ExecutionPolicy`指令來確認目前的保護級別

保護層級分為以下四種(節錄自[ITHelp](https://ithelp.ithome.com.tw/articles/10028377))

* `Restricted` ：關閉腳本檔的執行功能，這是預設的設定值。
* `AllSigned` ：只允許執行受信任發行者簽署過的腳本檔。
* `RemoteSigned` ：在本機電腦所撰寫的腳本檔，不需要簽署就可執行；但是從網際網路（例如：email、MSN Messenger）下載的腳本檔就必須經過受信任發行者的簽署才能執行。
* `Unrestricted` ：任何腳本檔皆可被執行，但是於執行網際網路下載的腳本檔時，會先出現警告的提示視窗。



雖然聽說預設的設置是`Restricted`但不曉得為啥我的設置是`RemoteSigned`，可能是微軟後來有變更過設定(?)

```powershell
PS C:\Users\admin\Desktop\powershelltest> Get-ExecutionPolicy
RemoteSigned
```

更正，這是跨平台版本的PowerShell的預設值，Windows PowerShell的預設值是Restricted沒錯





總之，如果Execution Policy不符合需求，可以透過`Set-ExecutionPolicy`指令來進行設定 (需admin權限)

```powershell
Set-ExecutionPolicy Unrestricted
```



更改起來如下

```powershell
PS C:\Windows\system32> Get-ExecutionPolicy
Restricted
PS C:\Windows\system32> Set-ExecutionPolicy RemoteSigned

執行原則變更
執行原則有助於防範您不信任的指令碼。如果變更執行原則，可能會使您接觸到 about_Execution_Policies 說明主題 (網址為
https:/go.microsoft.com/fwlink/?LinkID=135170) 中所述的安全性風險。您要變更執行原則嗎?
[Y] 是(Y)  [A] 全部皆是(A)  [N] 否(N)  [L] 全部皆否(L)  [S] 暫停(S)  [?] 說明 (預設值為 "N"): Y
PS C:\Windows\system32> Get-ExecutionPolicy
RemoteSigned
```



## Variables

其實變數的功能並沒有限定在Scripting才能使用，但我想大部分的應用情境應該都在Scripting中。

只要在文字前方加入`$`字元就代表我要使用變數。

```powershell
$myVar = 12345
```



### `'` 和 `"`的區別

大多數的情況沒有區別，但在使用參數的時候可以看到兩者間的不同

先上範例，假如這是我得腳本內容

```powershell
$myVar = 12345
$myVar
'$myVar'
'`$myVar'
"$myVar"
"`$myVar"
```

執行結果如下

```
12345
$myVar
`$myVar
12345
$myVar
```



簡單來說在`'`裡面寫什麼就顯示什麼

在`"`裡面輸入`$`+變數名稱則會顯示變數的值，但可以透過在前方加入`字元來避免顯示變數值

另外，`"`中還可以透過`$()`來撰寫邏輯

```powershell
$myVar = 12345
"`$myVar*2 = $($myVar*2)"
```

會輸出

```
$myVar*2 = 24690
```



## Parameters

宣告參數使用`Param()`

透過參數我們可以增加腳本的彈性

例如 new_item.ps1

```powershell
Param($Path)
New-Item $Path
Write-Host "File $Path was created"
```

我可以這樣使用它

```powershell
PS C:\Users\admin\Desktop\powershelltest> .\new_item.ps1 -Path "file1.txt"


    目錄: C:\Users\admin\Desktop\powershelltest


Mode                LastWriteTime         Length Name
----                -------------         ------ ----
-a----       2021/6/1  下午 03:19              0 file1.txt
File file1.txt was created


PS C:\Users\admin\Desktop\powershelltest> .\new_item.ps1 -Path "file2.txt"


    目錄: C:\Users\admin\Desktop\powershelltest


Mode                LastWriteTime         Length Name
----                -------------         ------ ----
-a----       2021/6/1  下午 03:20              0 file2.txt
File file2.txt was created


PS C:\Users\admin\Desktop\powershelltest> ls


    目錄: C:\Users\admin\Desktop\powershelltest


Mode                LastWriteTime         Length Name
----                -------------         ------ ----
-a----       2021/6/1  下午 03:19              0 file1.txt
-a----       2021/6/1  下午 03:20              0 file2.txt
-a----       2021/6/1  下午 03:10             68 new_item.ps1
```

