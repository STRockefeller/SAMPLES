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

```dart
import 'package:http/http.dart' as http;
import 'package:html/parser.dart' as parser;
import 'package:html/dom.dart' as dom;
```

2.get the data from the page you want

```dart
Future<List<String>> getData() async { 
  http.Response response = await http.get('website');
}
```

3.extract the data from the website

```dart
dom.Document document = parser.parse(response.body);
```

\4. depend on your need let's say you want to get all element with the article tag

```dart
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



## 20210324

### HandshakeException

ADV打開放著一段時間就跳例外了

![](https://i.imgur.com/Bn7Dt2o.png)

[github](https://github.com/flutter/flutter/issues/23045)上有相關討論，簡單的說就是跑`flutter doctor --android-licenses`

跑完如下，我忘記先跑一次`flutter doctor`看有沒有issue

![](https://i.imgur.com/hXx0BX5.png)

### Drawer

稍微修改一下Drawer，改成只有下半部為scrollable

```dart
new Drawer(
        child: Center(
          child: new Column(children: [
            myText('Novel Sort',Colors.orangeAccent,40),
            new Container(
              padding: EdgeInsets.all(20),
              margin: EdgeInsets.only(bottom: 30),
              child: new  SingleChildScrollView
              (
                scrollDirection: Axis.vertical,
                child:Column(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: elevatedButtons,
                ), 
              )
            ),
            
          ],)
        ),
      );
