# 疑難雜症

紀錄各種問題以及解決方法，比較複雜的會另撰寫筆記



## AVD unable to locate adb

[Reference:StackOverFlow](https://stackoverflow.com/questions/39036796/unable-to-locate-adb-using-android-studio)

1. on your android studio at the top right corner beside the search icon you can find the SDK Manager.
2. view android SDK location (this will show you your sdk path)
3. navigate to file explorer on your system, and locate the file path, this should be found something like Windows=> c://Users/johndoe/AppData/local/android (you can now see the sdk.) Mac=>/Users/johndoe/Library/Android/sdk
4. check the platform tools folder and see if you would see anything like adb.exe (it should be missing probably because it was corrupted and your antivirus or windows defender has quarantined it)
5. delete the platform tools folder
6. go back to android studio and from where you left off navigate to sdk tools (this should be right under android sdk location)
7. uncheck android sdk platform-tools and select ok. (this will uninstall the platform tools from your ide) wait till it is done and then your gradle will sync.
8. after sync is complete, go back and check the box of android sdk platform-tools (this will install a fresh one with new adb.exe) wait till it is done and sync project and then you are good to go.



## SetState Exception



自製爬蟲專案中遇到的問題，從Statefull畫面點選上一頁欲回Home Page時跳Exception 如下

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





## invalid depfile

參考資料:[StackOverFlow](https://stackoverflow.com/questions/60872753/invalid-depfile-flutter) [github issue](https://github.com/flutter/flutter/issues/21053)

我的情況是原本可以正常跑的專案，隔天就無法Debug看到的個Exception

Flutter doctor 也沒有問題

後來是跑 flutter clean後再次跑 flutter pub get 和 flutter pub upgrade 後發現原本用的 package 中有一個方法的簽章在新版本有所不同

另外還有把文件中的`import 'dart:html';`拿掉才能夠正常運作

![](https://i.imgur.com/4TbZCd9.png)

![](https://i.imgur.com/LNrmnwG.png)



## system ui isn't responding

參考資料:[StackOverFlow](https://stackoverflow.com/questions/63371056/the-system-ui-isnt-responding-in-android-emulator-flutter)

這個是ADV上的模擬器跳出的提醒視窗

解法一

> 1.Open AVD Manager. [![enter image description here](https://i.stack.imgur.com/mQP7q.jpg)](https://i.stack.imgur.com/mQP7q.jpg) 2.Click to edit button for your device. [![enter image description here](https://i.stack.imgur.com/bARFT.jpg)](https://i.stack.imgur.com/bARFT.jpg) 3.Select Hardware in the Graphics drop down menu. [![enter image description here](https://i.stack.imgur.com/y4jYv.jpg)](https://i.stack.imgur.com/y4jYv.jpg)

對我Emulator無效，依然有同樣的問題

---

解法二

> Open Android Studio. Navigate to Configure > AVD Manager. Under Actions > dropdown triangle on the right > Cold Boot Now:

有效

