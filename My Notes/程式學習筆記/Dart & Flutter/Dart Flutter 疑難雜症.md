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



## Fail to delete entry

這是在flutter更新途中出現的問題(手賤看到有新版本就想更新...)

狀況如下(一開始是`flutter upgrade`時出現的狀況跟下面差不多，後來我把視窗關掉執行`flutter doctor`如下)

```powershell
Microsoft Windows [版本 10.0.18363.815]
(c) 2019 Microsoft Corporation. 著作權所有，並保留一切權利。

C:\Users\admin>flutter doctor
Building flutter tool...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (9 tries left)

等候  0 秒後，按 CTRL+C 結束 ...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (8 tries left)

等候  0 秒後，按 CTRL+C 結束 ...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (7 tries left)

等候  0 秒後，按 CTRL+C 結束 ...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (6 tries left)

等候  0 秒後，按 CTRL+C 結束 ...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (5 tries left)

等候  4 秒後，按 CTRL+C 結束 ...
要終止批次工作嗎 (Y/N)? y

```



1.執行`flutter pub pub cache repair`

```powershell
C:\Users\admin>flutter pub pub cache repair
Building flutter tool...
Running pub upgrade...
Pub failed to delete entry because it was in use by another process.
This may be caused by a virus scanner or having a file
in the directory open in another application.
Error (1): Unable to 'pub upgrade' flutter tool. Retrying in five seconds... (9 tries left)

等候  1 秒後，按 CTRL+C 結束 ...
要終止批次工作嗎 (Y/N)? y
```

無效



2.以系統管理員身分執行

```powershell
C:\Windows\system32>flutter doctor
Building flutter tool...
Running pub upgrade...
Downloading Material fonts...                                    1,256ms
Downloading package sky_engine...                                  365ms
Downloading flutter_patched_sdk tools...                           962ms
Downloading flutter_patched_sdk_product tools...                   714ms
Downloading windows-x64 tools...                                    5.9s
Downloading windows-x64/font-subset tools...                       783ms
Doctor summary (to see all details, run flutter doctor -v):
[√] Flutter (Channel dev, 2.3.0-16.0.pre, on Microsoft Windows [Version 10.0.18363.815], locale zh-TW)
[√] Android toolchain - develop for Android devices (Android SDK version 30.0.3)
[√] Chrome - develop for the web
[√] Android Studio (version 4.1.0)
[√] VS Code
[√] Connected device (1 available)

• No issues found!
```

有效



## Android Toolchain

原本沒問題，執行完flutter upgrade後就出事啦，flutter每次都是更新就出事...

```powershell
C:\Users\admin>flutter doctor
Doctor summary (to see all details, run flutter doctor -v):
[√] Flutter (Channel dev, 2.3.0-24.0.pre, on Microsoft Windows [Version 10.0.18363.815], locale zh-TW)
[!] Android toolchain - develop for Android devices (Android SDK version 30.0.3)
    X cmdline-tools component is missing
      Run `path/to/sdkmanager --install "cmdline-tools;latest"`
      See https://developer.android.com/studio/command-line for more details.
    X Android license status unknown.
      Run `flutter doctor --android-licenses` to accept the SDK licenses.
      See https://flutter.dev/docs/get-started/install/windows#android-setup for more details.
[√] Chrome - develop for the web
[√] Android Studio (version 4.1.0)
[√] VS Code
[√] Connected device (1 available)

! Doctor found issues in 1 category.
```



```powershell
C:\Users\admin>flutter doctor --android-licenses
Android sdkmanager not found. Update to the latest Android SDK and ensure that the cmdline-tools are installed to resolve this.
```



看敘述原因應該來自於sdkmanager位置遺失

找了一下sdkmanager的路徑 `C:\Users\admin\AppData\Local\Android\Sdk\tools\bin` 是一個.bat檔案，試著照著提示install



```powershell
C:\Users\admin>C:\Users\admin\AppData\Local\Android\Sdk\tools\bin\sdkmanager.bat --install "cmdline-tools;latest"

ERROR: JAVA_HOME is not set and no 'java' command could be found in your PATH.

Please set the JAVA_HOME variable in your environment to match the
location of your Java installation.
```



哪那麼麻煩還要安裝java...

換個方法看能不能從android studio修正這個問題

在android studio 的 sdk manager-->android sdk-->sdk tools-->勾選 sdk command-line tools並安裝

之後再次執行flutter doctor

```powershell
C:\Windows\System32>flutter doctor
Doctor summary (to see all details, run flutter doctor -v):
[√] Flutter (Channel dev, 2.3.0-24.0.pre, on Microsoft Windows [Version 10.0.18363.815], locale zh-TW)
[√] Android toolchain - develop for Android devices (Android SDK version 30.0.3)
[√] Chrome - develop for the web
[√] Android Studio (version 4.1.0)
[√] VS Code
[√] Connected device (1 available)

• No issues found!
```

問題排除



## Could not receive a message from the daemon.

關熱點



## 加入`android.permission.WRITE_EXTERNAL_STORAGE`權限後，`getApplicationDocumentsDirectory()`跳例外`Null check operator used on a null value`

參考

https://stackoverflow.com/questions/59598533/flutter-getapplicationdocumentsdirectory-returns-null

https://medium.com/@sadabwasim/flutter-runtime-permission-d82461ff926c

很有用的資訊不過都過時了，現版本的flutter會因為null safety 混用而無法執行這個pub`simple_permissions`

```
The library 'package:simple_permissions/simple_permissions.dart' is legacy, and should not be imported into a null safe library.
```

https://dart.dev/null-safety/unsound-null-safety
