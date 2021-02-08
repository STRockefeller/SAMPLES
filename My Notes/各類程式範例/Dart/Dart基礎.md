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

