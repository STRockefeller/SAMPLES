# SignalR



[Reference:MSDN](https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio)



## 在VS asp.net core web project上的安裝方式

​	在 `方案總管`中，以滑鼠右鍵按一下專案，然後選取 `新增` `用戶端程式庫。`
​	在 `新增用戶端程式庫`對話方塊中，針對 [提供者]選取 `unpkg`。
​	針對 [程式庫]輸入 @microsoft/signalr@latest。
​	選取 [選擇特定檔案]、展開 [dist/browser]資料夾，然後選取 signalr.js 與 signalr.min.js。
​	將目標位置設定為 wwwroot/js/signalr/, 然後選擇 「安裝」 。
​	

PS.好像只有用到signalr.js 另一個不確定有沒有用

以2020/09 所寫的 AcientBulletinBoard 專案部分內容作為範例。

## 建立Hub類別



```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace AcientBulletinBoard.Hubs
{
    public class ChatRoomHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
```



## 加入服務以及註冊

Startup.cs

```C#
using SignalRChat.Hubs;
//...
        public void ConfigureServices(IServiceCollection services)
        {
            //...
            services.AddSignalR();
			//...
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
			//...
            app.UseEndpoints(endpoints =>
            {
				//...
                //endpoints.MapHub<上一步繼承Hub的Class> ("/Class Name 不分大小寫")
                endpoints.MapHub < ChatRoomHub > ("/ChatRoomHub");
            });
        }
```



## Script

簡單的建立好MVC之後

撰寫Typescript(使用TS的話要先 `npm install @microsoft/signalr`)

```typescript
import * as signalR from "@microsoft/signalr"

$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatRoomHub").build();
    (<HTMLButtonElement>document.getElementById("sendButton")).disabled = true;

    connection.on("ReceiveMessage", function (user, message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = user + " says " + msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.start().then(function () {
        (<HTMLButtonElement>document.getElementById("sendButton")).disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user: string = document.getElementById("userInput").nodeValue;
        var message: string = document.getElementById("messageInput").nodeValue;
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
});
```



### 連線物件

```typescript
var connection:signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/hub class名稱 不區分大小寫").build();
```



### 方法說明

```typescript
connection.on("Task觸發的事件名稱"function(){想做的事})
connection.start().then(function () {連線成功後想做的事});

//觸發Task的方式
connection.invoke("Task名稱",後面放參數)

//連接事件
var connection = new signalR.HubConnectionBuilder().withUrl("/Hub名稱").build();

//開始連接
connection.start()
.then(function(){
    // 連接成功後要做的事情
})
.catch(function(err){
    // 錯誤處理
});

//關閉連接
connection.stop().catch(function(err){
    // 錯誤處理
});

//監聽連接關閉瞬間
connection.onclose(function(e){
    // 關閉時想做的事
});
```





### Trouble Shooting

#### 找不到定義@microsoft/signalr

若import時遇到此問題(只有VS有問題，VSC不會報錯，原因不明)，嘗試以下作法，操作前建議先備份，不然問題可能會越改越多

1. 先確定import路徑無誤，接著嘗試各種路徑指示方法如

   ```typescript
   import * as SignalR from "@microsoft/signalr"
   import * as SignalR from "../@microsoft/signalr"
   import * as SignalR from "../../node_modules/@microsoft/signalr"
   import * as SignalR from "C:/Users/admin/source/repos/AcientBulletinBoard/AcientBulletinBoard/node_modules/@microsoft/signalr"
   ```

   



2. 各種重新安裝(註有些指令需求權限，以VS或VSC上的terminal無法執行，直接以commandline執行即可)(若解除安裝後資料夾還在可以手動刪除)

   ```
   npm uninstall @microsoft/signalr
   npm install @microsoft/signalr
   
   npm uninstall typescript
   npm install -g typescript
   npm install @types/jquery --save -dev
   ```
   
   



3. 加入path(最後是這個動作完成才成功)

   由於專案中並沒有tsconfig.json，故自己新增，作法有二

   1. `tsc --init`
   2. 直接在專案跟目錄新增json檔案

   加入內容如下(看起來像是廢話但真的有效)

   ```json
   "baseUrl": "./node_modules",
   "paths": {
       "@microsoft/*": [ "@microsoft/*" ]
     }
   ```

   加入tsconfig後若發生找不到命名空間 NodeJS

   在tsconfig.json加入以下內容

   ```json
     "compilerOptions": {
       "outDir": "../out-tsc/app",
       "module": "es6",
       "baseUrl": "",
       "types": [ "node" ]
     },
   ```



#### TS2532

> ```js
> error TS2532: Object is possibly 'undefined'.
> ```

方法一：想辦法讓物件不會有未定意的情況發生

方法二：編譯時不檢查，眼不見為淨

tsconfig.json

```json
"strictNullChecks": false,
```



#### TS7053

> ```js
> error TS7053: Element implicitly has an 'any' type because expression of type '出問題的物件名稱' can't be used to index type '{}'.
> ```

方法一：嚴格定義物件型別

方法二：編譯時不檢查，眼不見為淨

tsconfig.json

```json
"noImplicitAny": false,
```



#### JavaScript執行階段exports未經定義



```javascript
Object.defineProperty(exports, "__esModule", { value: true });
```

這行是由TS編譯成JS時，若有import外部module自動生成的作用不明

解決這個問題的關鍵似乎還是出在tsconfig.json上。[reference:StackOverFlow](https://stackoverflow.com/questions/43042889/typescript-referenceerror-exports-is-not-defined/43999062)



#### TS2688

//todo

#### TS2580

//todo



## View

Views\ChatRoom\Index.cshtml

```html
@*
    For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
*@
@{
}
@model ChatRoomModel

    <div class="container">
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-2">User</div>
            <div class="col-4"><input asp-for="@AcientBulletinBoard.Services.Helper._userData.name" type="text" id="userInput" /></div>
        </div>
        <div class="row">
            <div class="col-2">Message</div>
            <div class="col-4"><input type="text" id="messageInput" /></div>
        </div>
        <div class="row">&nbsp;</div>
        <div class="row">
            <div class="col-6">
                <input type="button" id="sendButton" value="Send Message" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-6">
            <ul id="messagesList"></ul>
        </div>
    </div>

@section Scripts{
    <script src="~/js/signalr/dist/browser/signalr.js"></script>
    <script>
    var connection = new signalR.HubConnectionBuilder().withUrl("/chatRoomHub").build();

        connection.on("ReceiveMessage", function (user, message) {
        var li = document.createElement("li");
        li.textContent = user+" : "+message;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.start().catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = "@AcientBulletinBoard.Services.Helper._userData.name";
        var message = document.getElementById("messageInput").value;
        connection.invoke("SendMessage", user, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
    </script>
    @*<script>var exports = { "__esModule": true };</script>
    <script src="~/js/ChatRoomIndex.js"></script>*@
}
```

這邊是直接把邏輯以js寫進去了(因為TS import一直失敗)

注意`<script src="~/js/signalr/dist/browser/signalr.js"></script>`指向[第一步安裝的signalr.js](#在VS asp.net core web project上的安裝方式)，漏了這段會顯示jquery例外 Promise 未經定義。

