# `const` `final` `late`

## Review with Questions

1. 請簡短說明三者的使用時機。

## Abstract

參考:

* https://codertw.com/%E7%A8%8B%E5%BC%8F%E8%AA%9E%E8%A8%80/88839/

常常搞混，所以寫筆記紀錄用法。

## Comparison

### `const`

只能被設一次值，在宣告處賦值，且值必須為編譯時常量；用於修飾常量。

```dart
const bar = 1000000;       // 定義常量值
// bar =13;   // 出現異常，const修飾的變數不能呼叫setter方法，即：不能設值，只能在宣告處設值
const atm = 1.01325 * bar; // 值的表示式中的變數必須是編譯時常量（bar）;
var c = 12;        
//  atm = 1 * c;  //出錯，因為c不是一個編譯時常量，即：非const修飾的變數（只有const修飾的變數才是編譯時常量）
```



```dart
// [] 建立一個空列表.
// const [] 建立一個空的不可變列表 (EIA).
var foo = const [];   // foo 目前是一個 EIA.
final bar = const []; // bar 永遠是一個 EIA.
const baz = const []; // baz 是一個編譯時常量 EIA.
//你可以改變 非final, 非const 修飾的變數,
// 即使它的值為編譯時常量值.
foo = [];
// 不能改變final和const修飾的變數的值.
// bar = []; // 未處理的異常.
// baz = []; // 未處理的異常.
```



### `final`

只能被設一次值，在宣告處賦值，值和普通變數的設值一樣，可以是物件、字串、數字等，用於修飾值的表示式不變的變數；

```dart
final name = 'Bob';   
// name = 'job'; //執行出錯，因為final修飾的變數不能呼叫其setter方法，即：不能設值
```



當為final修飾的值賦一個包含成員變數或方法的物件時：

1. 物件成員值能被修改，對於能夠新增成員的類（如List、Map）則可以新增或刪除成員

2. 變數本身例項不能被修改



### `late`

延遲初始化，使用到的時候才初始化這個屬性。