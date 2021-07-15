# 本地文件的讀寫

[Reference:Flutter.dev](https://flutter.dev/docs/cookbook/persistence/reading-writing-files)

主要是想要做應用程式的客製化設定功能，由於內容不多，不想大費周章使用資料庫或firebase，在本地創建文件紀錄似乎是個比較恰當的選擇。



另外附上json serialization的實用套件https://pub.dev/packages/json_serializable

檔案不大的情況下，flutter推薦不使用套件進行json serialization，詳細可以參考[這裡](https://flutter.dev/docs/development/data-and-backend/json)

有些內容不能直接被dart:convert序列化-->參考[這篇](https://stackoverflow.com/questions/16537114/how-to-convert-an-object-containing-datetime-fields-to-json-in-dart)解決

## Find the correct local path



### *Temporary directory*

A temporary directory (cache) that the system can clear at any time. On iOS, this corresponds to the [`NSCachesDirectory`](https://developer.apple.com/documentation/foundation/nssearchpathdirectory/nscachesdirectory). On Android, this is the value that [`getCacheDir()`](https://developer.android.com/reference/android/content/Context#getCacheDir()) returns.

### *Documents directory*

A directory for the app to store files that only it can access. The system clears the directory only when the app is deleted. On iOS, this corresponds to the `NSDocumentDirectory`. On Android, this is the `AppData` directory.



Example

```dart
Future<String> get _localPath async {
  final directory = await getApplicationDocumentsDirectory();

  return directory.path;
}
```



## Create a reference to the file location

記得要引用dart.io

Example

```dart
Future<File> get _localFile async {
  final path = await _localPath;
  return File('$path/counter.txt');
}
```



## Write data to the file



Example

```dart
Future<File> writeCounter(int counter) async {
  final file = await _localFile;

  // Write the file
  return file.writeAsString('$counter');
}
```



##  Read data from the file



Example

```dart
Future<int> readCounter() async {
  try {
    final file = await _localFile;

    // Read the file
    final contents = await file.readAsString();

    return int.parse(contents);
  } catch (e) {
    // If encountering an error, return 0
    return 0;
  }
}
```



## Complete example

```dart
import 'dart:async';
import 'dart:io';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:path_provider/path_provider.dart';

void main() {
  runApp(
    MaterialApp(
      title: 'Reading and Writing Files',
      home: FlutterDemo(storage: CounterStorage()),
    ),
  );
}

class CounterStorage {
  Future<String> get _localPath async {
    final directory = await getApplicationDocumentsDirectory();

    return directory.path;
  }

  Future<File> get _localFile async {
    final path = await _localPath;
    return File('$path/counter.txt');
  }

  Future<int> readCounter() async {
    try {
      final file = await _localFile;

      // Read the file
      final contents = await file.readAsString();

      return int.parse(contents);
    } catch (e) {
      // If encountering an error, return 0
      return 0;
    }
  }

  Future<File> writeCounter(int counter) async {
    final file = await _localFile;

    // Write the file
    return file.writeAsString('$counter');
  }
}

class FlutterDemo extends StatefulWidget {
  final CounterStorage storage;

  FlutterDemo({Key? key, required this.storage}) : super(key: key);

  @override
  _FlutterDemoState createState() => _FlutterDemoState();
}

class _FlutterDemoState extends State<FlutterDemo> {
  int _counter = 0;

  @override
  void initState() {
    super.initState();
    widget.storage.readCounter().then((int value) {
      setState(() {
        _counter = value;
      });
    });
  }

  Future<File> _incrementCounter() {
    setState(() {
      _counter++;
    });

    // Write the variable as a string to the file.
    return widget.storage.writeCounter(_counter);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Reading and Writing Files')),
      body: Center(
        child: Text(
          'Button tapped $_counter time${_counter == 1 ? '' : 's'}.',
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

