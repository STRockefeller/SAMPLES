# Route Note

[Reference ItHelp](https://ithelp.ithome.com.tw/articles/10203560)

[Reference ItHelp](https://ithelp.ithome.com.tw/articles/10193898)

## Abstract

> ASP.NET Core 透過路由(Routing)設定，將定義的 URL 規則找到相對應行為；當使用者 Request 的 URL 滿足特定規則條件時，則自動對應到相符的行為處理。

> 在**ASP.Net Core**中統一將路由(**Route**)以**Middleware**的形式進行包裝。



## 路由註冊

> RouterMiddleware 的路由註冊方式大致分為兩種：
>
> - 廣域註冊。如：`MapRoute`。
> - 區域註冊。如：`RouteAttribute`。
>
> 預設路由的順序如下：
>
> ![[鐵人賽 Day07] ASP.NET Core 2 系列 - 路由(Routing) - 流程](https://blog.johnwu.cc/images/i07-1.png)

> 在 *Startup.cs* 的 `ConfigureServices` 加入 Routing 的服務，並在 `Configure` 定義路由規則：
>
> *Startup.cs*
>
> ```cs
> // ...
> public class Startup
> {
>     public void ConfigureServices(IServiceCollection services)
>     {
>         services.AddRouting();
>     }
> 
>     public void Configure(IApplicationBuilder app)
>     {
>         var defaultRouteHandler = new RouteHandler(context =>
>         {
>             var routeValues = context.GetRouteData().Values;
>             return context.Response.WriteAsync($"Route values: {string.Join(", ", routeValues)}");
>         });
> 
>         var routeBuilder = new RouteBuilder(app, defaultRouteHandler);
>         routeBuilder.MapRoute("default", "{first:regex(^(default|home)$)}/{second?}");
> 
>         routeBuilder.MapGet("user/{name}", context => {
>             var name = context.GetRouteValue("name");
>             return context.Response.WriteAsync($"Get user. name: {name}");
>         });
> 
>         routeBuilder.MapPost("user/{name}", context => {
>             var name = context.GetRouteValue("name");
>             return context.Response.WriteAsync($"Create user. name: {name}");
>         });
> 
>         var routes = routeBuilder.Build();
>         app.UseRouter(routes);
>     }
> }
> ```
>
> 可以看到上面程式碼，建立了兩個物件：
>
> - **RouteHandler**：這個物件如同簡單路由的 `Run` 事件，當路由成立的時候，就會執行這個事件。
>
> - RouteBuilder
>
>   ：在這個物件定義路由規則，當 Requset URL 符合規則就會執行該事件。
>
>   - **MapRoute**：預設的路由規則，可以支援正規表示式(Regular Expressions)。
>   - **MapGet** & **MapPost**
>     HTTP Method 路由：同樣的 URL 可以透過不同的 HTTP Method，對應不同的事件。
>
> 第一個路由 `MapRoute` 定義：
>
> - URL 第一層透過正規表示式必需是 **default** 或 **home**，並放到路由變數 *first* 中。
> - URL 第二層可有可無，如果有的話，放到路由變數 *second* 中。
>
> 第二個路由 `MapGet` 定義：
>
> - 指定要是 HTTP Get
> - URL 第一層必需是 **user**。
> - URL 第二層必需要有值，放到路由變數 *name* 中。
>
> 第三個路由 `MapPost` 定義：
>
> - 指定要是 HTTP Post
> - URL 第一層必需是 **user**。
> - URL 第二層必需要有值，放到路由變數 *name* 中。
>
> 以上設定的路由結果如下：
>
> - `http://localhost:5000/default` 會顯示：
>   Route values: [first, default]
> - `http://localhost:5000/home/about` 會顯示：
>   Route values: [first, home], [second, about]
> - `http://localhost:5000/user/john` 透過 HTTP Get 會顯示：
>   Get user. name: john
> - `http://localhost:5000/user/john` 透過 HTTP Post 會顯示：
>   Create user. name: john

## RouteAttribute

> 預設 RouteAttribute 的優先順序高於 Startup 註冊的 MapRoute，所以當使用 `[Route]` 後，原本的 MapRoute 將不再對 Controller 或 Action 產生作用。
>
> ```cs
> [Route("[controller]")]
> public class UserController : Controller
> {
>     [Route("")]
>     public IActionResult Profile()
>     {
>         return View();
>     }
> 
>     [Route("change-password")]
>     public IActionResult ChangePassword()
>     {
>         return View();
>     }
> 
>     [Route("[action]")]
>     public IActionResult Other()
>     {
>         return View();
>     }
> }
> ```
>
> 以上設定的路由結果如下：
>
> - `http://localhost:5000/user` 會對應到 UserController 的 Profile()。
> - `http://localhost:5000/user/change-password` 會對應到 UserController 的 ChangePassword()。
> - `http://localhost:5000/user/other` 會對應到 UserController 的 Other()。
>
> > 若 Controller 設定了 `[Route]`，Action 就要跟著加 `[Route]`，不然會發生錯誤。
>
> 如果只有特定的 Action 需要改路由，也可以只加 Action。如下：
>
> ```cs
> public class UserController : Controller
> {
>     public IActionResult Profile()
>     {
>         return View();
>     }
> 
>     [Route("change-password")]
>     public IActionResult ChangePassword()
>     {
>         return View();
>     }
> 
>     public IActionResult Other()
>     {
>         return View();
>     }
> }
> ```
>
> - `http://localhost:5000/user/profile` 會對應到 UserController 的 Profile()。
> - `http://localhost:5000/change-password` 會對應到 UserController 的 ChangePassword()。
> - `http://localhost:5000/user/other` 會對應到 UserController 的 Other()。
>
> > 注意！如果 `[Route]` 是設定在 Action，路徑是由網站根路徑開始算。

## Sample

看似挺複雜的，這邊就來統整一下，另外寫個範例測試

首先Route主要分為兩種

* 寫在Startup.cs
* 寫在Controller

其中後者的寫法優先度高於前者

這邊以後者的方式作範例

首先把Route測試用的MVC都寫出來

TestRouteModel.cs

```C#
namespace MVCTest.Models
{
    public class TestRouteModel
    {
        public string modelString = "Now you are in test route";
    }
}

```

TestRouteController.cs

```C#
using Microsoft.AspNetCore.Mvc;

namespace MVCTest.Controllers
{
    public class TestRouteController : Controller
    {
        public IActionResult Index()
        {
            Models.TestRouteModel testRouteModel = new Models.TestRouteModel();
            return View(testRouteModel);
        }
    }
}

```

TestRoute/index.cshtml

```html
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model  TestRouteModel
<b>@Model.modelString</b>
```



然後在起始畫面寫一個連結用按鈕省麻煩

```html
<p><button type="button" class="btn btn-primary" id="btnGO">GO</button></p>
```

```javascript
$("#btnGO").click(function(){
    window.location.href="./RouteTest";
})
```



執行起來點連結顯示http response 404找不到網頁，因為還沒設定Route

接著來設定Route

在Controller中定義

改寫TestRouteController.cs 在IActionResult方法前面加入`[Route("RouteTest")]`

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCTest.Controllers
{
    public class TestRouteController : Controller
    {
        [Route("RouteTest")]
        public IActionResult Index()
        {
            Models.TestRouteModel testRouteModel = new Models.TestRouteModel();
            return View(testRouteModel);
        }
    }
}
```



接著再次執行就可以順利連結到內容了。



一般會將Route分層，如下

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCTest.Controllers
{
    [Route("RouteTest")]
    public class TestRouteController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            Models.TestRouteModel testRouteModel = new Models.TestRouteModel();
            return View(testRouteModel);
        }
    }
}
```

(如此一來頁面要到./RouteTest/Index/ 才找得到)

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCTest.Controllers
{
    [Route("RouteTest")]
    public class TestRouteController : Controller
    {
        [Route("")]
        public IActionResult Index()
        {
            Models.TestRouteModel testRouteModel = new Models.TestRouteModel();
            return View(testRouteModel);
        }
    }
}
```

(可以在./RouteTest/ 找到)