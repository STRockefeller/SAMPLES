# Dio

參考

https://learnku.com/articles/31768

https://pub.dev/packages/dio

## Abstract

在Flutter 開發中如果我們想要使用網絡請求，除了系統提供了`HttpClient`一般會使用第三方library，比如`http` 或`Dio`。

`Dio` 是一個強大的Dart Http 請求庫，支持攔截器，全局配置，FormData，請求取消，文件下載，超時等。



## Getting Start

添加依賴
在`pubspec.yaml`中添加依賴。

```yaml
dependencies:
 dio: 4.0.0
```


官網給出了一個簡單的例子：

```dart
import 'package:dio/dio.dart';
void getHttp() async {
  try {
    Response response = await Dio().get("http://liuwangshu.com");
    print(response);
  } catch (e) {
    print(e);
  }
}
```
如果想要發送一個GET 請求:

```dart
Response response;
response=await dio.get("/test?id=3&name=liuwangshu")
print(response.data.toString());
// 下方程式碼與上方功能相同
response=await dio.get("/test",data:{"id":3,"name":"liuwangshu"})
print(response.data.toString());
```
發送一個POST 請求:

```dart
response=await dio.post("/test",data:{"id":3,"name":"liuwangshu"})
```
發送一個FormData：

```dart
FormData formData = new FormData.from({
 "name": "liuwangshu",
 "age": 18,
 });
response = await dio.post("/info", data: formData);
```
還有很多示例在github上，地址為：https://github.com/flutterchina/dio。本文的目的不是重複介紹這些示例，而是寫一個網絡訪問的例子帶大家快速入門。

2.Dio 訪問網絡的例子
```dart
import 'dart:convert';
import 'dart:io';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: MyHomePage(),
    );
  }
}
class MyHomePage extends StatefulWidget {
  MyHomePage({Key key}) : super(key: key);
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  var _ipAddress = 'Unknown';

  _getIPAddress() async {//1
    var url = 'https://httpbin.org/ip';
    Dio _dio = Dio();
    String result;
    try {
      var response = await _dio.get(url);//2
      if (response.statusCode == HttpStatus.ok) {
        var data= jsonDecode(response.toString());//3
        result = data['origin'];
      } else {
        result =
        'Error getting IP status ${response.statusCode}';
      }
    } catch (exception) {
      result =exception.toString();
    }
    if (!mounted) return;
    setState(() {
      _ipAddress = result;//4
    });
  }

  @override
  Widget build(BuildContext context) {
    var spacer = SizedBox(height: 10.0);
    return Scaffold(
      body: Padding(
        padding: EdgeInsets.all(100.0),
        child: Column(
          children: <Widget>[
            Text('IP地址为：'),
            spacer,
            Text('$_ipAddress'),
            spacer,
           RaisedButton(
              onPressed: _getIPAddress,
              child: Text('请求网络'),
            ),
          ],
        ),
      ),
    );
  }
}
```
async和await是Dart語言用來支持異步編程的關鍵字，註釋1處的async關鍵字使得_getIPAddress方法變為了異步方法，註釋2處的await關鍵字只能在異步方法中使用，它使得註釋2處後面的代碼等到get請求返回後再執行。
如果網絡請求返回的狀態碼為200，就在註釋3處進行Json解析，將結果賦值給result變量。
當我們點擊界面上按鈕時，會調用_getIPAddress方法，在註釋4處將請求的結果賦值給_ipAddress並顯示在Text中。
網絡請求成功後，效果如下圖。


3.JSON 數據解析
常用的JSON 數據解析方式主要有三種，這里分別介紹下。

3.1 使用json.decode () 方法
第2小節的例子用到的就是json.decode ()方法，需要引入dart:convert庫。json.decode ()方法會將String類型數據解析成Map數據結構：Map<String, dynamic>,取數據的格式為object [key]。
上面例子中返回的JOSN數據為：

