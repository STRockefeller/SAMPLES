# Unit Testing

[Reference: ITHelp](https://ithelp.ithome.com.tw/articles/10252612)

[Reference: bUnit 官方文件](https://bunit.dev/docs/getting-started/index.html)

使用`BUnit`進行單元測試

專案可以先建立`NUnit單元測試專案`

**註**:`bUnit`搭配`xUnit` `NUnit` `MSTest`的使用方式略有不同，詳情請見[官方文件的說明](https://bunit.dev/docs/getting-started/writing-tests.html?tabs=nunit)，這篇筆記會以`NUnit`為主

## 安裝`bUnit`

直接進入NuGet套件管理員安裝`bunit`套件即可(筆記當下的穩定版本為1.1.5版)

接著加入專案參考

如果發生專案版本不相容的情況請修改`.csproj`檔案的以下片段

```xml
  <PropertyGroup>
    <TargetFramework>net5.0</TargetFramework>
```

或者可以從專案屬性修改目標專案類型。



## 使用`bUnit`測試Component

首先要先引用剛安裝的`bunit`套件

```C#
using Bunit;
```



接著就可以開始測試了

這邊直接引用ITHELP文章中的程式碼

```C#
[Test]
        public void IndexShouldRender()
        {            
            var ctx = new Bunit.TestContext();

            //cut = component under test
            var cut = ctx.RenderComponent<BlazorUITest.Pages.Index>();
            cut.MarkupMatches("<h1>Hello, world!</h1>");
        }
```



我的專案中的`index.razor`並沒有做過更動。

```html
@page "/"

<h1>Hello, world!</h1>

Welcome to your new app.

<SurveyPrompt Title="How is Blazor working for you?" />
```



測試結果，測試未通過

```
  訊息: 
    Bunit.HtmlEqualException : HTML comparison failed. 
    
    The following errors were found:
      1: The text at #text(1) was not expected.
      2: The element at div(1) was not expected.
    
    Actual HTML: 
    <h1>Hello, world!</h1>
    Welcome to your new app.
    <div class="alert alert-secondary mt-4" role="alert">
      <span class="oi oi-pencil mr-2" aria-hidden="true"></span>
      <strong>How is Blazor working for you?</strong>
      <span class="text-nowrap">
        Please take our
        <a target="_blank" class="font-weight-bold" href="https://go.microsoft.com/fwlink/?linkid=2137916">brief survey</a>
      </span>
      and tell us what you think.
    </div>
    
    Expected HTML: 
    <h1>Hello, world!</h1>
```



**解說**

透過

```C#
var ctx = new Bunit.TestContext();
var cut = ctx.RenderComponent<BlazorUITest.Pages.Index>();
```

產生受測元件

並以

```C#
cut.MarkupMatches("<h1>Hello, world!</h1>");
```

將其與預測內容進行比較。



---

修改一下測試項目，變為取出`<h1></h1>`裏頭的東西進行比對

```C#
        [Test]
        public void IndexShouldRender()
        {
            var ctx = new Bunit.TestContext();
            var cut = ctx.RenderComponent<BlazorAppWasm.Pages.Index>();
            cut.Find("h1").TextContent.MarkupMatches("Hello, world!");
        }
```

這次就順利通過測試了



**解說**

透過`Find()`方法選擇tag並以`TextContent`屬性取出裡面的值

這個做法感覺和寫爬蟲的時候有點類似，就是不知道有多個tag相符合的情況下會變成如何?

查了一下方法說明

```
        // 摘要:
        //     Returns the first element from the rendered fragment or component under test,
        //     using the provided cssSelector, in a depth-first pre-order traversal of the rendered
        //     nodes.
```

似乎只會回傳第一個相符的項目。



---

進階作法還有很多，等需要用到的時候再來查官方文件好了。