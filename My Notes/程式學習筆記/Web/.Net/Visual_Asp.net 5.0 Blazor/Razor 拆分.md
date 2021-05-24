# Razor 拆分



才發現Razor 元件是可以拆開來的，我比較喜歡這種拆開來的寫法以後應該會以這種寫法為主。

有點類似把`javascript`寫在`.html`檔案裡面和另外寫在`.js`裡面的差別



並不複雜，直接上範例

假設這是我原本的元件(這是我在寫Logger測試用的元件)

```razor
@*LogTestComponent.razor*@
@page "/log"
@using Microsoft.Extensions.Logging;
@inject ILogger<LogTestComponent> logger;

<h3>LogTestComponent</h3>
<button type="button" class="btn btn-primary" @onclick="onClickFunction">LOG INFO</button>

@code {
    void onClickFunction()
    {
        logger.LogInformation("Button Click");
    }
}
```



在同路徑(這個例子的話是Pages資料夾底下)新增類別並命名為`Razor檔案名稱+".cs"`(例如`LogTestComponent.razor.cs`)

然後將Class宣告改為Partial 並 using `Microsoft.AspNetCore.Components`

```c#
//LogTestComponent.razor.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorAppWasm.Pages
{
    public partial class LogTestComponent
    {
    }
}
```

然後就可以把C#的內容都搬過來了



最後變成如下

畫面部分

```html
@*LogTestComponent.razor*@
@page "/log"

<h3>LogTestComponent</h3>
<button type="button" class="btn btn-primary" @onclick="onClickFunction">LOG INFO</button>
```

邏輯部分

```C#
//LogTestComponent.razor.cs
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorAppWasm.Pages
{
    public partial class LogTestComponent
    {
        [Inject]
        private ILogger<LogTestComponent> logger { get; set; }
        void onClickFunction()
        {
            logger.LogInformation("Button Click");
        }
    }
}
```



Notice:

1. using都要重寫，不論是寫在razor畫面中或是import裡面的都讀不到
2. DI的寫法注意一下