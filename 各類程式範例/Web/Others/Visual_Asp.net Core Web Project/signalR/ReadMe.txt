主要參考微軟教學
https://docs.microsoft.com/zh-tw/aspnet/core/tutorials/signalr?view=aspnetcore-3.1&tabs=visual-studio

概括敘述下理解的內容
1.首先在VS asp.net core web project上的安裝方式
	在 [方案總管]中，以滑鼠右鍵按一下專案，然後選取 [新增][用戶端程式庫]。
	在 [新增用戶端程式庫]對話方塊中，針對 [提供者]選取 [unpkg]。
	針對 [程式庫]輸入 @microsoft/signalr@latest。
	選取 [選擇特定檔案]、展開 [dist/browser]資料夾，然後選取 signalr.js 與 signalr.min.js。
	將目標位置設定為wwwroot/js/信號器/, 然後選擇 「安裝」 。
	
	PS.好像只有用到signalr.js 另一個不確定有沒有用
2.在專案底下(的任意路徑)(個人沿用宏珀的習慣./Services/)(微軟範例是./Hubs/)建立cs項目
using Microsoft.AspNetCore.SignalR以及System.Threading.Tasks後在裡面建立一個class繼承Microsoft.AspNetCore.SignalR.Hub並在其中建立一個public async Task
	以下為微軟範例
	using Microsoft.AspNetCore.SignalR;
	using System.Threading.Tasks;
	namespace SignalRChat.Hubs
	{
	    public class ChatHub : Hub
 	   {
  	      public async Task SendMessage(string user, string message)
 	       {
  	          await Clients.All.SendAsync("ReceiveMessage", user, message);
 	       }
 	   }
	}
3.接著在startup.cs中using上一步驟cs的namespace
在ConfigureServices中加入services.AddSignalR();
在app.UseEndpoints(endpoints =>中加入endpoints.MapHub<ChatHub(第二步繼承Hub的Class)>("/hub class名稱 不區分大小寫");

4.html 引用的內容
<script src="第一步安裝的signalr.js路徑"></script>
漏了這段會顯示jquery例外 Promise 未經定義。

5.js/ts檔案的內容(微軟範例是js，但心得以較為熟悉的ts為主)
以ts撰寫的話要import * as signalR from "@microsoft/signalr"
以下內容都放在$(document).ready(function(){})裡面
var connection:signalR.HubConnection = new signalR.HubConnectionBuilder().withUrl("/hub class名稱 不區分大小寫").build();

connection.on("Task觸發的事件名稱"function(){想做的事})
connection.start().then(function () {連線成功後想做的事});

//觸發Task的方式
connection.invoke("Task名稱",後面放參數)


6.補充 以下引用自 https://ithelp.ithome.com.tw/articles/10203113

連接事件
var connection = new signalR.HubConnectionBuilder().withUrl("/Hub名稱").build();

開始連接
connection.start()
.then(function(){
    // 連接成功後要做的事情
})
.catch(function(err){
    // 錯誤處理
});

關閉連接
connection.stop().catch(function(err){
    // 錯誤處理
});

監聽連接關閉瞬間
connection.onclose(function(e){
    // 關閉時想做的事
});