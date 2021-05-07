# Blazor

[Reference: Microsoft Blazor Tutorial](https://dotnet.microsoft.com/learn/aspnet/blazor-tutorial/intro)

[Reference: MSDN Blazor todolist sample](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/build-a-blazor-app?view=aspnetcore-5.0)



我這次學習過程使用VSC做開發工具

## Create App

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



可以直接建置執行看看

```
dotnet watch run
```

一個簡單的Blazor網頁就跑出來啦



專案最外層有`Program.cs`和`Startup.cs`乍看之下和ASP.net WEB/MVC專案架構相似

