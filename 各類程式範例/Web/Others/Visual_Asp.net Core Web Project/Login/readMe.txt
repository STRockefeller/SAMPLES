登入驗證機制範例
1.版本1 2020/06/19
	1.1.Startup.cs
		using Microsoft.AspNetCore.Authentication.Cookies;
		
		1.1.1.ConfigureServices
		services.AddSingleton<登入驗證的Middleware>();
		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie();
		
		1.1.2.Configure
			//Cookies驗證
            app.UseWebSockets();
            CookiePolicyOptions cookiePolicyOptions = new CookiePolicyOptions();
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseMiddleware<登入驗證的Middleware>();
			
	1.2.網頁.ts
			1.2.1.以post形式傳送帳號密碼等資訊(jQuery ajax done/fail 差別在於HTTP response的回傳StatusCode)
			let data: object = new Object();
            data["account"] = this._account.Value;
            data["password"] = this._password.Value;
            Helper.Loading = true;
            $.post("/login", data)
                .done(() => {
                    location.href = "/pages/machine-status.html";
                })
                .fail((err) => {
                    Helper.Sleep(1000);
                    Helper.Loading = false;
                    this._account.State = FormControlState.Invalid;
                    this._password.State = FormControlState.Invalid;
                    this._account.Focus();
                })
                ;
	1.3.登入驗證的Middleware
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToLower();
            if (context.Request.Method == "POST" && path.EndsWith("/Login".ToLower()))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string account = form["account"];
                string password = form["password"];
                iBox.Services.User user = null;
                user = _server.Login(account, password);
                if (user != null)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    AuthenticationProperties authProperties = new AuthenticationProperties();
                    await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
                    context.Response.StatusCode = 200;
                    await context.Response.WriteAsync("Authorized");
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized");
                }
            }
            else if (path.EndsWith("/Logout".ToLower()))
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/");
            }
            else if (path.IndexOf("/pages/".ToLower()) == 0 && context.User.Identity.IsAuthenticated == false)
            {
                context.Response.Redirect("/");
            }
            else if (path.IndexOf("/backup/".ToLower()) == 0 && context.User.IsInRole("Administrators") == false)
            {
                context.Response.Redirect("/");
            }
            else
            {
                await next.Invoke(context);
            }
        }