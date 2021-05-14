# Login System

## ASP.Net Core Web

CIFactory時的做法

### Startup.cs

```C#
using Microsoft.AspNetCore.Authentication.Cookies;
```

ConfigureServices

```C#
		services.AddSingleton<登入驗證的Middleware>();
		services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie();
```

Configure

```C#
			//Cookies驗證
            app.UseWebSockets();
            CookiePolicyOptions cookiePolicyOptions = new CookiePolicyOptions();
            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            app.UseMiddleware<登入驗證的Middleware>();
```



### 網頁.ts

以post形式傳送帳號密碼等資訊(jQuery ajax done/fail 差別在於HTTP response的回傳StatusCode)

```typescript
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
```



### 登入驗證的Middleware

```C#
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;

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
```



## ASP .Net Core MVC

2020/09 正在撰寫新的專案，先前的作法有些部分不太適用，重新整理之。

Linq2DB在Core上使用仍不順利故仍是以SQLite語法操作資料庫



### 定義使用者類別

try裡面的內容以及setValueLinq()方法都是Linq2DB用的，但總是無法成功所以最後跑的都是System.Data.SQLite的方法

Services.UserData.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using LinqToDB;
using DataModels;

namespace AcientBulletinBoard.Services
{
    public class UserData
    {
        public enumRole role { get; set; }
        public string name { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public string emailAddress { get; set; }
        public enumCamp camp { get; set; }
        private User linqUser;
        private SQLiteConnection connection;
        public void logIn(string account, string password)
        {
            if (!accountCheck(account, password))
                return;
            if(linqUser!=null)
                setValueLinq();
        }
        private bool accountCheck(string account, string password)
        {
            try
            {
                using (var db = new UserDataDB())
                {
                    if (db.Users.Any(user => user.Account == account && user.Password == password))
                        linqUser = db.Users.Where(user => user.Account == account && user.Password == password).Single();
                    return db.Users.Any(user => user.Account == account && user.Password == password);
                }
            }
            catch (Exception ex)
            {
                connection = new SQLiteConnection("data source = C:\\Users\\admin\\source\\repos\\AcientBulletinBoard\\AcientBulletinBoard\\DataBases\\UserData.db");
                connection.Open();
                string commandString = $"Select * From Users Where Account = '{account}' And Password = '{password}';";
                SQLiteCommand command = new SQLiteCommand(commandString, connection);
                SQLiteDataReader dataReader = command.ExecuteReader();
                while(dataReader.Read())
                {
                    if(!dataReader[0].Equals(DBNull.Value))
                    {
                        this.account = dataReader["Account"].ToString();
                        this.password = dataReader["Password"].ToString();
                        this.name = dataReader["Name"].ToString();
                        this.emailAddress = dataReader["EmailAddress"].ToString();
                        switch (dataReader["Camp"].ToString())
                        {
                            case "Wei":
                                camp = enumCamp.Wei;
                                break;
                            case "Shu":
                                camp = enumCamp.Shu;
                                break;
                            case "Wu":
                                camp = enumCamp.Wu;
                                break;
                            case "God":
                                camp = enumCamp.God;
                                break;
                            case "Neutral":
                                camp = enumCamp.Neutral;
                                break;
                        }
                        switch (dataReader["Role"].ToString())
                        {
                            case "admin":
                                role = enumRole.admin;
                                break;
                            case "normal":
                                role = enumRole.normal;
                                break;
                            case "guest":
                                role = enumRole.guest;
                                break;
                        }
                        return true;
                    }
                }
                return false;
            }

        }
        private void setValueLinq()
        {
            account = linqUser.Account;
            password = linqUser.Password;
            name = linqUser.Name;
            emailAddress = linqUser.EmailAddress;
            switch (linqUser.Camp)
            {
                case "Wei":
                    camp = enumCamp.Wei;
                    break;
                case "Shu":
                    camp = enumCamp.Shu;
                    break;
                case "Wu":
                    camp = enumCamp.Wu;
                    break;
                case "God":
                    camp = enumCamp.God;
                    break;
                case "Neutral":
                    camp = enumCamp.Neutral;
                    break;
            }
            switch (linqUser.Role)
            {
                case "admin":
                    role = enumRole.admin;
                    break;
                case "normal":
                    role = enumRole.normal;
                    break;
                case "guest":
                    role = enumRole.guest;
                    break;
            }
        }
    }
    public enum enumRole
    {
        admin,
        normal,
        guest
    }
    public enum enumCamp
    {
        Wei,
        Shu,
        Wu,
        God,
        Neutral,
    }
}

```



### Middleware部分

#### 本體

Middlewares.AuthenticationMiddleware.cs

直接以Cookie紀錄登入狀態

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace AcientBulletinBoard.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string path = context.Request.Path.Value.ToLower();
            if (context.Request.Method == "POST" && path.EndsWith("/Login".ToLower()))
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string account = form["account"];
                string password = form["password"];
                Services.UserData user = new Services.UserData();
                user.logIn(account, password);
                if (user != null && user.name.Length>=1)
                {
                    context.Response.Cookies.Delete("User");
                    context.Response.Cookies.Append("User", user.name);
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
            else
            {
                await next.Invoke(context);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}

```



#### DI&註冊

Startup.cs

```C#
        public void ConfigureServices(IServiceCollection services)
        {
            //...
            services.AddSingleton<AuthenticationMiddleware>();
            //...
            //下面這個是Linq2DB用的，可以不寫
            services.AddLinqToDbContext<AppDataConnection>((provider, options) =>
            {
                options
                .UseSQLite(Configuration.GetConnectionString("Default"))
                .UseDefaultLogging(provider);
            });
        }
```

```C#
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthenticationMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
```



### View

簡單的登入畫面

Views\Home\Index.cshtml

```html
@{
    ViewData["Title"] = "Home Page";
}

<div class="logIn">
    <div class="card text-left">
        <img class="card-img-top" src="holder.js/100px180/" alt="">
        <div class="card-body">
            <h4 class="card-title">LogIn</h4>
            <div class="form-group">
                <label for=""></label>
                <div class="form-group">
                    <label for="">Accpunt</label>
                    <input type="text" name="" id="InpAccount" class="form-control" placeholder="" aria-describedby="helpId">
                </div>
                <div class="form-group">
                    <label for="">Password</label>
                    <input type="password" name="" id="InpPassword" class="form-control" placeholder="" aria-describedby="helpId">
                </div>
                <a name="" id="btnLogin" class="btn btn-primary" href="#" role="button">LogIn</a>
                <a name="" id="btnGuestLogin" class="btn btn-primary" href="#" role="button">Browse as a Guest</a>
            </div>
        </div>
    </div>
</div>

@section Scripts {
<script src="~/js/HomeIndex.js"></script>
}
```



### TypeScript

懶得Gulp所以直接寫在wwwroot裡面

另外訪客登入我直接在資料庫裏面預先寫了一個Guest帳號，不曉得一般來說是怎麼做的。

wwwroot\js\HomeIndex.ts

```typescript
$(document).ready(function () {
    $("#btnLogin").click(function () {
        let data: object = new Object();
        data["account"] = $("#InpAccount").val();
        data["password"] = $("#InpPassword").val();
        $.post("/login", data)
            .done(() => {
                location.href = "/PublicBulletinBoard";
            })
            .fail((err) => {
            })
            ;
    })
    $("#btnGuestLogin").click(function () {
        let data: object = new Object();
        data["account"] = "guest";
        data["password"] = "guest";
        $.post("/login", data)
            .done(() => {
                location.href = "/PublicBulletinBoard";
            })
            .fail((err) => {
            })
            ;
    })
})
```



### Linq2DB(失敗)

以下是Linq2DB的部分，運作**沒有成功**，會跳Configuration相關的Exception，先將寫法記錄下來方便以後查看

appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "Default": "data source = C:\\Users\\admin\\source\\repos\\AcientBulletinBoard\\AcientBulletinBoard\\DataBases\\UserData.db"
  }
}

