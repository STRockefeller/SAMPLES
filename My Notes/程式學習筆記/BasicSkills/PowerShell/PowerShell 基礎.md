# PowerShell 基礎



## 關於PowerShell

根據[微軟文件](https://docs.microsoft.com/zh-tw/learn/modules/introduction-to-powershell/2-what-is-powershell)對於PowerShell的簡介。

PowerShell 由兩個部分所組成，**command-line shell**與**scripting language**。

其以架構的形式為開頭，可將 Windows 中的管理工作自動化。 PowerShell 已成長為可用於許多類型工作的跨平台工具。

命令列殼層缺乏可以在其中使用滑鼠與圖形元素互動的圖形化介面。 相反地，您可以在電腦主控台中鍵入文字命令。 以下是使用主控台的一些優點：

- 與主控台互動通常比使用圖形化介面更快。
- 在主控台中，您可以執行批次的命令，因此其非常適用於持續整合管線的工作自動化。
- 您可以使用主控台與雲端資源及其他資源互動。
- 您可以將命令與指令碼儲存在文字檔中，並使用原始檔控制系統。 這可能是最大的優點之一，因為您的命令可重複且可稽核。 在許多系統中 (尤其是政府系統)，所有項目都必須經過追蹤、評估或「稽核」。 稽核涵蓋了從資料庫變更到指令碼所做變更的所有項目。



PowerShell 與傳統的殼層共用一些功能：

- **Built-in help system**：大部分的殼層都有某種說明系統，您可以在其中深入了解命令。 例如，您可以了解命令的作用及其支援的參數。 PowerShell 中的說明系統能提供命令的相關資訊，以及與線上說明文章整合。
- **Pipeline**：在傳統的殼層中，管線用於依序執行許多命令。 一個命令的輸出就是下一個命令的輸入。 PowerShell 會像傳統的殼層一樣實作此概念，但又因其會在文字上的物件運作而有所不同。 
- **Aliases**：別名是可用於執行命令的替代名稱。 PowerShell 支援使用一般別名，例如 `cls` (清除螢幕) 與 `ls` (列出檔案)。 因此，新的使用者可以使用他們對其他架構的知識，而不一定要記住常見命令的 PowerShell 名稱。



PowerShell 因幾種方式而與傳統的命令列殼層有所不同：

- **PowerShell 會在文字上的物件運作。** 在命令列殼層中，您必須執行其輸出與輸入可能不同的指令碼。 因此，您最後會花時間將輸出格式化，以及將需要的資料解壓縮。 相較之下，在 PowerShell 中，您可以使用物件作為輸入與輸出。 這表示您會花較少的時間格式化及解壓縮。

- **PowerShell 具有 Cmdlet。** PowerShell 中的命令稱為 Cmdlet。 不同於許多其他的殼層環境，在 PowerShell 中，Cmdlet 建置於通用執行階段上，而不是個別的可執行檔。 此特性提供在參數剖析與管線行為中一致的體驗。

  Cmdlet 通常會採用物件輸入並傳回物件。 PowerShell 中的核心 Cmdlet 建置於 .NET Core 中，而且是開放原始碼。 您可以使用來自社群與其他來源的更多 Cmdlet、指令碼與函式擴充 PowerShell。 或者，您也可以在 .NET Core 或 PowerShell 中建置自己的 Cmdlet。

- **PowerShell 有許多類型的命令。** PowerShell 中的命令可以是原生可執行檔、Cmdlet、函式、指令碼或別名。 您執行的每個命令都屬於這些類型的其中一種。 因為 Cmdlet 是一種命令類型，所以「命令」與 *Cmdlet* 兩個字通常會交互使用。





## Hot Key

節錄自[ITHELP](https://ithelp.ithome.com.tw/articles/10027481)

**上方向鍵**
按一下**上方向鍵**，就會回復到**最後一次**的指令歷程。所以如果指令歷程裡已經有 5 個指令，按下 3 次**上方向鍵**，就會回復到倒數第 3 次的指令。
如果還一直按下**上方向鍵**的話，就會回復到第 1 個指令。

**下方向鍵**
跟**上方向鍵**反向，也就是說：按一下**下方向鍵**就會往後一個指令歷程。
一直按下**下方向鍵**，會停在最後一次所輸入的指令。

**PgUp**
顯示第 1 個指令。

**PgDn**
顯示最後 1 個指令。

**左方向鍵**
在命令列中，將游標往左移動一個字元。通常是為了刪除或插入字元時，才會用到這個熱鍵。

**右方向鍵**
在命令列中，將游標往右移動一個字元。同樣也是為了刪除或插入字元時，才會用到。

**Home**
將游標移到命令列的最前面，以便插入字元。

**End**
將游標移到命令列的最後面，以便刪除字元或按下 Enter 鍵來執行指令。

**Ctrl + 左方向鍵**
在命令列中，將游標往左移動一個**「字」**。例如：命令列中已經有 dir c:\，且游標停在 \ 之後，此時按下 **Ctrl + 左方向鍵**，游標就會移到 c 的前面。

**Ctrl + 右方向鍵**
在命令列中，將游標往右移動一個**「字」**。

**Ctrl + C**
取消目前的指令，並自動換到新的一行，以便輸入新的指令。

**F1**
每按 1 次 **F1**，就會顯示上次指令的 1 個字元。例如：上次輸入的指令是 get-help，所以按 1 次 **F1**，會顯示 g；再按 1 次 **F1**，會顯示 ge。以此類推，按下 8 次**F1**，就顯示 get-help。

**F2**
按下 **F2** 鍵時，Windows PowerShell 就將上次指令的內容複製到按下 **F2** 之後，再輸入的字元（不包含該字元）為止。
例如：上次的指令是 dir c:\
先按下 **F2**，接著按下 :，就會顯示 dir c

在按下 **F2** 鍵之後，卻不想輸入字元，可按下 Enter 就可讓那個方塊消失。

**F3**
顯示上次的指令。這跟按 1 下 **上方向鍵** 是一樣的結果，但是要注意的是，再多按幾次 **F3**，是不會顯示前幾個指令，總之 **F3** 就只顯示上次的指令。

**F4**
從目前由游標所在位置開始（包含該字元）刪除按下 **F4** 後，所輸入的字元（不包含該字元）為止。
例如：命令列中所顯示的指令是 dir c:\，游標停在 i。
按下 **F4**，接著按下 \，就會顯示 d\

**F4** 有個好用的功能，那就是如果要刪除游標之後所有的字元，可以直接按下 Enter 鍵。

**F5**
跟 **上方向鍵** 一樣，每按 1 下 **F5** 就顯示前 1 個指令。

**F7**
直接顯示指令歷程清單，方便我們直接透過**上方向鍵**、**下方向鍵**、**PgUp**、**PgDn**來選取要執行的指令，選定之後，就按下 Enter 鍵即可執行。
如果不是要直接執行所選取的指令，而是要修改的話，於選定之後，按下**左方向鍵**或**右方向鍵**，然後加以修改，於修改完畢之後，按下 Enter 鍵即可執行。
如果不要選擇任何指令的話，只要按下 Esc 就可以關閉指令歷程清單。

**F8**
如果命令列中，沒有任何指令，那麼按下**F8**就會回復到**最後一次**的指令。
如果命令列已經輸入了部分指令，按下**F8**就會顯示**與已輸入的部分指令相符**的指令；再按一次 **F8**，就再顯示上一個**與已輸入的部分指令相符**的指令。

**F9**
這個要搭配按下 **F7** 之後所顯示的指令歷程清單一起使用，先看一下所要執行的指令之編號是幾號，然後按下 Esc 關閉指令歷程清單，接著按下 **F9**，輸入編號，再按下 Enter 鍵即可。
如果不要執行的話，只要按下 Esc 就可以了。

**tab**

有時候我們可以只知道 Windows PowerShell 指令的前幾個字，卻忘記完整的指令是什麼，這時不用到處去搬救兵，只要按下超好用的**「自動輸入完成」**Tab 鍵就可以了。
在 Windows PowerShell 裡，**「自動輸入完成」**無所不在，除了跟 cmd.exe 裡的一樣，就是可以輸入部分的檔案名稱或是目錄名稱，然後按下 **Tab 鍵**就會自動列出第一個相符的檔案或目錄。再按一次 **Tab 鍵**，就會列出第 2 個相符的檔案或目錄。以此類推，所以按下多次的 **Tab 鍵** 之後，就會回復列出第一個相符的檔案或目錄。

如果按得太快，過頭了，只要按下 **Shift + Tab 鍵** 就會往前列出相符的檔案或目錄。

Windows PowerShell 的**「自動輸入完成」**也可以使用萬用字元：
PS C:\> cd c:\pro*<tab>
PS C:\> cd 'C:\Program Files'

厲害的是連在參數中，都可以使用**「自動輸入完成」**：
PS C:\> dir 'C:\Program Files' | Out-File d:\files.txt -a<tab>
PS C:\> dir 'C:\Program Files' | Out-File d:\files.txt -Append



## Cmdlet

*Cmdlet* (發音為 "command-let") 是已編譯的命令。 

Cmdlet 可以在 .NET 或 .NET Core 中開發，並在 PowerShell 中叫用為命令。 PowerShell 安裝中有數千個 Cmdlet 可供使用。

Cmdlet 是根據「動詞-名詞」命名標準命名。 此模式可協助您了解其作用與搜尋方式。 也可協助 Cmdlet 開發人員建立一致的名稱。 您可以使用 `Get-Verb` Cmdlet 查看已核准的動詞清單。 動詞會以活動類型與功能組織。

以下是執行 `Get-Verb` 輸出的一部分：

```output
Verb        AliasPrefix Group          Description
----        ----------- -----          -----------
Add         a           Common         Adds a resource to a container, or atta…
Clear       cl          Common         Removes all the resources from a contai…
```

此清單會顯示動詞與其描述。 Cmdlet 開發人員應使用已核准的動詞，並確定動詞描述符合其 Cmdlet 的函式。

三個核心 Cmdlet 能讓您深入了解存在著哪些 Cmdlet，與其可執行的動作：

- **Get-Command**：`Get-Command` Cmdlet 會列出您系統上所有可用的 Cmdlet。 篩選清單以快速找出您需要的命令。
- **Get-Help**：使用 `Get-Help` 核心 Cmdlet 叫用內建的說明系統。 或是使用別名 `help` 命令叫用 `Get-Help`，但透過將回應分頁改善閱讀體驗。
- **Get-Member**：當您呼叫命令時，回應會是包含許多屬性的物件。 使用 `Get-Member` 核心 Cmdlet 向下鑽研該回應並深入了解。

### Get-Command

當您在殼層中執行 `Get-Command` Cmdlet 時，會取得 PowerShell 中所安裝每個命令的清單。 因為安裝了數千個命令，所以您需要一種方式以篩選回應，以便快速尋找需要的命令。

若要篩選清單，請記住 Cmdlet 的「動詞-名詞」命名標準。 例如，在 `Get-Random` 命令中，`Get` 是動詞，而 `Random` 是名詞。 在您想要的命令中，使用旗標將動詞或名詞設為目標。 您指定的旗標需要字串形式的值。 您可以將模式比對字元新增至該字串，以確定旗標值的表示，例如，該值應該以特定字串開頭或結尾。

這些範例將說明如何使用旗標篩選命令清單：

- **-Noun**：`-Noun` 旗標會將與名詞相關的命令名稱部分設為目標。 換言之，這會將連字號 (`-`) 後面的所有名稱設為目標。 以下是命令名稱的一般搜尋：

  PowerShell複製

  ```powershell
  Get-Command -Noun a-noun*
  ```

  此命令會搜尋其名詞部分以 `a-noun` 為開頭的所有 Cmdlet。

- **-Verb**：`-Verb` 旗標會將與動詞相關的命令名稱部分設為目標。 您可以將 `-Noun` 旗標與 `-Verb` 旗標合併，建立更詳細的搜尋查詢與類型。 以下為範例：

  PowerShell複製

  ```powershell
  Get-Command -Verb Get -Noun a-noun*
  ```

  現在您已縮小搜尋範圍，指定動詞部分必須符合 `Get`，而名詞部分必須符合 `a-noun`。

### Get-Help

您現在已了解如何使用 `Get-Command` 尋找您需要的命令。 此時，您可能會想要深入了解命令的作用，以及呼叫的各種方式。 若要深入了解您選擇的命令，請使用 `Get-Help` 核心 Cmdlet。 一般來說，您會透過以名稱指定，以及新增 `-Name` 旗標的方式叫用 `Get-Help` Cmdlet，該旗標包含您想要了解的 Cmdlet 名稱。 以下為範例：

PowerShell複製

```powershell
Get-Help -Name name-of-command
```

---

譬如我想知道`get-help`指令是怎麼運作的

```powershell
PS C:\Users\admin> get-help -name get-help

名稱
    Get-Help

語法
    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]


別名
    無


註解
    Get-Help 在此電腦上找不到此 cmdlet 的說明檔案。它只顯示部分說明。
        -- 若要下載並安裝包含此 cmdlet 之模組的說明檔案，請使用 Update-Help。
        -- 若要線上檢視此 cmdlet 的說明主題，請輸入: "Get-Help Get-Help -Online" 或
           移至 https://go.microsoft.com/fwlink/?LinkID=113316。
```



這邊不知道為什麼我更新動作一直無法完成 `UICulture`不論是設定`zh-TW` `en-US`還是不設定都一樣

```powershell
PS C:\Users\admin> Update-Help -UICulture en-US
Update-Help : 無法更新下列模組的說明:
'AppBackgroundTask, AppLocker, AppvClient, Appx, AssignedAccess, BitLocker, BitsTransfer, BranchCache, CimCmdlets, Conf
igCI, Defender, DirectAccessClientComponents, Dism, DnsClient, EventTracingManagement, International, iSCSI, ISE, Kds,
Microsoft.PowerShell.Archive, Microsoft.PowerShell.Core, Microsoft.PowerShell.Diagnostics, Microsoft.PowerShell.Host, M
icrosoft.PowerShell.LocalAccounts, Microsoft.PowerShell.Management, Microsoft.PowerShell.ODataUtils, Microsoft.PowerShe
ll.Security, Microsoft.PowerShell.Utility, Microsoft.WSMan.Management, MMAgent, MsDtc, NetAdapter, NetConnection, NetEv
entPacketCapture, NetLbfo, NetNat, NetQos, NetSecurity, NetSwitchTeam, NetTCPIP, NetworkConnectivityStatus, NetworkSwit
chManager, NetworkTransition, PackageManagement, PcsvDevice, PKI, PnpDevice, PowerShellGet, PrintManagement, ProcessMit
igations, Provisioning, PSDesiredStateConfiguration, PSScheduledJob, PSWorkflow, PSWorkflowUtility, ScheduledTasks, Sec
ureBoot, SmbShare, SmbWitness, StartLayout, Storage, TLS, TroubleshootingPack, TrustedPlatformModule, UEV, VpnClient, W
dac, Whea, WindowsDeveloperLicense, WindowsErrorReporting, WindowsSearch, WindowsUpdate'
存取被拒。命令無法更新 Windows PowerShell 核心模組的說明主題，或 $pshome\Modules 目錄中任何模組的說明主題。若要更新這些
說明主題，請使用 [以系統管理員身分執行] 命令啟動 Windows PowerShell，然後嘗試重新執行 Update-Help。
位於 線路:1 字元:1
+ Update-Help -UICulture en-US
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : InvalidOperation: (:) [Update-Help]，Exception
    + FullyQualifiedErrorId : UpdatableHelpSystemRequiresElevation,Microsoft.PowerShell.Commands.UpdateHelpCommand

Update-Help : 無法更新具有 UI 文化特性 {en-US} 之模組 'ConfigDefender, PSReadline, WindowsUpdateProvider' 的說明: 無法
針對 UI 文化特性 en-US 擷取 HelpInfo XML 檔案。請確定模組資訊清單中的 HelpInfoUri 屬性是有效的，或檢查您的網路連線，然
後再試一次該命令。
位於 線路:1 字元:1
+ Update-Help -UICulture en-US
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : ResourceUnavailable: (:) [Update-Help], Exception
    + FullyQualifiedErrorId : UnableToRetrieveHelpInfoXml,Microsoft.PowerShell.Commands.UpdateHelpCommand

Update-Help : 無法更新具有 UI 文化特性 {en-US} 之模組 'Microsoft.PowerShell.Operation.Validation' 的說明: 拒絕存取路徑
'C:\Program Files\WindowsPowerShell\Modules\Microsoft.PowerShell.Operation.Validation\1.0.1\en-US'。
位於 線路:1 字元:1
+ Update-Help -UICulture en-US
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : InvalidOperation: (:) [Update-Help]，Exception
    + FullyQualifiedErrorId : UnknownErrorId,Microsoft.PowerShell.Commands.UpdateHelpCommand
```

改用系統管理員權限執行也沒有用

```powershell
PS C:\Windows\System32> Update-Help                                                                                     Update-Help: Failed to update Help for the module(s) 'ConfigDefender, PSReadline, WindowsUpdateProvider' with UI culture(s) {zh-TW} : One or more errors occurred. (Response status code does not indicate success: 404 (The specified blob does not exist.).).
English-US help content is available and can be installed using: Update-Help -UICulture en-US.
PS C:\Windows\System32> Update-Help -UICulture en-US
Update-Help: Failed to update Help for the module(s) 'ConfigDefender, PSReadline, WindowsUpdateProvider' with UI culture(s) {en-US} : One or more errors occurred. (Response status code does not indicate success: 404 (The specified blob does not exist.).).
English-US help content is available and can be installed using: Update-Help -UICulture en-US.
```



#### 更新說明

根據預設，新版本的 PowerShell 不會包含說明系統。 第一次執行 `Get-Help` 時，系統會要求您安裝說明檔。 您也可以執行 `Update-Help` Cmdlet 以安裝說明檔。 因為對 `Update-Help` 的呼叫會下載許多說明檔，所以根據預設，每天只能擷取一次命令。 您可以使用 `-Force` 旗標覆寫此擷取行為。

相較於 Linux 或 macOS，您可以在 Windows 上以不同的方式更新說明檔。 此程序的不同之處，在於當您執行 `Update-Help` Cmdlet 時，會透過比對電腦的文化特性 (Culture)，在網際網路上擷取說明檔。 Windows 上已安裝文化特性 (Culture)，但 Linux 與 macOS 卻沒有。 因此，當您在 Linux 與 macOS 上更新說明檔時，必須指定文化特性 (Culture)。

以下是範例命令：

```powershell
Update-Help -UICulture en-US -Verbose
```

此命令指定 `-UICulture` 旗標。 該旗標會為其提供 `en-US` 值，這會擷取 US-English 說明檔。 若要在 macOS 或 Linux 上更新說明檔，請使用對應到您電腦文化特性 (Culture) 的文化特性 (Culture)。

#### 探索說明區段

當您在 Cmdlet 上叫用 `Get-Help` 時，會傳回說明頁面。 此頁面包含許多區段。 您可能會看到這些常見的區段：

- **NAME**：此區段提供命令的名稱。
- **SYNTAX**：此區段將說明使用旗標 (有時也允許參數) 的組合呼叫命令的方式。
- **ALIASES**：此區段列出命令的所有別名。 別名是命令的不同名稱，可用於叫用命令。
- **REMARKS**：此區段提供要執行哪些命令的資訊，以取得此命令的更多協助。
- **PARAMETERS**：此區段提供有關參數的詳細資料。 其中列出參數的類型、較長的描述，以及可接受的值 (如適用)。

#### 篩選說明回應

若您不想要顯示完整的說明頁面，請將旗標新增至 `Get-Help` 命令，以縮小回應範圍。 以下是一些您可以使用的旗標：

- **完整**：此旗標會傳回詳細的說明頁面。 該頁面會指定您無法在標準回應中取得的資訊，例如參數、輸入與輸出。
- **Detailed**：此旗標的回應看起來像標準回應，但其包含參數的區段。
- **範例**：此旗標只會傳回範例 (如果有的話)。
- **Online**：此旗標會開啟您命令的網頁。
- **Parameter**：此旗標需要參數名稱作為引數。 其會列出特定參數的屬性。

例如，您可以使用下列命令，只傳回說明頁面的 "Examples" 區段。

```powershell
Get-Help Get-FileHash -Examples
```

#### 改善閱讀體驗

`Get-Help` 命令會傳回整個說明頁面。 此頁面可能無法提供最佳的閱讀體驗。 您可能必須捲動至您感興趣的部分。 較佳的方法是使用 `help` 別名。 `help` 別名會將 `Get-Help` 傳送至函式中，以確定您的輸出可以逐行閱讀。 這也讓回應可逐頁閱讀。

### Get-Member

當 Cmdlet 執行時，其會傳回物件。 當您叫用 Cmdlet 時，您看到的回應都已格式化，而且不一定代表回應的所有可用資訊。 若要深入了解，請使用 Cmdlet `Get-Member` 檢查物件。

`Get-Member` Cmdlet 的目的是為了在您執行的命令上方進行「傳送」，以便篩選輸出。 `Get-Member` 的一般命令列叫用如下列範例所示：

```bash
Get-Process -Name name-of-process | Get-Member
```

此命令會先呼叫 `Get-Process` 來產生物件結果。 該結果會使用直立線符號 (`|`) 作為輸入傳遞給 `Get-Member`。 傳遞輸入後，您會取得包含 `Name`、`MemberType` 及 `Definition` 資料行的資料表結果， 以及傳回物件的類型。

#### 依類型搜尋

回應的第一行 (執行 `Get-Member` 命令) 是傳回物件的類型。 當您知道類型後，即可搜尋在相同類型上運作的其他 Cmdlet。 探索這些相關命令，以便在工作領域中快速增長您的知識。

假設您叫用的 PowerShell 命令會列出特定處理序其所有成員。 其結果的前幾個資料列會類似下列輸出：

```output
  TypeName: System.Diagnostics.Process

Name                       MemberType     Definition
----                       ----------     ----------
Handles                    AliasProperty  Handles = Handlecount
```

第一個資料列表示其類型為 `System.Diagnostics.Process`。 使用此類型作為搜尋引數來尋找其他使用此類型的 Cmdlet。 以下是範例命令：

```powershell
Get-Command -ParameterType Process
```

其結果是一份在此類型上運作的 Cmdlet 清單。 透過使用 `Get-Member` 及學習如何解譯 PowerShell 的結果，您即可逐步地深入了解 PowerShell。



#### 使用 Select-Object 篩選 Get-Member 結果

當執行 `Get-Member` 時，其結果可能會「相當冗長」。 亦即，系統會傳回許多資料列。 其物件可能具有事件與方法之類的屬性。 若想讓結果不要那麼冗長，您可篩選特定資料行，並同時決定要顯示哪些資料行。 請記住，所傳回答案已經是回應中所有資料行的子集。

請查看包含許多資料行的 `Get-Member` 回應。 透過引進 Cmdlet `Select-Object`，即可選擇要在回應中顯示哪些資料行。 此命令需要以逗號分隔的資料行名稱清單或例如星號 (`*`) 此萬用字元來表示所有資料行。

當在 `Select-Object Name, MemberType` 的內容中使用命令 `Select-Object` 時，只需指定所要的資料行即可。 在此情況下，這些資料行為 `Name` 及 `MemberType`。 此篩選模式會傳回輸出，該輸出會包含較少的資料行。 篩選結果的範例如下：

```powershell
Name                           MemberType
----                           ----------
Handles                     AliasProperty
```

您也可以依資料列篩選回應。 例如，您可使用旗標 `-MemberType Method` 指定您想要其成員類型為方法的資料列。 例如，如果想要找出並執行特定的方法，您可能只想要顯示特定的資料列。



---

我的練習

拿上面用過的`get-help`指令的結果來操作

```powershell
PS C:\Users\admin> get-help -name get-help

名稱
    Get-Help

語法
    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]

    Get-Help [[-Name] <string>]  [<CommonParameters>]


別名
    無


註解
    Get-Help 在此電腦上找不到此 cmdlet 的說明檔案。它只顯示部分說明。
        -- 若要下載並安裝包含此 cmdlet 之模組的說明檔案，請使用 Update-Help。
        -- 若要線上檢視此 cmdlet 的說明主題，請輸入: "Get-Help Get-Help -Online" 或
           移至 https://go.microsoft.com/fwlink/?LinkID=113316。
```



使用`get-member`

```powershell
PS C:\Users\admin> get-help -name get-help | Get-Member


   TypeName: ExtendedCmdletHelpInfo

Name                     MemberType   Definition
----                     ----------   ----------
Equals                   Method       bool Equals(System.Object obj)
GetHashCode              Method       int GetHashCode()
GetType                  Method       type GetType()
ToString                 Method       string ToString()
alertSet                 NoteProperty object alertSet=null
aliases                  NoteProperty string aliases=無...
Category                 NoteProperty string Category=Cmdlet
CommonParameters         NoteProperty bool CommonParameters=True
Component                NoteProperty object Component=null
description              NoteProperty object description=null
details                  NoteProperty ExtendedCmdletHelpInfo#details details=@{name=Get-Help; noun=Help; verb=Get}
examples                 NoteProperty object examples=null
Functionality            NoteProperty object Functionality=null
inputTypes               NoteProperty ExtendedCmdletHelpInfo#inputTypes inputTypes=@{inputType=}
ModuleName               NoteProperty string ModuleName=Microsoft.PowerShell.Core
Name                     NoteProperty string Name=Get-Help
nonTerminatingErrors     NoteProperty string nonTerminatingErrors=
parameters               NoteProperty ExtendedCmdletHelpInfo#parameters parameters=@{parameter=System.Object[]}
PSSnapIn                 NoteProperty PSSnapInInfo PSSnapIn=Microsoft.PowerShell.Core
relatedLinks             NoteProperty ExtendedCmdletHelpInfo#relatedLinks relatedLinks=@{navigationLink=System.Manag...
remarks                  NoteProperty string remarks=Get-Help 在此電腦上找不到此 cmdlet 的說明檔案。它只顯示部分說明...
returnValues             NoteProperty ExtendedCmdletHelpInfo#returnValues returnValues=@{returnValue=}
Role                     NoteProperty object Role=null
Synopsis                 NoteProperty string Synopsis=...
Syntax                   NoteProperty ExtendedCmdletHelpInfo#syntax Syntax=@{syntaxItem=System.Object[]}
WorkflowCommonParameters NoteProperty bool WorkflowCommonParameters=False
xmlns:command            NoteProperty string xmlns:command=http://schemas.microsoft.com/maml/dev/command/2004/10
xmlns:dev                NoteProperty string xmlns:dev=http://schemas.microsoft.com/maml/dev/2004/10
xmlns:maml               NoteProperty string xmlns:maml=http://schemas.microsoft.com/maml/2004/10
```

`Select-Object Name`

```powershell
PS C:\Users\admin> get-help -name get-help | Get-Member |Select-Object Name

Name
----
Equals
GetHashCode
GetType
ToString
alertSet
aliases
Category
CommonParameters
Component
description
details
examples
Functionality
inputTypes
ModuleName
Name
nonTerminatingErrors
parameters
PSSnapIn
relatedLinks
remarks
returnValues
Role
Synopsis
Syntax
WorkflowCommonParameters
xmlns:command
xmlns:dev
xmlns:maml
```

備註`|`運算子用於建立pipeline，我會在後面的筆記中詳談。



## Commands

指令這種東西講不完，就挑一些重點說明一下吧

### $PSVersionTable

確認PowerShell版本，執行結果如下

```powershell
PS C:\Users\admin> $PSVersionTable

Name                           Value
----                           -----
PSVersion                      5.1.18362.752
PSEdition                      Desktop
PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0...}
BuildVersion                   10.0.18362.752
CLRVersion                     4.0.30319.42000
WSManStackVersion              3.0
PSRemotingProtocolVersion      2.3
SerializationVersion           1.1.0.1
```



值得一提的是這個結果為**物件輸出**，也就是我們可以透過`.`單獨調閱個別屬性的資料。

```powershell
PS C:\Users\admin> $PSVersionTable.PSVersion

Major  Minor  Build  Revision
-----  -----  -----  --------
5      1      18362  752
```



```powershell
PS C:\Users\admin> $PSVersionTable.PSVersion.Major
5
```



#### 關於 PowerShell 的版本

這邊補充說明一下，微軟現在有兩種PowerShell同時在更新

分別是Windows作業系統內建的Windows PowerShell

以及跨平台使用的PowerShell

兩邊的版本號是不一樣的，我先前以為我的Windows PowerShell版本是舊版跑去github下載新版本安裝，結果安裝到跨平台的PowerShell

在MSDN中有說明到

> PowerShell 7 會分別從 Windows PowerShell 安裝到目錄。 如此一來，您就能同時安裝 PowerShell 7 與 Windows PowerShell 5.1。 針對 PowerShell Core 6.x，PowerShell 7 會就地升級並移除 PowerShell Core 6.x。

事實上我安裝完成後確實同時存在兩種PowerShell



現在(2021/06)我所使用的PowerShell版本分別是

Windows PowerShell

```powershell
PS C:\Windows\system32> $PSVersionTable.PSVersion

Major  Minor  Build  Revision
-----  -----  -----  --------
5      1      18362  752     
```

PowerShell

```powershell
PS C:\Users\admin\Desktop\powershelltest> $PSVersionTable.PSVersion

Major  Minor  Patch  PreReleaseLabel BuildLabel
-----  -----  -----  --------------- ----------
7      1      3
```



至於兩者的區別，目前看來是幾乎相同

於部分指令可能有些不同，如`Get-WmiObject`指令只有在Windows PowerShell可以作用



### Get-Alias

查看別名，PowerShell整合了很多過去DOS的指令以及linux系統的指令，都被編入alias中，讓我們可以透過熟悉的語法完成熟悉的操作

他自身也有一個alias `gal`

```powershell
CommandType     Name                                               Version    Source
-----------     ----                                               -------    ------
Alias           gal -> Get-Alias
```



#### 進階操作

**Get aliases by name**

```powershell
Get-Alias -Name gp*, sp* -Exclude *ps
```

**Get aliases for a cmdlet**

```powershell
Get-Alias -Definition Get-ChildItem
```

**Get aliases by property**

```powershell
Get-Alias | Where-Object {$_.Options -Match "ReadOnly"}
```

**Get aliases by name and filter by beginning letter**

```powershell
Get-Alias -Definition "*-PSSession" -Exclude e* -Scope Global
```



### Get-ChildrenItem

不多說先上alias就懂了 

    PS C:\Users\admin\Desktop\powershelltest> Get-Alias -Definition Get-ChildItem
    
    CommandType     Name                                               Version    Source
    -----------     ----                                               -------    ------
    Alias           dir -> Get-ChildItem
    Alias           gci -> Get-ChildItem
    Alias           ls -> Get-ChildItem

`ls` `dir` `gci` 看到前兩個alias就知道他是甚麼了

執行起來大概長這樣

```powershell
PS C:\Users\admin\Desktop\powershelltest> Get-ChildItem

    Directory: C:\Users\admin\Desktop\powershelltest

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---          2021/6/1 上午 09:20          25633 ls.txt
-a---         2021/5/31 下午 04:47            185 new_item.ps1
-a---          2021/6/1 上午 08:54            361 pwd_result.txt
```



不過，僅此而已的話就沒必要記錄在筆記裏頭了，加碼來學習一下前面的`Mode`欄位`-a---`是什麼意思吧

其實只要透過指令`Get-Help Get-ChildItem -Examples`就能找到答案

```
d - Directory
a - Archive
r - Read-only
h - Hidden
s - System
l - Reparse point, symlink, etc.
```



往下尋找使用參數 --recurse

Example

```powershell
PS C:\Users\admin\Desktop\powershelltest> gci

    Directory: C:\Users\admin\Desktop\powershelltest

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d----         2021/7/12 下午 01:21                dsad
-a---         2021/6/28 下午 03:10            510 test20210622.ps1

PS C:\Users\admin\Desktop\powershelltest> gci -Recurse

    Directory: C:\Users\admin\Desktop\powershelltest

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
d----         2021/7/12 下午 01:21                dsad
-a---         2021/6/28 下午 03:10            510 test20210622.ps1

    Directory: C:\Users\admin\Desktop\powershelltest\dsad

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---         2021/7/12 下午 01:21              0 hello.txt
```



### New-Item

用於建立檔案



```powershell
PS C:\Users\admin\Desktop\powershelltest> New-Item new_item.ps1


    目錄: C:\Users\admin\Desktop\powershelltest


Mode                LastWriteTime         Length Name
----                -------------         ------ ----
-a----      2021/5/31  下午 04:31              0 new_item.ps1
```



### Out-File

將原本要在dos上顯示的結果輸出成文字檔的指令



例如

```pow
PS C:\Users\admin\Desktop\powershelltest> "hahaha" | Out-File haha.txt
```

就會生成一個`haha.txt`檔案內容是`hahaha`



```powershell
PS C:\Users\admin\Desktop\powershelltest> pwd | Out-File pwd_result.txt
```

同理，這行指令可以生成一個含有pwd執行結果的`pwd_result.txt`

```powershell
Path
----
C:\Users\admin\Desktop\powershelltest
```



`Out-File`的機制是如果目標位置沒有該檔名的檔案就會生成一個，如果已經有了就會將其複寫。

如果想要不更動現有內容，在文件的下方加入新的內容，可以透過參數`Append`進行設定。



```powershell
PS C:\Users\admin\Desktop\powershelltest> dir | Out-File pwd_result.txt -Append
```

剛才的`pwd_result文件就會變成

```powershell
Path
----
C:\Users\admin\Desktop\powershelltest


    Directory: C:\Users\admin\Desktop\powershelltest

Mode                 LastWriteTime         Length Name
----                 -------------         ------ ----
-a---         2021/5/31 下午 04:47            185 new_item.ps1
-a---          2021/6/1 上午 08:50             55 pwd_result.txt
```



#### `>`運算子

`>`運算子的作用和`Out-File`指令十分相似(至少我是沒看出甚麼差別啦)

```powershell
PS C:\Users\admin\Desktop\powershelltest> pwd | Out-File pwd_result.txt
```

我們可以用以下方法來達成上例的功能

```powershell
PS C:\Users\admin\Desktop\powershelltest> pwd > pwd_result.txt
```



`Append`的作用也可以達成，只要將`>`寫兩遍成為`>>`即可

```powershell
PS C:\Users\admin\Desktop\powershelltest> dir >> pwd_result.txt
```



**註**:

`>`是運算子而非`Out-File`的alias

事實上`Out-File`並不具有alias

```pow
PS C:\Users\admin\Desktop\powershelltest> Get-Alias -Definition Out-File
Get-Alias: This command cannot find a matching alias because an alias with the definition 'Out-File' does not exist.
```



### Measure-Object

取得總數

Example

```powershell
PS C:\Users\admin\Desktop\Git\git-game-v2> git ls-files
.gitignore
.replit
AllFiles/Graph/main.cpp
AllFiles/Graph/prim.h
AllFiles/LICENSE
AllFiles/cleanbuild.mk
AllFiles/definitions.mk
AllFiles/dumpvar.mk
AllFiles/factory_ramdisk.mk
AllFiles/five/bash/spam/spam.sh
AllFiles/four/four-file1
AllFiles/four/four-file2
AllFiles/four/four-file3
AllFiles/four/four-file4
AllFiles/four/four-file5
AllFiles/github.html
AllFiles/hello.cpp
AllFiles/oem_image.mk
AllFiles/one/array/array.cpp
AllFiles/one/one-file1
AllFiles/one/one-file2
AllFiles/one/one-file3
AllFiles/one/one-file4
AllFiles/one/one-file5
AllFiles/pdk_config.mk
AllFiles/sdk-addon.mk
AllFiles/three/linux/GNU/LinusTorvalds/linus
AllFiles/two/algorithms/c++/c++.cpp
AllFiles/ucr-cs100
README.md
PS C:\Users\admin\Desktop\Git\git-game-v2> git ls-files|Measure-Object


Count    : 30
Average  :
Sum      :
Maximum  :
Minimum  :
Property :
```

