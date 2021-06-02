# Pipeline

## 建立Pipeline

直立線符號字元 (`|`) 是用於連接 Cmdlet。

 **直立線符號前面的 Cmdlet 輸出，成為直立線符號後面的 Cmdlet 輸入。** 

這樣銜接 Cmdlet 可以建立比單一 Cmdlet 更強大又複雜的陳述式。 此銜接稱為「Pipeline」，可以包含一個或多個直立線符號和 Cmdlet。

建構Pipeline時需要應用下列概念：

- **Pipeline評估**：Pipeline中的 Cmdlet 依特定順序來評估。 藉由了解評估流程，即可進一步了解如何連接兩個或以上的 Cmdlet。
- **協助程式結構**：建立的陳述式越長 (以直立線符號字元分隔)，Pipeline就變得越複雜。 此程序的其中一部分是篩選所需資料。 Cmdlet 協助程式與運算子可以更輕鬆地進行篩選工作。
- **篩選和格式化準則**：套用篩選函式及運算子時，請遵循可靠的篩選和格式化準則。 這些準則有助於更有效率撰寫每個陳述式，產生更容易閱讀及方便使用的結果。

### Pipeline輸入評估

大部分 Cmdlet 都有兩種使用方式：

- 您可只呼叫特定 Cmdlet，並將值指派給必要的參數。
- 您可以在Pipeline中使用 Cmdlet，Pipeline是較長的運算式，而 Cmdlet 操作的輸入通常是呼叫另一個 Cmdlet 的結果。

PowerShell 可讓 Cmdlet 建立者指定 `Accept pipeline input?` 欄位，以此來區別這兩種方法。 此欄位會採用布林值作為值。 如果值為 true，則 PowerShell 接受Pipeline資料。

在學習過程中，您必須瞭解：

- Cmdlet 所採用輸入參數的類型。
- 處理參數的順序。
- 如何提供資料。

瞭解上述各項，才能以有助於解決問題的方式合併適當的 Cmdlet 陳述式。

#### Pipeline中的評估順序

Cmdlet 通常接受多個專用於Pipeline的參數。

您要如何知道系統會優先嘗試使用哪個參數輸入？

假設一個實例。 `Get-Help Get-Process -Full` 命令傳回 `Get-Process` 命令說明區段的詳細清單。 `INPUTS` 區段及 `PARAMETERS` 區段會顯示三個可能的輸入：

- **System.string []**：這個基本類型會連接到參數 `-Name`。
- **System.Int32**：此輸入的參數稱為 `-Id`。
- **System.Diagnostics.Process[]**：此複雜類型與稱為 `-InputObject` 的參數建立關聯。

`PARAMETERS` 區段不只包含這三個參數而已。 但只有這些參數的 `Accept pipeline input?` 欄位設為 true。 參數會納入Pipeline評估中，因為此處只評估接受Pipeline輸入的參數。

PowerShell 依下列順序評估輸入：

步驟 1 - **依值 (依類型)**：首先，PowerShell 嘗試比對輸入與複雜類型。 也就是嘗試「依值」比對。 在上述範例中，PowerShell 評估輸入看起來是否像 `System.Diagnostics.Process[]`。

如果不相符，PowerShell 就前往下一個步驟。

步驟 2 - **依屬性名稱**：接下來，PowerShell 嘗試比對輸入與較簡單的資料類型，亦即 `-Name` 參數或 `-Id` 參數。 在這兩個參數的 `PARAMETERS` 區段中，您會看到下列輸出：

```output
Accept pipeline input?       true (ByPropertyName)
```

此資訊會告訴您參數接受Pipeline輸入，但其也具有 `ByPropertyName` 陳述式。 `ByPropertyName` 陳述式表示參數應會收到物件，其中包含名為 `Name` 或 `Id` 的屬性，以符合 `Get-Process` 輸入參數需求。 因此，物件應該如下列輸出範例所示：

```output
{
   Name: 'a name of a process'
}
```