```

DataModels.AppDataConnection.cs

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB.Configuration;
using LinqToDB.Data;


namespace AcientBulletinBoard.DataModels
{
    public class AppDataConnection : DataConnection
    {
        public AppDataConnection(LinqToDbConnectionOptions<AppDataConnection> options)
            : base(options)
        {

        }
    }
}

```

DataModels.LinqUser.tt

```C#
<#@ template language="C#" debug="True" hostSpecific="True"                        #>
<#@ output extension=".generated.cs"                                               #>
<#@ include file="$(LinqToDBT4SQLiteTemplatesDirectory)LinqToDB.SQLite.Tools.ttinclude" #>
<#@ include file="$(LinqToDBT4SQLiteTemplatesDirectory)PluralizationService.ttinclude"  #>
<# //@ include file="$(ProjectDir)LinqToDB.Templates\LinqToDB.SQLite.Tools.ttinclude" #>
<# //@ include file="$(ProjectDir)LinqToDB.Templates\PluralizationService.ttinclude"  #>
<#
	/*
		1. Create new *.tt file (e.g. MyDatabase.tt) in a folder where you would like to generate your data model
		   and copy content from this file to it. For example:

			MyProject
				DataModels
					MyDatabase.tt

		2. Modify the connection settings below to connect to your database.

		3. Add connection string to the web/app.config file:

			<connectionStrings>
				<add name="MyDatabase" connectionString="Data Source=MyDatabase.sqlite" providerName="SQLite" />
			</connectionStrings>

		4. To access your database use the following code:

			using (var db = new MyDatabaseDB())
			{
				var q =
					from c in db.Customers
					select c;

				foreach (var c in q)
					Console.WriteLine(c.ContactName);
			}

		5. See more at https://linq2db.github.io/articles/T4.html

		IMPORTANT: if running .tt file gives you error like this:
		"error : Failed to resolve include text for file: C:\...\$(LinqToDBT4<SOME_DB>TemplatesDirectory)LinqToDB.<DB_NAME>.Tools.ttinclude"
		check tt file properties.
		Custom tool must be set to TextTemplatingFileGenerator, not TextTemplatingFilePreprocessor or any other value.
	*/

	NamespaceName = "DataModels";

	// to configure GetSchemaOptions properties, add them here, before load metadata call

	LoadSQLiteMetadata(@"C:\Users\admin\source\repos\AcientBulletinBoard\AcientBulletinBoard\DataBases\", "UserData.db");
//	LoadSQLiteMetadata(string connectionString);

	// to adjust loaded database model before generation, add your code here, after load metadata, but before GenerateModel() call

	GenerateModel();
#>
```

