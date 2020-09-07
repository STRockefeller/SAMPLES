# ASP.net Core MVC Note

[TOC]

## 專案相關

[TOP](#ASP.net Core MVC Note)

### 專案建立

建議設定

* 專案建立時把SSL認證取消勾選
* 取消HTTPS
* Debug選擇專案名稱而非IIS

---

### 加入MVC服務

Startup.ConfigureServices中加入`services.AddMvc();`或者` AddMvcCore();`

```C#
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
        }
```

---

### 設定MVC路由

以下兩個方法擇一

1. 在 Configure 對 `app:IApplicationBuilder` 使用 UseMvcWithDefaultRoute 方法註冊 MVC 預設路由的 Middleware 

```C#
public void Configure(IApplicationBuilder app)
    {
        app.UseMvcWithDefaultRoute();
    }
```

2. 或者使用預設Startup.cs  Configure 最後方的部分 Controller 和 Action 指向最初執行的內容

```C#
 app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=About}/{id?}");
            });
```



2020/09 整理筆記註記：再次建立MVC專案在這個步驟使用以上方法時，VS會跳出*not supported while using Endpoint Routing*警告，並且Configure方法最下方已無法找到`app.UseMvc()`取而代之的是`app.UseEndpoints()`，並且會提示如要使用方法必須Disable endpoint Routing.，可採取的處理作法如下。

#### Trouble:[Using 'UseMvc' to configure MVC is not supported while using Endpoint Routing]

[reference: stack overflow](https://stackoverflow.com/questions/57684093/using-usemvc-to-configure-mvc-is-not-supported-while-using-endpoint-routing)

若[以上兩個方法](#設定MVC路由)出現如上警告

I found the solution, in the following official documentation "[Migrate from ASP.NET Core 2.2 to 3.0](https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio)":

There are 3 approaches:

> 1. Replace UseMvc or UseSignalR with UseEndpoints.

In my case, the result looked like that

```cs
  public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        //Old Way
        services.AddMvc();
        // New Ways
        //services.AddRazorPages();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
        });

    }
}
```

> OR
> \2. Use AddControllers() and UseEndpoints()

```cs
public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

    }
}
```

> OR
> \3. Disable endpoint Routing. As the exception message suggests and as mentioned in the following section of documentation: [use mvcwithout endpoint routing](https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio#use-mvc-without-endpoint-routing)

```cs
services.AddMvc(options => options.EnableEndpointRouting = false);
//OR
services.AddControllers(options => options.EnableEndpointRouting = false);
```

### 雜項提醒

#### 引用本地檔案

引用本地css/js時，與非MVC專案相同，記得將檔案放入wwwroot中，否則不論以什麼路徑寫法都無法找到檔案。(也可以寫gulp省事)

#### HTTPS

在建立專案時，若有勾選，則Debug時可能會有部分腳本被封鎖而無法動作，若不慎忘記取消勾選，可以在專案屬性-->偵錯-->Web伺服器設定-->應用程式URL(P):中將https改為http。

## MVC 框架

[TOP](#ASP.net Core MVC Note)

[reference](https://www.as-creative.com.tw/%E6%99%82%E4%BA%8B/asp-net-mvc-controller-and-view-%E7%9A%84%E6%87%89%E7%94%A8/)

[reference](https://ithelp.ithome.com.tw/articles/10202127)

[reference](https://ithelp.ithome.com.tw/articles/10193590)

---

### Common

MVC 一種軟體架構模式，把系統分成三個種核心，分別為:Model, View, Controller。
主要將網頁分成邏輯處理(物件操作)、視覺呈現與路由控制(發送、接收請求)，各種元件
處理不同的工作，強調職責分離，開發與維護人員可以更快速對於目的與問題，找到該
處理的程式，讓程式的修改與功能擴充簡化，提高程式可用性。

在不同的原件中，各有自己的特色：
[**Model**](#Model) : 功能（實現演算法等等）、進行資料管理和資料庫設計(可以實現具體的功能)。包含所有的邏輯、物件，內容豐富。
[**Controller**](#Controller) : 負責轉發請求，對請求進行處理。盡量輕量，這裡盡量不撰寫邏輯與物件，而以路由以傳遞資料為主。
[**View**](#View) : 圖形介面設計。僅呈現，故盡量單純的呈現即可。

![](https://2.bp.blogspot.com/-YUWtsOlOtQY/Vz58E8CMBOI/AAAAAAAAbiQ/eXGYjaWnA9kDZZ0ESTeMiuJy2a__ZVdwQCLcB/s640/001.png)

![](https://blog.johnwu.cc/images/i06-2.png)

![](https://blog.johnwu.cc/images/i06-3.gif)



---

### Model

主要是用來裝載資料的容器，而資料加工及資料存取的工作則委派給BLL及DAL層處理。
又可分為資料庫的原始Model及呈現畫面用的ViewModel。
ViewModel在設計時盡量保持純淨(不要加入帶有邏輯的程式碼)，若逼不得已則以擴充方法(Extend Method)方式為其擴充。

---

### View

負責將資料呈現到畫面上。
一個View僅能對應一個Model，若需要多個則使用ViewModel進行組合。



#### 檔案路徑關係

View 跟 Controller 有相互的對應關係，預設在 Controller 使用 `View` 方法回傳結果，會從以下目錄尋找對應的 `*.cshtml`：

1. *Views\{ControllerName}\{ActionName}.cshtml*
   尋找與 Controller 同名的子目錄，再找到與 Action 同名的 `*.cshtml`。
   如下方Controller範例中的 `HomeController.Index()`，就會找專案目錄下的 `Views\Home\Index.cshtml` 檔案。
2. *Views\Shared\{ActionName}.cshtml*
   如果 Controller 同名的子目錄，找不到 Action 同名的 `*.cshtml`。就會到 Shared 目錄找。
   如下方Controller範例中的 `HomeController.Index()`，就會找專案目錄下的 `Views\Shared\Index.cshtml` 檔案。

#### View的顯示流程

1. 由Controller決定要顯示的內容
2. _ViewStart.cshtml為View的起始點
3. Shares\\_Layout.cshtml決定整體版面
4. 最後在_Layout.cshtml裡面`@RenderBody()`的部分顯示所呼叫的內容



#### _Layout.cshtml

Visual Studio建立MVC專案的預設檔案。

_Layout.cshtml內容中，我們能找到 @RenderBody() 這個語法，功能是呈現內容(content)：
目的是將 controller\action 所回傳的 view 嵌入在 _Layout 的@RenderBody() 所在位置。如此
一來，外部樣式(header, footer, or side bar)即可共用，網頁切換時，不需要每個畫面撰寫重
複的html，而只須依據不同的controller與action，嵌入不同的 view。

我們也能將其他header, sidebar區塊作為另外建立patial view，再使用 @Html.Partial 與
@Html.RenderPartial語法嵌入。當然，這又是另一種應用了。當_Layout.cshtml版面撰寫好
了，我們只需專注在每一個partial view即可。

---

### Controller

負責指揮工作流程及執行順序。
是所有請求(Request)的第一線入口，所以驗證及授權通常也會在進到Controller時檢查。
決定回應(Response)瀏覽器的狀態代碼(HttpStatusCode)及內容。

習慣上，會把Controller類別後綴**Controller**並放在Controllers資料夾(NameSpace)下

例如以Visual Studio 建立專案後可以看到的預設Controller 類別

```C#
namespace MVCTest.Controllers
{
    public class HomeController : Controller{}
}
```

#### IActionResult

可以注意到預設的HomeController裡面有以下片段

```C#
        public IActionResult Index()
        {
            return View();
        }
```

`IActionResult` 回傳的方式可以有很多種，透過繼承 `Controller` 後，就可以使用 `Controller` 的方法：

- **View**
  以上例來說，透過回傳 `View` 方法，可以找到該 Controller & Action 對應的 `*.cshtml`， 並且把 UserModel 傳給 View 使用。
- **HTTP Status Code**
  回應包含 HTTP Status。常用的回應有 `Ok`、`BadRequest`、`NotFound` 等。
  例如：`return BadRequest("Internal Server Error")`，會回應 HTTP Status 400 及 **Internal Server Error** 字串。
- **Redirect**
  可以把 Request 轉給其他的 Action 或 URL。轉向的方法有 `Redirect`、`LocalRedirect`、`RedirectToAction`、`RedirectToRoute` 等。
  例如：`return RedirectToAction("Login", "Authentication")`，就會把 Request 轉向到 **AuthenticationController** 的 **Login()**。
- **Formatted Response**
  回應時指定 **Content-Type**。Web API 的回傳通常都用這種方式，序列化物件順便標註 **Content-Type**。
  例如：`return Json(user)`，會將物件序列化成 JSON 字串，並在 HTTP Headers 帶上 **Content-Type=application/json**。



---

### MVC Sample

在Models資料夾底下新增TestMVCModel.cs

```C#
namespace MVCTest.Models
{
    public class TestMVCModel
    {
        public string testString = "This is test string defined in model.";
    }
}
```

在Controllers資料夾底下新增TestMVCController.cs (這裡若選擇新增空白控制器則會預設繼承Controller並包含Index方法)

建立一個Model物件並將作為View()的參數

注意此處回傳IActionResult的方法名稱為**action名稱**同時也必須是對應**view的名稱**。

```C#
namespace MVCTest.Controllers
{
    public class TestMVCController : Controller
    {
        public IActionResult Index()
        {
            Models.TestMVCModel mvcTest = new Models.TestMVCModel();
            return View(model:mvcTest);
        }
    }
}
```

在Views底下新增TestMVC資料夾並加入Index.cshtml(名稱必須同Controller中的action)(這裡若選擇新增空白檢視器會得到`@{}`)

以@model加入對應的Model

```html
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model MVCTest.Models.TestMVCModel
Test string is" <b>@Model.testString</b>".
```



最後去改Startup Config方法中的endpoint middleware中的MVC預設路徑，讓專案執行時直接顯示剛才新增的內容

```C#
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TestMVC}/{action=Index}/{id?}");
            });
```

註:如果是.net core 2.2以前的版本應該是`app.UseMvc`



接著在Debug模式下執行，建議將執行媒介改為專案名稱(預設應該是IIS)

![](https://i.imgur.com/ir9NVpC.png)

很好，成功顯示出了寫在TestMVCModel中的內容。

但在View中明明只寫了`Test string is" <b>@Model.testString</b>".`為什麼網頁上卻能看到NAV和Footer?

搜尋後發現顯示的內容來自Views\Shared裡面的cshtml檔案，簡單來說，這個資料夾裡面的檔案就是所謂的[共用物件](#View的顯示流程)，NAV Footer Menu等等每個頁面都要使用的內容就寫在這裡。

過去撰寫ASP.net Core Web專案而未使用MVC架構時，共用的NAV寫法是將內容存到`JQuery<HTMLElement>`當中，並在其他頁面的ts檔中import進去。



###　傳遞資料的方法

#### Controller to View

##### ViewData

* 只存在於一個頁面

* 使用key-value 方式

* Controller side sample

  `ViewData["message"]="my message;"`

* View side sample

  `@ViewData["message"]`



##### ViewBag

* 只存在於一個頁面

* 使用dynamic型別

* Controllwe side sample

  `ViewBag.message="my message;"`

* View side sample

  `@ViewBag.message`



##### TempData

* 只存在於一個Request

* 可以跨Action

* Controller side sample

  `TempData["message"]="mymessage;"`

* View side sample

  `@TempData["message"]`

### View的動態內容

[Reference](https://www.cnblogs.com/willick/p/3410855.html)

總體來說，View中的内容可以靜態和動態兩個部分。靜態内容一般是html元素，而動態内容指的是在程式執行的時候動態生成的内容。以下是在VIEW加入動態內容的幾種常見作法：

- **Inline code**：小片段程式，如 if 和 foreach。
- **Html helper方法**：用來生成HTML元素，如view model、ViewBag等。
- **Section**：在指定的位置插入内容。
- **Partial view**：存在於一個單獨的VIEW文件中，最為子内容可在多個VIEW中共享。
- **Child action**：調用 controller 中的 action 來返回一個view，並將結果插入到輸出流中。

#### Section

Razor View 引擎支援將View中的一部分内容分離出來，以便在需要的地方重複利用，減少了程式碼的冗餘。

可以看到預設的Views\Shared\_Layout.cshtml 中在`<footer></footer>`的下方就有用到section`@RenderSection("Scripts", required: false)`

試著使用這個section插入內容。改寫MVCTestView/index.cshtml

```C#
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model MVCTest.Models.TestMVCModel

Test string is" <b>@Model.testString</b>".
<p><b id="scriptStatus"></b></p>
<p><button type="button" class="btn btn-primary" id="btnGO">GO</button></p>
    
@section Scripts {
    <div>this is section Scripts</div>
	<script>
            $(document).ready(function () {
                $("#scriptStatus").text("Script active");
            });
            $("#btnGO").click(function () {
                window.location.href = "./RouteTest";
            });
     </script>
}
```

為方便閱讀把@section寫在文章對應的相對位置處，`@RenderSection("Scripts", required: false)`是寫在最後面，所以這邊也寫到後方，前面的內容會跑到`@RenderBody`裡面。

![](https://i.imgur.com/P29Ed8v.png)

執行後可以看到，插入的內容確實有加入html中。



當然，依一樣的格式，我們也可以自己決定插入位置以及內容

如改寫layout

```C#
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MVCTest</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    @RenderSection("Header",false)
    <header>
        <nav class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">MVCTest</a>
                <button class="navbar-toggler" type="button" data-toggle="collapse" data-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse d-sm-inline-flex flex-sm-row-reverse">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2020 - MVCTest - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
    @RenderSection("Scripts", required: false)
    @RenderSection("Footer",false)
</body>
</html>
```

注意要是RenderSection使用`public Task<HtmlString> RenderSectionAsync(string name);`這個多載(沒有傳入required參數)，則頁面中若未定義該名稱的內容就會跳錯誤，換句話說有requred=false則可以隨意插入或不插入內容，反之則必須插入內容。

插入內容端，作法很簡單

```C#
@section Header{}
@section Footer{}
```

也可以做一些運算

```C#
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model MVCTest.Models.TestMVCModel

@section Header{ 
    <div class="Header">
        @for (int i = 0; i < 10; i++)
            @i
        </div>
}
Test string is" <b>@Model.testString</b>".
<p><b id="scriptStatus"></b></p>
<p><button type="button" class="btn btn-primary" id="btnGO">GO</button></p>
@section Footer{
    <div>this is section Footer</div>
    <script>
        $(document).ready(function () {
            $("#scriptStatus").text("Script active");
        });
        $("#btnGO").click(function () {
            window.location.href = "./RouteTest";
        });
    </script>
}
```

結果如下

![](https://i.imgur.com/aVpNgQe.png)

#### Partial View

Partial view是將部分 Razor 和 Html 標籤放在一個獨立的View文件中，以便在不同的地方重複利用。

在Shared底下建立新的檢視，並勾選建立成局部檢視

![](https://i.imgur.com/l9h3nDA.png)

接著在裡面加入一點訊息

```html
@*
    For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860
*@
<div>
    This is the message from the partial view.
    @Html.ActionLink("This is a link to the Index action", "Index")
</div> 
```

在Controller中新增一個action

```C#
        public ActionResult List()
        {
            return View();
        }
```

View中加入對應的cshtml(List.cshtml)

```html
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@{
    ViewBag.Title = "List";
    Layout = null;
}
<h3>This is the /Views/TestMVC/List.cshtml View</h3>
@Html.Partial("MyPartialView")
```



執行並進入TestMVC/List

![](https://i.imgur.com/9nzBdUp.png)