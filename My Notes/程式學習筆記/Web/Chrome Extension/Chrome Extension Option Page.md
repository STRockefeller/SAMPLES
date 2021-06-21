# Option Page and `chrome.storage`

參考

https://ithelp.ithome.com.tw/articles/10188720

筆記待補。



文章裡面沒有特別講，但是要記得加入permissions才能使用`chrome.storage`

```
...
  "permissions": [
    "storage"
  ],
...
```





Google 給的範例中

```js
// Saves options to chrome.storage
function save_options() {
  var color = document.getElementById('color').value;
  var likesColor = document.getElementById('like').checked;
  chrome.storage.sync.set({
    favoriteColor: color,
    likesColor: likesColor
  }, function() {
    // Update status to let user know options were saved.
    var status = document.getElementById('status');
    status.textContent = 'Options saved.';
    setTimeout(function() {
      status.textContent = '';
    }, 750);
  });
}

// Restores select box and checkbox state using the preferences
// stored in chrome.storage.
function restore_options() {
  // Use default value color = 'red' and likesColor = true.
  chrome.storage.sync.get({
    favoriteColor: 'red',
    likesColor: true
  }, function(items) {
    document.getElementById('color').value = items.favoriteColor;
    document.getElementById('like').checked = items.likesColor;
  });
}
document.addEventListener('DOMContentLoaded', restore_options);
document.getElementById('save').addEventListener('click',
    save_options);
```



`chrome.storage.sync.get` 和 `chrome.storage.sync.set`兩個方法都是非同步的，sync代表該資料會在使用者的帳號上同步，而非方法同步，後方的function就是該方法的callback ，它會在資料獲取完畢後被執行，一開始搞混這個造成不少麻煩。

這份[文件](https://developer.chrome.com/docs/extensions/reference/storage/)中有詳細說明：

get

function

Promise

Gets one or more items from storage.

The get function looks like this:

```
get(keys?: string | string[] | object, callback: function) => {...}
```

- keys

  string | string[] | object optional

  A single key to get, list of keys to get, or a dictionary specifying default values (see description of the object). An empty list or object will return an empty result object. Pass in `null` to get the entire contents of storage.

- callback

  function

  Callback with storage items, or on failure (in which case [`runtime.lastError`](https://developer.chrome.com/docs/extensions/reference/runtime/#property-lastError) will be set).

  The parameter should be a function that looks like this:

  `(items: object) => {...}`

  - items

    object

    Object with items in their key-value mappings.

