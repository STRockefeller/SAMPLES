建議
1.專案建立時把SSL認證取消勾選
2.Debug選擇專案名稱而非IIS
3.於wwwroot根目錄中只保留一個html檔案且於startup Configure方法中加入  app.UseDefaultFiles();就可以直接導向該頁面而非預設畫面。