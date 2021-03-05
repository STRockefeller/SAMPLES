# Stock Crawler

Flutter專案還沒寫完就開新坑，嘛-想寫啥就寫啥

原本是想找找有沒有API可以串，不過看看幾乎都是證券商的收費項目。

好吧，我就堅持做我的免費仔，搞個爬蟲就好

## 20210303

### 參考資料

* https://blog.shiangsoft.com/stock-price-clawer/
* https://ithelp.ithome.com.tw/articles/10232735
* https://ithelp.ithome.com.tw/articles/10215471
* https://www.finlab.tw/%E8%B6%85%E7%B0%A1%E5%96%AE%E5%8F%B0%E8%82%A1%E6%AF%8F%E6%97%A5%E7%88%AC%E8%9F%B2%E6%95%99%E5%AD%B8/

### 規劃

專案類型還沒決定，總之先寫成dll好了，之後比較方便

目標網站暫定為[goodinfo](https://goodinfo.tw/StockInfo/index.asp)和[台灣證券交易所](https://www.twse.com.tw/zh/)

以VS建立專案，類型是`.net core 類別庫`

ORM 選用 EF Core ，順便當作練習(過去使用linq2sql的經驗較多)

### EF Core Problem

建立migrations和db時，再次遇到`the specified deps.json ...`找不到的問題，先前是換個電腦就解決了

當初始終沒有找到排除方法，這次再稍稍嘗試看看能不能排除。

這次主要參考[Github上的issue回報](https://github.com/dotnet/efcore/issues/16386)

> ### Error solution
> The error appears when programming in Microsoft.AspNetCore.MVC with NET 5.0, when trying to create a Migration and a DataBase.
> By typing in the package manager console:
>
> **PM> Add-Migration InitialCreate (or Update-Database**)
> Build started.
> Build succeeded.
> ***The specified deps.json [C: \ VSProjects \ RazorPagesMovie \ RazorPagesMovie.deps.json] does not exis***t, and the migration is not created and then not all the tables in the DataBase are created.
>
> **Solution:**
>
> 1. In appsettings.json, in "DefaultConnection" remove the final part "MultipleActiveResultSets = true"
> 2. In the package manager console (PowerShell), and having installed "Dotnet":
>    a) dotnet tool uninstall --global dotnet-ef (To Remove it only if it exists)
>    b) dotnet tool install --global dotnet-ef (To Install it if it does not exist)
>    c) dotnet ef (To see that it is installed correctly)
>    d) dotnet ef migrations add InitialCreate
>    e) dotnet ef database update
>
> In **View -> SQL Object Explorer,** you will find the DataBase with its tables.

```
PS C:\Users\admin\source\repos\StockCrawlerTW_ByRockefeller> dotnet tool uninstall --global dotnet-ef
PS C:\Users\admin\source\repos\StockCrawlerTW_ByRockefeller> dotnet tool install --global dotnet-ef
已成功安裝工具 'dotnet-ef' ('5.0.3' 版)。
PS C:\Users\admin\source\repos\StockCrawlerTW_ByRockefeller> dotnet ef migrations add InitialCreate
No project was found. Change the current working directory or use the --project option.
Build started...
Done. To undo this action, use 'ef migrations remove'
PS C:\Users\admin\source\repos\StockCrawlerTW_ByRockefeller> dotnet ef database update
No project was found. Change the current working directory or use the --project option.
PS C:\Users\admin\source\repos\StockCrawlerTW_ByRockefeller> dotnet ef database update --project StockCrawlerTW_ByRockefeller
Build started...
Build succeeded.
Applying migration '20210303050711_InitialCreate'.
Done.
```

結果意外地順利

跑完目錄如下

![](https://i.imgur.com/JvZMY7m.png)

db裡面長這樣

![](https://i.imgur.com/LIbXKob.png)

![](https://i.imgur.com/ZrXDDIE.png)

當然這個部分預計還會陸續新增，因為目前也還沒想好要做成怎樣，不過好像還需要重建DB似乎有點麻煩

### 將資料庫操作的方法獨立出來

暫定如下圖

![](https://i.imgur.com/bh9qR3w.png)

### Crawler-Good info

喜聞樂見的爬蟲環節

上面幾個步驟包含table構成，DB操作方法等都還只是暫定，待Crawler寫完後應該會修正不少



首先是goodinfo

goodinfo的個股資訊url構成長這樣 `https://goodinfo.tw/StockInfo/StockDetail.asp?STOCK_ID=2330`

先試著把下圖的基本資訊爬出來

![](https://i.imgur.com/PK3Zfyd.png)

檢視網頁原始碼常常看到這種破爛排版，不過還是找到了

![](https://i.imgur.com/CKXMZ74.png)



名稱從`<title>`找

```html
<title>(2330)台積電 個股市況總覽 - Goodinfo!台灣股市資訊網</title>
```



#### innerText 亂碼

測試程式回傳`System.InvalidOperationException: 'Sequence contains no elements'`

逐步執行檢查發現爬到的html中innerText是亂碼

![](https://i.imgur.com/VeiMGtr.png)

這個問題可以透過修改`HtmlWeb`物件的Encoding來解決，我改成UTF-8就能正確顯示內容了

```C#
webClient.OverrideEncoding = Encoding.UTF8;
```

但是...

![](https://i.imgur.com/XIP6TbV.png)

好吧，我早該想到了orz

照這樣看來其他網站也不給爬的機會應該不小



---

### Crawler-台灣證券交易所

good info 不給爬，那來試試政府的網站好了

url格式

`https://www.twse.com.tw/zh/stockSearch/showStock?stkNo=2330`

目前粗略測試是可以爬到的，但我不會看證券交易所的資料...

今天先到這裡，下次繼續研究。

---

## 20210304

### Crawler-鉅亨

證券交易所不會看，這次試試鉅亨

url格式

```
https://invest.cnyes.com/twstock/TWS/2330
```



這次有爬到內容，終於可以繼續下去了

來檢視一下目標

![](https://i.imgur.com/iIC3gtb.png)



看來開盤前的資訊比較少，本來還以為開盤前會顯示前一天的資料，看來之後可能需要新增爬取歷史資料的方法

更新一下股票資訊

![](https://i.imgur.com/5gbbegh.png)

爬出來長這樣

![](https://i.imgur.com/6oaXxm8.png)



更新一下查詢方法

![](https://i.imgur.com/Uc1HD5W.png)

### 更改架構

希望單一股票不只儲存最新爬到的資訊而是可以將過去爬的也保留下來

* 把`id`作為單純標示使用
* 新增`codeName`作為股票代號



#### Debug評估逾時問題

![](https://i.imgur.com/kHtZtvn.png)

然後我的`stock`物件會被認定為null導致執行失敗

```
The data is NULL at ordinal 10. This method can't be called on NULL values. Check using IsDBNull before calling.
```

猜測或許和資料庫格式變動有關

![](https://i.imgur.com/mmAeOVg.png)

明顯發生資料位移的情況

猜測沒錯，把資料先手動刪除後就正常了



---

現在爬出來是這個樣子

![](https://i.imgur.com/yint2uW.png)

排序之後像這樣

![](https://i.imgur.com/IccaUky.png)

**理想狀態**



好，OK，這個專案先到此為止，之後要怎麼運用這個DLL再說。