# Dart 非同步

這篇主要整理Dart非同步的一些做法，內容會跟以往C#所學做一些比較

[Reference:程式前沿](https://codertw.com/%E7%A8%8B%E5%BC%8F%E8%AA%9E%E8%A8%80/754694/)

[Reference:ITHelp](https://ithelp.ithome.com.tw/articles/10215207)

[Reference:Romain Rastel](https://medium.com/flutter-community/dart-what-are-mixins-3a72344011f3)

[Reference:iter01](https://iter01.com/441301.html)



## 前言

### 耗時操作的處理

- **處理方式一：** 多執行緒，比如Java、C++，我們普遍的做法是開啟一個新的執行緒（Thread），在新的執行緒中完成這些非同步的操作，再通過執行緒間通訊的方式，將拿到的資料傳遞給主執行緒。
- **處理方式二：** 單執行緒+事件迴圈，比如JavaScript、Dart都是基於單執行緒加事件迴圈來完成耗時操作的處理。

至於單執行緒+事件迴圈怎麼做到

- 因為我們的一個應用程式大部分時間都是處於空閒的狀態的，並不是無限制的在和使用者進行互動。
- 比如等待使用者點選、網路請求資料的返回、檔案讀寫的IO操作，這些等待的行為並不會阻塞我們的執行緒。
- 這是因為類似於網路請求、檔案讀寫的IO，我們都可以基於非阻塞呼叫。

### 阻塞式呼叫和非阻塞式呼叫

如果想搞懂這個點，我們需要知道作業系統中的**阻塞式**呼叫和**非阻塞式**呼叫的概念。

- 阻塞和非阻塞關注的是**程式在等待呼叫結果（訊息，返回值）時的狀態。**
- **阻塞式呼叫：** 呼叫結果返回之前，當前執行緒會被掛起，呼叫執行緒只有在得到呼叫結果之後才會繼續執行。
- **非阻塞式呼叫：** 呼叫執行之後，當前執行緒不會停止執行，只需要過一段時間來檢查一下有沒有結果返回即可。

而我們開發中的很多耗時操作，都可以基於這樣的 **非阻塞式呼叫**：

- 比如網路請求本身使用了Socket通訊，而Socket本身提供了select模型，可以進行`非阻塞方式的工作`；
- 比如檔案讀寫的IO操作，我們可以使用作業系統提供的`基於事件的回撥機制`；

這些操作都不會阻塞我們單執行緒的繼續執行，我們的執行緒在等待的過程中可以繼續去做別的事情：喝杯咖啡、打把遊戲，等真正有了響應，再去進行對應的處理即可。

這時，我們可能有兩個問題：

- **問題一：** 如果在多核CPU中，單執行緒是不是就沒有充分利用CPU呢？這個問題，我會放在後面來講解。
- **問題二：** 單執行緒是如何來處理網路通訊、IO操作它們返回的結果呢？答案就是事件迴圈（Event Loop）。

> 節錄自iter01



## Event Loop

Event Loop 的原理就是將需要處理的事件放入一個Event Queue 中，當Event Queue 非空時，不斷取出事件並執行。

以下是[iter0](https://iter01.com/441301.html)所寫的虛擬碼

```dart
// 這裡我使用陣列模擬佇列, 先進先出的原則
List eventQueue = []; 
var event;

// 事件迴圈從啟動的一刻，永遠在執行
while (true) {
  if (eventQueue.length > 0) {
    // 取出一個事件
    event = eventQueue.removeAt(0);
    // 執行該事件
    event();
  }
}
```



## Flutter 執行的流程

- Dart的入口是main函式，所以`main函式中的程式碼`會優先執行；
- main函式執行完後，會啟動一個事件迴圈（Event Loop）就會啟動，啟動後開始執行佇列中的任務；
- 首先，會按照先進先出的順序，執行 `微任務佇列（Microtask Queue）`中的所有任務；
- 其次，會按照先進先出的順序，執行 `事件佇列（Event Queue）`中的所有任務；



![程式碼執行順序](https://i.iter01.com/images/bd1752ed3a68e84564bd74ace77fcb176c23f86a2427a07257083fe1f743b97e.jpg)

## Future

A `Future` is used to represent a potential value, or error, that will be available at some time in the future.

### Example

一樣採用iter01的例子，不過由於dart pad 沒辦法import dart:io所以就沒有實際嘗試了。

#### Sync

模擬網路請求'3秒

```dart
import "dart:io";

main(List<String> args) {
  print("main function start");
  print(getNetworkData());
  print("main function end");
}

String getNetworkData() {
  sleep(Duration(seconds: 3));
  return "network data";
}
```

結果

```
main function start
// 等待3秒
network data
main function end
```



#### Async

```dart
import "dart:io";

main(List<String> args) {
  print("main function start");
  print(getNetworkData());
  print("main function end");
}

Future<String> getNetworkData() {
  return Future<String>(() {
    sleep(Duration(seconds: 3));
    return "network data";
  });
}
```

這次以`Future<String>`作為回傳值

結果

```
main function start
Instance of 'Future<String>'
main function end
```

沒有發生延遲但拿到的是`Future<String>`的實例而不是`String`

**獲取Future得到的結果**

有了Future之後，如何去獲取請求到的結果：通過.then的回撥：

```dart
main(List<String> args) {
  print("main function start");
  // 使用變數接收getNetworkData返回的future
  var future = getNetworkData();
  // 當future例項有返回結果時，會自動回撥then中傳入的函式
  // 該函式會被放入到事件迴圈中，被執行
  future.then((value) {
    print(value);
  });
  print(future);
  print("main function end");
}
```

上面程式碼的執行結果：

```
main function start
Instance of 'Future<String>'
main function end
// 3s後執行下面的程式碼
network data
```

**執行中出現異常**

如果呼叫過程中出現了異常，拿不到結果，如何獲取到異常的資訊呢？

```dart
import "dart:io";

main(List<String> args) {
  print("main function start");
  var future = getNetworkData();
  future.then((value) {
    print(value);
  }).catchError((error) { // 捕獲出現異常時的情況
    print(error);
  });
  print(future);
  print("main function end");
}

Future<String> getNetworkData() {
  return Future<String>(() {
    sleep(Duration(seconds: 3));
    // 不再返回結果，而是出現異常
    // return "network data";
    throw Exception("網路請求出現錯誤");
  });
}
```

上面程式碼的執行結果：

```
main function start
Instance of 'Future<String>'
main function end
// 3s後沒有拿到結果，但是我們捕獲到了異常
Exception: 網路請求出現錯誤
```



### Future 的鏈式呼叫

上面程式碼我們可以進行如下的改進：

- 我們可以在then中繼續返回值，會在下一個鏈式的then呼叫回撥函式中拿到返回的結果

```dart
import "dart:io";

main(List<String> args) {
  print("main function start");

  getNetworkData().then((value1) {
    print(value1);
    return "content data2";
  }).then((value2) {
    print(value2);
    return "message data3";
  }).then((value3) {
    print(value3);
  });

  print("main function end");
}

Future<String> getNetworkData() {
  return Future<String>(() {
    sleep(Duration(seconds: 3));
     return "network data1";
  });
}
```

列印結果如下：

```
main function start
main function end
// 3s後拿到結果
network data1
content data2
message data3
```

### Future 的使用

Future的使用過程：

- 1、建立一個Future（可能是我們建立的，也可能是呼叫內部API或者第三方API獲取到的一個Future，總之你需要獲取到一個Future例項，Future通常會對一些非同步的操作進行封裝）；
- 2、通過`.then`(成功回撥函式)的方式來監聽Future內部執行完成時獲取到的結果；
- 3、通過`.catchError`(失敗或異常回撥函式)的方式來監聽Future內部執行失敗或者出現異常時的錯誤資訊；

### Future 的狀態

#### uncompleted

- 執行Future內部的操作時（在上面的案例中就是具體的網路請求過程，我們使用了延遲來模擬），我們稱這個過程為未完成狀態

#### completed

- 當Future內部的操作執行完成，通常會返回一個值，或者丟擲一個異常。
- 這兩種情況，我們都稱Future為完成狀態。

### Future其他API

```dart
Future.value(value)
```

- 直接獲取一個完成的Future，該Future會直接呼叫then的回撥函式

```dart
main(List<String> args) {
  print("main function start");

  Future.value("哈哈哈").then((value) {
    print(value);
  });

  print("main function end");
}
```

列印結果如下：

```
main function start
main function end
哈哈哈
```

疑惑：為什麼立即執行，但是`哈哈哈`是在最後列印的呢？

- 這是因為Future中的then會作為新的任務會加入到事件佇列中（Event Queue），加入之後你肯定需要排隊執行了

```dart
Future.error(object)
```

- 直接獲取一個完成的Future，但是是一個發生異常的Future，該Future會直接呼叫`catchError`的回撥函式

```dart
main(List<String> args) {
  print("main function start");

  Future.error(Exception("錯誤資訊")).catchError((error) {
    print(error);
  });

  print("main function end");
}
```

列印結果如下：

```
main function start
main function end
Exception: 錯誤資訊
```

`Future.delayed(時間, 回撥函式)`

- 在延遲一定時間時執行回撥函式，執行完回撥函式後會執行then的回撥；
- 之前的案例，我們也可以使用它來模擬，但是直接學習這個API會讓大家更加疑惑；

```dart
main(List<String> args) {
  print("main function start");

  Future.delayed(Duration(seconds: 3), () {
    return "3秒後的資訊";
  }).then((value) {
    print(value);
  });

  print("main function end");
}
```



##  await / async

`await`、 `async`可以讓我們用**同步的程式碼格式**，去實現**非同步的呼叫過程**。並且，通常一個`async`的函式會返回一個`Future`

回顧`Future`生成的方法:

1. 透過`Future`的constructor
2. 透過`Future`的API
3. 透過`async` function

`await` `async`的特色就是把非同步的內容作為同步使用，如將上例修改如下

```dart
Future<String> getNetworkData() async {
  var result = await Future.delayed(Duration(seconds: 3), () {
    return "network data";
  });

  return result;
}
```

- 我們現在可以像同步程式碼一樣去使用Future非同步返回的結果；
- 等待拿到結果之後和其他資料進行拼接，然後一起返回；
- 返回的時候並不需要包裝一個Future，直接返回即可，但是返回值會預設被包裝在一個Future中；



將兩者放在一起更明顯

```dart

main(List<String> args) {
  print("main function start");
  Future<String> myFuture = getNetworkData();
  myFuture.then((v){print(v);});
  //print(getNetworkData());
  print("main function end");
}

//以Future的constructor實例化並回傳
Future<String> getNetworkData() => Future<String>(()=>"network data");

//async await 的用法
Future<String> getNetworkData2() async{
  String result = await Future.delayed(Duration(seconds: 3),()=>"network data");
  return result;
}
```

再來細講一下

```dart
Future<String> getNetworkData2() async{
  String result = await Future.delayed(Duration(seconds: 3),()=>"network data");
  return result;
}
```

```dart
Future.delayed(Duration(seconds: 3),()=>"network data");
```

回傳的是一個`Future<String>`物件，它可以是`Uncompleted`或`Completed`的狀態

```dart
await Future.delayed(Duration(seconds: 3),()=>"network data");
```

回傳的是`String`，它會等後方的內容處理完畢才得到結果的`String`

所以這時 `return result`，回傳的是一個`String`，但`async`又把它包裝在`Future`裡面，所以方法回傳值依然是`Future<String>`



## 補充

### Future 的更多用法

### 

#### factory Future(FutureOr computation())

Future的構造方法，創建一個基本的Future

```
var future = Future(() {
print("哈哈哈");
});
print("111");
//111
//哈哈哈
```

你會發現，結果是打印“哈哈哈”字符串是在後面才輸出的，原因是默認future是異步執行的。如果改成下面的代碼，在future前面加上await關鍵字，則會先打印“哈哈哈”字符串。await會等待直到future執行結束後，才繼續執行後面的代碼。

這個跟上面的事件循環是有關係的，轉發大佬的解釋：main方法中的普通代碼都是同步執行的，所以肯定是main打印先全部打印出來，等main方法結束後會開始檢查microtask中是否有任務，若有則執行，執行完繼續檢查microtask，直到microtask列隊為空。所以接著打印的應該是microtask的打印。最後會去執行event隊列。

```
var future = await Future(() {
print("哈哈哈");
});
print("111");
//哈哈哈
//111
```

#### Future.value()

創建一個返回指定value值的Future

```
var future=Future.value(1);
print(future);
//Instance of 'Future<int>'
```

#### Future.delayed（）

創建一個延遲執行的future。
例如下面的例子，利用Future延遲兩秒後可以打印出字符串。

```
var futureDelayed = Future.delayed(Duration(seconds: 2), () {
print("Future.delayed");
return 2;
});
```

### 

#### Future.foreach(Iterable elements, FutureOr action(T element))

根據某個集合，創建一系列的Future，並且會按順序執行這些Future。
比如下面的例子，根據{1,2,3}創建3個延遲對應秒數的Future。執行結果為1秒後打印1，再過2秒打印2，再過3秒打印3，總時間為6秒。

```
Future.forEach({1,2,3}, (num){
return Future.delayed(Duration(seconds: num),(){print(num);});
});
```

#### Future.wait ( Iterable<Future> futures,{bool eagerError: false, void cleanUp(T successValue)})

用來等待多個future完成，並收集它們的結果。那這樣的結果就有兩種情況了：

- 如果所有future都有正常結果返回：則future的返回結果是所有指定future的結果的集合
- 如果其中一個future有error返回：則future的返回結果是第一個error的值。

比如下面的例子，也是創建3個延遲對應秒數的Future。結果是總時間過了3秒後，才輸出[1, 2, 3]的結果。可以與上面的例子對比一下，一個是順序執行多個Future，一個是異步執行多個Future。

```
var future1 = new Future.delayed(new Duration(seconds: 1), () => 1);
var future2 =
new Future.delayed(new Duration(seconds: 2), () => 2);
var future3 = new Future.delayed(new Duration(seconds: 3), () => 3);
Future.wait({future1,future2,future3}).then(print).catchError(print);
//運行結果： [1, 2, 3]
```

將future2和future3改為有error拋出，則future.wait的結果是這個future2的error值。

```
var future1 = new Future.delayed(new Duration(seconds: 1), () => 1);
var future2 =new Future.delayed(new Duration(seconds: 2), () => throw “throw error2");
var future3 = new Future.delayed(new Duration(seconds: 3), () => throw “throw error3");
Future.wait({future1,future2,future3}).then(print).catchError(print);
//運行結果： throw error2
```

#### Future.any（futures）

返回的是第一個執行完成的future的結果，不會管這個結果是正確的還是error的。（感覺這個的使用場景沒幾個的樣子，想不到有什麼場景要用到這個。）

例如下面的例子，使用Future.delayed（）延遲創建了三個不同的Future，第一個完成返回的是延遲1秒的那個future的結果。

```
 Future
.any([1, 2, 5].map(
(delay) => new Future.delayed(new Duration(seconds: delay), () => delay)))
.then(print)
.catchError(print);
```

#### Future.doWhile()

類似java中doWhile的用法，重複性地執行某一個動作，直到返回false或者Future，退出循環。

使用場景:適用於一些需要遞歸操作的場景。

比如我就是要爬某個平臺的商品分類的數據，那每個分類下面又有相應的子分類。那我就得拿對應父級分類的catId去請求接口，拿到這個分類下面的所有子分類。同樣的，對所有的子分類，也要進行一樣的操作，遞歸直到這個分類是一個葉子節點，也就是該節點下面沒有子分類（有個leaf為true的字段）。哈哈，代碼我就沒貼了，其實也就是一個遞歸操作，直到滿足條件退出future。

例如下面的例子，生成一個隨機數進行等待，直到十秒之後，操作結束。

```
void futureDoWhile(){
var random = new Random();
var totalDelay = 0;
Future
.doWhile(() {
if (totalDelay > 10) {
print('total delay: $totalDelay seconds');
return false;
}
var delay = random.nextInt(5) + 1;
totalDelay += delay;
return new Future.delayed(new Duration(seconds: delay), () {
print('waited $delay seconds');
return true;
});
})
.then(print)
.catchError(print);
}
//輸出結果：
I/flutter (11113): waited 5 seconds
I/flutter (11113): waited 1 seconds
I/flutter (11113): waited 3 seconds
I/flutter (11113): waited 2 seconds
I/flutter (11113): total delay: 12 seconds
I/flutter (11113): null
```

#### Future.microtask(FutureOr computation())

創建一個在microtask隊列運行的future。

在上面講過，microtask隊列的優先級是比event隊列高的，而一般future是在event隊列執行的，所以Future.microtask創建的future會優先於其他future進行執行。

例如下面的代碼，

```
Future((){
print("Future event 1");
});
Future((){
print("Future event 2");
});
Future.microtask((){
print("microtask event");
});
//輸出結果
//microtask event
//Future event 1
//Future event 2
```



## 處理Future的結果

Flutter提供了下面三個方法，讓我們來註冊回調，來監聽處理Future的結果。

```
Future<R> then<R>(FutureOr<R> onValue(T value), {Function onError});
Future<T> catchError(Function onError, {bool test(Object error)});
Future<T> whenComplete(FutureOr action());
```

#### Future.then（）

用於在Future完成的時候添加回調。

注意：then（）的返回值也是一個Future對象。所以我們可以使用鏈式的方法去使用Future，將前一個Future的輸出結果作為後一個Future的輸入，可以寫成鏈式調用。

例如下面的代碼，將前面的future結果作為後面Future的輸入。

```
Future.value(1).then((value) {
return Future.value(value + 2);
}).then((value) {
return Future.value(value + 3);
}).then(print);
//打印結果為6
```

#### Future.cathcError（）

註冊一個回調，來處理有error的Future

```
new Future.error('boom!').catchError(print);
```

與then方法中的onError的區別：then方法裡面也有個onError的參數，也可以用來處理錯誤的Future。

兩者的區別，onError只能處理當前的錯誤Future，而不能處理其他有錯誤的Future。catchError可以捕獲到Future鏈中拋出的所有錯誤。

所以通常的做法是使用catchError來捕捉Future中的所有錯誤，不建議使用then方法中的onError方法。不然每個future的then方法都要加上onError回調的話，就比較麻煩了，而且代碼看起來也是有點亂。

下面是兩個捕捉錯誤的例子。

在那個拋出錯誤的future的then方法裡添加onError回調的話，onError會優先被調用

```
Future.value(1).then((value) {
return Future.value(value + 2);
}).then((value) {
throw "error";
}).then(print, onError: (e) {
print("onError find a error");
}).catchError((e) {
print("catchError find a error");
});
//輸出結果為onError find a error
```

使用catchError來監聽鏈式調用Future裡面拋出來的錯誤。

```
Future.value(1).then((value) {
throw "error";
}).then((value) {
return Future.value(3);
}).then(print).then((value) {
}).catchError((e) {
print("catchError find a error");
});
//輸出結果為catchError find a error"
```

#### Future.whenComplete

類似Java中的finally，Future.whenComplete總是在Future完成後調用，不管Future的結果是正確的還是錯誤的。

注意：Future.whenComplete的返回值也是一個Future對象。

```
Future.delayed(Duration(seconds: 3),(){
print("哈哈");
}).whenComplete((){
print("complete");
});
//哈哈
//complete
```