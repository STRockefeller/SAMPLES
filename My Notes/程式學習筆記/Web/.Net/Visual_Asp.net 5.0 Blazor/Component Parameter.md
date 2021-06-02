# Component Parameter



## [Parameter]標籤



以下節錄自[ITHELP](https://ithelp.ithome.com.tw/articles/10245286)

> #### Paramter用法
>
> 1. 設定一個給呼叫端用的屬性
> 2. 在該屬性上設定 [Parameter] 這個attribute
> 3. 屬性須設定為internal以上層級
> 4. 可傳不同型態的值，除了可傳簡單型別物件之外，也可傳物件、事件過去

簡單的說就是設定在自製元件被呼叫時可以傳入的參數內容

例如(假如這個元件的類別名稱叫做`BindButton`)

```C#
<button type="button" class="btn btn-primary">@buttonText</button>

@code {
    [Parameter]
    public string buttonText { get; set; }
}
```



在其他地方只要用如下方式就可以調用該元件，並同時賦予參數

```html
<BindButton buttonText="OOXX" />
```



備註: Visual Studio 2019 Community 這個部分的參考關係時靈時不靈有時會在BindButton下方顯示紅色波浪線表示無法理解，輸入類別名稱時也常常不會有自動輸入的提示，但可以正常編譯執行，另外VSC似乎就沒有這方面的問題。



## Arbitrary Parameter



### 目的

在上例中，我使用`[Parameter]`標籤來定義<button> tag 的內容。

其實在實作上，除了內容之外，利用參數來設定tag attributes的情況也很常見。

我們同樣可以透過`[Parameter]`標籤來設定元件的tag attributes，如下

```C#
<button type="@buttonType" class="@buttonClass">@buttonText</button>

@code {
    [Parameter]
    public string buttonText { get; set; }
    [Parameter]
    public string buttonType { get; set; }
    [Parameter]
    public string buttonClass { get; set; }
}
```

但是隨著所需設定的屬性增加，使用`[Parameter]`標籤來設定就會越來越不方便。

為此微軟提供一個新的方法去解學這個問題，就是Arbitrary Parameter

當然不僅限於設定標籤屬性，只要涉及大量的參數設定都可以考慮到這個作法。



### 使用方式



把`[Parameter]` 後方加入個參數變成` [Parameter(CaptureUnmatchedValues = true)]`

[MSDN文件](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.parameterattribute.captureunmatchedvalues?view=aspnetcore-5.0)說明如下

> Gets or sets a value that determines whether the parameter will capture values that don't match any other parameter.



### 範例

直接拿上面的Button做修改

Button Component (TestButton.razor)

```C#
<button @attributes="attr">@buttonText</button>

@code {
    [Parameter]
    public string buttonText { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> attr { get; set; }
}
```



Page

```C#
<TestButton buttonText="LADIDA" class="btn btn-primary" type="button" />
```



輸出就會如下

```html
<button class="btn btn-primary" type="button">LADIDA</button>
```



加入Console輸出來看看更詳細的資訊吧

TestButton.razor

```C#
<button @attributes="attr">@buttonText</button>

@code {
    [Parameter]
    public string buttonText { get; set; }
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> attr { get; set; }
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        string printStr = "OnParameterSet Called." +
        $"\n buttonText={buttonText}" +
        "\n attr=";
        foreach (var item in attr)
            printStr += $"{item.Key} : {item.Value.ToString()}\n";
        Console.WriteLine(printStr);
    }
}
```



輸出

```
OnParameterSet Called.
 buttonText=LADIDA
 attr=class : btn btn-primary
type : button
```





### 補充 關於 @attributes

[MSDN](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#attribute-splatting-and-arbitrary-parameters)

> Components can capture and render additional attributes in addition to the component's declared parameters. Additional attributes can be captured in a dictionary and then *splatted* onto an element when the component is rendered using the [`@attributes`](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#attributes) Razor directive attribute. This scenario is useful for defining a component that produces a markup element that supports a variety of customizations.

> `@attributes` allows a component to render non-declared attributes.

簡單的說就是可以透過`Dictionary<string, object>`的形式輸入標籤屬性。



我決定另外寫一篇筆記紀錄這些 Razor Directives，這邊就不贅述了。



## EventCallBack

[Reference:MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/blazor/components/event-handling?view=aspnetcore-5.0#eventcallback)

Blazor Component 有著父元件和子元件的概念，例如我有一個Page Component和一個Button Component，當我要在Page中放置該Button時習慣上我會將Page作為父元件而Button作為子元件。

父子元件之間資料傳遞的方法有二:

1. 父元件傳遞資料到子元件: 使用`[Parameter]`標籤設定，上面有提過了。
2. 子元件傳遞資料到父元件: 使用 `EventCallBck`。



### 使用方法



在子元件中定義`EventCallback`物件

並定義一個方法觸發這個物件的`InvokeAsync`方法



EventCallback定義如下

```C#
public readonly struct EventCallback : IEventCallback
    {
        //
        // 摘要:
        //     Gets a reference to the Microsoft.AspNetCore.Components.EventCallbackFactory.
        public static readonly EventCallbackFactory Factory;
        //
        // 摘要:
        //     Gets an empty Microsoft.AspNetCore.Components.EventCallback.
        public static readonly EventCallback Empty;

        //
        // 摘要:
        //     Creates the new Microsoft.AspNetCore.Components.EventCallback.
        //
        // 參數:
        //   receiver:
        //     The event receiver.
        //
        //   delegate:
        //     The delegate to bind.
        public EventCallback(IHandleEvent? receiver, MulticastDelegate? @delegate);

        //
        // 摘要:
        //     Gets a value that indicates whether the delegate associated with this event dispatcher
        //     is non-null.
        public bool HasDelegate { get; }

        //
        // 摘要:
        //     Invokes the delegate associated with this binding and dispatches an event notification
        //     to the appropriate component.
        //
        // 參數:
        //   arg:
        //     The argument.
        //
        // 傳回:
        //     A System.Threading.Tasks.Task which completes asynchronously once event processing
        //     has completed.
        public Task InvokeAsync(object? arg);
        //
        // 摘要:
        //     Invokes the delegate associated with this binding and dispatches an event notification
        //     to the appropriate component.
        //
        // 傳回:
        //     A System.Threading.Tasks.Task which completes asynchronously once event processing
        //     has completed.
        public Task InvokeAsync();
    }
```



父元件中再傳入相應的方法做執行



### 範例



#### 範例1

子元件

```html
@*EventCallBackTestButton.razor*@
<button type="button" class="btn btn-primary" @onclick="@onClickFunction">Event Call Back Test</button>
```

```C#
//EventCallBackTestButton.razor.cs
using Microsoft.AspNetCore.Components;

namespace BlazorAppWasm.Shared.ComponentItems
{
    public partial class EventCallBackTestButton
    {
        [Parameter]
        public EventCallback onClickCallBack { get; set; }
        public void onClickFunction() => onClickCallBack.InvokeAsync();
    }
}
```



父元件

```html
<EventCallBackTestButton onClickCallBack="onClickFunction"/>
```

對`onClickCallBack`參數傳入我想要子元件做的事情`onClickFunction`





#### 範例2

前面那個例子有點太單純了，感覺沒練習到，所以再試著想個複雜一點點的應用方式。



子元件

```html
@*EventCallBackChangeColorButton.razor*@
<div class="form-group">
    <label for="">Color Specification</label>
    <input type="color"
           class="form-control" name="" id="" aria-describedby="helpId" placeholder="" @bind="color">
</div>
<button type="button" class="btn btn-primary" @onclick="changeColor">Set Color</button>
```

```C#
//EventCallBackChangeColorButton.razor.cs
using Microsoft.AspNetCore.Components;

namespace BlazorAppWasm.Shared.ComponentItems
{
    public partial class EventCallBackChangeColorButton
    {
        private string color;
        [Parameter]
        public EventCallback<string> OnClickCallback { get; set; }
        private void changeColor() => OnClickCallback.InvokeAsync(color);
    }
}
```



父元件

```html
@page "/log"
@using BlazorAppWasm.Shared.ComponentItems;

<h3 style="color:@color">LogTestComponent</h3>

<EventCallBackChangeColorButton OnClickCallback="changeColor"/>
```

```C#
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;

namespace BlazorAppWasm.Pages
{
    public partial class LogTestComponent
    {
        string color = "black";
        void changeColor(string color) => this.color = color;
    }
}
```



流程就是在子元件選取顏色然後透過EventCallBack讓父元件決定子元件的按鈕要做甚麼事情同時從子元件傳遞顏色給父元件。



#### 心得

說實在的這兩個例子都沒有把這個功能做很好的應用，但撰寫的過程中我大致對於`EventCallback`的用處有了一些更具體的看法，對於大多數較為單純的情況下`EventCallback`幾乎都沒有出場的餘地，或許可以使用`EventCallback`，但是使用`EventCallback`並沒有明顯的好處，就如同上面兩個例子。

如果說必須使用`EventCallback`將資訊由子元件傳遞到父元件的情形，大該是父元件傳遞參數條件進子元件後還會在子元件進行一連串的運算或根據使用者操作得出運算結果再丟回來的情形吧。



## Cascading Parameter

參數共享的做法。

以下節錄自[ITHELP文章](https://ithelp.ithome.com.tw/articles/10247260)

> **級聯值和參數**提供一個便利的方式，讓元件能夠提供值給底下的所有子元件，只需要兩個步驟：
>
> 1. 傳遞方元件使用 `CascadingValue` 元件如：
>
> ```csharp
> <CascadingValue Name="CascadeParam1"
>                 Value="CascadeParam1">
>   <Child></Child>
> </CascadingValue>
> ```
>
> 1. 接收方元件的參數增加 `CascadingParameter` 屬性如：
>
> ```csharp
> [CascadingParameter(Name = "CascadeParam1")]
> protected string CascadeParam1 { get; set; }
> ```
>
> 這樣一來只要有元件使用 `CascadingValue` 元件來設定值，在 `CascadingValue` 元件底下所有的子元件都能夠準確接收其內容，是不是很簡單呀！但實際上的行為是，參數如果加了 `CascadingParameter` 屬性變成級聯參數，則會自動依照規則從上層元件中找到對應的級聯元件，並取得其值。
>
> 這邊重點介紹幾個 `CascadingValue` 元件的屬性成員：
>
> - Name(string) - 選填，設定名稱，通過指定名稱，使後代組件能夠接收值；如果未指定名稱，則子代組件將根據它們所請求的類型來接收值。
> - Value(TValue) - 提供的值。
> - IsFixed(Boolean) - 如果為 true，則表示在元件的生命週期內 Value 不會更改；用於性能優化，使框架跳過設定的更改通知，用於此情況才會特別標識。
>
> 而 `CascadingParameter` 屬性就很簡單，只有一個屬性 `Name`：
>
> - 設定時，規則為尋找**名稱相同**且**最接近**的 `CascadingValue` 元件。
> - 沒有設定時，規則為尋找**型別相同**且**最接近**並且**沒有命名**的 `CascadingValue` 元件。
>
> 只要掌握這個規則，在撰寫時就不會有搞不清楚的狀況了。