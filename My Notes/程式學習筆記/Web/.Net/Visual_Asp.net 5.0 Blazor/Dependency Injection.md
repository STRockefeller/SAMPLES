# Dependency Injection

概述的部分我在ASP.net core MVC筆記裡面有提過了，這邊就不重複了

本來是打算合併到基礎篇筆記裡，但是我發現我對DI的熟悉度不是很夠，所以還是另外紀錄一篇盡量寫詳細點。

## [複習]ASP.net core MVC專案中的DI

### 註冊

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

### 注入

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

## Blazor 的 DI

回到正題，blazor專案與ASP.net core專案有許多相似之處，所以就根據上面複習筆記的內容來說明這次有甚麼相同或相異之處。

### 註冊

#### 無須註冊的內容

- HttpClient：提供請求http資源的方法
- IJSRuntime：提供Javascript runtime物件，注入後可使用 Javascript
- NavigationManager：包含處理路由導向和狀態的helper



#### 生命週期

同ASP.net core 專案

1. Singleton — 在第一次request產生物件後就會一直存在，直到整個應用程式關閉，所以只要建立後就會一直共用。
2. Scoped — 每次request時，都會new一個新的物件，在request期間都會共用該物件，這個物件會在request結束時Dispose。
3. Transient — 每次要求時就會產生一個物件。



補充(來自[MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-5.0&pivots=webassembly)):

> Blazor WebAssembly 應用程式目前不具有 DI 範圍的概念。 `Scoped`-註冊的服務行為類似 `Singleton` 服務。



#### Blazor Server

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
// Register an existing object instance
services.AddSingleton(existingObject);

// Register an existing object instance, injected via an interface
services.AddSingleton<ISomeInterface>(implementingInstance);

// Lazy created instance, with manual build process and access to the current IServiceProvider
services.AddSingleton<ISomeInterface>(serviceProvider => new ImplementingType(.......));
```



#### Blazor Assembly



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



### 注入



拿上面寫的Sample為例子，如果我要在Component中注入可以這樣寫

```C#
@page "/TestComponent"
@inject Sample sample;
<h3>TestComponent</h3>
<p class="content">@sample</p>
```



其實就是在ASP.net core MVC 專案中VIEW的注入方式。





## SAMPLE

隨便寫點東西來練習看看。

訂個簡單的目標，**把Blazor WebAssembly 專案中Counter Page的計數器透過DI改寫成不會隨著進出頁面而重置的類型**。

根據這個要求我希望有一個**唯一的計數器物件**(也可以透過static property實現，但這樣就沒意思了。)

所以理所當然的選擇將服務註冊為**Singleton**

類別如下(每次被實例化的時候print出一段訊息，方便確認是否有實現單例)

```C#
    public interface IDICounter 
    {
        public void increase();
    }
    public class DICounter : IDICounter
    {
        public int count { get; set; }
        public DICounter()
        {
            count = 0;
            Console.WriteLine("DICounter 被實例化了" + this.GetHashCode());
        }
        public void increase()
        {
            count++;
        }
    }
```

接著在Main進行註冊

```C#
builder.Services.AddSingleton<IDICounter, DICounter>();
```



Page/Counter.razor

```C#
@page "/counter"
@inject BlazorAppWasm.DICounter counter;

<h1>Counter</h1>

<p>Current count: @counter.count</p>

<button class="btn btn-primary" @onclick="onclickFunction">Click me</button>

@code{
    private void onclickFunction()
    {
        Console.WriteLine("Clicked");
        counter.increase();
    }
}
```

執行結果是我沒辦法進入Counter頁面，點選連結(或修改網址)後畫面會停留在上一個待過的頁面，沒有跳例外，也沒有當掉，就是無法進入Counter頁面而已。然後`DICounter`的建構式並沒有被執行過。

試過之後才知道我對這個機制真的很不了解。

以往在MVC專案中，習慣使用於Controller注入的方法，都是在Controller的建構式中注入已實例化的物件，但是現在這個注入到razor component中的做法，我甚至連物件應該在哪個階段被實例化都不曉得(是`OnInitialized`還是`Counter`的建構式?還是有其他可能?)

好吧，換個方式，這次我直接在Main中實例化物件再進行註冊。

```C#
builder.Services.AddSingleton<IDICounter>(dic=>new DICounter());
```

razor component 如下，基本上沒有改動，只是加了一些敘述方便判斷執行順序。

```C#
@page "/counter"
@inject BlazorAppWasm.DICounter DIC;

<h1>Counter</h1>

<p>Current count: @DIC.count</p>

<button class="btn btn-primary" @onclick="onclickFunction">Click me</button>

@code{       
    public Counter()
    {
        Console.WriteLine("Counter Page Constructor");
    }
    protected override void OnInitialized()
    {
        Console.WriteLine("Counter Page OnInitialized");
        base.OnInitialized();
    }
    private void onclickFunction()
    {
        Console.WriteLine("Clicked");
        DIC.increase();
    }
}
```



執行結果符合預期，成功使計數器不會因為換頁而重置。

順便一提，我第一次點進Counter頁面的時候顯示

```
Counter Page Constructor
DICounter 被實例化了478255340
Counter Page OnInitialized
```

第二次則是

```
Counter Page Constructor
Counter Page OnInitialized
```

* `DICounter`只有在第一次載入時被實例化，之後都是直接使用先前實例化的物件，成功實現單例。
* 執行順序是 Component的建構式=>DI物件的實例化=>OnInitialized方法





### 疑難待解

依然無法確定是不是如果要使用addSingleton就必須傳入已實例化的物件

第一種寫法真的是錯的嗎?





## 補充

### ILoger

剛發現這個滿好用的東西，內容沒有很多不適合另開筆記，所以決定先加入DI筆記的內容。

顧名思義這一個用來記錄Log的介面。我們可以透過DI注入到Page中。

參考資料:[MSDN](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/logging?view=aspnetcore-5.0&pivots=webassembly)

Loggers respect app startup configuration.

The `using` directive for [Microsoft.Extensions.Logging](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging) is required to support [IntelliSense](https://docs.microsoft.com/en-us/visualstudio/ide/using-intellisense) completions for APIs, such as [LogWarning](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions.logwarning) and [LogError](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.loggerextensions.logerror).

The following example demonstrates logging with an [ILogger](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger) in components.

`Pages/Counter.razor`:



```razor
@page "/counter"
@using Microsoft.Extensions.Logging;
@inject ILogger<Counter> logger;

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        logger.LogWarning("Someone has clicked me!");

        currentCount++;
    }
}
```

The following example demonstrates logging with an [ILoggerFactory](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.iloggerfactory) in components.

`Pages/Counter.razor`:



```razor
@page "/counter"
@using Microsoft.Extensions.Logging;
@inject ILoggerFactory LoggerFactory

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        var logger = LoggerFactory.CreateLogger<Counter>();
        logger.LogWarning("Someone has clicked me!");

        currentCount++;
    }
}
```

