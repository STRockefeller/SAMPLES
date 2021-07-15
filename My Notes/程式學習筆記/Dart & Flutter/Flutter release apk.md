# Flutter APK Release

[Reference](https://www.codegrepper.com/code-examples/whatever/build+apk+in+flutter+in+visual+code)

[Reference:Flutter.dev](https://flutter.dev/docs/deployment/android)

本篇僅記錄release android apk的方法，不包含App bundle以及ios ipa事實上windows OS並不能接輸出ipa

```powershell
PS C:\Users\admin\AndroidStudioProjects\atelier_ladida> flutter build ipa
Building for iOS is only supported on macOS.
```

App bundle有用到的時候可能會另外紀錄一篇筆記，也可能直接加入這篇

ios的部分可能必須使用虛擬機或雲端環境才能搞定(參考[這篇文章](https://stackoverflow.com/questions/47006906/developing-for-ios-device-in-windows-environment-with-flutter))，總之都很麻煩，有空再試試。

## Icons

設定各種大小的Icon，可以使用[flutter_launcher_icons](https://pub.dev/packages/flutter_launcher_icons)套件來省掉一些麻煩

在`pubspec.yaml`加入依賴之後將Icon圖放到assets資料夾，接著在`pubspec.yaml`添加以下程式碼指向圖片路徑、設定檔案名稱等等

```yaml
flutter_icons:
  image_path: "assets/icons/ic_launcher.png"
  android: true # can specify file name here e.g. "ic_launcher"
  ios: true # can specify file name here e.g. "My-Launcher-Icon"
```

在command line輸入下方程式碼，剩下的套件會幫我們完成

```powershell
flutter pub get
flutter pub run flutter_launcher_icons:main
```

## Permissions

**特別注意**:在debug的時候會預設開啟所有權限而不需要特別設定，打包前務必確認應用程式需要甚麼權限。

到`\android\app\src\main\AndroidManifest.xml`設定android權限要求

例如如果我需要使用網路

```xml
<manifest>
    <application>
        <!--Something Here-->
    </application>
    <uses-permission android:name="android.permission.INTERNET"/>
</manifest>
```

其他權限可以參考[Reference](https://codertw.com/android-%E9%96%8B%E7%99%BC/345051/)



## Version name and version code

參考[這篇](https://developer.android.com/studio/publish/versioning)

在pubspec.yaml修改

```
version: 1.0.0+1
```

前面是version name 

後面是version code 整數，數字越大版本越新

## Visual Studio Code

查到如下指令

```
flutter build apk --split-per-abi
```

---

### What is ABI?

先google一下`abi`是甚麼

全稱為application binary interface

定義如下

> Different Android devices use different CPUs, which in turn support different instruction sets. Each combination of CPU and instruction set has its own Application Binary Interface (ABI). An ABI includes the following information: The CPU instruction set (and extensions) that can be used.

就我的理解是根據不同的環境下(CPU, Operation System)，硬體可以編譯執行的Binary Code也有所不同，對於這些Binary Code的規範就成了所謂的ABI，所以APP只要支援更多種的ABI就能在更多不同類型的設備上順利安裝執行。

---



再來看看help吧

```powershell
PS C:\Users\admin\AndroidStudioProjects\novel_crawler_by_rockefeller> flutter build apk --help
Build an Android APK file from your app.

This command can build debug and release versions of your application. 'debug' builds support debugging and a quick development cycle. 'release' builds don't support debugging and are suitable for deploying to app stores. If you are deploying the app to the Play Store, it's recommended to use app bundles or split the APK to reduce the APK size. Learn more at:

 * https://developer.android.com/guide/app-bundle
 * https://developer.android.com/studio/build/configure-apk-splits#configure-abi-split

Global options:
-h, --help                  Print this usage information.
-v, --verbose               Noisy logging, including all shell commands executed.
                            If used with "--help", shows hidden options. If used with "flutter doctor", shows additional diagnostic information. (Use "-vv" to force    
                            verbose logging in those cases.)
-d, --device-id             Target device id or name (prefixes allowed).
    --version               Reports the version of this tool.
    --suppress-analytics    Suppress analytics reporting when this command runs.

Usage: flutter build apk [arguments]
-h, --help                          Print this usage information.
    --[no-]tree-shake-icons         Tree shake icon fonts so that only glyphs used by the application remain.
                                    (defaults to on)
-t, --target=<path>                 The main entry-point file of the application, as run on the device.
                                    If the "--target" option is omitted, but a file name is provided on the command line, then that is used instead.
                                    (defaults to "lib\main.dart")
    --debug                         Build a debug version of your app.
    --profile                       Build a version of your app specialized for performance profiling.
    --release                       Build a release version of your app (default mode).
    --flavor                        Build a custom app flavor as defined by platform-specific build setup.
                                    Supports the use of product flavors in Android Gradle scripts, and the use of custom Xcode schemes.
    --[no-]pub                      Whether to run "flutter pub get" before executing this command.
                                    (defaults to on)
    --build-number                  An identifier used as an internal version number.
                                    Each build must have a unique identifier to differentiate it from previous builds.
                                    It is used to determine whether one build is more recent than another, with higher numbers indicating more recent build.
                                    On Android it is used as "versionCode".
                                    On Xcode builds it is used as "CFBundleVersion".
    --build-name=<x.y.z>            A "x.y.z" string used as the version number shown to users.
                                    For each new version of your app, you will provide a version number to differentiate it from previous versions.
                                    On Android it is used as "versionName".
                                    On Xcode builds it is used as "CFBundleShortVersionString".
    --split-debug-info=<v1.2.3/>    In a release build, this flag reduces application size by storing Dart program symbols in a separate file on the host rather than in                                    the application. The value of the flag should be a directory where program symbol files can be stored for later use. These symbol   
                                    files contain the information needed to symbolize Dart stack traces. For an app built with this flag, the "flutter symbolize"       
                                    command with the right program symbol file is required to obtain a human readable stack trace.
                                    This flag cannot be combined with "--analyze-size".
    --[no-]obfuscate                In a release build, this flag removes identifiers and replaces them with randomized values for the purposes of source code
                                    obfuscation. This flag must always be combined with "--split-debug-info" option, the mapping between the values and the original    
                                    identifiers is stored in the symbol map created in the specified directory. For an app built with this flag, the "flutter symbolize"                                    command with the right program symbol file is required to obtain a human readable stack trace.

                                    Because all identifiers are renamed, methods like Object.runtimeType, Type.toString, Enum.toString, Stacktrace.toString,
                                    Symbol.toString (for constant symbols or those generated by runtime system) will return obfuscated results. Any code or tests that  
                                    rely on exact names will break.
    --dart-define=<foo=bar>         Additional key-value pairs that will be available as constants from the String.fromEnvironment, bool.fromEnvironment,
                                    int.fromEnvironment, and double.fromEnvironment constructors.
                                    Multiple defines can be passed by repeating "--dart-define" multiple times.
    --[no-]null-assertions          Perform additional null assertions on the boundaries of migrated and un-migrated code. This setting is not currently supported on   
                                    desktop devices.
    --[no-]analyze-size             Whether to produce additional profile information for artifact output size. This flag is only supported on "--release" builds. When 
                                    building for Android, a single ABI must be specified at a time with the "--target-platform" flag. When building for iOS, only the   
                                    symbols from the arm64 architecture are used to analyze code size.
                                    This flag cannot be combined with "--split-debug-info".
    --split-per-abi                 Whether to split the APKs per ABIs. To learn more, see:
                                    https://developer.android.com/studio/build/configure-apk-splits#configure-abi-split
    --target-platform               The target platform for which the app is compiled.
                                    [android-arm (default), android-arm64 (default), android-x86, android-x64 (default)]
    --[no-]track-widget-creation    Track widget creation locations. This enables features such as the widget inspector. This parameter is only functional in debug mode                                    (i.e. when compiling JIT, not AOT).
                                    (defaults to on)

Run "flutter help" to see global options.
```

在`--split-per-abi`的說明中有個[連結](https://developer.android.com/studio/build/configure-apk-splits#configure-abi-split)，似懂非懂。

總之來執行看看

```powershell
PS C:\Users\admin\AndroidStudioProjects\novel_crawler_by_rockefeller> flutter build apk --split-per-abi

Building without sound null safety
For more information see https://dart.dev/null-safety/unsound-null-safety

WARNING: [Processor] Library 'C:\Users\admin\.gradle\caches\modules-2\files-2.1\org.robolectric\shadows-framework\4.3\150103d5732c432906f6130b734e7452855dd67b\shadows-framework-4.3.jar' contains references to both AndroidX and old support library. This seems like the library is partially migrated. Jetifier will try to rewrite the library anyway.
 Example of androidX reference: 'androidx/test/runner/lifecycle/Stage'
 Example of support library reference: 'android/support/annotation/NonNull'
Running Gradle task 'assembleRelease'...
Running Gradle task 'assembleRelease'... Done                     535.4s
√ Built build\app\outputs\flutter-apk\app-armeabi-v7a-release.apk (5.8MB).
```

![](https://i.imgur.com/BGx6yQi.png)

