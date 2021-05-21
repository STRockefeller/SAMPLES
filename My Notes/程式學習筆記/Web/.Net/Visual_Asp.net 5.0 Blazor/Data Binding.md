# Data Binding

[Reference:MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/blazor/components/data-binding?view=aspnetcore-5.0)

[Reference:Blazor University](https://blazor-university.com/components/one-way-binding/)



## @+變數

其實就是隱式 Razor 運算式的作法

直接顯示出該變數值，是 One Way Binding 的常見做法



例如

```C#
@page "/Bind"
<h3>@title</h3>

@code {
    string title = "BindTestComponent";
}
```



就會輸出

```html
<h3>BindTestComponent</h3>
```





## @bind="變數" / @bind="@+變數"



Two Way Binding 的實現

可以在某些標籤中加入繫結的資料

```C#
@page "/Bind"
<h3>@title</h3>
<input @bind="inputValue1" type="text">
<br>
<b>your input value is : @inputValue1</b>
<br>
<input @bind="@inputValue2" type="text">
<br>
<b>your input value is : @inputValue2</b>
@code {
    string title = "BindTestComponent";
    string inputValue1;
    string inputValue2;
}
```

輸出結果是當input欄位被輸入內容後輸入Enter或lose focus就會把內顯示到下方

至於`@bind="變數"` ` @bind="@+變數"`兩種寫法，目前看來是沒有區別。MSDN的範例中使用的是前者，但網路上也有看過第二種寫法。

當然宣告變數的時候也可以同時給予初始值，例如將上例改寫成`string inputValue1="initial value";`就能在載入頁面的時候看到input欄位上顯示設定好的初始值。

## event binding



直接上MSDN的範例

```C#
@page "/bind-event"

<p>
    <input @bind="InputValue" @bind:event="oninput" />
</p>

<p>
    <code>InputValue</code>: @InputValue
</p>

@code {
    private string InputValue { get; set; }
}
```



歸納一下

* 寫法是將`@bind:event`寫到`@bind`後方進行補充，指名binding的時機點。

* 這邊的event並不是要在C#中自己定義event(我試過，也不允許這樣做)，而是這些Tag自身的事件如`onclick` `onchange`等等



其他事件具體可以參考[MDN](https://developer.mozilla.org/en-US/docs/Web/API/GlobalEventHandlers)



繼續修改範例

```C#
@page "/Bind"
<h3>@title</h3>
<input @bind="inputValue1" @bind:event="oninput" type="text">
<br>
<b>your input value is : @inputValue1</b>
<br>
<input @bind="@inputValue2" type="text">
<br>
<b>your input value is : @inputValue2</b>
@code {
    string title = "BindTestComponent";
    string inputValue1 = "initial value";
    string inputValue2;
}
```





