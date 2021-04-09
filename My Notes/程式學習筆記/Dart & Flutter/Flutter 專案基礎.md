# Flutter 專案基礎

## 建立專案以及執行

### 新專案

#### Android Studio

Create a new flutter project --> Flutter application -->然後一直NEXT就可以了

#### CMD

開啟命令提示字元，透過 Flutter SDK 指令 `flutter create [Project Name]` 來建立專案，它會在當前目錄下依專案名稱建立同名資料夾，並將鄉專案執行所需檔案都一並建立。

### Emulator

#### Android Studio

從AVD Manager開啟。

#### VSC

目前還沒有設定成功過，**待補充**。

可以透過AVD Manager打開Emulator後執行程式。



##### 2021/04/06 補充VSC設定方式

1. 先安裝plugin `Android iOS Emulator

   ![](https://i.imgur.com/Maw5Udk.png)

2. 到Settings設定SDK路徑(可以從SDK Mannager查)

   ![](https://i.imgur.com/b76V4Ul.png)

   ![](https://i.imgur.com/ol4ugGi.png)

3. 設定完成後於VSC下`ctrl`+`shift`+`p`搜尋emulator執行`Flutter: Launch Emulator`即可

### 執行

#### Android Studio

選擇SDK Device (也可以選chrome) 然後main.dart 直接執行

#### VSC

Start Debugging 的 Combo box 下拉 選擇 Dart & Flutter 後方選擇Flutter(還有一個Flutter test不知道能不能用) 執行

#### running gradle task 'assembledebug' 

初次開啟不論是android studio或是VSC在執行時都卡在這裡。

然後原因是，**等地不夠久**，VSC等約莫二十分鐘後就成功跑出畫面來了，並且執行第二次後就快很多了

接著再拿android studio跑一次，也順利顯示出畫面了

## Flutter 檔案結構

**android**：與Android相關的程式碼以及設定，例如要開啟讀取或寫入權限就要到裡面的AndroidManifest填寫。

**ios**：同樣的ios資料夾就是放和iOS相關的程式與以及設定。

**build**：不會使用到的資料夾，裡面存放系統產生的檔案，

**lib**：放Flutter程式碼的資料夾，之後寫的程式碼檔案都會放在這，目前裡面只有剛剛執行的`main.dart`。

**test**：放測試用程式碼，目前裡面只有一個檔案，用來測試程式是否有按照預期方式執行。

------

其他檔案的用途分別是：
**.gitignore**：若你有使用版本控制軟體但有些不重要的檔案或是機密檔案不能傳上去，就在這裡註明忽略他們。

**.metadata**：IDE用來追蹤flutter project用

**.package**：管理第三方或是IDE package的檔案，會自動產生與更新內容

**.pubspec.lock**：保存目前pubspec.yaml的資訊，確保其他人使用時能夠下載到同樣版本的packages

**.pubspec.yaml**：我們**唯一**會去修改的檔案，例如使用第三方package，或要引入媒體檔案等等

## 關於main.dart

以新專案的main.dart做說明，我把註解拿掉如下。

```dart
import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);
  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;

  void _incrementCounter() {
    setState(() {
      _counter++;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(widget.title),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            Text(
              'You have pushed the button this many times:',
            ),
            Text(
              '$_counter',
              style: Theme.of(context).textTheme.headline4,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _incrementCounter,
        tooltip: 'Increment',
        child: Icon(Icons.add),
      ), 
    );
  }
}

```



### 程式進入點

```dart
void main() {
  runApp(MyApp());
}
```

從`void main()`開始，然後執行`runApp()`

> runApp()會將給定的widget填滿整個螢幕，這裡傳給它名為MyApp的widget物件。 

### MyApp

```dart
class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        primarySwatch: Colors.blue,
        visualDensity: VisualDensity.adaptivePlatformDensity,
      ),
      home: MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}
```

這個class就是進入App後第一時間顯示的內容

首先他繼承自`StatelessWidget`表示這個widget在**被建立之後就固定住，不會再更改**。

當widget被建立時會呼叫**build**這個function，回傳的就是要顯示的內容。

**title**：這個標題在App是看不到的，而是會在手機管理App程式的地方。

**theme**：介面的顏色風格

**home**：App的首頁，接受的值為widget物件，這裡傳入MyHomePage這個widget。

### MyHomePage

在`MyApp`的`Widget build(BuildContext context)`方法回傳的`MaterialApp`中`home`的參數類型

```dart
class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);
  final String title;
  @override
  _MyHomePageState createState() => _MyHomePageState();
}
```

可以看到的是，`MyHomePage`繼承的是`StatefulWidget`，相對於`StatelessWidget`表示其可能隨著時間而有所變化。

```dart
_MyHomePageState createState() => _MyHomePageState();
```

這行我左看右看都覺得它少了個`new`，沒有Google到相關寫法，透過Android Studio看到`(new) _MyHomePageState()`

好吧，它真的把new省略了



### _MyHomePageState

```dart
class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;

  void _incrementCounter() {
    setState(() {
      _counter++;
    });
  }
```

註:dart中沒有`public` `internal` `protected` `private` 等修飾詞。class 或 property 名稱的前方加上`_`代表`private`，其餘表`public`



