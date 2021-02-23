#  Mixin

`Mixin`是個我以往沒見過的關鍵字，似乎在Dart撰寫中繞不開的檻，故特此撰寫一篇筆記來記錄學習過程。

[Reference:Xyphuz](https://wst24365888.github.io/dart-mixin/)

[Reference:Manoel](https://medium.com/@manoelsrs/dart-extends-vs-implements-vs-with-b070f9637b36)

[Reference:Wikipedia](https://zh.wikipedia.org/wiki/Mixin)

## Dart 中的繼承機制

### Extends

在Dart中，`extends`只能繼承**一個**類別

寫個例子

```dart
abstract class ClassA
{
  String property;
  void method();
}
class ClassB extends ClassA
{
  @override
  void method()=>this.property="ClassB";
}
void main()
{
  ClassA itemA = new ClassB();
  ClassB itemB = new ClassB();
  itemA.method();
  itemB.method();
  print(itemA.property);
  print(itemB.property);
}
```

註:

* 在Dart中`@override`可寫可不寫，不寫也是一樣的運作方式。
* 在Dart中似乎沒有`new`複寫`method`的作法。

結果

```
ClassA
ClassA
```



**重點整理**

* `extends`只能繼承**一個**類別
* `extends`繼承後，子類別**必須**`override`父類別中的`abstract method`

### Implements

功能類似`extends`

在Dart中，`implements`後面能加上**多個**類別

將上例的`extends`改為`implements`得到如下得錯誤信息



![image-20210223101516028](https://i.imgur.com/4mNDbgg.png)

原因是`implements`必須將父類別的屬性初始化

加入`String property = "";`後就沒有錯誤了

```dart
abstract class ClassA
{
  String property;
  void method();
}
class ClassB implements ClassA
{
  String property = "";
  @override
  void method()=>this.property="ClassB";
}
void main()
{
  ClassA itemA = new ClassB();
  ClassB itemB = new ClassB();
  itemA.method();
  itemB.method();
  print(itemA.property);
  print(itemB.property);
}
```

輸出結果一樣就不提了。

另外試著寫寫看`implement`兩個`class`

```dart
abstract class ClassA
{
  String property;
  void method();
}
class ClassC
{
  void method2()=>{};
}
class ClassB implements ClassA, ClassC
{
  String property = "";
  @override
  void method()=>this.property="ClassB";
  @override
  void method2()=>{};
}
```

可以看到`ClassC`中`method2`已經被實作了，但是`ClassB` `implements`了`ClassC`之後**必須**再`override` `method2`一次



**重點整理**

* `implements`後方可以加入**多個**class
* 使用`implements`的子類別**必須**重作父類別的所有內容
  * 無論父類別的屬性是否初始化，子類別都必須將其初始化
  * 無論父類別的方法是否實作，子類別都必須實作

### with(mixin)

#### with

`with`本身沒啥特別的，重點在後面的`mixin`

總之先實際試試看

---

`with`一個`abstract class`

```dart
abstract class ClassA
{
  String propertyA;
  void methodA();
}
class ClassB with ClassA
{
  @override
  void methodA()=>{};
}
```

一樣必須`override` `abstract method`

---

`with`一個`class`

```dart
class ClassA
{
  String propertyA="";
  void methodA()=>{print("methodA")};
}
class ClassB with ClassA
{
}
```

這種情況和使用`extends`一樣。

(如果用`implements`就必須重寫`propertyA`和`methodA()`)

---

`with`多個`class`，一樣把輸出結果寫到後方註解

```dart
class ClassA
{
  String propertyA="A";
  void methodA()=>print("methodA");
}
abstract class ClassB
{
  String propertyB;
  void methodB();
}
class ClassC with ClassA, ClassB
{
  void methodB()=>print("methodB in ClassC");
}
void main()
{
  ClassC item = new ClassC();
  item.methodA();       //methodA 
  item.methodB();       //methodB in ClassC
  print(item.propertyA);//A 
  print(item.propertyB);//null
}
```

---

**重點整理**

* `with`可以繼承**多個**`class`
* `with`後方可接`class` `abstract class` 或 `mixin`

#### mixin

接著就是本筆記的重點`mixin`

引用[中文WIKI](https://zh.wikipedia.org/wiki/Mixin)對MIXIN的敘述

> **Mixin**是[物件導向程式設計語言](https://zh.wikipedia.org/wiki/面向对象程序设计语言)中的[類](https://zh.wikipedia.org/wiki/类_(计算机科学))，提供了方法的實現。其他類可以存取mixin類的方法而不必成為其子類。

> mixin為使用它的class提供額外的功能，但自身卻不單獨使用（不能單獨生成實例物件，屬於抽象類）。因為有以上限制，Mixin類通常作為功能模組使用，在需要該功能時「混入」，而且不會使類的關係變得複雜。使用者與Mixin不是「is-a」的關係，而是「-able」關係

> Mixin也可以看作是帶實現的[interface](https://zh.wikipedia.org/w/index.php?title=接口_(面向对象编程)&action=edit&redlink=1)。這種[設計模式](https://zh.wikipedia.org/wiki/设计模式)實現了[依賴反轉原則](https://zh.wikipedia.org/wiki/依赖反转原则)。



總之直接寫個例子，邊做邊學吧

已知

1. 我是人類
2. 我家的公主是一隻貓
3. 我和公主都會睡覺

別吐槽為啥沒有把睡覺列為人類和貓咪的基本技能，總之實作如下

```dart
abstract class Human{}
abstract class Cat{}
abstract class Sleepable{void sleep();}

//我是人類，人類會的我都會，然後我還會睡覺
class Me extends Human implements Sleepable
{
  void sleep()=>print("Zzz..");
}

//公主是貓，貓會的公主都會，然後公主還會睡覺
class Princess extends Cat implements Sleepable
{
  void sleep()=>print("Zzz..");
}
```

(我不知道有沒有sleepable這個單字和它是不是這個用法，不過這不重要)

在這個例子中明明兩者睡覺的行為是一樣的，但我卻實作了兩次

若能在`Sleepable`中實作方法並且子類別直接沿用，則較符合此處的需求

`extends`能滿足這個需求，但我們無法`extends`第二個`class`

這時`with`就能派上用場了，以`with`改寫如下

```dart
abstract class Human{}
abstract class Cat{}
class Sleepable
{
  void sleep()=>print("Zzz..");
}
class Me extends Human with Sleepable{}
class Princess extends Cat with Sleepable{}
```

**重點來啦**

這裡可以用上`mixin`在`Sleepable`上

用法很簡單就是拿`mixin`關鍵字取代`class`或`abstract class`

```dart
abstract class Human{}
abstract class Cat{}
mixin Sleepable
{
  void sleep()=>print("Zzz..");
}
class Me extends Human with Sleepable{}
class Princess extends Cat with Sleepable{}
```

那這個`mixin`和原本的`class`有什麼不同?

* `mixin`不能被`extends`
  * 比如`class Me extends Sleepable{}`是不被允許的
  * 但可以被`implement`(但依然需要重作所有方法屬性)

##### mixin on

用於限制`mixin`的使用

直接改上例

```dart
abstract class Human{}
abstract class Cat{}
mixin Sleepable on Human
{
  void sleep()=>print("Zzz..");
}
class Me extends Human with Sleepable{}
class Princess extends Cat with Sleepable{} //這行會報錯，因為現在只有人類才能睡覺
```



##### linear

表`mixin`的特性

當`with`後方的多個`mixin`中有重複出現的方法的場合

1. 若子類別有`override`則優先採用
2. 出現在後方的`mixin`優先採用

```dart
abstract class Human{}
mixin Sleepable
{
  void sleep()=>print("Zzz..");
}
mixin SleepAgain
{
  void sleep()=>print("5 more minutes plz");
}
class Me extends Human with Sleepable, SleepAgain{}
void main()
{
  Me me = new Me();
  me.sleep();
}
```

輸出

```
5 more minutes plz
```



## 重點總結



* `extends`只能繼承**一個**類別
* `extends`繼承後，子類別**必須**`override`父類別中的`abstract method`

* `implements`後方可以加入**多個**class
* 使用`implements`的子類別**必須**重作父類別的所有內容
  * 無論父類別的屬性是否初始化，子類別都必須將其初始化
  * 無論父類別的方法是否實作，子類別都必須實作

* `with`可以繼承**多個**`class`
* `with`後方可接`class` `abstract class` 或 `mixin`

* `mixin`不能被`extends`
  * 但可以被`implement`(但依然需要重作所有方法屬性)

* 可以透過`mixin A on B`限制只有`B`或其子類別才能`with A`
* 當`with`後方的多個`mixin`中有重複出現的方法的場合
  1. 若子類別有`override`則優先採用
  2. 出現在後方的`mixin`優先採用