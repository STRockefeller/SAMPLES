# Cookies and Session

Reference:

https://ithelp.ithome.com.tw/articles/10249151

https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?tabs=aspnetcore2x&view=aspnetcore-5.0

---

## Review with Questions

請簡短說明cookies和session的區別以及使用時機。 [Ans:Cookies](#Cookies) [Ans:Session](#Session)

試著舉例能加強Session安全性的具體作法。 [Ans](#安全性)

---

## 簡介

基本上 HTTP 是沒有紀錄狀態的協定，但可以透過 Cookies 將 Request 來源區分出來，並將部分資料暫存於 Cookies 及 Session，是寫網站常用的用戶資料暫存方式。
本篇將介紹如何在 ASP.NET Core 使用 Cookie 及 Session。

> 同步發佈至個人部落格：
> [John Wu's Blog - [鐵人賽 Day11\] ASP.NET Core 2 系列 - Cookies & Session](https://blog.johnwu.cc/article/ironman-day11-asp-net-core-cookies-session.html)

### Cookies

Cookies 是將用戶資料存在 Client 的瀏覽器，每次 Request 都會把 Cookies 送到 Server。
在 ASP.NET Core 中要使用 Cookie，可以透過 `HttpContext.Request` 及 `HttpContext.Response` 存取：

*Startup.cs*

```cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app)
        {
            app.Run(async (context) =>
            {
                string message;

                if (!context.Request.Cookies.TryGetValue("Sample", out message))
                {
                    message = "Save data to cookies.";
                }
                context.Response.Cookies.Append("Sample", "This is Cookies.");
                // 刪除 Cookies 資料
                //context.Response.Cookies.Delete("Sample");

                await context.Response.WriteAsync($"{message}");
            });
        }
    }
}
```

從 HTTP 可以看到傳送跟收到的 Cookies 資訊：

![[鐵人賽 Day11] ASP.NET Core 2 系列 - Cookies & Session - Cookies](https://blog.johnwu.cc/images/i11-1.png)

> 當存在 Cookies 的資料越多，封包就會越大，因為每個 Request 都會帶著 Cookies 資訊。

### Session

Session 是透過 Cookies 內的唯一識別資訊，把用戶資料存在 Server 端記憶體、NoSQL 或資料庫等。
要在 ASP.NET Core 使用 Session 需要先加入兩個服務：

- **Session 容器**
  Session 可以存在不同的地方，透過 DI `IDistributedCache` 物件，讓 Session 服務知道要將 Session 存在哪邊。
  *(之後的文章會介紹到 `IDistributedCache` 分散式快取)*
- **Session 服務**
  在 DI 容器加入 Session 服務。並將 Session 的 Middleware 加入 Pipeline。

*Startup.cs*

```cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 將 Session 存在 ASP.NET Core 記憶體中
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            // SessionMiddleware 加入 Pipeline
            app.UseSession();

            app.Run(async (context) =>
            {
                context.Session.SetString("Sample", "This is Session.");
                string message = context.Session.GetString("Sample");
                await context.Response.WriteAsync($"{message}");
            });
        }
    }
}
```

HTTP Cookies 資訊如下：

![[鐵人賽 Day11] ASP.NET Core 2 系列 - Cookies & Session - Session](https://blog.johnwu.cc/images/i11-2.png)

可以看到多出了 `.AspNetCore.Session`，`.AspNetCore.Session` 就是 Session 的唯一識別資訊。
每次 Request 時都會帶上這個值，當 Session 服務取得這個值後，就會去 Session 容器找出專屬這個值的 Session 資料。

#### 物件型別

以前 ASP.NET 可以將物件型別直接存放到 Session，現在 ASP.NET Core Session 不再自動序列化物件到 Sesson。
如果要存放物件型態到 Session 就要自己序列化了，這邊以 JSON 格式作為範例：

*Extensions\SessionExtensions.cs*

```cs
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MyWebsite.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
```

透過上例擴充方法，就可以將物件存取至 Session，如下：

```cs
using MyWebsite.Extensions;
// ...
var user = context.Session.GetObject<UserModel>("user");
context.Session.SetObject("user", user);
```

#### 安全性

雖然 Session 資料都存在 Server 端看似安全，但如果封包被攔截，只要拿到 `.AspNetCore.Session` 就可以取到該用戶資訊，也是有風險。
有些安全調整建議實作：

- **SecurePolicy**
  限制只有在 HTTPS 連線的情況下，才允許使用 Session。如此一來變成加密連線，就不容易被攔截。
- **IdleTimeout**
  修改合理的 Session 到期時間。預設是 20 分鐘沒有跟 Server 互動的 Request，就會將 Session 變成過期狀態。
  (20分鐘有點長，不過還是要看產品需求。)
- **Name**
  沒必要將 Server 或網站技術的資訊爆露在外面，所以預設 Session 名稱 `.AspNetCore.Session` 可以改掉。

```cs
// ...
public void ConfigureServices(IServiceCollection services)
{
    services.AddDistributedMemoryCache();
    services.AddSession(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.Name = "mywebsite";
        options.IdleTimeout = TimeSpan.FromMinutes(5);
    });
}
```

### 強型別

由於 Cookies 及 Session 預設都是使用字串的方式存取資料，弱型別無法在開發階段判斷有沒有打錯字，還是建議包裝成強型別比較好。
而且直接存取 Cookies/Session 的話邏輯相依性太強，對單元測試很不友善，所以還是建議包裝一下。

*Wappers\SessionWapper.cs*

```cs
using Microsoft.AspNetCore.Http;
using MyWebsite.Extensions;
// ...

public interface ISessionWapper
{
    UserModel User { get; set; }
}

public class SessionWapper : ISessionWapper
{
    private static readonly string _userKey = "session.user";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionWapper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ISession Session
    {
        get
        {
            return _httpContextAccessor.HttpContext.Session;
        }
    }

    public UserModel User
    {
        get
        {
            return Session.GetObject<UserModel>(_userKey);
        }
        set
        {
            Session.SetObject(_userKey, value);
        }
    }
}
```

在 DI 容器中加入 `IHttpContextAccessor` 及 `ISessionWapper`，如下：

*Startup.cs*

```cs
// ...
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSingleton<ISessionWapper, SessionWapper>();
}
```

- **IHttpContextAccessor**
  ASP.NET Core 實作了 `IHttpContextAccessor`，讓 `HttpContext` 可以輕鬆的注入給需要用到的物件使用。
  由於 `IHttpContextAccessor` 只是取用 `HttpContext` 實例的接口，用 **Singleton** 的方式就可以供其它物件使用。

在 Controller 就可以直接注入 `ISessionWapper`，以強型別的方式存取 Session，如下：

*Controllers/HomeController.cs*

```cs
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Wappers;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISessionWapper _sessionWapper;

        public HomeController(ISessionWapper sessionWapper)
        {
            _sessionWapper = sessionWapper;
        }

        public IActionResult Index()
        {
            var user = _sessionWapper.User;
            _sessionWapper.User = user;
            return Ok(user);
        }
    }
}
```



## 實作

(以下內容以asp.net 5.0為主)

### Practice-Cookies

首先是View的部分簡單建立個輸入畫面和post邏輯

`Views/CookiesAndSession/Index.cshtml`

```cshtml
<div class="form-group">
    <label for="">Title</label>
    <input type="text"
           class="form-control" name="" id="inpTitle" aria-describedby="helpId" placeholder="">
</div>
<div class="form-group">
    <label for="">Message</label>
    <input type="text"
           class="form-control" name="" id="inpMessage" aria-describedby="helpId" placeholder="">
</div>
<button type="button" class="btn btn-primary" id="btnWriteCookies">Write Cookies</button>
<script>
    $("#btnWriteCookies").click(function () {
        var data = new Object();
        data[0] = $("#inpTitle").val();
        data[1] = $("#inpMessage").val();
        $.post("/wc", data);
    });
</script>
```

Controller-不做甚麼事，只是負責把View顯示出來

`Controllers\CookiesAndSessionController.cs`

```cs
using Microsoft.AspNetCore.Mvc;

namespace WebApplicationMVC0714.Controllers
{
    [Route("/CookiesAndSession")]
    public class CookiesAndSessionController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
```

Middleware-執行寫入cookie的動作

`MiddleWares\CookiesAndSessionMiddleWare.cs`

```cs
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplicationMVC0714.MiddleWares
{
    public class CookiesAndSessionMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path.EndsWith("/wc"))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string title = form["0"];
                string message = form["1"];
                context.Response.Cookies.Append(title,message);
                await context.Response.WriteAsync($"Write Cookie {title} : {message}");
            }
            await next.Invoke(context);
        }
    }
}
```

註冊DI容器並使用

```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        //...
        services.AddSingleton<CookiesAndSessionMiddleWare>();
        //...
    }
	//...
    app.UseMiddleware<CookiesAndSessionMiddleWare>();
    //...
}
```





成果

![](https://i.imgur.com/8T1F4ly.png)

加入讀取Cookies後

![](https://i.imgur.com/gEj6HOO.png)



### Practice-Session

延續上方實作範例

註冊Session並加入Middleware

```cs
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebsite
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //...
            services.AddDistributedMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app)
        {
            //...
            app.UseSession();
			//...
        }
    }
}
```

其他部分寫起來和cookie其實沒差很多，不特別說明了

成果

![](https://i.imgur.com/sqEINCp.png)

![](https://i.imgur.com/d6eKVT1.png)

![](https://i.imgur.com/7irRRSe.png)

![](https://i.imgur.com/tTSdWHu.png)



### 完整程式碼

View

```cs
<div class="form-group">
    <label for="">Title</label>
    <input type="text"
           class="form-control" name="" id="inpTitle" aria-describedby="helpId" placeholder="">
</div>
<div class="form-group">
    <label for="">Message</label>
    <input type="text"
           class="form-control" name="" id="inpMessage" aria-describedby="helpId" placeholder="">
</div>
<button type="button" class="btn btn-primary" id="btnWriteCookies">Write Cookies</button>
<button type="button" class="btn btn-primary" id="btnReadCookies">Read Cookies</button>
<button type="button" class="btn btn-primary" id="btnWriteSession">Write Session</button>
<button type="button" class="btn btn-primary" id="btnReadSession">Read Session</button>
<script>
    $(".btn").click(function (sender) {
        var data = new Object();
        data[0] = $("#inpTitle").val();
        data[1] = $("#inpMessage").val();
        var postPath;
        switch (sender.target.id) {
            case "btnWriteCookies":
                postPath = "/wc";
                break;
            case "btnReadCookies":
                postPath = "/rc";
                break;
            case "btnWriteSession":
                postPath = "/ws";
                break;
            case "btnReadSession":
                postPath = "/rs";
                break;
            default:
                postPath = "";
                break;
        }
        $.post(postPath, data);
    });
</script>
```



Middleware

```CS
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplicationMVC0714.MiddleWares
{
    public class CookiesAndSessionMiddleWare : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path.EndsWith("/wc"))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string title = form["0"];
                string message = form["1"];
                context.Response.Cookies.Append(title,message);
                await context.Response.WriteAsync($"Write Cookie {title} : {message}");
            }
            else if (path.EndsWith("/rc"))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string title = form["0"];
                string message;
                await context.Response.WriteAsync(context.Request.Cookies.TryGetValue(title, out message) ?
                    $"Read Cookie {title} \nThe value is {message}" :
                    "Read data fail");
            }
            else if (path.EndsWith("/ws"))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string title = form["0"];
                string message = form["1"];
                context.Session.SetString(title,message);
                await context.Response.WriteAsync($"Write Session {title} : {message}");
            }
            else if (path.EndsWith("/rs"))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string title = form["0"];
                string message = context.Session.GetString(title);
                await context.Response.WriteAsync($"Read Session {title} \nThe value is {message}" );
            }
            await next.Invoke(context);
        }
    }
}
```

