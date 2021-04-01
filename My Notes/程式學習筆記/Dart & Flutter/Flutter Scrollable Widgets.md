# Scrollable Widgets

嚴格來說這個標題下的不是很精準，Scrollable是目的，而Widgets是達成目的的手段，本身不一定有Scrollable的特性。

主要紀錄`SingleChildScrollView` `ListView` `ListBody`等Widgets的用法。(之後如果有接觸到類似的Widget會再進行補充)



## SingleChildScrollView 

[Api](https://api.flutter.dev/flutter/widgets/SingleChildScrollView-class.html)

> A box in which a single widget can be scrolled

在`SingleChildScrollView `底下的`child`會變為Scrollable，另外可以透過`constraints`設置一些條件限制

常用的`child` Widgets有`Column` `ListBody`等等

我自己測試過若把`SingleChildScrollView `放在`Column`底下，則依然有機會造成OverFlow情況，如以下結構

```dart
new Column(
    children: new List<Widget>[
        new Text(),
        new SingleChildScrollView(
            child: new Column(
                widget1,
                widget2,
                ...,
            ),
        ),
    ],
)
```

同樣以這個結構說明

假如我要放的widget1, widget2, ... (以下合稱`MyWidgets`)本身就會超出範圍，則

* `Column`>`MyWidgets`  ==> 會超出範圍
* `SingleChildScrollView` >`Column`>`MyWidgets`   ==> 不會超出範圍，Scrollable
* `Column` >`SingleChildScrollView` >`Column`>`MyWidgets`   ==> 會超出範圍
* `Stack` >`SingleChildScrollView` >`Column`>`MyWidgets`   ==> 不會超出範圍，但上方的物件不會被顯示出來(被蓋掉)

以下是我推測的原因

1. 外層的Widget如`Column` `Row` `Container` 等等，會根據其`child` 或`children`改變他的大小，並且沒有限制
2. Scrollable Widget 如 `SingleChildScrollView`  即便其`child`過大也不會超出範圍
3. 最外層套上一般Widget如`Column`時，他會無限擴大自己直到能裝下所有的子項目，即便其中包含Scrollable Widget



以下是API範例

```dart
/// Flutter code sample for SingleChildScrollView

// In this example, the children are spaced out equally, unless there's no more
// room, in which case they stack vertically and scroll.
//
// When using this technique, [Expanded] and [Flexible] are not useful, because
// in both cases the "available space" is infinite (since this is in a viewport).
// The next section describes a technique for providing a maximum height constraint.

import 'package:flutter/material.dart';

void main() => runApp(const MyApp());

/// This is the main application widget.
class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  static const String _title = 'Flutter Code Sample';

  @override
  Widget build(BuildContext context) {
    return const MaterialApp(
      title: _title,
      home: MyStatelessWidget(),
    );
  }
}

/// This is the stateless widget that the main application instantiates.
class MyStatelessWidget extends StatelessWidget {
  const MyStatelessWidget({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return DefaultTextStyle(
      style: Theme.of(context).textTheme.bodyText2!,
      child: LayoutBuilder(
        builder: (BuildContext context, BoxConstraints viewportConstraints) {
          return SingleChildScrollView(
            child: ConstrainedBox(
              constraints: BoxConstraints(
                minHeight: viewportConstraints.maxHeight,
              ),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                mainAxisAlignment: MainAxisAlignment.spaceAround,
                children: <Widget>[
                  Container(
                    // A fixed-height child.
                    color: const Color(0xffeeee00), // Yellow
                    height: 120.0,
                    alignment: Alignment.center,
                    child: const Text('Fixed Height Content'),
                  ),
                  Container(
                    // Another fixed-height child.
                    color: const Color(0xff008000), // Green
                    height: 120.0,
                    alignment: Alignment.center,
                    child: const Text('Fixed Height Content'),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}

```



## ListView



在flutter SingleChildScrollView 的 API 說明中有這一段話

> When you have a list of children and do not require cross-axis shrink-wrapping behavior, for example a scrolling list that is always the width of the screen, consider [ListView](https://api.flutter.dev/flutter/widgets/ListView-class.html), which is vastly more efficient that a [SingleChildScrollView](https://api.flutter.dev/flutter/widgets/SingleChildScrollView-class.html) containing a [ListBody](https://api.flutter.dev/flutter/widgets/ListBody-class.html) or [Column](https://api.flutter.dev/flutter/widgets/Column-class.html) with many children.

就是說在功能單純的情況下ListView會是比較有效率的選項