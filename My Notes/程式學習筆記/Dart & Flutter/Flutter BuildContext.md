# BuildContext

[reference:FlutterApi:BuildContext](https://api.flutter.dev/flutter/widgets/BuildContext-class.html)

[reference](https://juejin.cn/post/6844903777565147150)

A handle to the location of a widget in the widget tree.

`BuildContext`是一個`Class`，關係到整個Flutter專案的頁面架構，並且有點難懂，這邊就特別拿出來記錄學習過程。

這東西很常見，但就是不知道在幹啥用的，初始專案中的`_MyHomePageState`類別中就包含以下內容。

```dart
  @override
  Widget build(BuildContext context){
      //...
  }
```

然而`context`物件在初始專案中沒有被使用到，要了解它必須先找到有使用到的Sample比較好切入。

以下是[官方API](https://api.flutter.dev/flutter/widgets/BuildContext-class.html)提供的Sample

```dart
  @override
  Widget build(BuildContext context) {
    // here, Scaffold.of(context) returns null
    return Scaffold(
      appBar: AppBar(title: Text('Demo')),
      body: Builder(
        builder: (BuildContext context) {
          return TextButton(
            child: Text('BUTTON'),
            onPressed: () {
              // here, Scaffold.of(context) returns the locally created Scaffold
              Scaffold.of(context).showSnackBar(SnackBar(
                content: Text('Hello.')
              ));
            }
          );
        }
      )
    );
  }
```

以下節錄[官方API](https://api.flutter.dev/flutter/widgets/BuildContext-class.html)的描述

> [BuildContext](https://api.flutter.dev/flutter/widgets/BuildContext-class.html) objects are passed to [WidgetBuilder](https://api.flutter.dev/flutter/widgets/WidgetBuilder.html) functions (such as [StatelessWidget.build](https://api.flutter.dev/flutter/widgets/StatelessWidget/build.html)), and are available from the [State.context](https://api.flutter.dev/flutter/widgets/State/context.html) member. Some static functions (e.g. [showDialog](https://api.flutter.dev/flutter/material/showDialog.html), [Theme.of](https://api.flutter.dev/flutter/material/Theme/of.html), and so forth) also take build contexts so that they can act on behalf of the calling widget, or obtain data specifically for the given context.

> Each widget has its own [BuildContext](https://api.flutter.dev/flutter/widgets/BuildContext-class.html), which becomes the parent of the widget returned by the [StatelessWidget.build](https://api.flutter.dev/flutter/widgets/StatelessWidget/build.html) or [State.build](https://api.flutter.dev/flutter/widgets/State/build.html) function. (And similarly, the parent of any children for [RenderObjectWidget](https://api.flutter.dev/flutter/widgets/RenderObjectWidget-class.html)s.)

重點整理

* `BuildContext`物件通常是作為`WidgetBuilder`方法的參數
  * `WidgetBuilder`包含了`StatelessWidget`和`StatefulWidget`的`build`方法
  * 一部分的靜態方法也可以使用`BuildContext`物件
* 每一個`Widget`物件都會有自己的`BuildContext`物件



以下是另一個範例

```dart
import 'package:flutter/material.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: FirstPage(),
    );
  }
}

class FirstPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: FlatButton(
            onPressed: () {
              Navigator.of(context)
                  .push(MaterialPageRoute(builder: (context) => SecondPage()));
            },
            child: Text('GO')),
      ),
    );
  }
}

class SecondPage extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(),
    );
  }
}

```

這是一個簡單的兩個頁面來回切換的範例

試著分析一下

* 在`FlatButton`中的`onPressed`屬性執行的方法

  * `Navigator.of()`這是一個`Navigator`的靜態方法，回傳`NavigatorState`物件

    [官方API](https://api.flutter.dev/flutter/widgets/Navigator/of.html)對它的說明如下

    > The state from the closest instance of this class that encloses the given context.

    目前我依然不太了解它的作用，**待補充**。

    但總之，我們可以透過這個方法取得`NavigatorState`物件

  * 通過這個`NavigatorState`物件，我們就可以執行`push()`方法

    > Push the given route onto the navigator.

    ```dart
    push<T extends Object>
    ```

  * 被`push`的`route`是一個`MaterialPageRoute`物件

    * 在這個新被實例化的`MaterialPageRoute`物件中設定`builder`屬性傳入的方法

      ```dart
      (context) => SecondPage()
      ```

      這邊類似C#的`Lambda Expression`，前面的`(context)`代表`BuilderContext`型別的參數

      當然不一定要寫成`context`，可以隨意做自己喜歡的命名如`A` `b` `_`等等，反正dart會知道要傳神麼進去

* 第二個頁面也就是`SecondPage`，如果只給他一個空白的`appBar`如下

  ```dart
      return Scaffold(
        appBar: AppBar(),
      );
  ```

  在這個`appBar`的左上方(約在原本`leading`的位置)會出現指向左邊的箭頭`<-`

  點擊箭頭會導回上一頁

  試著給它加上`leading`如下

  ```dart
      return Scaffold(
        appBar: AppBar(
          leading: new Icon(Icons.face)
        ),
      );
  ```

  則箭頭不見，也無法透過`leading`導回上一頁



---

拉回BuildContext

```dart
abstract class BuildContext {
  /// The current configuration of the [Element] that is this [BuildContext].
  Widget get widget;

  /// The [BuildOwner] for this context. The [BuildOwner] is in charge of
  /// managing the rendering pipeline for this context.
  BuildOwner get owner;
  ...
```

可以看到`BuildContext`是一個`abstract class`



 