# Entity Framework

[Reference:淺談 Entity Framework 與 ADO.NET 差異](http://www.kangting.tw/2019/06/entity-framework_23.html)

[Reference:MSDN](https://docs.microsoft.com/zh-tw/ef/core/get-started/overview/first-app?tabs=netcore-cli)

[Reference:程式學習之路](https://medium.com/sally-thinking/%E7%A8%8B%E5%BC%8F%E5%AD%B8%E7%BF%92%E4%B9%8B%E8%B7%AF-day28-c-ado-net-entity-framework-%E7%89%A9%E4%BB%B6%E9%97%9C%E8%81%AF%E5%B0%8D%E6%87%89-4b27943af679)

[Reference:ITHELP](https://ithelp.ithome.com.tw/articles/10196856)



## 甚麼是Entity Framework

> Entity Framework 是ASP.NET MVC 串接底層資料來源最重要的技術之一，由於ASP.NET MVC的彈性相當大，事實上你並不一定要使用 Entity Framework 就能利用 ASP.NET MVC 開網站，直接使用傳統 ASP.NET 開發人員熟悉的 ADO.NET 也是可行，只是如此一來便無法獲得 Entity Framework 帶來的好處，例如強型別資料模型處理等等，運用內建的精簡化領域模型設計，這對於應付大型網站開發相當有用，相較於 ADO.NET 更適於用在網路開發環境，可以大幅簡化大規模系統開發的複雜度。
>
> 傳統的 .NET 應用程式透過 ADO.NET 提供的類別，傳送 SQL 敘述回後端資料庫引擎進行解譯，執行特定的資料操作，然後取得回傳的結果。由於直接對資料庫進行存取，因此前端程式與後端資料庫結構緊密結合，導致未來資料庫更新與維護的困難，開發人員透過良好的架構與分層設計，降低前端程式與資料庫間的耦合性，現在藉由 Entity Framework 我們可以更容易處理相關的問題，建立實體資料模型反映真實世界的問題，並且透過附加的設計對應底層資料結構，避免程式與資料庫間的直接關聯。
>
> Entity Framework 之前，在預設的情形下，使用 ADO.NET 的開發人員必須直接針對底層資料庫物件撰寫程式進行互動，包含實體資料表與各種資料庫元素，例如預存程序、檢視表等等。來到 Entity Framework ，開發人員不再針對資料表進行存取，取而代之的是實體資料模型。

> ![](https://1.bp.blogspot.com/-VWsG9hZJeZA/UP9w4eQGYUI/AAAAAAAACGo/R7kOSmbS1LI/s640/V101.jpg)

> 上方是傳統的資料存取架構，其中透過 ADO.NET 傳送 SQL 與資料庫進行互動，下方是採用 Entity Framework 的資料存取方案，其中間接的透過實體資料模型傳送 SQL 指令，而開發人員所需處理的則是 LINQ 或是 Entity SQL 敘述，Entity Framework 會自動將它們轉換成為對應的關聯式 SQL 。

Entity Framework(太常懶得打，後面叫它EF就好)，是一種[ORM](https://zh.wikipedia.org/wiki/%E5%AF%B9%E8%B1%A1%E5%85%B3%E7%B3%BB%E6%98%A0%E5%B0%84)，目前看來和先前使用過的LinqToDB定位差不多，作為程式和資料庫中間過度的角色。

這次使用的版本是 Entity Framework Core

## EF Core



### 安裝EF Core

這次使用 asp.net core console 專案來做測試



安裝方法似乎有很多種，我這邊是直接從NuGet安裝了`Microsoft.EntityFrameworkCore`以及`Microsoft.EntityFrameworkCore.Sqlite`



### 建立Model

以下是MSDN的範例

```C#
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFGetStarted
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=blogging.db");
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new List<Post>();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
```

`DbContext` 是 EF Core 跟資料庫溝通的主要類別，透過繼承 `DbContext` 可以定義跟資料庫溝通的行為。
首先我們先建立一個類別繼承 `DbContext`，同時建立 DbSet。

EF Core 會幫我們把 DbSet 轉換成資料表。

### 建立DataBase

安裝NuGet套件 Microsoft.EntityFrameworkCore.Tools

還有Microsoft.EntityFrameworkCore.Design

執行 `Add-Migration InitialCreate` (在工具-->NuGet套件管理-->**套件管理主控台**(以下簡稱PMC)執行，而不是PowerShell)

以及 `Update-Database`(PMC)

執行成功的話，專案中會出現blogging.db檔案(或其他名稱.db)

### CRUD

MSDN範例

```C#
using System;
using System.Linq;

namespace EFGetStarted
{
    class Program
    {
        static void Main()
        {
            using (var db = new BloggingContext())
            {
                // Create
                Console.WriteLine("Inserting a new blog");
                db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a blog");
                var blog = db.Blogs
                    .OrderBy(b => b.BlogId)
                    .First();

                // Update
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://devblogs.microsoft.com/dotnet";
                blog.Posts.Add(
                    new Post
                    {
                        Title = "Hello World",
                        Content = "I wrote an app using EF Core!"
                    });
                db.SaveChanges();

                // Delete
                Console.WriteLine("Delete the blog");
                db.Remove(blog);
                db.SaveChanges();
            }
        }
    }
}
```



### 執行

執行結果

```
Inserting a new blog
Unhandled exception. Microsoft.EntityFrameworkCore.DbUpdateException: An error occurred while updating the entries. See the inner exception for details.
 ---> Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 1: 'no such table: Blogs'.
   at Microsoft.Data.Sqlite.SqliteException.ThrowExceptionForRC(Int32 rc, sqlite3 db)
   at Microsoft.Data.Sqlite.SqliteCommand.PrepareAndEnumerateStatements(Stopwatch timer)+MoveNext()
   at Microsoft.Data.Sqlite.SqliteCommand.GetStatements(Stopwatch timer)+MoveNext()
   at Microsoft.Data.Sqlite.SqliteDataReader.NextResult()
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteReader(CommandBehavior behavior)
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at System.Data.Common.DbCommand.ExecuteReader()
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReader(RelationalCommandParameterObject parameterObject)
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.Execute(IRelationalConnection connection)
   --- End of inner exception stack trace ---
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.Execute(IRelationalConnection connection)
   at Microsoft.EntityFrameworkCore.Update.Internal.BatchExecutor.Execute(IEnumerable`1 commandBatches, IRelationalConnection connection)
   at Microsoft.EntityFrameworkCore.Storage.RelationalDatabase.SaveChanges(IList`1 entries)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChanges(IList`1 entriesToSave)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChanges(DbContext _, Boolean acceptAllChangesOnSuccess)
   at Microsoft.EntityFrameworkCore.Storage.NonRetryingExecutionStrategy.Execute[TState,TResult](TState state, Func`3 operation, Func`3 verifySucceeded)
   at Microsoft.EntityFrameworkCore.ChangeTracking.Internal.StateManager.SaveChanges(Boolean acceptAllChangesOnSuccess)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChanges(Boolean acceptAllChangesOnSuccess)
   at Microsoft.EntityFrameworkCore.DbContext.SaveChanges()
   at EntityFrameworkTest.Program.Main() in C:\Users\admin\source\repos\EntityFrameworkTest\EntityFrameworkTest\Program.cs:line 15

```

失敗。

關於這點在MSDN中有說明

> 執行 .NET Core 主控台應用程式時，Visual Studio 會使用不一致的工作目錄。 (請參閱 [dotnet/project-system#3619](https://github.com/dotnet/project-system/issues/3619)) 這會導致擲回例外狀況：「沒有這個表格： 部落格」。 更新工作目錄：
>
> - 以滑鼠右鍵按一下專案，然後選取 [編輯專案檔]
>
> - 在 *TargetFramework* 屬性下方新增下列內容：
>
>   ```xml
>   <StartWorkingDirectory>$(MSBuildProjectDirectory)</StartWorkingDirectory>
>   ```
>
> - 儲存檔案
>
> 現在您可以執行應用程式：
>
> - [偵錯] > [啟動但不偵錯]

照做試試，修改完成後如下

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.1</TargetFramework>
    <StartWorkingDirectory>$(MSBuildProjectDirectory)</StartWorkingDirectory>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="5.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="5.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="5.0.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>

</Project>
```

之後可順利執行

---

### 異常情況

以下錯誤僅發生在我的筆電上，後使用桌機照MSDN流程跑都未發生此問題，一切順利。

在建立DB的階段

執行後會顯示`The specified deps.json [C:\Users\admin\source\repos\EntityFrameworkTest\EntityFrameworkTest\EntityFrameworkTest.deps.json] does not exist`，不確定是否正常。

有查到一些[參考資料](https://magnussundstrom.se/blog/deps-json-does-not-exist)但都不管用，VS版本也從16.7.0更新到16.8.3依然無用

![image-20210107113409077](C:\Users\admin\AppData\Roaming\Typora\typora-user-images\image-20210107113409077.png)

事實上指令執行結果都是successed，**執行程式後**blogging.db也有成功建立在專案根目錄以及debug目錄底下。

是的，觀察之後發現database是在程式執行後才產生的，並非執行PMC指令後生成，PMC指令下完後只多出了bin和obj兩個資料夾

而生成的database名稱會同於在建立Model階段`options.UseSqlite`方法的參數。

```C#
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=blogging.db");
```

其實最初我是認為這段只是指名我這個DBContext對應的目標DB，但似乎不只如此。

已逐步執行下方[CRUD的MSDN範例程式](CRUD)確認DB建立是在哪個環節完成:

* 並不是DBContext子類別的建構式
* 在`db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });`時，有執行到`UseSqlite`方法，但並沒有生成DB
* 最後程式於`db.SaveChanges();`跳Exception但同時生成了DB

但從SaveChanges往回查只查到了一個Virtual方法而已。

執行階段修改完專案黨後，依然跳no such table

---