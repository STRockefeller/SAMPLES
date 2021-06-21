# Chrome Extension

Reference

https://ithelp.ithome.com.tw/articles/10186039

https://medium.com/@dailyfluttermonster/how-to-create-a-flutter-chrome-extension-1293d75b887b



現在找到大部分的文章都是適用於manifest version 2，無法在現在(2021/06)最新的manifest version 3正常運作，官方有提供[MV2轉MV3的相關指南](https://developer.chrome.com/docs/extensions/mv3/intro/mv3-migration/)，我也會在閱讀MV3的文件之後更新筆記內容。



## 基本結構

首先整理一個Chrome Extension的最低要求



1. 建立一個資料夾

2. 在裡面建立以下檔案

   * `manifest.json`
   * 一個`.html`檔案(`popup.html`)
   * 一個19px*19px大小的`.png`檔案(`Icon.png`)

3. 給`manifest.json`如下內容

   ```json
   {
     "manifest_version": 2,
   
     "name": "Getting started example",
     "description": "This extension shows a Google Image search result for the current page",
     "version": "1.0",
   
     "browser_action": {
       "default_icon": "Icon.png",
       "default_popup": "popup.html"
     },
     "permissions": [
       "activeTab",
       "https://ajax.googleapis.com/"
     ]
   }
   ```

   

於Chrome的擴充元件頁面開啟開發者選項，選擇資料夾匯入即可



### 說明

`manifest.json`

```json
"manifest_version": 2,
"version": "1.0",
```

這兩個屬性是必須的沒有的話Chrome也會跳錯誤提示所以也不用特別記

`name` 和 `description`沒什麼好講的，就字面上的意思

```json
  "browser_action": {
    "default_icon": "Icon.png",
    "default_popup": "popup.html"
  },
```

定義了預設的html和icon，[browser_action](https://developer.chrome.com/docs/extensions/reference/browserAction/)代表在瀏覽器啟動期間作用，如果要在特定網站作用要使用[page_action](https://developer.chrome.com/docs/extensions/reference/pageAction/)



`permissions`表示擴充功能的[許可權限](https://developer.chrome.com/docs/extensions/mv3/declare_permissions/)



`*.htm;`

預設的html會在功能圖示被點擊時呼叫，和一般的html差不多，也可以引入css和js，因為CSP的關係，請盡量避免使用inline js/css





## 使用Flutter

修改`index.html`將`<script>`搬出來放到獨立的`.js`檔案，並且引用`main.dart.js`(這東西是建置後才會出現的，所以常常忘記orz)

修改`web`資料夾底下的`manifest.json`，加入一些必要項目，移除一些不被允許的項目。(嘗試安裝擴充功能時，Chrome就會顯示`manifest.json`有哪些屬性不能使用，再把他們刪掉就可以了。)



最後大概會長這個樣子

```json
{
    "manifest_version": 2,
    "version": "1.0",
    "name": "pixiv_search_helper",
    "short_name": "pixiv_search_helper",
    "description": "A new Flutter project.",
    "browser_action":
    {
        "default_icon":"icons/Icon-192.png",
        "default_popup": "index.html"
    },
    "permissions": [
        "activeTab",
        "https://ajax.googleapis.com/"
      ]
}
```

把預設專案的`manifest.json`修改完畢後就可以安裝到Chrome上面了

寫完之後以`flutter build web`指令建置，之後將build/web資料夾作為擴充功能安裝目錄選擇。

但是使用後會出現錯誤如下

```
Refused to execute inline script because it violates the following Content Security Policy directive: "script-src 'self' blob: filesystem:". Either the 'unsafe-inline' keyword, a hash ('sha256-DXZAqNzvAqk1i8OcZsvDGpMOOq7SQYHYiot6xzbp/po='), or a nonce ('nonce-...') is required to enable inline execution.
```

原因是出在CSP上。

### CSP

指的是content security policy，有點複雜，預計會再寫一篇筆記記錄它。(這篇[文章](https://devco.re/blog/2014/04/08/security-issues-of-http-headers-2-content-security-policy/)寫得不錯，幫了我不少忙。)

CSP會出於資安考量限制擴充功能文件的內容，限制的方式有很多。

如果選擇`"manifest_version": 2,`則會有一個預設的CSP`script-src 'self'; object-src 'self'`，這算是相當嚴格的限制

如果想要放寬限制，可以自己設定`"content_security_policy":`屬性

經過一段時間的嘗試，我得出如下設定可以正常執行flutter預設的專案內容(我另外把`index`裡面的`<script>`移出去另外建立了js檔案)

```json
"content_security_policy": "script-src 'self' 'unsafe-eval';script-src-elem 'self' http://localhost https://unpkg.com/; object-src 'self'",
```

至於有沒有更好的做法，以後有想到再來補充。

`https://unpkg.com/`是為了讓`main.dart.js`的內容通過，但我搞不明白為啥`main.dart.js`沒有被算在`self`裡面。



### 輸出畫面變成小方塊的情形

#### 描述

點action後跳出的畫面(預設畫面)變成一個很小的正方形，無論放了多大的Widget進去都只會顯示一小部分，另外用純html撰寫測試就不會有這種情形

#### 解決辦法

不要在預設的html畫面使用`<script>`插入腳本，改使用`<iframe>`



假設我的文件如下

index.html

```html
<html>
    <head>
        <!--my head-->
    </head>
    <body>
        <!--my body-->
        <script>my script</script>
        <script src="main.dart.js"></script>
    </body>
</html>
```



**NG**

index.html

```html
<html>
    <head>
        <!--my head-->
    </head>
    <body>
        <!--my body-->
        <script src="index.js"></script>
        <script src="main.dart.js"></script>
    </body>
</html>
```

index.js

```js
my script
```



**Good**

index.html

```html
<html>
    <head>
        <!--my head-->
    </head>
    <body>
        <!--my body-->
        <iframe src="index.htm" frameborder="0"></iframe>
    </body>
</html>
```

index.htm

```html
<html>
    <head>
    </head>
    <body>
        <script src="index.js"></script>
        <script src="main.dart.js"></script>
    </body>
</html>
```

index.js

```js
my script
```



## 使用Blazor