如何使用 `Get-Process` 及直立線符號來測試評估程序？ 您可呼叫 Cmdlet，該 Cmdlet 會傳回具有所需圖形的物件。 或者，您也可以使用稱為 `pscustomobject` 的結構。 您可以使用 `pscustomobject` 結構來建立自訂 PowerShell 物件。 若要使用此結構，請建構自訂物件，然後傳給 `Get-Process`，如下列 PowerShell 陳述式所示：

PowerShell複製

```powershell
[pscustomobject]@{ Name='name of process' } | Get-Process
```

此陳述式會先建立具有 `Name` 屬性的自訂 PowerShell 物件，以及要指派給該物件的值。 接著，該陳述式會將物件、陳述式以及值與 `Get-Process` Cmdlet 連接。 此結果即為您的處理序清單。

### 協助程式結構

您已經了解如何使用 `Name` 參數，依其顯示名稱尋找特定的處理序。 您也可以使用協助程式 Cmdlet `Where-Object`。 此 Cmdlet 會採用運算子與運算式。 其會一起處理物件的清單並傳回結果，其中所有記錄都符合篩選陳述式。

看看下列範例，其中使用 `Where-Object` 協助程式：

PowerShell複製

```powershell
Get-Process | Where-Object Name -eq name-of-my-process
```

`Where-Object` 協助程式 Cmdlet 可增加使用彈性。 使用此 Cmdlet 建立查詢，以尋找 `Name` 以外的屬性，並使用運算子來協助您更準確比對結果。

#### 運算子

您可在 `Where-Object` Cmdlet 中使用多種運算子。 這些運算子並未與 Cmdlet 繫結。 下列範例示範如何在命令提示字元中使用運算子：

PowerShell複製

```powershell
'a string' -eq 'some other string'
```

在主控台執行此陳述式時，將會評估為 false。

您很可能會使用這些常見的運算子：

- **-eq**：此運算子會檢查其前面的值是否等於後面的值。

- **-gt**：此大於運算子會檢查某個數字是否大於其用於比較的數字。

- **-lt**：此小於運算子會檢查某個數字是否小於其用於比較的數字。

- **-Match**：此運算子會比對正則運算式的值。 例如，您可使用這個運算子尋找所有以字母 `V` 開頭的處理序：

  ```powershell
  Get-Process | Where-Object Name -Match "^V.*"
  ```
  
- **-Contains**：此運算子會檢查集合是否包含特定值。



### 練習

官方提供的指令範例

```powershell
Get-Process | Where-Object CPU -gt 1000 | Sort-Object CPU -Descending | Select-Object -First 3
```

基本上後面看起來就是平常的sql語法，感覺差不多像

```C#
processList.Where(p => p.CPU > 1000).OrderByDescending(p => p.CPU).Take(3);
```



`Get-Process`會列出目前的所有處理程序的資訊，文字量太大這邊就不展示了，不過可以來看看他的member

