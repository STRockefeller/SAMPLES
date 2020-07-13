同非MVC專案
建議
1.專案建立時把SSL認證取消勾選
2.Debug選擇專案名稱而非IIS
3.於wwwroot根目錄中只保留一個html檔案且於startup Configure方法中加入  app.UseDefaultFiles();就可以直接導向該頁面而非預設畫面。

------------------------------------------------------------------------------------------------------------------------------------

MVC only
1.加入 MVC 的服務(REF https://blog.yowko.com/aspdotnet-core-addmvc-addmvccore/)
	Startup.ConfigureServices中加入services.AddMvc();或者 AddMvcCore();

2.設定MVC路由 以下兩個方法擇一
2.1.在 Configure 對 IApplicationBuilder 使用 UseMvcWithDefaultRoute 方法註冊 MVC 預設路由的 Middleware 

public void Configure(IApplicationBuilder app)
    {
        app.UseMvcWithDefaultRoute();
    }
2.2.或者使用預設Startup.cs 最後方的部分 Controller 和 Action 指向最初執行的內容
 app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=About}/{id?}");
            });


4.關於Controller:
4.1.必須繼承Controller才能使用View
4.2.以下節錄自 https://ithelp.ithome.com.tw/articles/10193590
IActionResult 回傳的方式可以有很多種，透過繼承 Controller 後，就可以使用 Controller 的方法：

View
以上例來說，透過回傳 View 方法，可以找到該 Controller & Action 對應的 *.cshtml， 並且把 UserModel 傳給 View 使用。
HTTP Status Code
回應包含 HTTP Status。常用的回應有 Ok、BadRequest、NotFound 等。
例如：return BadRequest("Internal Server Error")，會回應 HTTP Status 400 及 Internal Server Error 字串。
Redirect
可以把 Request 轉給其他的 Action 或 URL。轉向的方法有 Redirect、LocalRedirect、RedirectToAction、RedirectToRoute 等。
例如：return RedirectToAction("Login", "Authentication")，就會把 Request 轉向到 AuthenticationController 的 Login()。
Formatted Response
回應時指定 Content-Type。Web API 的回傳通常都用這種方式，序列化物件順便標註 Content-Type。
例如：return Json(user)，會將物件序列化成 JSON 字串，並在 HTTP Headers 帶上 Content-Type=application/json。


5.關於VIEW
View 跟 Controller 有相互的對應關係，預設在 Controller 使用 View 方法回傳結果，會從以下目錄尋找對應的 *.cshtml：

Views\{ControllerName}\{ActionName}.cshtml
尋找與 Controller 同名的子目錄，再找到與 Action 同名的 *.cshtml。
如上例 HomeController.Index()，就會找專案目錄下的 Views\Home\Index.cshtml 檔案。
Views\Shared\{ActionName}.cshtml
如果 Controller 同名的子目錄，找不到 Action 同名的 *.cshtml。就會到 Shared 目錄找。
如上例 HomeController.Index()，就會找專案目錄下的 Views\Shared\Index.cshtml 檔案。