# Web api

[Reference:MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio)

## MSDN Tutorial

總之先照著微軟的教學一步步做個簡單的API吧

### 環境

Visual Studio 2019 v16.4 以上

.Net Core 3.1 SDK 以上

### 專案建立

![](https://i.imgur.com/lte6Ywt.png)

WEB應用程式-->API

建立完後直接執行看看，得到以下json

```json
[{"date":"2020-10-13T08:21:28.8393405+08:00","temperatureC":29,"temperatureF":84,"summary":"Bracing"},{"date":"2020-10-14T08:21:28.8455282+08:00","temperatureC":4,"temperatureF":39,"summary":"Cool"},{"date":"2020-10-15T08:21:28.8455337+08:00","temperatureC":-2,"temperatureF":29,"summary":"Mild"},{"date":"2020-10-16T08:21:28.8455341+08:00","temperatureC":37,"temperatureF":98,"summary":"Cool"},{"date":"2020-10-17T08:21:28.8455344+08:00","temperatureC":2,"temperatureF":35,"summary":"Scorching"}]
```

內容格式應該是來自專案預設的`WeatherForecast.cs`

```C#
using System;

namespace TodoApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
```

### 新增模型類別

依範例指示再專案中新增`Models`資料夾並新增`TodoItem.cs`，內容如下

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
```

### 新增資料庫內容

MSDN是使用EF，正好沒有使用過，順便學習一下

先安裝NuGet套件 **Microsoft.EntityFrameworkCore.SqlServer**

接著於`Models`底下新增`TodoContext.cs`，內容如下

```C#
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
```

### Register the database context

> In ASP.NET Core, services such as the DB context must be registered with the [dependency injection (DI)](https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1) container. The container provides the service to controllers.

在`Startup.cs`加入以下內容

```C#
//...
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
//...
namespace TodoApi
{
    public class Startup
    {
        //...
        public void ConfigureServices(IServiceCollection services)
        {
            //...
                        services.AddDbContext<TodoContext>(opt =>
               opt.UseInMemoryDatabase("TodoList"));
            //...
        }
        //...
    }
}
```

- 將資料庫內容新增至 DI 容器。
- 指定資料庫內容將會使用記憶體內部資料庫。

### Scaffold a controller

於`Controllers`資料夾右鍵新增`Scaffold項目`，新增`使用 Entity Framework 執行動作的 API 控制器`

![](https://i.imgur.com/zmo1eoy.png)

![](https://i.imgur.com/nqLtUfu.png)

接著報錯，跟說好得不一樣啊

![](https://i.imgur.com/hXCkHrn.png)

嘗試新增一個Constructor無引數多載，還是報錯，只是內容不太一樣

![](https://i.imgur.com/zwRGCBe.png)

用Terminal去跑，依然失敗

![](https://i.imgur.com/tHDaC8l.png)

