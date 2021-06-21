# Manifest Version 3

剛好有個完整的manifest V2 的擴充功能要改成 manifest V3，順便把過程記錄下來。

主要參考google 官方文件 https://developer.chrome.com/docs/extensions/mv3/intro/mv3-migration/



按順序紀錄

### 改版本號

manifest.json

```json
"manifest_version": 2,
```

```json
"manifest_version": 3,
```



---

### Host permissions

MV3的Host permissions 要另外宣告，不過我這次沒用到，需要的話參考

https://developer.chrome.com/docs/extensions/mv3/intro/mv3-migration/#host-permissions

---

### 改CSP

首先改格式

manifest.json

```json
"content_security_policy": "script-src 'self' ;script-src-elem 'self'; object-src 'self'",
```

```json
    "content_security_policy": {
        "script-src": "self",
        "script-src-elem": "self",
        "object-src": "self"
    },
```

然後規範有所變更

In addition, MV3 disallows certain CSP modifications for `extension_pages` that were permitted in MV2. The `script-src,` `object-src`, and `worker-src` directives may only have the following values:

- `self`
- `none`
- Any localhost source, (`http://localhost`, `http://127.0.0.1`, or any port on those domains)

這樣我似乎應該直接刪掉了?



### 改action

MV3不再區分`page_action`和`browser_action`

manifest.json

```json
	"browser_action": {
        "default_icon": "icons/Icon-192.png",
        "default_popup": "popup.html"
    },
```

```json
    "action": {
        "default_icon": "icons/Icon-192.png",
        "default_popup": "popup.html"
    },
```



---

### Web-accessible resources

一樣這次沒用到，之後有用到再說。



### Background service workers

>The "background.scripts" key cannot be used with manifest_version 3. Use the "background.service_worker" key instead.
>
>The "background.persistent" key cannot be used with manifest_version 3. Use the "background.service_worker" key instead

- Replace `background.page` or `background.scripts` with `background.service_worker` in manifest.json. Note that the `service_worker` field takes a string, not an array of strings.
- Remove `background.persistent` from manifest.json.
- Update background scripts to adapt to the service worker execution context.

background.scripts不再適用

maniffest.json

```json
    "background": {
        "scripts": ["event.js"],
        "persistent": true
    }
```

```json
    "background": {
        "service_worker": "event.js"
    }
```

當然只是這樣改會導致我的右鍵選單完全沒有加入到，還須修改腳本內容才行。

[stackoverflow 這篇](https://stackoverflow.com/questions/38190701/why-does-chrome-contextmenus-create-multiple-entries)很有幫助

查看event.js的錯誤清單

```
Unchecked runtime.lastError: Extensions using event pages or Service Workers must pass an id parameter to chrome.contextMenus.create
```

先根據提示把每個選項加入id，加入id有助於在非persistent設定下避免重複創建，尤其service_worker會頻繁的重啟，實際測試下發現js在執行到重覆創建時會跳錯誤，但不影響執行也不會重複創建。想要避免重複創建可以加入該項目是否存在的判別，或者在每次執行創建前清空選項。

```typescript
    var parent = chrome.contextMenus.create({
        "title": "Search %s by",
        "contexts": ['all'],
    });
```

```typescript
    var parent = chrome.contextMenus.create({
        "id" : "parent",
        "title": "Search %s by",
        "contexts": ['all'],
    });
```

新的錯誤

```
Unchecked runtime.lastError: Extensions using event pages or Service Workers cannot pass an onclick parameter to chrome.contextMenus.create. Instead, use the chrome.contextMenus.onClicked event.
```

根據提示改用onclicked，官方文件 https://developer.chrome.com/docs/extensions/reference/contextMenus/



```typescript
function onClickUnsplash(info, tab): void { link("https://unsplash.com/s/photos/" + info.selectionText) }
		var Unsplash = chrome.contextMenus.create({
            "id":"Unsplash",
            "title": "Unsplash",
            "type": "normal",
            "contexts": ['all'],
            "parentId": parent,
            "onclick": onClickUnsplash
        });
```

```typescript
function onClickUnsplash(info, tab): void { link("https://unsplash.com/s/photos/" + info.selectionText) }
chrome.contextMenus.onClicked.addListener(function (info, tab): void {
    switch (info.menuItemId) {
        case "Unsplash":
            onClickUnsplash(info, tab);
            break;
            //....
    }
});
		var Unsplash = chrome.contextMenus.create({
            "id":"Unsplash",
            "title": "Unsplash",
            "type": "normal",
            "contexts": ['all'],
            "parentId": parent,
        });
```





這個部分改很多，具體參考https://developer.chrome.com/docs/extensions/mv3/migrating_to_service_workers/

### 改options_ui

> chrome_style option cannot be used with manifest version 3.

```json
    "options_ui": {
        "page": "options.html",
        "chrome_style": true
    },
```

```json
    "options_ui": {
        "page": "options.html"
    },
```