DataModels.LinqUser.generated.cs

```C#
//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;

using LinqToDB;
using LinqToDB.Mapping;

namespace DataModels
{
	/// <summary>
	/// Database       : UserData
	/// Data Source    : UserData
	/// Server Version : 3.24.0
	/// </summary>
	public partial class UserDataDB : LinqToDB.Data.DataConnection
	{
		public ITable<User> Users { get { return this.GetTable<User>(); } }

		public UserDataDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public UserDataDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table("Users")]
	public partial class User
	{
		[Column("role"),         Nullable] public string Role         { get; set; } // string(max)
		[Column("name"),         Nullable] public string Name         { get; set; } // string(max)
		[Column("account"),      Nullable] public string Account      { get; set; } // string(max)
		[Column("password"),     Nullable] public string Password     { get; set; } // string(max)
		[Column("emailAddress"), Nullable] public string EmailAddress { get; set; } // string(max)
		[Column("camp"),         Nullable] public string Camp         { get; set; } // string(max)
	}
}

#pragma warning restore 1591
```

Startup.cs

```C#
        public void ConfigureServices(IServiceCollection services)
        {
			//...
            services.AddLinqToDbContext<AppDataConnection>((provider, options) =>
            {
                options
                .UseSQLite(Configuration.GetConnectionString("Default"))
                .UseDefaultLogging(provider);
            });
        }
```

