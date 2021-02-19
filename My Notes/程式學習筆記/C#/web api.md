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

#### 找不到`UseInMemoryDatabase`方法

跟著範例做到這裡的時候跳錯誤查[MSDN](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.inmemorydbcontextoptionsextensions.useinmemorydatabase?view=efcore-3.1)得知:

> - Namespace:
>
>   [Microsoft.EntityFrameworkCore](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore?view=efcore-3.1)
>
> - Assembly:
>
>   Microsoft.EntityFrameworkCore.InMemory.dll
>
> - Package:
>
>   Microsoft.EntityFrameworkCore.InMemory v3.1.0

Namespace沒錯，那就找`Microsoft.EntityFrameworkCore.InMemory.dll`去NuGet找`Microsoft.EntityFrameworkCore.InMemory`還真的找到，安裝完後就OK啦

### Scaffold a controller

於`Controllers`資料夾右鍵新增`Scaffold項目`，新增`使用 Entity Framework 執行動作的 API 控制器`

![](https://i.imgur.com/zmo1eoy.png)

![](https://i.imgur.com/nqLtUfu.png)



生出來的東西長這樣

TodoItemsController.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        private bool TodoItemExists(long id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
```

### Examine the PostTodoItem create method

在[上一步](Scaffold a controller)生成的程式碼裡面找到這段

```C#
        // POST: api/TodoItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }
```

將`return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);`

改成`return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);`

以下是MSDN原文

> 上述程式碼是 HTTP POST 方法，如屬性所指示 [`[HttpPost\]`](https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.httppostattribute) 。 該方法會從 HTTP 要求本文取得待辦事項的值。
>
> 如需詳細資訊，請參閱[使用 Http[Verb\] 屬性的屬性路由](https://docs.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#attribute-routing-with-httpverb-attributes)。
>
> [CreatedAtAction](https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.createdataction) 方法：
>
> - 成功時會傳回 HTTP 201 狀態碼。 對於可在伺服器上建立新資源的 HTTP POST 方法，其標準回應是 HTTP 201。
> - 將 [Location](https://developer.mozilla.org/docs/Web/HTTP/Headers/Location) 標頭新增至回應。 `Location`標頭會指定新建立之待辦事項的[URI](https://developer.mozilla.org/docs/Glossary/URI) 。 如需詳細資訊，請參閱 [10.2.2 201 Created](https://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html) (已建立 10.2.2 201)。
> - 參考 `GetTodoItem` 動作以建立 `Location` 標頭的 URI。 C# `nameof` 關鍵字是用來避免在 `CreatedAtAction` 呼叫中以硬式編碼方式寫入動作名稱。

接著依教學指示安裝PostMan測試

PostMan設定

![](https://i.imgur.com/xDP6qQN.png)

把Files-->Setting-->SSL certificate verification關掉(MSDN建議測試完後再次啟用)

接著就可以開始測試了

先把web api run起來接著PostMan依照以下流程跑

> - Create a new request.
> - Set the HTTP method to `POST`.
> - Set the URI to `https://localhost:<port>/api/TodoItems`. For example, `https://localhost:5001/api/TodoItems`.
> - Select the **Body** tab.
> - Select the **raw** radio button.
> - Set the type to **JSON (application/json)**.
> - In the request body enter JSON for a to-do item:
>
> ```json
> {
>   "name":"walk dog",
>   "isComplete":true
> }
> ```

(new request 點+號就有了)

然後就出問題啦

![](https://i.imgur.com/AvyQdbH.png)

錯誤內容

> Error: write EPROTO 1999612088:error:100000f7:SSL routines:OPENSSL_internal:WRONG_VERSION_NUMBER:../../third_party/boringssl/src/ssl/tls_record.cc:242:

SSL確定已經關掉了啊

不知道是不是沒連到，把API關掉再試一次

這次的錯誤是

> Error: connect ECONNREFUSED 127.0.0.1:5002

兩次不一樣，看來有其他原因

嘗試透過IIS跑專案，仍是同樣的錯誤內容

參考[Stackoverflow](https://stackoverflow.com/questions/62658941/error-write-eproto-34557064error100000f7ssl-routinesopenssl-internalwrong)把https改成http後成功了，結果如下(難道是因為我建立專案的時候順手把https的check取消了嗎)

![](https://i.imgur.com/Xo3qTL8.png)

#### Test the location header URI with Postman

以下MSDN原文步驟

> - Select the **Headers** tab in the **Response** pane.
>
> - Copy the **Location** header value:
>
>   ![Postman 主控台的 [標頭] 索引標籤](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api/_static/3/create.png?view=aspnetcore-3.1)
>
> - Set the HTTP method to `GET`.
>
> - Set the URI to `https://localhost:<port>/api/TodoItems/1`. For example, `https://localhost:5001/api/TodoItems/1`.
>
> - Select **Send**.

照做一樣把https改為http，結果如下

![](https://i.imgur.com/sivqcHu.png)

header

![](https://i.imgur.com/xuBJsqg.png)



### Examine the GET methods

> These methods implement two GET endpoints:
>
> - `GET /api/TodoItems`
> - `GET /api/TodoItems/{id}`
>
> Test the app by calling the two endpoints from a browser or Postman. For example:
>
> - `https://localhost:5001/api/TodoItems`
> - `https://localhost:5001/api/TodoItems/1`
>
> A response similar to the following is produced by the call to `GetTodoItems`:
>
> ```json
>[
>   {
>     "id": 1,
>     "name": "Item1",
>     "isComplete": false
>   }
> ]
> ```
> 
#### Test Get with Postman
>
> - Create a new request.
> - Set the HTTP method to **GET**.
> - Set the request URI to `https://localhost:<port>/api/TodoItems`. For example, `https://localhost:5001/api/TodoItems`.
> - Set **Two pane view** in Postman.
> - Select **Send**.
>
> This app uses an in-memory database. If the app is stopped and started, the preceding GET request will not return any data. If no data is returned, [POST](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio#post) data to the app.

這兩節就前面步驟的說明而已

### Routing and URL paths

修改自動生成Controller的Route

```C#
[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly TodoContext _context;

    public TodoItemsController(TodoContext context)
    {
        _context = context;
    }
```

`[controller]`改成Controller名稱(不區分大小寫)

後面解釋這段

```C#
// GET: api/TodoItems/5
[HttpGet("{id}")]
public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
{
    var todoItem = await _context.TodoItems.FindAsync(id);

    if (todoItem == null)
    {
        return NotFound();
    }

    return todoItem;
}
```

當有個get請求`api/TodoItems/5`， `GetTodoItem`方法會將後方的`5`當作`id`來搜尋

### The PutTodoItem method

> Examine the `PutTodoItem` method:
>
> C#複製
>
> ```csharp
> // PUT: api/TodoItems/5
> [HttpPut("{id}")]
> public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
> {
>     if (id != todoItem.Id)
>     {
>         return BadRequest();
>     }
> 
>     _context.Entry(todoItem).State = EntityState.Modified;
> 
>     try
>     {
>         await _context.SaveChangesAsync();
>     }
>     catch (DbUpdateConcurrencyException)
>     {
>         if (!TodoItemExists(id))
>         {
>             return NotFound();
>         }
>         else
>         {
>             throw;
>         }
>     }
> 
>     return NoContent();
> }
> ```
>
> `PutTodoItem` is similar to `PostTodoItem`, except it uses HTTP PUT. The response is [204 (No Content)](https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html). According to the HTTP specification, a PUT request requires the client to send the entire updated entity, not just the changes. To support partial updates, use [HTTP PATCH](https://docs.microsoft.com/zh-tw/dotnet/api/microsoft.aspnetcore.mvc.httppatchattribute).
>
> If you get an error calling `PutTodoItem`, call `GET` to ensure there's an item in the database.

試試看

Put算是修改資料，所以要先有資料才能修改，跟Post格式相似

比如我們重新啟動API

用GET確認是不是空的

`GET` `http://localhost:5002/api/TodoItems/`

![](https://i.imgur.com/f5zZKtS.png)

但我們需要先有資料所以先POST一點東西上去

`POST` `http://localhost:5002/api/TodoItems/`

body

```json
{
    "id":2,
    "name":"post id 2",
    "isComplete":true
}
```

![](https://i.imgur.com/1M3iydM.png)



用GET確認看看

`GET` `http://localhost:5002/api/TodoItems/2`

![](https://i.imgur.com/10UxH2p.png)

現在可以來試試PUT了

`PUT` `http://localhost:5002/api/TodoItems/2`

![](https://i.imgur.com/6SPhCm8.png)

return 204 是正常的，我們的程式碼中，沒出問題最後就是 `return NoContent();`

用GET檢查

`GET` `http://localhost:5002/api/TodoItems/2`

![](https://i.imgur.com/Q83I9jU.png)

成功

---

現在來試試PUT資料到現在沒有內容的id:1

`PUT` `http://localhost:5002/api/TodoItems/1`

![](https://i.imgur.com/Cn9IWKK.png)

回傳405失敗

### DELETE

試試刪除，沿用上例，試著把剛剛建立的ID2資料刪除

`DELETE` `http://localhost:5002/api/TodoItems/2`

![](https://i.imgur.com/0lpFYjL.png)

確認

`GET` `http://localhost:5002/api/TodoItems/`

![](https://i.imgur.com/QWHWv4B.png)