```powershell
PS C:\Users\admin> Get-Process | Get-Member


   TypeName: System.Diagnostics.Process

Name                       MemberType     Definition
----                       ----------     ----------
Handles                    AliasProperty  Handles = Handlecount
Name                       AliasProperty  Name = ProcessName
NPM                        AliasProperty  NPM = NonpagedSystemMemorySize64
PM                         AliasProperty  PM = PagedMemorySize64
SI                         AliasProperty  SI = SessionId
VM                         AliasProperty  VM = VirtualMemorySize64
WS                         AliasProperty  WS = WorkingSet64
Disposed                   Event          System.EventHandler Disposed(System.Object, System.EventArgs)
ErrorDataReceived          Event          System.Diagnostics.DataReceivedEventHandler ErrorDataReceived(System.Objec...
Exited                     Event          System.EventHandler Exited(System.Object, System.EventArgs)
OutputDataReceived         Event          System.Diagnostics.DataReceivedEventHandler OutputDataReceived(System.Obje...
BeginErrorReadLine         Method         void BeginErrorReadLine()
BeginOutputReadLine        Method         void BeginOutputReadLine()
CancelErrorRead            Method         void CancelErrorRead()
CancelOutputRead           Method         void CancelOutputRead()
Close                      Method         void Close()
CloseMainWindow            Method         bool CloseMainWindow()
CreateObjRef               Method         System.Runtime.Remoting.ObjRef CreateObjRef(type requestedType)
Dispose                    Method         void Dispose(), void IDisposable.Dispose()
Equals                     Method         bool Equals(System.Object obj)
GetHashCode                Method         int GetHashCode()
GetLifetimeService         Method         System.Object GetLifetimeService()
GetType                    Method         type GetType()
InitializeLifetimeService  Method         System.Object InitializeLifetimeService()
Kill                       Method         void Kill()
Refresh                    Method         void Refresh()
Start                      Method         bool Start()
ToString                   Method         string ToString()
WaitForExit                Method         bool WaitForExit(int milliseconds), void WaitForExit()
WaitForInputIdle           Method         bool WaitForInputIdle(int milliseconds), bool WaitForInputIdle()
__NounName                 NoteProperty   string __NounName=Process
BasePriority               Property       int BasePriority {get;}
Container                  Property       System.ComponentModel.IContainer Container {get;}
EnableRaisingEvents        Property       bool EnableRaisingEvents {get;set;}
ExitCode                   Property       int ExitCode {get;}
ExitTime                   Property       datetime ExitTime {get;}
Handle                     Property       System.IntPtr Handle {get;}
HandleCount                Property       int HandleCount {get;}
HasExited                  Property       bool HasExited {get;}
Id                         Property       int Id {get;}
MachineName                Property       string MachineName {get;}
MainModule                 Property       System.Diagnostics.ProcessModule MainModule {get;}
MainWindowHandle           Property       System.IntPtr MainWindowHandle {get;}
MainWindowTitle            Property       string MainWindowTitle {get;}
MaxWorkingSet              Property       System.IntPtr MaxWorkingSet {get;set;}
MinWorkingSet              Property       System.IntPtr MinWorkingSet {get;set;}
Modules                    Property       System.Diagnostics.ProcessModuleCollection Modules {get;}
NonpagedSystemMemorySize   Property       int NonpagedSystemMemorySize {get;}
NonpagedSystemMemorySize64 Property       long NonpagedSystemMemorySize64 {get;}
PagedMemorySize            Property       int PagedMemorySize {get;}
PagedMemorySize64          Property       long PagedMemorySize64 {get;}
PagedSystemMemorySize      Property       int PagedSystemMemorySize {get;}
PagedSystemMemorySize64    Property       long PagedSystemMemorySize64 {get;}
PeakPagedMemorySize        Property       int PeakPagedMemorySize {get;}
PeakPagedMemorySize64      Property       long PeakPagedMemorySize64 {get;}
PeakVirtualMemorySize      Property       int PeakVirtualMemorySize {get;}
PeakVirtualMemorySize64    Property       long PeakVirtualMemorySize64 {get;}
PeakWorkingSet             Property       int PeakWorkingSet {get;}
PeakWorkingSet64           Property       long PeakWorkingSet64 {get;}
PriorityBoostEnabled       Property       bool PriorityBoostEnabled {get;set;}
PriorityClass              Property       System.Diagnostics.ProcessPriorityClass PriorityClass {get;set;}
PrivateMemorySize          Property       int PrivateMemorySize {get;}
PrivateMemorySize64        Property       long PrivateMemorySize64 {get;}
PrivilegedProcessorTime    Property       timespan PrivilegedProcessorTime {get;}
ProcessName                Property       string ProcessName {get;}
ProcessorAffinity          Property       System.IntPtr ProcessorAffinity {get;set;}
Responding                 Property       bool Responding {get;}
SafeHandle                 Property       Microsoft.Win32.SafeHandles.SafeProcessHandle SafeHandle {get;}
SessionId                  Property       int SessionId {get;}
Site                       Property       System.ComponentModel.ISite Site {get;set;}
StandardError              Property       System.IO.StreamReader StandardError {get;}
StandardInput              Property       System.IO.StreamWriter StandardInput {get;}
StandardOutput             Property       System.IO.StreamReader StandardOutput {get;}
StartInfo                  Property       System.Diagnostics.ProcessStartInfo StartInfo {get;set;}
StartTime                  Property       datetime StartTime {get;}
SynchronizingObject        Property       System.ComponentModel.ISynchronizeInvoke SynchronizingObject {get;set;}
Threads                    Property       System.Diagnostics.ProcessThreadCollection Threads {get;}
TotalProcessorTime         Property       timespan TotalProcessorTime {get;}
UserProcessorTime          Property       timespan UserProcessorTime {get;}
VirtualMemorySize          Property       int VirtualMemorySize {get;}
VirtualMemorySize64        Property       long VirtualMemorySize64 {get;}
WorkingSet                 Property       int WorkingSet {get;}
WorkingSet64               Property       long WorkingSet64 {get;}
PSConfiguration            PropertySet    PSConfiguration {Name, Id, PriorityClass, FileVersion}
PSResources                PropertySet    PSResources {Name, Id, Handlecount, WorkingSet, NonPagedMemorySize, PagedM...
Company                    ScriptProperty System.Object Company {get=$this.Mainmodule.FileVersionInfo.CompanyName;}
CPU                        ScriptProperty System.Object CPU {get=$this.TotalProcessorTime.TotalSeconds;}
Description                ScriptProperty System.Object Description {get=$this.Mainmodule.FileVersionInfo.FileDescri...
FileVersion                ScriptProperty System.Object FileVersion {get=$this.Mainmodule.FileVersionInfo.FileVersion;}
Path                       ScriptProperty System.Object Path {get=$this.Mainmodule.FileName;}
Product                    ScriptProperty System.Object Product {get=$this.Mainmodule.FileVersionInfo.ProductName;}
ProductVersion             ScriptProperty System.Object ProductVersion {get=$this.Mainmodule.FileVersionInfo.Product...
```



