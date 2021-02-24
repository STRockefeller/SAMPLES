# Novel Crawler

## 20210223

### 題目選擇

目前學習Flutter約一周，許多觀念都還似懂非懂，是時候做個不大不小的專案來鍛鍊一下了

選擇'Novel Crawler的原因主要是因為之前在C#有做過類似的專案，再者我認為爬蟲技巧很實用

### 期望有所精進的技能

* 爬蟲和其他網路相關技能
* 資料庫技能
* 基本的APP排版能力

### 專案屬性

因為沒打算發布所以Package name就沒設定了



### Debug 失敗

出師不利，新專案打開就沒辦法DEBUG

不論是Android studio 或是 Visual Studio Code 都跳出如下警報

![](https://i.imgur.com/UkvFluQ.png)

* 試著開回舊專案，則能夠正常DEBUG，AVD應該是正常的

* flutter doctor也沒能找到問題，電腦環境OK

* 改以Chrome 進行DEBUG也正常，程式碼本身也沒問題



[github](https://github.com/flutter/flutter/issues/60465)上有找到這個issue，但似乎每個人的情況都不太一樣

再一次以Command Line建立乾淨的新專案，然後跑flutter run

![](https://i.imgur.com/8VxV9ZS.png)

依然無用



試著更新flutter

![](https://i.imgur.com/FFw3g5A.png)

flutter run -v

結果跑了一長串還是失敗。

好吧我放棄了，先用Chrome來跑吧



### 規劃APP內容

#### App Bar

共用，就先放個上一頁，還有Title就好

#### Home Page

預計放小說網站的選擇，不過應該暫時'只會有一個

#### 選擇網站後

執行爬蟲，目標是放在這個頁面的熱門書籍連結，以及分類清單(放在Drawer裡面)

選擇分類後爬書名，選擇書名後爬章節目錄，選擇章節後爬內文



### 規劃Class

Class Diagram就不畫了，這邊簡單分類就好，細節邊寫邊想

* abstract class Website
  * String homePageUrl
  * List<String> getSort()
  * List<String> getNovelList()
  * List<String> getChapterList()
  * List<Article> getNovelContent()
* class Novel
  * String novelName
  * String author
  * List<Article> articles
* class Article
  * String chapterTitle
  * String chapterContent

## 20210224

### AVD DEBUG 問題排除

意外的搞定了[Debug 失敗](Debug 失敗)的問題，趕緊來記錄一下作法



![](https://i.imgur.com/ypceeSa.png)

首先，我在SDK MANAGER 發現以上螢光筆標示的三個項目可以更新(標示為`-`)，將其更新後打開AVD執行`flutter run`



![](https://i.imgur.com/5bSm6xH.png)



這次執行會非常久，先是卡在`Launching lib\main.dart on sdk gphone x86 arm in debug mode...`好一段時間，接著開始安裝一些東西，接著和往常一樣等待`Running Gradle task 'assembleDebug'...`結束後畫面就出來啦。



### 共用控制項整理

建立一個新的Class專門放共用控制項，一方面是減少重複撰寫，另一方面是我覺得flutter原本的控制項參數太多，看著就累。

```dart
import 'package:flutter/material.dart';

class MyControls
{
  static Text myText(String data,Color color,double size) => new Text(data,style: new TextStyle(color: color,fontSize: size));
  static Text commonText(String data) => myText(data, Colors.black, 15);
  static Text novelTitleText(String data) => myText(data, Colors.black, 15);
  static Text novelContentText(String data) => myText(data, Colors.black, 15);

  static List<Widget> downloadAction(Function function) => <Widget>[IconButton(icon: Icon(Icons.download_outlined),onPressed: function,)];

  static AppBar commonAppBar(String barTitle,Function downloadFunction) =>
   new AppBar(title: Text(barTitle.isNotEmpty?barTitle:"Rockefeller's Novel Crawler"),actions: downloadAction(downloadFunction));
}
```

先這樣，之後應該還會加入Drawer和其他有的沒的，反正到時候再補充



### 在 Home Page 生成 ElevatedButton 用於選擇網站

建新檔案

```dart
import 'package:flutter/material.dart';
import 'package:novel_crawler_by_rockefeller/MyRoute.dart';

class Site
{
  Site(String siteName,String url)
  {
    this.siteName=siteName;
    this.url=url;
  }
  String siteName;
  String url;
  void _link()=>MyRoute.routeUrl(url);
  ElevatedButton generateLinkButton() => new ElevatedButton(onPressed: _link, child: new Text(siteName));
}
class SiteList
{
  static List<Site> list = <Site>[];;
  static void addSite(String siteName,String url){list.add(new Site(siteName,url));}
  static Column generateWidget()
  {
    List<ElevatedButton> linkList;
    list.forEach((site) {linkList.add(site.generateLinkButton());});
    return new Column(
      mainAxisAlignment: MainAxisAlignment.spaceEvenly,
      children: linkList,
    );
  }
}
```

主要用來

1. 紀錄網站(主頁)列表，之後可能會透過資料庫儲存
2. 生成HomePage的網站列表



#### the method '' was called on null

這裡遇到一些狀況，疑似我的靜態屬性到scaffold裡面就會變成null

比如說

```dart
class _MyHomePageState extends State<MyHomePage> {
  @override
  Widget build(BuildContext context) {
    SiteList.addSite("CZBooks", "czbooks.net");
    return Scaffold(
      appBar: MyControls.commonAppBar("",()=>{}),
      body: Center(
      )
    );
  }
}
```

執行結果如下

![](https://i.imgur.com/koaVZfc.png)

這邊的`add`應該是指

```dart
static void addSite(String siteName,String url){list.add(new Site(siteName,url));}
```

後面的`list.add`

如果把`SiteList.addSite("CZBooks", "czbooks.net");`移動到`main()`則沒有跳錯

[Stack OverFlow](https://stackoverflow.com/questions/52046870/flutter-the-method-was-called-on-null)有找到這個狀況，似乎和初始化有關，但我這邊的List<Site>是有宣告初值的

以DartPad測試類似情況沒有問題

![](https://i.imgur.com/lCtAoIZ.png)

![](https://i.imgur.com/fBYVi6k.png)



是不是在`build`裡面執行外部方法就會變這樣? 山不轉路轉!

```dart
class _MyHomePageState extends State<MyHomePage> {
  Column _buttonGenerate()
  {
    SiteList.addSite("CZBook", "czbook.net");
    return SiteList.generateWidget();
  }
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: MyControls.commonAppBar("",()=>{}),
      body: _buttonGenerate(),
    );
  }
}
```

結果依然行不通。



##### 問題排除

多試幾次就找出癥結點了

同樣用class Test嘗試

```dart
class Test
{
  static List<String> list = [];
  static void test(){list.add("value");}
}
```

我發現如果我在`_MyHomePageState`再次賦予`list`實例則可以正常使用該屬性和相關方法。

依同理，我在`_MyHomePageState`再次賦予`SiteList.list`實例後，始可正常使用。



### Home Page 排版



```dart
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: MyControls.commonAppBar("",()=>{}),
      body:new Container(margin: EdgeInsets.all(50),padding: EdgeInsets.symmetric(horizontal: 80),color: Colors.orangeAccent,child: _buttonGenerate(),)
    );
  }
```

![](https://i.imgur.com/W74MFK2.png)

其實我原本覺得不用padding就能達到這個效果了，結果只下margin all(50) 的情況下還OK，加入Column後連Container都一起縮到畫面的最左邊了，目前先加入padding撐場面，不過我想如果其他螢幕尺寸的話，排版會變得很怪，待修正。



### CZBooksHomePage

先做好預設版面以及連結方式，這邊要提一下，頁面切換寫好之後似乎不能靠VSC的hot reload刷新到AVD中，**必須停止Debug再重新run一次才行**，一開始還以為是寫錯，結果都找不到問題。

順便改一下結構，如下

```
│  main.dart
│  MyControls.dart
│  MyRoute.dart
│  SiteList.dart
│
└─View
        CZBooksHomePage.dart
        MyHomePage.dart
```

接著就到了重點的爬蟲部分了

在[StackOverFlow](https://stackoverflow.com/questions/51865729/flutter-how-to-parse-data-from-a-website-to-a-list-view)有討論到一些方法，這次決定先來試試[web_scraper 0.0.8](https://pub.dev/packages/web_scraper)這個package

package安裝完成馬上來試試，

![](https://i.imgur.com/3kSiucP.png)

好吧，果然沒那麼簡單。

推測可能是非同步的關係，今天先休息，明天再研究。