```json
{
 "origin": "111.193.7.70, 111.193.7.70"
}
```
對於不復雜的JOSN 數據使用json.decode () 方法是一個不錯的選擇，但實際項目中的JOSN 數據會復雜一些，如果每次取數據都用object [key]，那麼很容易出錯。

3.2 手動編寫實體類
用一個model來保存數據，使得數據序列化。
我們可以新建一個ip.dart：
```dart
class Ip {
  String origin;

  Ip(this.origin);

  Ip.fromJson(Map<String, dynamic> json) : origin = json['origin'];

  Map<String, dynamic> toJson() => {
        "origin": origin,
      };
}
```
然後修改main.dart:
```dart
import 'dart:convert';
import 'dart:io';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'model/ip.dart';
...
class _MyHomePageState extends State<MyHomePage> {
  var _ipAddress = 'Unknown';
  _getIPAddress() async {
    var url = 'https://httpbin.org/ip';
    Dio _dio = Dio();
    String result;
    try {
      var response = await _dio.get(url);
      if (response.statusCode == HttpStatus.ok) {
        var data= jsonDecode(response.toString());
        var ip=Ip.fromJson(data);//1
        result = ip.origin;
      } 
    ...
  }
...
}
```
註釋1 處通過Ip.fromJson 就可以獲取ip 的實體，這樣就可以完成賦值等操作了。

3.3 自動生成實體類
一般項目中Json數據會比較繁多，每次重複寫實體類的模版代碼顯然枯燥無意義，可以使用一些工具來生成實體類。
比如使用網頁自動生成：  https://app.quicktype.io/  。轉換的實體類如下所示。
```dart
import 'dart:convert';

Ip ipFromJson(String str) => Ip.fromJson(json.decode(str));

String ipToJson(Ip data) => json.encode(data.toJson());

class Ip {
    String origin;

    Ip({
        this.origin,
    });
    
    factory Ip.fromJson(Map<String, dynamic> json) => new Ip(
        origin: json["origin"],
    );
    
    Map<String, dynamic> toJson() => {
        "origin": origin,
    };
}
```
除了使用網頁生成，還可以使用開源的JSONFormat4Flutter：
https://github.com/debuggerx01/JSONFormat4...

使用json_serializable
除了以上提到的工具，還可以使用json_serializable。
json_serializable是一個自動化的源代碼生成器，可以方便的生成JSON實體類。
首先在pubspec.yaml加入如下依賴，

```yaml
dependencies:
 json_annotation: ^2.4.0

dev_dependencies:
 build_runner: ^1.0.0
 json_serializable: ^3.0.0
```
在AS的Terminal中運行flutter packages get命令，用來在項目中使用這些新的依賴項。
接著實現ip實體類。
```dart
import 'package:json_annotation/json_annotation.dart';

// ip.g.dart 将在我们运行生成命令后自动生成
part 'ip.g.dart';

//告诉生成器，这个类需要生成Model类
@JsonSerializable()
class Ip{
 Ip(this.origin);

 String origin;

 factory Ip.fromJson(Map<String, dynamic> json) => _$IpFromJson(json);
 Map<String, dynamic> toJson() => _$IpToJson(this);
}
```
關鍵的在於@JsonSerializable ()，它用於告訴生成器，Ip 類是需要生成的Model 類。在Terminal 窗口運行如下命令生成ip.g.dart 文件。


flutter packages pub run build_runner build
如何使用見3.2 節就可以了，使用json_serializable 可以讓我們忽略Ip 類中的任何手動JSON 序列化。源代碼生成器會在同一個目錄下生成Ip.g.dart，如下所示。

// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'ip.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

Ip _$IpFromJson(Map<String, dynamic> json) {
  return Ip(json['origin'] as String);
}

Map<String, dynamic> _$IpToJson(Ip instance) =>
    <String, dynamic>{'origin': instance.origin};
