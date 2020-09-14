//sample1

var a = 123;
var b = "abc";
var c = null;

console.log( a && b );        // "abc"
console.log( a || b );        // 123

console.log( c && a );        // null
console.log( c || b );        // "abc"
console.log( c || a );        // 123




if( a && b ) { ... } //正常執行

if( a || b ) { ... } //正常執行

/*
在 JavaScript 這門程式語言當中，我們可以分成兩種「值」：

那些經過 ToBoolean 轉換後得到 false 的值
以及其他的值，通常這些最後都會變成 true
這不是在講廢話，我知道你看完都硬了，快收起你的拳頭。

Falsy 與 Truthy: 論 Boolean 的型別轉換
前面講過，JavaScript 這門程式語言，我們可以分成兩種「值」，第一種就是經過 ToBoolean 轉換後會變成 false 的部分：

Undefined
Null
+0, -0, or NaN
空字串 "" 或 ''

來源: ECMAScript® 2017 Language Specification: 7.1.2 ToBoolean



如果是上面列出的幾種情況，那麼透過 ToBoolean 轉換就會變成 false，而其他的部分都會是 true。

而那些轉換後會得到 false 結果的，我們通常稱這些叫 「falsy」值，而其他會變成 true 的部分，則是 「truthy」值。

聽起來好像很好理解嘛，讓我們來猜猜下面程式片段會得到什麼：
*/

Boolean("false")     // ?

Boolean("0")         // ?

Boolean("''")        // ?

/*
猜對了嗎？ 答案都是 true。

裡面只有 "''" 看起來比較像 false，但規範裡寫著是「空字串」 "" 或 ''，可不是「雙引號包覆單引號」喔。

然而還有一些容易讓人搞混的地方：

Boolean( {} )

Boolean( [] )

Boolean( function(){} )
以上這些也都是 true。

*/


var a = 123;
var b = "abc";
var c = null;

console.log( a && b );        // "abc"
console.log( a || b );        // 123

console.log( c && a );        // null
console.log( c || b );        // "abc"
console.log( c || a );        // 123

/*
說好的 true 跟 false 呢？ 誰跟你說好

來看看 ECMAScript: 12.13Binary Logical Operators 規範怎麼說：

The value produced by a && or || operator is not necessarily of type Boolean. The value produced will always be the value of one of the two operand expressions.

簡單來說，透過 && 或 || 所產生的值不一定會是 Boolean，而是兩者其中之一。

&& 與 || 運算子在判斷的時候，會先對左邊的數值進行檢查。

如果是 Boolean 類型就再做後續的判斷，如果不是？那就會透過 ToBoolean 判斷是「falsy」或「truthy」來轉換成對應的 true 跟 false 。
對 || 運算子來說，若第一個數值轉換為 true，則回傳第一個數值，否則回傳第二個數值。
對 && 運算子來說，若第一個數值轉換為 true，則回傳第二個數值，否則回傳第一個數值。
所以，在 if 條件判斷當中，JavaScript 會針對回傳後的數值再度做 ToBoolean 判斷是「falsy」或「truthy」，
這也就是為什麼在 && 與 || 的結果可以用來當作 true 與 false 的判斷了。

所以說，未來如果看到這類想騙人的題目：
*/

!!'false' ==  !!'true'    // ? -->true
!!'false' === !!'true'    // ? -->true

