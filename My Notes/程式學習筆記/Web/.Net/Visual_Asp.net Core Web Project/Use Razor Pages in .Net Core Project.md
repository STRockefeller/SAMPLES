# Razor Pages in ASP.NET Core

Reference:

[MSDN](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-5.0&tabs=visual-studio#razor-pages)

## Abstract

自接觸過blazor摒棄前端三俠完全以C#撰寫靜態網頁的功能後，就深深為此著迷，抱著期待查了一下發現asp.net core果然也有類似的作法。

### Notice:

* 這個做法只適用於.Net 5.0以上的專案
* 理論上適用於所有.net core web專案，撰寫筆記當下是使用MVC專案模板測試。



## Getting Start

### DI

先完成DI容器的注入和endpoits的設定



```cs
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
    }
```



```cs
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
```



### `Pages` Folder

在專案目錄下建立名為"Pages"的資料夾(我一開始把它放在View/Pages底下Debug老半天才發現錯在這)

razor page 的路由不會經過Controller所以他們在檔案總管的路徑會直接對應到url，詳細對照如下

| File name and path            | matching URL               |
| :---------------------------- | :------------------------- |
| */Pages/Index.cshtml*         | `/` or `/Index`            |
| */Pages/Contact.cshtml*       | `/Contact`                 |
| */Pages/Store/Contact.cshtml* | `/Store/Contact`           |
| */Pages/Store/Index.cshtml*   | `/Store` or `/Store/Index` |



### `cshtml` files

接著就可以開始寫razor pages，注意和blazor專案不同，我們使用的是razor view (`.cshtml`)而非 razor component

(`.razor`)

我們依然可以如同razor component將`.cshtml`檔案拆分為`.cshtml`+`.cshtml.cs`

如下範例

RazorTest.cshtml

```cshtml
@page
@model WebApplicationMVC0714.Pages.RazorTestModel

<p>Hello @Model.Message</p>
```

RazorTest.cshtml.cs

```cs
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplicationMVC0714.Pages
{
    public class RazorTestModel:PageModel
    {
        public string Message { get; set; } = "World";
    }
}
```



根據上例一一說明細節



#### `@page`

> `@page` makes the file into an MVC action - which means that it handles requests directly, without going through a controller. `@page` must be the first Razor directive on a page. `@page` affects the behavior of other [Razor](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0) constructs. Razor Pages file names have a *.cshtml* suffix.



簡而言之Razor Page的頁面最開頭一定要使用這個指示詞



#### `@model`

很遺憾的 `.cshtml`不像`.razor`可以直接連結到邏輯部分，依然使用asp.net core的model定義方式





#### 邏輯部分

首先要先引用`Microsoft.AspNetCore.Mvc.RazorPages`

> blazor中要引用的是 `Microsoft.AspNetCore.Components`



然後 class 要繼承 `PageModel`(`Microsoft.AspNetCore.Components.PageModel`)

class名稱可以隨便取(不同於blazor是partial class名稱一定要和razor component一樣)



## 隨便嘗試

### 如果像blazor一樣在@page後方定義路由有用嗎?

![](https://i.imgur.com/ctVMOao.png)

答案是肯定的，順帶一提，原本的URL就變成404了



### razor component能否直接使用?

![](https://i.imgur.com/9Fte9Ry.png)

很可惜似乎沒辦法

再找找發現MSDN有相關[文章](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/prerendering-and-integration?view=aspnetcore-5.0&pivots=server)(這篇翻譯很怪建議讀英文)，把blazor整合asp.net core裡面，似乎有些複雜，應該會另外撰寫筆記。



### 那能否在cshtml中引用razor元件?

先說結果，不行

測試條件如下

BindButton.razor

```razor
<button type="button" class="btn btn-primary">@buttonText</button>

@code {
    [Parameter]
    public string buttonText { get; set; }
}
```

TestRazor.cshtml

```cshtml
@page "/t"
@model WebApplicationMVC0714.Pages.RazorTestModel

<p>Hello @Model.Message</p>
<BindButton buttonText="OOXX" />
```



最後Button並沒有被顯示出來

### 只要繼承PageModel的class即便檔案沒有同名也可以被傳入嗎?

答案是可以

![](https://i.imgur.com/HkYDHVO.png)