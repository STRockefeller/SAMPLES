# Middleware Note

[reference MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1)

[reference ithelp](https://ithelp.ithome.com.tw/articles/10203041)

[reference ithelp](https://ithelp.ithome.com.tw/articles/10192682)

![imgMiddleware](https://blog.johnwu.cc/images/i03-1.png)

從發出請求(Request)之後，到接收回應(Response)這段來回的途徑上，用來處理特定用途的程式。比較常見的**Middleware**有身份驗證(Identity)、路由(Routing)或回應壓縮(Response Compression)等。

**Middleware**預設在`Startup`中`Configure`設定，**ASP.Net Core**預設內建了許多好用的**Middleware**，如驗證(Authentication)、回應壓縮(Response Compression)、URL重寫(URL Rewriting)等，如需要更詳細的資訊可以參考[MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1)。

## 於Startup中使用middleware

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
  >  // ...
  >  public void Configure(IApplicationBuilder app)
  >  {
  >      app.Use(async (context, next) => 
  >      {
  >          await context.Response.WriteAsync("First Middleware in. \r\n");
  >          await next.Invoke();
  >          await context.Response.WriteAsync("First Middleware out. \r\n");
  >      });
  > 
  >      app.Use(async (context, next) => 
  >      {
  >          await context.Response.WriteAsync("Second Middleware in. \r\n");
  >          await next.Invoke();
  >          await context.Response.WriteAsync("Second Middleware out. \r\n");
  >      });
  > 
  >      app.Use(async (context, next) => 
  >      {
  >          await context.Response.WriteAsync("Third Middleware in. \r\n");
  >          await next.Invoke();
  >          await context.Response.WriteAsync("Third Middleware out. \r\n");
  >      });
  > 
  >      app.Run(async (context) =>
  >      {
  >          await context.Response.WriteAsync("Hello World! \r\n");
  >      });
  >  }
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
  >  // ...
  >  public void Configure(IApplicationBuilder app)
  >  {
  >      app.Use(async (context, next) => 
  >      {
  >          await context.Response.WriteAsync("First Middleware in. \r\n");
  >          await next.Invoke();
  >          await context.Response.WriteAsync("First Middleware out. \r\n");
  >      });
  > 
  >      app.Map("/second", mapApp =>
  >      {
  >          mapApp.Use(async (context, next) => 
  >          {
  >              await context.Response.WriteAsync("Second Middleware in. \r\n");
  >              await next.Invoke();
  >              await context.Response.WriteAsync("Second Middleware out. \r\n");
  >          });
  >          mapApp.Run(async context =>
  >          {
  >              await context.Response.WriteAsync("Second. \r\n");
  >          });
  >      });
  > 
  >      app.Run(async context =>
  >      {
  >          await context.Response.WriteAsync("Hello World! \r\n");
  >      });
  >  }
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

## 建立middleware 類別

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

3. 在Configure裡面加入服務`services.AddSingleton<Middleware_Class>();`

   這個步驟只有繼承了`IMiddleware`的類別才要做(如果繼承了`IMiddleware`卻沒有加入服務會跳Exception)

4. 處理就response不處理就next



### 範例

新建了兩個Middleware類別，其中一個繼承了`IMiddleware`，另一個則無

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



### Extension

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



