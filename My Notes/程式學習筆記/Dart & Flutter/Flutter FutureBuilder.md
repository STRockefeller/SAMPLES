# FutureBuilder

## Review with Questions



## Abstract

參考

* [ITHelp](https://ithelp.ithome.com.tw/articles/10250044)
* [FlutterAPI](https://api.flutter.dev/flutter/widgets/FutureBuilder-class.html)

`FutureBuilder`是一種是用於非同步UI更新的方法

```dart
 const FutureBuilder({
    Key key,
    this.future,
    this.initialData,
    @required this.builder,
  }) : assert(builder != null),
       super(key: key);
```



## Getting Started

拿下例解說(來自[這篇文章](https://medium.com/%E9%9F%93%E6%96%87%E5%AD%B8%E7%BF%92%E7%AD%86%E8%A8%98/%E7%94%A8-flutter-google-sheets-%E5%BB%BA%E7%AB%8B%E5%96%AE%E5%AD%97%E6%9C%AC-app-c51f198cfbf3))

```dart
Widget buildVocabList() =>
      FutureBuilder<List<VocabData>>(
        future: getVocabDataList(),
        builder: (BuildContext context, AsyncSnapshot snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return Container();
          }
          final vocabList = snapshot.data;
          return (isListViewMode) ? buildListView(vocabList) : buildPageView(vocabList);
        },
      );
```

1. 一般會透過`FutureBuilder<T>`來指定所要回傳的內容
2. `future`屬性是一個回傳`Future<T>`的方法，同時也是我們要等待的目標
3. `builder`屬性回傳最終要的Widget