加上資料篩選`where cpu > 1000`

```powershell
PS C:\Users\admin> Get-Process | ? CPU -gt 1000

Handles  NPM(K)    PM(K)      WS(K)     CPU(s)     Id  SI ProcessName
-------  ------    -----      -----     ------     --  -- -----------
    574      42   482168     569360   1,612.83  24216  18 chrome
```

`?`是`Where-Object`的別名，在`GET-ALIAS`的第一行就可以看到了，感覺很方便所以就把它記起來了



## 使用格式化和篩選

使用 PowerShell 時，基於幾個理由，「篩選」和「格式化」是很重要的概念，務必瞭解。 首先，您想建立管線來產生您要的結果。 其次，您希望做得有效率，包括如何從網路上提取資料，以及如何確保結果有用。

### 向左篩選

在管線陳述式中，「向左篩選」表示儘早篩選出所要的結果。 您可以將「向左」視為「提早」，因為 PowerShell 陳述式是由左至右執行。 其概念是要確保您操作的資料集越小越好，以讓陳述式快速且有效率。 當命令由較大的資料存放區所支援，或您透過網路傳回結果時，此準則就真的發揮作用。

以下列陳述式為例：

PowerShell複製

```powershell
Get-Process | Select-Object Name | Where-Object Name -eq name-of-process
```

此陳述式會先擷取電腦上的所有處理序。 最後，該陳述式會將回應格式化，並只列出 `Name` 屬性。 此陳述式需要操作所有處理序、嘗試格式化回應，最後還要篩選，所以並不遵循「向左篩選」準則。

建議先篩選再格式化，如下列陳述式所示。

PowerShell複製

```powershell
Get-Process | Where-Object Name -eq name-of-process | Select-Object Name
```

一般而言，提供篩選的 Cmdlet 會比使用 `Where-Object` 更有效率。 以下是上述陳述式的較具效率版本：

PowerShell複製

```powershell
Get-Process -Name name-of-process | Select-Object Name
```

在此版本中，參數 `-Name` 會執行篩選。

### 向右格式化

「向左篩選」表示盡可能「提早」篩選，而「向右格式化」卻表示在陳述式中盡可能「延遲」格式化。 最常用來格式化輸出的 Cmdlet 是 `Format-Table` 和 `Format-List`。 根據預設，大部分的 Cmdlet 會將輸出格式化為資料表。 如果要讓輸出分欄顯示屬性，請使用 `Format-List` Cmdlet 將輸出重新格式化為清單。