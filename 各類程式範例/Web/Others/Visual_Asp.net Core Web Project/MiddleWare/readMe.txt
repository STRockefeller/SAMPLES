理解尚不完全，嘗試解釋之

界接RES和REQ的閘道(?

1.Middleware class ：繼承IMiddleware(似乎也可以不繼承)
2.在Startup.cs ConfigureServices方法裡，services.AddSinglrton<Middleware_Class>();
3.startup.cs app.UseMiddleware<Middleware_Class>();
4.處理就response不處理就next



ps1.如果InvokeAsync方法內不能執行await請檢察是否未using threading.task或者方法未宣告為async
