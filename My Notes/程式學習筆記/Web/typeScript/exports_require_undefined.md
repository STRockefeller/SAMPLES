# exports / require is undefined error

這個問題我在.net專案有遇過一次，當時似乎是靠gulp解決的(?)印象不是很清楚了，現在(2021/06)寫chrome extension又再次遇到便把它紀錄下來。



譬如我在`typescript`中引用本地`jquery`以及`bootstrap`

```typescript
import 'jquery';
import 'bootstrap';
```

`tsc`後變成

```javascript
"use strict";
exports.__esModule = true;
require("jquery");
require("bootstrap");
```



其實這個錯誤並不限定於typescript，更準確來說應該是來源於`node.js`，簡單來說就是有一部分的npm語法可以正常在本地端運行，但丟到伺服器上面就會出問題。

https://stackoverflow.com/questions/43042889/typescript-referenceerror-exports-is-not-defined

https://stackoverflow.com/questions/31931614/require-is-not-defined-node-js



## browserify

https://browserify.org/

在Stackoverflow中有人提到這個工具，有略為嘗試過，大致上就是把npm的各種東西都搬出來放到你的js中，我原本一百行左右的js被輸出成18000+行，以chrome extension來說至少不會導入就跳錯誤了，不過實際執行的時候jquery的`$`還是無法被辨識。

不過即便可行這項工具依然不符合我的需求，動輒一兩萬行的程式碼實在過於冗長，況且現今的chrome extension不允許minify並且要求盡量高可讀性，或許將來寫.net專案的時候可以考慮看看。



## 避免在`javascript`中引用其他檔案

逃避雖然可恥，但有用!

不是辦法的辦法:

1. 妥協使用cdn，以這次的chrome extension來說，cdn在設定完CSP後是可以正常使用的，應該說我在最初的功能開發階段都是以cdn完成的，但是如果打算上架功能，google官方是不建議使用cdn的(可能有連結失效的風險)

2. 使用純javascript撰寫，以chrome extension來說應該是最佳做法，一方面做到了更好的可讀性和更精簡的程式碼，另一方面規模小的專案改起來也很快，但如果是其他大規模的專案，別說不引用npm，光是不能引用自己的其他檔案也很麻煩。

   * 註: 不要使用網路上convert jquery to javascript 工具，改完還要修改很多地方，跟自己重寫沒差太多。

3. 在html中引用npm套件，這個做法也大致可行，目前發現兩個缺點

   * 有些js的呼叫不通過html，例如chrome extension在背景運作的js (註:manifest version 3不使用下例的這種寫法)

     ```json
         "background":{
             "scripts":["event.js"],
             "persistent":true
         }
     ```

   * 有些npm套件本身依然有無法被認可的程式片段，例如`popper.js`中就有以下內容報錯

     ```js
     export default Popper;
     ```

     錯誤內容:

     ```
     Uncaught SyntaxError: Unexpected token 'export'
     ```

     

