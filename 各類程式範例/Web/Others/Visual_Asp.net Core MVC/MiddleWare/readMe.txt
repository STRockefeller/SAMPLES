REF
1.MSDN https://docs.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-2.1
2.ithelp
https://ithelp.ithome.com.tw/articles/10203041
https://ithelp.ithome.com.tw/articles/10192682


1.App.Use
般會使用Use進行自訂的Middleware擴充，能透過呼叫next()指定執行下一層Middleware(可加入條件判斷決定是否呼叫)，也可指定在管線回流時所要執行的行為。
Middleware 的註冊方式是在 Startup.cs 的 Configure 對 IApplicationBuilder 使用 Use 方法註冊。
大部分擴充的 Middleware 也都是以 Use 開頭的方法註冊，例如：

UseMvc() ：MVC 的 Middleware
UseRewriter() ：URL rewriting 的 Middleware

2.App.Run
Run 是 Middleware 的最後一個行為，以上面圖例來說，就是最末端的 Action。
它不像 Use 能串聯其他 Middleware，但 Run 還是能完整的使用 Request 及 Response。

3.App.Map
Map 是能用來處理一些簡單路由的 Middleware，可依照不同的 URL 指向不同的 Run 及註冊不同的 Use。
主要在判斷路由規則是否符合預期，符合則執行區間內容。