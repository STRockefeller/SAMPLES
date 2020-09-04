# ASP.net Core MVC Note

[TOC]

## 專案相關

[TOP](#ASP.net Core MVC Note)

### 專案建立

建議設定

* 專案建立時把SSL認證取消勾選
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





## Web專案相關

[TOP](#ASP.net Core MVC Note)

### MiddleWare

[reference MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1)

[reference ithelp](https://ithelp.ithome.com.tw/articles/10203041)

[reference ithelp](https://ithelp.ithome.com.tw/articles/10192682)

![imgMiddleware](https://blog.johnwu.cc/images/i03-1.png)

從發出請求(Request)之後，到接收回應(Response)這段來回的途徑上，用來處理特定用途的程式。比較常見的**Middleware**有身份驗證(Identity)、路由(Routing)或回應壓縮(Response Compression)等。

**Middleware**預設在`Startup`中`Configure`設定，**ASP.Net Core**預設內建了許多好用的**Middleware**，如驗證(Authentication)、回應壓縮(Response Compression)、URL重寫(URL Rewriting)等，如需要更詳細的資訊可以參考[MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1)。

#### 於Startup中使用middleware

* App.Use
  一般會使用Use進行自訂的Middleware擴充，能透過呼叫next()指定執行下一層Middleware(可加入條件判斷決定是否呼叫)，也可指定在管線回流時所要執行的行為。
  Middleware 的註冊方式是在 Startup.cs 的 Configure 對 IApplicationBuilder 使用 Use 方法註冊。
  大部分擴充的 Middleware 也都是以 Use 開頭的方法註冊，例如：

  UseMvc() ：MVC 的 Middleware
  UseRewriter() ：URL rewriting 的 Middleware

  > Middleware 範例
  >
  > ```C#
  > // ...
  > public class Startup
  > {
  >     // ...
  >     public void Configure(IApplicationBuilder app)
  >     {
  >         app.Use(async (context, next) => 
  >         {
  >             await context.Response.WriteAsync("First Middleware in. \r\n");
  >             await next.Invoke();
  >             await context.Response.WriteAsync("First Middleware out. \r\n");
  >         });
  > 
  >         app.Use(async (context, next) => 
  >         {
  >             await context.Response.WriteAsync("Second Middleware in. \r\n");
  >             await next.Invoke();
  >             await context.Response.WriteAsync("Second Middleware out. \r\n");
  >         });
  > 
  >         app.Use(async (context, next) => 
  >         {
  >             await context.Response.WriteAsync("Third Middleware in. \r\n");
  >             await next.Invoke();
  >             await context.Response.WriteAsync("Third Middleware out. \r\n");
  >         });
  > 
  >         app.Run(async (context) =>
  >         {
  >             await context.Response.WriteAsync("Hello World! \r\n");
  >         });
  >     }
  > }
  > ```
  >
  > 輸出
  >
  > > First Middleware in.  
  > >
  > > Second Middleware in.  
  > >
  > > Third Middleware in.  
  > >
  > > Hello World!  
  > >
  > > Third Middleware out.  
  > >
  > > Second Middleware out.  
  > >
  > > First Middleware out. 

  > Request 流程
  >
  > ![](https://blog.johnwu.cc/images/pasted-114.gif)

* App.Run
  Run 是 Middleware 的最後一個行為，以[上面圖例](#Middleware)來說，就是最末端的 Action。
  它不像 Use 能串聯其他 Middleware，但 Run 還是能完整的使用 Request 及 Response。

* App.Map
  Map 是能用來處理一些簡單路由的 Middleware，可依照不同的 URL 指向不同的 Run 及註冊不同的 Use。
  主要在判斷路由規則是否符合預期，符合則執行區間內容。

  > 範例
  >
  > *tartup.cs*
  >
  > ```cs
  > // ...
  > public class Startup
  > {
  >     // ...
  >     public void Configure(IApplicationBuilder app)
  >     {
  >         app.Use(async (context, next) => 
  >         {
  >             await context.Response.WriteAsync("First Middleware in. \r\n");
  >             await next.Invoke();
  >             await context.Response.WriteAsync("First Middleware out. \r\n");
  >         });
  > 
  >         app.Map("/second", mapApp =>
  >         {
  >             mapApp.Use(async (context, next) => 
  >             {
  >                 await context.Response.WriteAsync("Second Middleware in. \r\n");
  >                 await next.Invoke();
  >                 await context.Response.WriteAsync("Second Middleware out. \r\n");
  >             });
  >             mapApp.Run(async context =>
  >             {
  >                 await context.Response.WriteAsync("Second. \r\n");
  >             });
  >         });
  > 
  >         app.Run(async context =>
  >         {
  >             await context.Response.WriteAsync("Hello World! \r\n");
  >         });
  >     }
  > }
  > ```
  >
  > 開啟網站任意連結，會顯示：
  >
  > ```
  > First Middleware in. 
  > Hello World! 
  > First Middleware out. 
  > ```
  >
  > 開啟網站 `http://localhost:5000/second`，則會顯示：
  >
  > ```
  > First Middleware in. 
  > Second Middleware in. 
  > Second. 
  > Second Middleware out. 
  > First Middleware out. 
  > ```

### 建立middleware 類別

1. Middleware class ：繼承IMiddleware(似乎也可以不繼承)

   如果在Visual studio 新增時，選擇中介程式類別，會有一些基礎方法在裡面，如果要繼承IMiddleware介面的話，要實作`public Task InvokeAsync(HttpContext context, RequestDelegate next)`方法，並移除原先生成的其他內容。

2. 註冊

   1. 全域註冊

      在Startup.cs ConfigureServices方法裡註冊所寫的Middleware名稱

      ```C#
      public class Startup
      {
          // ...
          public void Configure(IApplicationBuilder app)
          {
              app.UseMiddleware<Middleware_Class>();
              // ...
          }
      }
      ```

   2. 區域註冊

      Middleware 也可以只套用在特定的 Controller 或 Action。

      在Controller類別裡面加入以下內容

      ```C#
      // ..
      [MiddlewareFilter(typeof(Middleware_Class))]
      public class HomeController : Controller
      {
          // ...
      
          [MiddlewareFilter(typeof(Middleware_Class(can be another middleware)))]
          public IActionResult Index()
          {
              // ...
          }
      }
      ```

3. 在Configure裡面加入服務`services.AddSinglrton<Middleware_Class>();`

   這個步驟只有繼承了IMiddleware的類別才要做(如果繼承了IMiddleware卻沒有加入服務會跳Exception)

4. 處理就response不處理就next



#### 範例

新建了兩個Middleware類別，其中一個繼承了IMiddleware，另一個則無

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MVCTest.MiddleWares
{
    public class TestMVCMiddleware:IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);
        }
    }

```

```C#
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace MVCTest.MiddleWares
{
    public class TestMVCNoImpMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMVCNoImpMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {

            return _next(httpContext);
        }
    }

```



於Configure註冊，注意類別名稱寫在<泛型位置>，注意註冊順序，Middleware會按照順序被呼叫。

```C#
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			//...
            app.UseMiddleware<TestMVCMiddleware>();
            app.UseMiddleware<TestMVCNoImpMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=TestMVC}/{action=Index}/{id?}");
            });
        }
```



於ConfigureServices加入服務(只有繼承了IMiddleware的類別)

```C#
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddSingleton<TestMVCMiddleware>();
        }
```



#### Extension

指細觀察的話會發現`app.UseRouting();` ` app.UseAuthorization();` `app.UseMvc();` 等等很多預設的middleware都會用一個靜態方法包裝呼叫方式都相當簡潔。



自製的 Middleware 也可以做到一樣的事情，以上兩例修改

```C#
    public static class TestMVCMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMVCMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMVCMiddleware>();
        }
    }

    public static class TestMVCNoImpMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestMVCNoImpMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TestMVCNoImpMiddleware>();
        }
    }
```

註:Visual Studio新增的中介軟體類別已經包含擴展類別了，可以直接使用不需修改。



Configure註冊方式就可以加以修改

```C#
            app.UseMiddleware<TestMVCMiddleware>();
            app.UseMiddleware<TestMVCNoImpMiddleware>();
```

```C#
            app.UseTestMVCMiddleware();
            app.UseTestMVCNoImpMiddleware();
```