```

發現改完之後又再次出現over flow的情況

![](https://i.imgur.com/E5zwIAt.png)

嘗試看看其他Widget



改用ListBody，效果如下，但依然有超出範圍的情況

![](https://i.imgur.com/Pnv6rnW.png)



在[StackOverFlow](https://stackoverflow.com/questions/56318969/bottom-overflowed-using-singlechildscrollview)看到似乎原因出在我把Scrollable Widget放在Column底下，

將Column改為Stack試試

![](https://i.imgur.com/e48ecaJ.png)

現在已經沒有超出範圍了，但是我想要固定的文字沒有被顯示出來(其實有只是被蓋掉了)



## 20210326

### invalid depfile

昨天還可以正常開的專案今天執行就跳這個Excetion，

flutter doctor 也沒有問題

![](https://i.imgur.com/hvJfBCb.png)

後來在

1. flutter clean
2. flutter pub get
3. flutter pub upgrade

後發現我的`http.get(url)`都出現了橙色波浪線

查看後發現`http.get()`的方法簽章變為

```dart
Future<Response> get(Uri url, {Map<String, String> headers})
```

去[pubdev](https://pub.dev/packages/http)看到它的changelog中有一條

> - **Breaking** All APIs which previously allowed a `String` or `Uri` to be passed now require a `Uri`.

所以原因就是出在以前能用String型別參數現在已經不能使用了



https://github.com/flutter/flutter/issues/53005

結果還是一樣的問題，最後是把文件中的`import 'dart:html';`拿掉才能夠正常運作

![](https://i.imgur.com/4TbZCd9.png)

![](https://i.imgur.com/LNrmnwG.png)



## 20210329

### Crawler 實作

```dart
class CZBookNovelListCrawler extends INovelListCrawler
{
  @override
  Future<List<Novel>> StartCrawlerAsync(String url) async
  {
    DateTime getDate(dom.Element e)
    {
      List<String> ls = e.text.split("-");
      return new DateTime(int.parse(ls[0]),int.parse(ls[1]),int.parse(ls[2]));
    }
    //var urls = url.split('/');
    //http.Response response = await http.get(Uri.https(urls[2], urls[3]+'/'+urls[4]));
    http.Response response = await http.get(Uri.https(url,''));
    dom.Document doc = parser.parse(response.body);
    List<Novel> novels = new List<Novel>();
    List<dom.Element> novelElements = doc.getElementsByClassName('novel-item-wrapper');
    novelElements.forEach((element) {
      novels.add(new Novel(element.getElementsByClassName('novel-item-title').first.text,
      element.getElementsByClassName('novel-item-author').first.getElementsByTagName('a').first.text,
      element.getElementsByClassName('novel-item-date').map((e)=>getDate(e)).first,
      new ChapterList(element.getElementsByClassName('novel-item-title').first.text,
      element.getElementsByClassName('novel-item-cover-wrapper').first.getElementsByTagName('a').first.attributes['href']))
      );
    });
    return novels;
  }
  
}
```

目前未完成，新的http.get()方法對不熟uri結構的我來說相當不友善。研究中。



## 20210330

首先提一下昨天的Crawler實作，基本上沒有寫錯，但`Uri.https()`的`unencodePath`是以`'/'`開頭的，加上斜線後就能用了



### 非同步以及Statefull Widget 設計

目前這個分類書單的頁面依然採用`StatelessWidget`，不過其中的內容需要等待非同步的Crawler完成爬蟲，因此我將body的部分設計成`StatefulWidget`，大致如下

```dart
class CZBooksBookListPageBody extends StatefulWidget
{
  String url;
  CZBooksBookListPageBody(url){this.url=url;}
  @override
  State<StatefulWidget> createState() => new _CZBooksBookListPageBody(url);
}
class _CZBooksBookListPageBody extends State<CZBooksBookListPageBody>
{
  String url;
  List<Novel> novels;
  CZBookNovelListCrawler crawler;
  _CZBooksBookListPageBody(String url){this.url=url;}
  Future getDataAsync() async { 
    crawler = new CZBookNovelListCrawler();
    setState(() async{
      novels = await crawler.StartCrawlerAsync(url);
      print('set state');
    });
  } 
  ListView getBody()
  {
    sleep(Duration(seconds:1));
    if(novels?.isEmpty ?? true){return new ListView();}
    List<Widget> widgets;
    novels.forEach((novel) {
      widgets.add (
        new Container(
        padding: EdgeInsets.all(5),
        child: new Column(children: [
          MyControls.commonText('Name: '+novel.name),
          MyControls.commonText('Author: '+novel.author),
          MyControls.commonText('Last Update: '+novel.lastUpdateDate.toString()),
          new ElevatedButton(onPressed: ()=>{}, child: MyControls.commonText('Go To Chapter List'))
        ],)
        )
      );
     });
     return new ListView(children: widgets);
  }
  @override
  Widget build(BuildContext context) {
    getDataAsync();
    print('get body');
    return getBody();
  }
}
```

不過目前非同步的動作依然沒有處理好，下圖是我在Debug模式下觀察到的程式動作順序

![](https://i.imgur.com/LCPxk5I.png)

我期望它在set state 後再次 build 然後刷新畫面，然而並沒有。



## 20210406

### 非同步時序問題

繼續處理前次遇到的狀況

這次嘗試加入callback (參考[STACKOVERFLOW](https://stackoverflow.com/questions/54846280/calling-setstate-during-build-without-user-interaction/54847918))

`WidgetsBinding.instance.addPostFrameCallback((_) => setState(() {}));`

結果就成功了(但我還是不清楚原理，等搞懂後再寫一篇筆記說明好了)

![](https://i.imgur.com/wLnrd23.png)

DEBUG顯示的時序部分如下

```
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body fail
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body fail
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body fail
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body fail
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body fail
I/flutter (16540): Instance of 'Response'
I/flutter (16540): return novels
I/flutter (16540): set state
I/flutter (16540): Enter Scrawler Method
I/flutter (16540): get body ?
I/flutter (16540): get body success
I/flutter (16540): Instance of 'Response'
I/flutter (16540): return novels
I/flutter (16540): set state
I/flutter (16540): Instance of 'Response'
I/flutter (16540): return novels
I/flutter (16540): set state
I/flutter (16540): Instance of 'Response'
I/flutter (16540): return novels
I/flutter (16540): set state
I/flutter (16540): Instance of 'Response'
I/flutter (16540): return novels
I/flutter (16540): set state
```

前半部和我預想的滿接近的，就是在另一個TASK完成前不斷獲取資料，後面得到結果後為甚麼還會重複去取得實例就有點怪了，不過目前是可以使用的，這個部分就等之後再做優化。



### 排版整理

主要兩個部分

* 書名爬到的內容有太多空白的部分了，使用`trim()`方法去掉([API參考](https://api.dart.dev/stable/2.12.0/dart-core/String/trim.html))，基本和C#的用法類似但不能選擇要過濾掉的內容
* `Column`內部`children`太擁擠，想將其隔開，由於內部我沒有使用`Container`所以沒有`padding`或`margin`這些屬性可以設定，查了一下別人的作法，發現可以直接透過插入`SizedBox(height: 10)`將其隔開，確實十分方便。

![](https://i.imgur.com/TOPp6P1.png)





## 20200415



### Chapter List Page

一開始本來想以先前桌面程式的做法直接套到APP上，但後來想想覺得不太現實，如果說在這個畫面把整本書幾百上千章的內容全部爬完，那實在是對使用者耐心的一大考驗。(而且下載功能就有點多餘了)，目前的想法就是在這裡顯示章節目錄，點選章節後再連結到對應的內文。雖然有點麻煩但確實是比較妥當的做法。

這邊多衍生了一個問題，就是在內文中的章節跳換該怎麼處理，好比CZBooks的章節URL並沒有規律，所以我勢必得把這個頁面爬取的章節清單暫時儲存起來，在章節跳換的時候才能避免重複爬取。

不過這個部分可以先容後再議，現在先一步步把畫面做出來要緊。

```dart

import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:novel_crawler_by_rockefeller/MyControls.dart';

class ChapterListBody extends StatefulWidget
{
  @override
  State<StatefulWidget> createState() {
    // TODO: implement createState
    throw UnimplementedError();
  }
  
}
class _ChapterListBody extends State<ChapterListBody>
{
  @override
  Widget build(BuildContext context) {
    // TODO: implement build
    throw UnimplementedError();
  }
  
}
class ChapterListPage extends StatelessWidget
{
  String title;
  String url;
  ChapterListBody body = new ChapterListBody();
  ChapterListPage(String title,String url)
  {
    this.title=title;
    this.url = url;
  }
  @override
  Widget build(BuildContext context) {
    return new Scaffold(
      appBar: MyControls.commonAppBar(title, ()=>{}),
      body: body,
      );
  }
}
```

基本的架構就依照前一個頁面，採用`StatelessWidget`但在body的部分採用`StatefulWidget`。