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