# Basic widgets

[Reference:flutter.dev](https://flutter.dev/docs/development/ui/widgets/basics)

[Reference:ITHelp](https://ithelp.ithome.com.tw/articles/10215654)

## AppBar

![](https://i.imgur.com/8JZmsjA.png)

上方藍色的部分通稱`AppBar`

![](https://flutter.github.io/assets-for-api-docs/assets/material/app_bar.png)

> Appbar就是介面上方藍色的區域
> 可以把它細分成4個部分，其實還有一個部分是彈性的空間。
>
> - leading：通常就是放Logo的地方，你可以選擇單純的Icon或是IconButton
> - title：痾..就是個標題
> - actions：可以放常用的IconButton，例如搜尋、購物車按鈕之類的
> - bottom：可以用來放TabBar，也就是點選會轉換畫面的按鈕，範例中的笑臉和哭臉。

以下節錄[ITHelp](https://ithelp.ithome.com.tw/articles/10215654)中的程式片段

```dart
        appBar: AppBar(
          title: Text("AppBar Title"),
          leading: Icon(FontAwesomeIcons.dragon),
          actions: <Widget>[
            IconButton(
              icon: Icon(FontAwesomeIcons.search),
              onPressed: null,
            )
          ],
          bottom: TabBar(
            tabs: tabList.map((choice) {
              return Tab(
                text: choice.title,
                icon: Icon(choice.icon),
              );
            }).toList(),
          ),
        ),
```

> ※這個例子中我有使用到font_awesome這個套件提供的icon，在使用前麻煩到**pubspec.yaml**的dependencies處加上`font_awesome_flutter: any`，並在Terminal輸入或是上方點選**Packages get**指令來下載套件。

先`import`

```dart
import 'package:font_awesome_flutter/font_awesome_flutter.dart';
```

然後`pubspec.yaml`修改如下

```yaml
dependencies:
  flutter:
    sdk: flutter
  font_awesome_flutter: any
```

註:

1. 注意縮排，第一次寫成

   ```yaml
   dependencies:
     flutter:
       sdk: flutter
     	font_awesome_flutter: any
   ```

   安裝失敗

2. 2021/02/18 現版本的Android Studio上方的按鈕不叫**Packages get**而是**Pub Get**

3. 不過即使安裝完成圖片也都讀不出來(可能死圖了吧)

### actions

以下官方文件內容

> # `{List<Widget> actions}`
>
> {@template flutter.material.appbar.actions} A list of Widgets to display in a row after the `*title*` widget.
>
> Typically these widgets are `*IconButton*`s representing common operations. For less common operations, consider using a `*PopupMenuButton*` as the last action.
>
> The `*actions*` become the trailing component of the `*NavigationToolbar*` built by this widget. The height of each action is constrained to be no bigger than the `*toolbarHeight*`. {@endtemplate}

簡單歸納

* 型別是`List<Widget>`

  * 所以如果要將

    ```dart
            actions: <Widget>[IconButton(icon: Icon(Icons.face_sharp))],
    ```

    變成兩張臉，要寫成

    ```dart
            actions: <Widget>[IconButton(icon: Icon(Icons.face_sharp)),
                             IconButton(icon: Icon(Icons.face_sharp))],
    ```

    而不是

    ```dart
            actions: <Widget>[IconButton(icon: Icon(Icons.face_sharp),
                             icon: Icon(Icons.face_sharp))],
    ```

    好吧，這是我剛才幹的蠢事

* 內容物的`Widget`可以基本看做`IconButton`，因為絕大多數的情況只會這樣用。

* 把泛型的`<Widget>`拿掉似乎不影響呈現結果

  ```dart
          actions: [IconButton(icon: Icon(Icons.face_sharp)),
                    IconButton(icon: Icon(Icons.face_sharp))],
  ```

  

## Column and Row

![](https://i.imgur.com/oHHvwQH.png)

> 簡單的用兩個例子來說明
> Column用作垂直布局，例子中使用7個Text widget，分別放ptt各版標題。
> Row用作水平布局，例子中是模擬ListView中的[ListTile Widget](https://api.flutter.dev/flutter/material/ListTile-class.html)
> ![img](https://i.imgur.com/dgj1C9g.png) ![img](https://i.imgur.com/Kr8D0PZ.png)
>
> Column和Row比較常用的屬性是`mainAxisAlignment`用來設定內容物在主軸的對齊方式。
> 有以下幾種可以選擇，效果我想看圖應該很明顯就不再介紹了。
> ![img](https://i.imgur.com/DN9uGQm.png)

以下節錄[ITHelp](https://ithelp.ithome.com.tw/articles/10215654)中的程式片段

```dart
      body: Column(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: <Widget>[
          Text(
            'Gossiping  綜合 ◎[八卦]',
            style: TextStyle(fontSize: 20.0, color: Colors.purple),
          ),
          Text(
            'C_Chat  閒談 ◎[希洽] 票選出爐感謝各位積極參與',
            style: TextStyle(fontSize: 20.0, color: Colors.indigo),
          ),
          Text(
            'Baseball  棒球 ◎[棒球] 中職30 Baseball is Life',
            style: TextStyle(fontSize: 20.0, color: Colors.red),
          ),
          Text(
            'LoL  遊戲 ◎[LoL] 恭喜JT獲得2019LMS夏季冠軍',
            style: TextStyle(fontSize: 20.0, color: Colors.amber),
          ),
          Text(
            'Stock  學術 ◎新增板主noldorelf',
            style: TextStyle(fontSize: 20.0, color: Colors.cyan),
          ),
          Text(
            'Lifeismoney  省錢 ◎[省錢]',
            style: TextStyle(fontSize: 20.0, color: Colors.pinkAccent),
          ),
          Text(
            'NBA  籃球 ◎[NBA] 休賽季',
            style: TextStyle(fontSize: 20.0, color: Colors.brown),
          ),
        ],
      ),
```

```dart
      body: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: <Widget>[
          Icon(FontAwesomeIcons.google, color: Colors.red,),
          Expanded(
            child: Text(
                'Google有限公司是源自美國的跨國科技公司，為Alphabet Inc.的子公司，業務範圍涵蓋網際網路廣告、網際網路搜尋、雲端運算等領域，開發並提供大量基於網際網路的產品與服務，其主要利潤來自於AdWords等廣告服務。'),
          ),
          IconButton(
            icon: Icon(Icons.arrow_forward_ios),
            onPressed: (){},
          )
        ],
      ),
```

## Container

> Container是一個容器，你可以設定它的長寬、背景顏色、邊界距離，甚至還可以讓它變形。

> 下方這個範例的介面是由外層青色Container包一個橘色Container，而橘色Container再包一個Text widget所組成。
> 並且在青色Container我設定了上下左右各50的padding(這樣才看的到青色部分)。
> ![https://ithelp.ithome.com.tw/upload/images/20190916/201195500O4w1KcMen.png](https://ithelp.ithome.com.tw/upload/images/20190916/201195500O4w1KcMen.png)
> 接著來看變形的效果是如何，我讓青色Container的Z軸偏移0.1，橘色Container則是偏移-0.2，這樣看起來是不是很有設計感呀。
> ![img](https://i.imgur.com/5eBr5OB.png)

以下節錄[ITHelp](https://ithelp.ithome.com.tw/articles/10215654)中的程式片段

```dart
        body: Container(
          transform: Matrix4.rotationZ(0.1),
          color: Colors.tealAccent,
          padding: EdgeInsets.all(50.0),
          child: Container(
            transform: Matrix4.rotationZ(-0.2),
            constraints: BoxConstraints.expand(),
            color: Colors.deepOrange,
            child: Center(child: Text('Flutter Container Child', style: TextStyle(fontSize: 60.0),)),
          ),
```

---

這邊試了一下，

```dart
	body: Container(
        color: Colors.amber,
        padding: EdgeInsets.all(50),
        child: Container(
          color: Colors.blue,
          child: Center(child: Text('Hello'),),
        ),
      ),
```

比較特別的是如果我直接寫

```dart
child: Container(
          color: Colors.blue,
          child: Text('Hello')
        ),
```

畫面會擠在左上角，兩個Container和文字都是。

而去掉`child: Text('Hello')`則兩個Container都會在中間



### padding

[Reference:FlutterApi:EdgeInsets](https://api.flutter.dev/flutter/painting/EdgeInsets-class.html)

[Reference:FlutterApi:EdgeInsetsGeometry](https://api.flutter.dev/flutter/painting/EdgeInsetsGeometry-class.html)

簡單的說明一下，關於padding

1. `padding` 要求的參數型別是`EdgeInsetsGeometry `
2. `EdgeInsetsGeometry `是`EdgeInsets`的基底型別(父類別)
3. `EdgeInsets.all(50)`回傳一個四周50單位距離padding的`EdgeInsets`物件

關於`EdgeInsets`物件實例化的方法

1. `EdgeInsets.all(8.0)`全方向padding
2. `EdgeInsets.symmetric(vertical: 8.0)`兩個方向padding，當然也可以透過設定`horizontal`參數值來決定左右的padding
3. `EdgeInsets.only(left: 40.0)`單方向padding



### transform

The transformation matrix to apply before painting the container.

參數要求型別是`Matrix4`

[Reference:FlutterApi:Matrix4](https://api.flutter.dev/flutter/vector_math_64/Matrix4-class.html)

API裡有許多形變方法

目前試了一下Rotation系列的

1. `RotationX `效果看起來像是下方Margin
2. `RotationY` 效果看起來像是右方方Margin
3. `RotationZ `效果看起來像是以左上角為中心順時針方向旋轉

## Icon

> 在前面的範例中已經有使用到icon這個元件，用法也相當的簡單。這裡想要提的是雖然Flutter本身已經內建了許多不錯的icon了([點這裡](https://api.flutter.dev/flutter/material/Icons-class.html)查看所有icon。)
> 你也可以使用font_awesome這個套件提供的icon，它提供更多的選擇和類型，安裝方法可以參考前面Appbar備註的地方。

## Image

從網路載入

```dart
// 載入網路資源，輸入圖片網址App就會自動去下載並載入
Image.network('https://titangene.github.io/images/cover/flutter.jpg')

// 一樣是從網路下載圖片，不過在下載好後會用淡入的動畫呈現
// 其中的kTransparentImage是transparent_image套件提供的透明圖片
// 需要在pubspec.yaml加上transparent_image: ^1.0.0來安裝套件
FadeInImage.memoryNetwork(
                 placeholder: kTransparentImage,
                 image:'https://titangene.github.io/images/cover/flutter.jpg')

// 一樣是淡入的效果，但在載入前會先用本地端圖片呈現(也可使用gif圖檔喔)
FadeInImage.assetNetwork(
                 placeholder: 'assets/waiting.gif',
                 image:'https://titangene.github.io/images/cover/flutter.jpg')
```

從本地載入

引入本地端圖片步驟：
a. 在project內新增「assets」資料夾，或任何你喜歡的名字都可
b. 到pubspec.yaml最下方flutter處添加

```dart
assets:
    -assets/   <-你的資料夾名稱
    //表示要引入assets資料夾內所有媒體資源
```

c. 即可使用assets內資源，例如在assets資料夾內有cute_cat.png檔案，就可以在程式中使用`Image.assets('assets/cute_cat.png')`引入圖片。

## Drawer

以下片段節錄自[Flutter官方範例](https://api.flutter.dev/flutter/material/Scaffold/drawer.html)

```dart
      drawer: Drawer(
        child: Center(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: <Widget>[
              const Text('This is the Drawer'),
              ElevatedButton(
                onPressed: _closeDrawer,
                child: const Text('Close Drawer'),
              ),
            ],
          ),
        ),
      ),
      // Disable opening the drawer with a swipe gesture.
      drawerEnableOpenDragGesture: false,
```

`drawerEnableOpenDragGesture`屬性表示是否可透過滑動拉出Drawer，true代表可以。



### ElevatedButton

[Reference:FlutterApi:ElevatedButton](https://api.flutter.dev/flutter/material/ElevatedButton-class.html)

在API中沒有找到名為Button的class

關於 `onPressed`

1. `void Function() onPressed`代表他只能傳入一個方法簽章為`void fn(void)`的方法，也就是說沒辦法在這裡傳入方法參數。
2. 參數寫方法名稱就好不用`()`，有點像`delegate`的作法。
3. 沒定義`onPressed`的話，不會跳錯(但有提示)，執行後button會無法點擊



### Open / Close Drawer

其實不用特別寫方法就可以透過滑動螢幕左方來開關了(前提是沒有把`drawerEnableOpenDragGesture`設為`false`)



官方範例中，有另外寫出開關的方法

```dart
  void _openDrawer() {
    _scaffoldKey.currentState.openDrawer();
  }

  void _closeDrawer() {
    Navigator.of(context).pop();
  }
```



## BottomNavigationBar

以下節錄[ITHelp](https://ithelp.ithome.com.tw/articles/10215654)中的程式片段

```Dart
class _MyHomePageState extends State<MyHomePage> {
  int _selectedIndex = 0;
  static const List<Widget> _widgetOption = <Widget>[
    Icon(Icons.star, size: 200.0,),
    Icon(Icons.mood_bad, size: 200.0,),
    Icon(Icons.wb_sunny, size: 200.0,),
  ];
  void _onItemTap(int index) {
    setState(() => _selectedIndex = index);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text("Scaffold example"),
          leading: Icon(
            Icons.face
          ),
        ),
        bottomNavigationBar: BottomNavigationBar(items: [
          BottomNavigationBarItem(
              icon: Icon(Icons.star)),
          BottomNavigationBarItem(
              icon: Icon(Icons.mood_bad)),
          BottomNavigationBarItem(
              icon: Icon(Icons.wb_sunny)),
        ],
        onTap: _onItemTap,
        currentIndex: _selectedIndex,),
        body: Center(child: _widgetOption.elementAt(_selectedIndex),));
  }
}
```

![](https://i.imgur.com/HzpWZ5M.gif)



## SnackBar

以下節錄[ITHelp](https://ithelp.ithome.com.tw/articles/10215654)中的程式片段

```dart
class _MyHomePageState extends State<MyHomePage> {
  SnackBar _snackBar1 = SnackBar(content: Text("You Click First Button"));
  SnackBar _snackBar2 = SnackBar(content: Text("You Click Second Button"));
  SnackBar _snackBar3 = SnackBar(content: Text("You Click Third Button"));

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text("Scaffold example"),
          leading: Icon(Icons.face),
        ),
        body: Builder(
          builder: (BuildContext context) {
            return Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: <Widget>[
                  ButtonTheme(
                    minWidth: 200.0,
                    height: 100,
                    child: RaisedButton(
                      onPressed: () {
                        Scaffold.of(context).removeCurrentSnackBar();
                        Scaffold.of(context).showSnackBar(_snackBar1);
                      },
                      child: Text('First Button'),
                    ),
                  ),
                  ButtonTheme(
                    buttonColor: Colors.deepOrange,
                    minWidth: 200.0,
                    height: 100,
                    child: RaisedButton(
                      onPressed: () {
                        Scaffold.of(context).removeCurrentSnackBar();
                        Scaffold.of(context).showSnackBar(_snackBar2);
                      },
                      child: Text('Second Button'),
                    ),
                  ),
                  ButtonTheme(
                    buttonColor: Colors.deepPurple,
                    minWidth: 200.0,
                    height: 100,
                    child: RaisedButton(
                      onPressed: () {
                        Scaffold.of(context).removeCurrentSnackBar();
                        Scaffold.of(context).showSnackBar(_snackBar3);
                      },
                      child: Text('Third Button'),
                    ),
                  ),
                ],
              ),
            );
          },
        ));
  }
}
```

![](https://i.imgur.com/x1u2aLN.gif)

