using System.Threading

//SpinWait.SpinUntil(() => { return 'PS1''PS2'; }, 'PS3');
//PS1 這裡回傳true會跳離；若回傳false則繼續執行
//PS2 這裡的方法會被不斷執行至回傳true或設定時間到達
//PS3 設定時間(ms)，若時間到仍未得到true回傳值則跳脫
SpinWait.SpinUntil(() => { return false; }, 5000);
//範例 總之就是等5秒

//另一種寫法(較不推薦)
Thread.Sleep(5000);

//---
//使用Task
//同步
Task.Delay(5000).Wait();
//非同步
await Task.Delay(5000);