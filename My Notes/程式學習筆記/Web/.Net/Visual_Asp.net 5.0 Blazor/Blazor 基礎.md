# Blazor

[Reference: Microsoft Blazor Tutorial](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)

[Reference: MSDN Blazor todolist sample](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/build-a-blazor-app?view=aspnetcore-5.0)

[Reference:ITHelp](https://ithelp.ithome.com.tw/users/20130058/ironman/3429)

我這次學習過程使用VSC做開發工具

## Create App

Blazor的專案分為Server 和 Web Assembly 兩種，具體比較可以看[ITHelp上的這篇文章](https://ithelp.ithome.com.tw/articles/10238867)

大致可以理解成Server包含前後端，Web Assembly差不多等同靜態網頁

### Blazor Server

老樣子使用`dotnet new`指令來建立

```
dotnet new blazorserver -o BlazorApp --no-https
```

執行看看

```
PS C:\Users\admin> dotnet new blazorserver -o BlazorApp --no-https
The template "Blazor Server App" was created successfully.
This template contains technologies from parties other than Microsoft, see https://aka.ms/aspnetcore/5.0-third-party-notices for details.

Processing post-creation actions...
Running 'dotnet restore' on BlazorApp\BlazorApp.csproj...
  正在判斷要還原的專案...
  已還原 C:\Users\admin\BlazorApp\BlazorApp.csproj (145 ms 內)。
Restore succeeded.
```



內容物

```
Mode                LastWriteTime         Length Name
----                -------------         ------ ----
d-----       2021/5/7  下午 04:52                bin
d-----       2021/5/7  下午 04:49                Data
d-----       2021/5/7  下午 04:52                obj
d-----       2021/5/7  下午 04:49                Pages
d-----       2021/5/7  下午 04:49                Properties
d-----       2021/5/7  下午 04:49                Shared
d-----       2021/5/7  下午 04:49                wwwroot
-a----       2021/5/7  下午 04:49            387 App.razor
-a----       2021/5/7  下午 04:49            195 appsettings.Development.json
-a----       2021/5/7  下午 04:49            192 appsettings.json
-a----       2021/5/7  下午 04:49            141 BlazorApp.csproj
-a----       2021/5/7  下午 04:49            717 Program.cs
-a----       2021/5/7  下午 04:49           1753 Startup.cs
-a----       2021/5/7  下午 04:49            392 _Imports.razor
```



可以直接建置執行看看

```
dotnet watch run
```

一個簡單的Blazor網頁就跑出來啦



專案最外層有`Program.cs`和`Startup.cs`乍看之下和ASP.net WEB/MVC專案架構相似



### Blazor Web Assembly



```
dotnet new blazorwasm -n BlazorDemo
```



內容物

```
Mode                LastWriteTime         Length Name
----                -------------         ------ ----
d-----      2021/5/10  下午 04:44                obj
d-----      2021/5/10  下午 04:44                Pages
d-----      2021/5/10  下午 04:44                Properties
d-----      2021/5/10  下午 04:44                Shared
d-----      2021/5/10  下午 04:44                wwwroot
-a----      2021/5/10  下午 04:44            387 App.razor
-a----      2021/5/10  下午 04:44            483 BlazorAppWasm.csproj
-a----      2021/5/10  下午 04:44            765 Program.cs
-a----      2021/5/10  下午 04:44            389 _Imports.razor
```

比較大的差別是少了`Startup.cs`



## 各檔案的作用

Server 專案和以往寫過的其他ASP.Net專案相似度較高，這次就主要拿Web Assembly專案做範本來看看各個檔案的作用為何



### Program.cs

包含做為程式進入點的`Main()`方法，不論是何種專案都預設有這個檔案，但內容有些許不同。



Server新專案預設內容如下

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

```

和許多ASP.net專案很像，進入程式後就執行指定的`Startup `class的內容



Web Assembly新專案預設內容如下

```C#
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorAppWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
```

因為Web Assembly專案並沒有`Startup.cs`，所以理所當然的在`Program.cs`做的事情也有所不同

首先實作了一個`WebAssemblyHostBuilder`物件，很顯然是一個實現了Builder Pattern的類別，最終目的就是透過

```C#
public WebAssemblyHost Build();
```

建構一個`WebAssemblyHost`物件。

```C#
public sealed class WebAssemblyHostBuilder
    {
        //
        // 摘要:
        //     Gets an Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostConfiguration
        //     that can be used to customize the application's configuration sources and read
        //     configuration attributes.
        public WebAssemblyHostConfiguration Configuration { get; }
        //
        // 摘要:
        //     Gets the collection of root component mappings configured for the application.
        public RootComponentMappingCollection RootComponents { get; }
        //
        // 摘要:
        //     Gets the service collection.
        public IServiceCollection Services { get; }
        //
        // 摘要:
        //     Gets information about the app's host environment.
        public IWebAssemblyHostEnvironment HostEnvironment { get; }
        //
        // 摘要:
        //     Gets the logging builder for configuring logging services.
        public ILoggingBuilder Logging { get; }

        //
        // 摘要:
        //     Creates an instance of Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder
        //     using the most common conventions and settings.
        //
        // 參數:
        //   args:
        //     The argument passed to the application's main method.
        //
        // 傳回:
        //     A Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.
        public static WebAssemblyHostBuilder CreateDefault(string[] args = null);
        //
        // 摘要:
        //     Builds a Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost
        //     instance based on the configuration of this builder.
        //
        // 傳回:
        //     A Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHost object.
        public WebAssemblyHost Build();
        //
        // 摘要:
        //     Registers a Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1
        //     instance to be used to create the System.IServiceProvider.
        //
        // 參數:
        //   factory:
        //     The Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1.
        //
        //   configure:
        //     A delegate used to configure the TBuilder. This can be used to configure services
        //     using APIS specific to the Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1
        //     implementation.
        //
        // 類型參數:
        //   TBuilder:
        //     The type of builder provided by the Microsoft.Extensions.DependencyInjection.IServiceProviderFactory`1.
        //
        // 備註:
        //     Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.ConfigureContainer``1(Microsoft.Extensions.DependencyInjection.IServiceProviderFactory{``0},System.Action{``0})
        //     is called by Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.Build
        //     and so the delegate provided by configure will run after all other services have
        //     been registered.
        //     Multiple calls to Microsoft.AspNetCore.Components.WebAssembly.Hosting.WebAssemblyHostBuilder.ConfigureContainer``1(Microsoft.Extensions.DependencyInjection.IServiceProviderFactory{``0},System.Action{``0})
        //     will replace the previously stored factory and configure delegate.
        public void ConfigureContainer<TBuilder>(IServiceProviderFactory<TBuilder> factory, Action<TBuilder> configure = null);
    }
```



```C#
public sealed class WebAssemblyHost : IAsyncDisposable
    {
        //
        // 摘要:
        //     Gets the application configuration.
        public IConfiguration Configuration { get; }
        //
        // 摘要:
        //     Gets the service provider associated with the application.
        public IServiceProvider Services { get; }

        //
        // 摘要:
        //     Disposes the host asynchronously.
        //
        // 傳回:
        //     A System.Threading.Tasks.ValueTask which respresents the completion of disposal.
        [AsyncStateMachine(typeof(<DisposeAsync>d__16))]
        public ValueTask DisposeAsync();
        //
        // 摘要:
        //     Runs the application associated with this host.
        //
        // 傳回:
        //     A System.Threading.Tasks.Task which represents exit of the application.
        //
        // 備註:
        //     At this time, it's not possible to shut down a Blazor WebAssembly application
        //     using imperative code. The application only stops when the hosting page is reloaded
        //     or navigated to another page. As a result the task returned from this method
        //     does not complete. This method is not suitable for use in unit-testing.
        public Task RunAsync();
    }
```



### App.razor

這個檔案在兩種專案的預設內容都相同

```xml
<Router AppAssembly="@typeof(Program).Assembly" PreferExactMatches="@true">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <LayoutView Layout="@typeof(MainLayout)">
            <p>Sorry, there's nothing at this address.</p>
        </LayoutView>
    </NotFound>
</Router>
```



用處是路由處裡，會根據有沒有找到對應的路由回傳相應的內容



### wwwroot

和其他ASP.net專案一樣用來放一些靜態資料。



### Pages / Share Folder

裡面的內容和其他ASP.net core專案很相似，作用我想也差不多，共用的放Share其他放Pages，就是檔案的副檔名都是`.razor`而不是`.cshtml`，具體有甚麼不一樣暫時不曉得，以後補充。



## Route

路由的部分因為太簡單了所以就寫在基礎篇，不另外寫了

這次只要在Pages裏頭的razor頁面開頭定義`@page`就可以了，例如以預設專案的程式碼來看

Counter.razor

```razor
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

最開頭的`@page "/counter"`代表我可以在網域名稱後加入"/counter"連結到這個頁面



## Dependency Injection

概述的部分我在ASP.net core MVC筆記裡面有提過了，這邊就不重複了

### [複習]ASP.net core MVC專案中的DI

#### 註冊

複習一下，在ASP.net core專案中，我們透過DI容器來實作物件

例如

```C#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }
}
```

> **services** 就是一個 DI 容器。
> 此例把 MVC 的服務註冊到 DI 容器，等到需要用到 MVC 服務時，才從 DI 容器取得物件實例。

基本上除了靜態類別以外都可以丟進去

例如(Sample是自定義類別)

```C#
public interface ISample
{
    int Id { get; }
}

public class Sample : ISample
{
    private static int _counter;
    private int _id;

    public Sample()
    {
        _id = ++_counter;
    }

    public int Id => _id;
}
```



```C#
// ...
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ...
        services.AddScoped<ISample, Sample>();
    }
}
```

- 第一個泛型為注入的類型
  建議用 Interface 來包裝，這樣在才能把相依關係拆除。
- 第二個泛型為實做的類別

#### 注入

ASP.NET Core 的 DI 是採用 Constructor Injection，也就是說會把實例化的物件從建構子傳入。
如果要取用 DI 容器內的物件，只要在建構子加入相對的 Interface 即可。例如：

*Controllers\HomeController.cs*

```cs
public class HomeController : Controller
{
    private readonly ISample _sample;

    public HomeController(ISample sample)
    {
        _sample = sample;
    }

    public string Index() {
        return $"[ISample]\r\n"
             + $"Id: {_sample.Id}\r\n"
             + $"HashCode: {_sample.GetHashCode()}\r\n"
             + $"Tpye: {_sample.GetType()}";
    }
}
```

輸出內容如下：

```
[ISample]
Id: 1
HashCode: 14145203
Tpye: MyWebsite.Sample
```

ASP.NET Core 實例化 Controller 時，發現建構子有 ISample 這個類型的參數，就把 Sample 的實例注入給該 Controller。

> 每個 Request 都會把 Controller 實例化，所以 DI 容器會從建構子注入 ISample 的實例，把 sample 存到欄位 _sample 中，就能確保 Action 能夠使用到被注入進來的 ISample 實例。



當然還有在其他地方如`View` `MiddleWare`等等注入的方法，詳情就看MVC筆記，不贅述了



---

### Blazor 的 DI

回到正題，blazor專案與ASP.net core專案有許多相似之處，所以就根據上面複習筆記的內容來說明這次有甚麼相同或相異之處。

#### 註冊

##### 無須註冊的內容

- HttpClient：提供請求http資源的方法
- IJSRuntime：提供Javascript runtime物件，注入後可使用 Javascript
- NavigationManager：包含處理路由導向和狀態的helper



##### 生命週期

同ASP.net core 專案

1. Singleton — 在第一次request產生物件後就會一直存在，直到整個應用程式關閉，所以只要建立後就會一直共用。
2. Scoped — 每次request時，都會new一個新的物件，在request期間都會共用該物件，這個物件會在request結束時Dispose。
3. Transient — 每次要求時就會產生一個物件。





##### Blazor Server

Blazor Server 專案與先前的ASP.net Core專案十分的相似

DI容器同樣是`Startup.ConfigureServices(IServiceCollection services)`中的`services`

以下是專案預設內容節錄

```C#
	public class Startup
	{
        //...
		public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }
        //...
	}
```



註冊方式其實可以細分

```C#
//指定介面和實作類別
services.AddSingleton<IMovieRepository, MovieRepository>();

//直接註冊new好的物件
MovieRepository movieRepository = new MovieRepository();
services.AddSingleton(movieRepository);

//指定介面和new好的物件
services.AddSingleton<IMovieRepository>(movieRepository);
```



##### Blazor Assembly



Assembly 專案的註冊方式稍微不同，因為沒有Startup.cs了取而代之的是直接將註冊寫到`Program.cs`裡的`Main`方法

一樣拿專案預設內容作範例

```C#
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
```



注意：注入的寫法也有一點點的區別，因為沒有`IServiceCollection`實例，所以使用靜態方法。

```C#
public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory) where TService : class;
```



裡如我這邊想要註冊我自己寫的Sample類別，我可以這樣寫

```C#
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<ISample, Sample>();

            await builder.Build().RunAsync();
        }
    }
    public interface ISample { }
    public class Sample : ISample { }
```



#### 注入



拿上面寫的Sample為例子，如果我要在Component中注入可以這樣寫

```C#
@page "/TestComponent"
@inject Sample sample;
<h3>TestComponent</h3>
<p class="content">@sample</p>
```



其實就是在ASP.net core MVC 