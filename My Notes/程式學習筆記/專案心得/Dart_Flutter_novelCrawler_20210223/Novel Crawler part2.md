# Novel Crawler

## 20210225

筆記寫太多了閱讀不易，分第二的檔案出來

### could not receive message from daemon

昨天還正常的專案今天打開又發生了第一次Debug失敗的那個錯誤，這次沒發現可以更新的SDK，換個關鍵字去Google就找到了這次問題的元兇

![](https://i.imgur.com/wec3wWk.png)

[StackOverFlow](https://github.com/gradle/gradle/issues/14094)

沒錯，只要把win10筆電的行動熱點關掉就行了orz

我在想前天遇到的狀況可能也是這個原因，而不是SDK未更新



>  事情太多了沒空寫，今天到此為止



## 20210322

時隔一個月繼續，超怠惰

### Crawler

[Reference:StackOverFlow](https://stackoverflow.com/questions/51865729/flutter-how-to-parse-data-from-a-website-to-a-list-view)

i created a full application that shows how to parse HTML and extract data from it you can [find it here](https://github.com/Rahiche/flutter_jobs_app) but the idea is simple :

1.import this 3 libraries for html parsing

```
import 'package:http/http.dart' as http;
import 'package:html/parser.dart' as parser;
import 'package:html/dom.dart' as dom;
```

2.get the data from the page you want

```
Future<List<String>> getData() async { 
  http.Response response = await http.get('website');
}
```

3.extract the data from the website

```
dom.Document document = parser.parse(response.body);
```

\4. depend on your need let's say you want to get all element with the article tag

```
document.getElementsByTagName('article')
```

and then you can iterate throw all article using for-each and do the same to get the data inside the article . also consider making a model class for the article so you can mangle that easily later on



原本有試過webscraper的方法不過看起來這個比較簡單一點，就改採用這個做法了



先寫成這樣試試看

```dart

import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:novel_crawler_by_rockefeller/MyControls.dart';
//import 'package:web_scraper/web_scraper.dart';
import 'package:http/http.dart' as http;
import 'package:html/parser.dart' as parser;
import 'package:html/dom.dart' as dom;

class CZBooksHomePage extends StatefulWidget
{
  @override
  State<StatefulWidget> createState() => new _CZBooksHomePageState();
}
class _CZBooksHomePageState extends State<CZBooksHomePage>
{
  List<Map<String,String>> novelSort;

  Future<dom.Document> getData() async { 
  http.Response response = await http.get('https://czbooks.net/');
  return parser.parse(response.body);
  } 

  void fetchProducts() async {
    dom.Document document = await getData();
    if(document != null)
    {
      setState(() {
        novelSort = document.getElementsByClassName('nav menu').first.getElementsByTagName('a').map((d)=>{d.text:d.attributes['href']});
      });
    }
  }
  List<Widget> sortList()
  {
    List<Widget> res = new List<Widget>();
    if(novelSort==null){return res;}
    novelSort.forEach((sort) { 
      res.add(new Text(
        sort.keys.first,
        style: TextStyle(fontSize: 20.0, color: Colors.purple)
      ));
    });
  }
  @override
  Widget build(BuildContext context) {
    fetchProducts(); //不用await不知道行不行
    return new Scaffold(
      appBar: MyControls.commonAppBar("CZBooks", ()=>{}),
      body: Column(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: sortList(),
      ),
    );
  }
}
```

結果執行時報錯

![](https://i.imgur.com/7VLD2uU.png)



簡單的說就是爬出來的結果型別為`MappedListIterable<Element, Map<String, String>>`而非預想的`List<Map<String,String>>`



![](https://i.imgur.com/hVgULh2.png)

![](https://i.imgur.com/BkJOLDy.png)

我想要的資料看起來是有順利爬到的



![](https://i.imgur.com/oj03b3g.png)

結果只是把

```dart
setState(() {
        novelSort = document.getElementsByClassName('nav menu').first.getElementsByTagName('a').map((d)=>{d.text:d.attributes['href']});
      });
```

改成

```dart
setState(() {
        var ns = document.getElementsByClassName('nav menu').first.getElementsByTagName('a').map((d)=>{d.text:d.attributes['href']});
        novelSort = List.from(ns);
      });
```

就能動了

まあ、どうでもいい

---

當然列出文字只是測試，實際是要一個連結到該分類的書單

這個列表好像放到Drawer裡面比較適當



### setState Exception

從剛剛試作的畫面點選上一頁欲回Home Page時跳Exception 如下

![](https://i.imgur.com/pQRI9eS.png)

```
Exception has occurred.
FlutterError (setState() called after dispose(): _CZBooksHomePageState#a5d61(lifecycle state: defunct, not mounted)
This error happens if you call setState() on a State object for a widget that no longer appears in the widget tree (e.g., whose parent widget no longer includes the widget in its build). This error can occur when code calls setState() from a timer or an animation callback.
The preferred solution is to cancel the timer or stop listening to the animation in the dispose() callback. Another solution is to check the "mounted" property of this object before calling setState() to ensure the object is still in the tree.
This error might indicate a memory leak if setState() is being called because another object is retaining a reference to this State object after it has been removed from the tree. To avoid memory leaks, consider breaking the reference to this object during dispose().)
```

參考[StackOverFlow](https://stackoverflow.com/questions/49340116/setstate-called-after-dispose)

將SetState之前加入條件mounted

```dart
if(document != null && this.mounted)
    {
      setState(() {
        var ns = document.getElementsByClassName('nav menu').first.getElementsByTagName('a').map((d)=>{d.text:d.attributes['href']});
        novelSort = List.from(ns);
      });
    }
```

在測試就沒再發生問題了



### Drawer

再來把剛剛爬到的分類表放到Drawer裡面並連結到對應頁面



寫完呈現如下

![](https://i.imgur.com/HyualOu.png)

還會提醒我 over flow 真貼心

看能不能做成可捲動的Drawer



[StackOverFlow](https://stackoverflow.com/questions/57869679/implement-a-scrollable-drawer-with-flutter-it-shows-a-bottom-overflow-when-i-ro)真不錯隨便查都有解

> Use `SingleChildScrollView` like this
>
> ```dart
> drawer: Drawer(
> child: SingleChildScrollView(
>         scrollDirection: Axis.vertical,
>         child: Column(
>         children: <Widget>[
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>                   YourWidgetsHere(),
>         ]),),)
> ```
>
> Or
>
> ```dart
> Drawer(
>         child: ListView(
>           children: <Widget>[           
> ListTile(
>               dense: true,
>               title: Text("Example"),
>               leading: new Image.asset(
>                 "assets/images/example.png",
>                 width: 20.0,
>               ),
>             ),
> ]),), 
> ```

改一改就OK了

![](https://i.imgur.com/GuQbFi2.png)