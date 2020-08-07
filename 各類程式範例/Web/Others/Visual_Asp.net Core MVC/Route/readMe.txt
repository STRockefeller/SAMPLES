簡易版routing 可以透過 Map達成，範例如下
// ...
public class Startup
{
    // ...
    public void Configure(IApplicationBuilder app)
    {
        // ...
        app.Map("/first", mapApp =>
        {
            mapApp.Run(async context =>
            {
                await context.Response.WriteAsync("First. \r\n");
            });
        });
        app.Map("/second", mapApp =>
        {
            mapApp.Run(async context =>
            {
                await context.Response.WriteAsync("Second. \r\n");
            });
        });
    }
}

1.加入 Routing 的服務：
在 Startup.cs 的 ConfigureServices中加入services.AddRouting();

