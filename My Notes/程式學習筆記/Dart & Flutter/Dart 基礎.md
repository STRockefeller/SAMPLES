# Dart 基礎

[Reference:ITHELP](https://ithelp.ithome.com.tw/articles/10215198)

[Reference:DartDev](https://dart.dev/tutorials)

[Reference:tutorialspoint](https://www.tutorialspoint.com/dart_programming/index.htm)

從頭學習新的程式語言筆記就寫詳細點吧

先使用[DartPad](https://dartpad.dev/)作練習用。

---

## 關於Dart

引用WikiPedia

> **Dart** is a client-optimized[[8\]](https://en.wikipedia.org/wiki/Dart_(programming_language)#cite_note-8) programming language for [apps](https://en.wikipedia.org/wiki/Mobile_app) on multiple platforms. It is developed by [Google](https://en.wikipedia.org/wiki/Google) and is used to build mobile, desktop, server, and web applications.[[9\]](https://en.wikipedia.org/wiki/Dart_(programming_language)#cite_note-9)
>
> Dart is an [object-oriented](https://en.wikipedia.org/wiki/Object-oriented_programming), [class-based](https://en.wikipedia.org/wiki/Class-based_programming), [garbage-collected](https://en.wikipedia.org/wiki/Garbage_collection_(computer_science)) language with [C](https://en.wikipedia.org/wiki/C_(programming_language))-style [syntax](https://en.wikipedia.org/wiki/Syntax_(programming_languages)).[[10\]](https://en.wikipedia.org/wiki/Dart_(programming_language)#cite_note-10) Dart can [compile](https://en.wikipedia.org/wiki/Compiler) to either [native code](https://en.wikipedia.org/wiki/Machine_code) or [JavaScript](https://en.wikipedia.org/wiki/JavaScript). It supports [interfaces](https://en.wikipedia.org/wiki/Interface_(object-oriented_programming)), [mixins](https://en.wikipedia.org/wiki/Mixin), [abstract classes](https://en.wikipedia.org/wiki/Abstract_class), [reified](https://en.wikipedia.org/wiki/Reification_(computer_science)) [generics](https://en.wikipedia.org/wiki/Generic_programming), and [type inference](https://en.wikipedia.org/wiki/Type_inference).[[11\]](https://en.wikipedia.org/wiki/Dart_(programming_language)#cite_note-11)



## 環境設置

參考[tutorialspoint的教學](https://www.tutorialspoint.com/dart_programming/dart_programming_environment.htm)應該就行了，目前先用[DartPad](https://dartpad.dev/)作練習用，之後嘗試環境建置如果有比較特別的狀況再補充。



## Hello World

打開[DartPad](https://dartpad.dev/)預設會顯示以下內容

```dart
void main() {
  for (int i = 0; i < 5; i++) {
    print('hello ${i + 1}');
  }
}
```

可以看到程式進入點也是` void main () `，程式碼風格大體上和C家族很相似，字串顯示的部分比較特別，不過也是很直觀，在C#應該長這樣`$"hello {i+1}"`。



## Data Types

型別分為以下幾種

- Numbers
- Strings
- Booleans
- Lists
- Maps

### Numbers

分成整數的`int`和浮點數的`double`

### Strings

`String `(S大寫)用於表示UTF-16編碼的字串，字串內容可用`''`或`""`包起來。

另外可以用`Runes`表示UTF-32編碼，[參考資料](https://api.dart.dev/stable/2.10.5/dart-core/Runes-class.html)。

插入變數的做法

```dart
"Hello ${name} !!"
```



### Booleans

關鍵字`bool`，值分為`true`和`false`。

### Lists

表有序的物件集合

```dart
void main() { 
   var lst = new List(3); //3代表List的長度，new List()代表長度為0
   lst[0] = 12; 
   lst[1] = 13; 
   lst[2] = 11; 
   print(lst); 
}
```

```dart
void main() { 
   var num_list = [1,2,3]; //用[]來設定初始內容
   print(num_list); 
}
```

```dart
void main() { 
   var lst = new List(); 
   lst.add(12); 
   lst.add(13); 
   print(lst); 
} 
```

List沒有distinct方法，不過可以透過以下方式做到相同效果

```dart
List<Item> items;
items.toSet().toList();
```



### Maps

key - value 配對的集合，類似於Dictionary或Hash Table

```dart
void main() { 
   var details = {'Usrname':'tom','Password':'pass@123'}; 
   print(details); 
}
```

> ```
> {Usrname: tom, Password: pass@123}
> ```

```Dart
void main() { 
   var details = {'Usrname':'tom','Password':'pass@123'}; 
   details['Uid'] = 'U1oo1'; 
   print(details); 
} 
```

> ```
> {Usrname: tom, Password: pass@123, Uid: U1oo1}
> ```

```dart
void main() { 
   var details = new Map(); 
   details['Usrname'] = 'admin'; 
   details['Password'] = 'admin@123'; 
   print(details); 
} 
```

> ```
> {Usrname: admin, Password: admin@123}
> ```

### Dynamic Types

關鍵字`dynamic`，就我的理解相當於宣告一個弱型別的變數。

試寫了一下

```dart
void main() {
  dynamic d = 123;
  print(d);
  d = "asd";
  print(d);
}
```

> 123 asd



## Operators

### Arithmetic Operators

基本和C家族的一樣

`+` `-` `*` `/` `%` `++` `--`

`~/`代表除法取整數商

### Equality and Relational Operators

基本和C家族的一樣

`>` `>=` `<` `<=` `=` `==` `!=`

### Type test Operators

型別判斷用回傳Boolean 關鍵字 `is` `is!`

```dart
void main() {
  int a = 123;
  dynamic b =456;
  print(a is int);
  print(b is! int);
}
```

> true false

### Bitwise Operators

基本上也是類似C家族

| Logic       | Operator |
| ----------- | -------- |
| And         | a&b      |
| Or          | a\|b     |
| Xor         | a^b      |
| Not         | ~a       |
| Left Shift  | a<<b     |
| Right Shift | a>>b     |



### Assignment Operators

同樣類似C家族，很好理解。

這個部分直接貼 [範例](https://www.tutorialspoint.com/dart_programming/dart_programming_assignment_operators.htm)



The following example shows how you can use the assignment operators in Dart −

[Live Demo](http://tpcg.io/dHVftW)

```
void main() { 
   var a = 12; 
   var b = 3; 
     
   a+=b; 
   print("a+=b : ${a}"); 
     
   a = 12; b = 13; 
   a-=b; 
   print("a-=b : ${a}"); 
     
   a = 12; b = 13; 
   a*=b; 
   print("a*=b' : ${a}"); 
     
   a = 12; b = 13; 
   a/=b;
   print("a/=b : ${a}"); 
     
   a = 12; b = 13; 
   a%=b; 
   print("a%=b : ${a}"); 
}    
```

It will produce the following **output** −

```
a+=b : 15                         
a-=b : -1                             
a*=b' : 156                                    
a/=b :0.9230769230769231                       
a%=b : 12
```



### Logical Operators

| Logic | Operators |
| ----- | --------- |
| And   | a&&b      |
| Or    | a\|\|b    |
| Not   | !a        |



### Conditional Expressions

* Condition?expr1:expr2

  跟C家族一樣的用法BJ4

* expr1??expr2

  If **expr1** is non-null, returns its value; otherwise, evaluates and returns the value of **expr2**

```dart
void main() { 
   var a = null; 
   var b = 12; 
   var res = a ?? b; 
   print(res); 
}
```

> 12



## Loops

一樣是`for` `while` `do while`

用法和C一模一樣，沒什麼值得一提的



### foreach

dart foreach的用法和C#有一點點不一樣，特別提一下

假設我有一個物件如下

```dart
List<Item> items = <Item>[];
```

如果要歷遍`items`有以下兩種做法

作法一

```dart
for(Item item in items){
    //...
}
```

作法二

```dart
items.forEach((item){/*...*/});
```



順帶一提，[官方並不推薦使用作法二](https://dart-lang.github.io/linter/lints/avoid_function_literals_in_foreach_calls.html)

## Decision Making

`if` `else if` `else `用法和C一模一樣，沒什麼值得一提的

值得一提的是`switch`裡面`case`的內容用`{}`包起來，如下。

```dart
void main() { 
   var grade = "A"; 
   switch(grade) { 
      case "A": {  print("Excellent"); } 
      break; 
     
      case "B": {  print("Good"); } 
      break; 
     
      case "C": {  print("Fair"); } 
      break; 
     
      case "D": {  print("Poor"); } 
      break; 
     
      default: { print("Invalid choice"); } 
      break; 
   } 
}  
```

