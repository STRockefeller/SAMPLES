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

```
C:\Users\admin\AndroidStudioProjects\novel_crawler>flutter run -v
[ +123 ms] executing: [C:\flutter/] git -c log.showSignature=false log -n 1 --pretty=format:%H
[ +132 ms] Exit code 0 from: git -c log.showSignature=false log -n 1 --pretty=format:%H
[   +1 ms] f8cd24de95b16b5a1ce6ebc0716154271fbf6252
[   +1 ms] executing: [C:\flutter/] git tag --points-at f8cd24de95b16b5a1ce6ebc0716154271fbf6252
[ +101 ms] Exit code 0 from: git tag --points-at f8cd24de95b16b5a1ce6ebc0716154271fbf6252
[   +1 ms] 1.27.0-4.0.pre
[  +58 ms] executing: [C:\flutter/] git rev-parse --abbrev-ref --symbolic @{u}
[  +83 ms] Exit code 0 from: git rev-parse --abbrev-ref --symbolic @{u}
[        ] origin/dev
[        ] executing: [C:\flutter/] git ls-remote --get-url origin
[  +81 ms] Exit code 0 from: git ls-remote --get-url origin
[        ] https://github.com/flutter/flutter.git
[ +139 ms] executing: [C:\flutter/] git rev-parse --abbrev-ref HEAD
[  +80 ms] Exit code 0 from: git rev-parse --abbrev-ref HEAD
[        ] dev
[ +314 ms] Artifact Instance of 'AndroidGenSnapshotArtifacts' is not required, skipping update.
[   +2 ms] Artifact Instance of 'AndroidInternalBuildArtifacts' is not required, skipping update.
[   +4 ms] Artifact Instance of 'IOSEngineArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'FlutterWebSdk' is not required, skipping update.
[  +10 ms] Artifact Instance of 'WindowsEngineArtifacts' is not required, skipping update.
[   +3 ms] Artifact Instance of 'MacOSEngineArtifacts' is not required, skipping update.
[   +4 ms] Artifact Instance of 'LinuxEngineArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'LinuxFuchsiaSDKArtifacts' is not required, skipping update.
[   +3 ms] Artifact Instance of 'MacOSFuchsiaSDKArtifacts' is not required, skipping update.
[   +5 ms] Artifact Instance of 'FlutterRunnerSDKArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'FlutterRunnerDebugSymbols' is not required, skipping update.
[ +157 ms] executing: C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe devices -l
[ +240 ms] List of devices attached
                    emulator-5554          device product:sdk_gphone_x86_arm model:sdk_gphone_x86_arm device:generic_x86_arm transport_id:4
[  +14 ms] C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe -s emulator-5554 shell getprop
[ +208 ms] Artifact Instance of 'AndroidInternalBuildArtifacts' is not required, skipping update.
[   +2 ms] Artifact Instance of 'IOSEngineArtifacts' is not required, skipping update.
[  +12 ms] Artifact Instance of 'WindowsEngineArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'MacOSEngineArtifacts' is not required, skipping update.
[   +5 ms] Artifact Instance of 'LinuxEngineArtifacts' is not required, skipping update.
[   +5 ms] Artifact Instance of 'LinuxFuchsiaSDKArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'MacOSFuchsiaSDKArtifacts' is not required, skipping update.
[   +1 ms] Artifact Instance of 'FlutterRunnerSDKArtifacts' is not required, skipping update.
[   +2 ms] Artifact Instance of 'FlutterRunnerDebugSymbols' is not required, skipping update.
[ +156 ms] Running "flutter pub get" in novel_crawler...
[   +9 ms] executing: [C:\Users\admin\AndroidStudioProjects\novel_crawler/] C:\flutter\bin\cache\dart-sdk\bin\pub.bat --verbose get --no-precompile
[+1008 ms] FINE: Pub 2.13.0-30.0.dev
[ +110 ms] MSG : Resolving dependencies...
[  +44 ms] SLVR: fact: novel_crawler is 1.0.0+1
[  +13 ms] SLVR: derived: novel_crawler
[  +35 ms] SLVR: fact: novel_crawler depends on flutter any from sdk
[   +3 ms] SLVR: fact: novel_crawler depends on cupertino_icons ^1.0.2
[   +4 ms] SLVR: fact: novel_crawler depends on flutter_test any from sdk
[   +1 ms] SLVR:   selecting novel_crawler
[   +5 ms] SLVR:   derived: flutter_test any from sdk
[   +1 ms] SLVR:   derived: cupertino_icons ^1.0.2
[   +4 ms] SLVR:   derived: flutter any from sdk
[  +18 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on flutter any from sdk
[   +3 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on test_api 0.2.19
[   +6 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on path 1.8.0
[   +2 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on fake_async 1.2.0
[   +5 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on clock 1.1.0
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on stack_trace 1.10.0
[   +5 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on vector_math 2.1.0
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on async 2.5.0
[   +4 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on boolean_selector 2.1.0
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on characters 1.1.0
[   +3 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on charcode 1.2.0
[   +6 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on collection 1.15.0
[   +3 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on matcher 0.12.10
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on meta 1.3.0
[   +4 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on source_span 1.8.0
[   +5 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on stream_channel 2.1.0
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on string_scanner 1.1.0
[   +4 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on term_glyph 1.2.0
[   +1 ms] SLVR:   fact: flutter_test 0.0.0 from sdk depends on typed_data 1.3.0
[   +3 ms] SLVR:     selecting flutter_test 0.0.0 from sdk
[   +6 ms] SLVR:     derived: typed_data 1.3.0
[   +2 ms] SLVR:     derived: term_glyph 1.2.0
[   +3 ms] SLVR:     derived: string_scanner 1.1.0
[   +5 ms] SLVR:     derived: stream_channel 2.1.0
[   +4 ms] SLVR:     derived: source_span 1.8.0
[   +4 ms] SLVR:     derived: meta 1.3.0
[   +1 ms] SLVR:     derived: matcher 0.12.10
[   +1 ms] SLVR:     derived: collection 1.15.0
[   +3 ms] SLVR:     derived: charcode 1.2.0
[   +6 ms] SLVR:     derived: characters 1.1.0
[   +1 ms] SLVR:     derived: boolean_selector 2.1.0
[   +3 ms] SLVR:     derived: async 2.5.0
[   +4 ms] SLVR:     derived: vector_math 2.1.0
[   +1 ms] SLVR:     derived: stack_trace 1.10.0
[   +4 ms] SLVR:     derived: clock 1.1.0
[   +4 ms] SLVR:     derived: fake_async 1.2.0
[   +1 ms] SLVR:     derived: path 1.8.0
[   +1 ms] SLVR:     derived: test_api 0.2.19
[   +3 ms] SLVR:       selecting cupertino_icons 1.0.2
[   +7 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on characters 1.1.0
[   +3 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on collection 1.15.0
[   +1 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on meta 1.3.0
[   +4 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on typed_data 1.3.0
[   +5 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on vector_math 2.1.0
[   +1 ms] SLVR:       fact: flutter 0.0.0 from sdk depends on sky_engine any from sdk
[   +5 ms] SLVR:         selecting flutter 0.0.0 from sdk
[   +1 ms] SLVR:         derived: sky_engine any from sdk
[   +3 ms] SLVR:         fact: typed_data 1.3.0 depends on collection ^1.15.0
[   +6 ms] SLVR:           selecting typed_data 1.3.0
[   +3 ms] SLVR:             selecting term_glyph 1.2.0
[   +1 ms] SLVR:             fact: string_scanner 1.1.0 depends on charcode ^1.2.0
[  +13 ms] SLVR:             fact: string_scanner 1.1.0 depends on source_span ^1.8.0
[  +15 ms] SLVR:               selecting string_scanner 1.1.0
[   +4 ms] SLVR:               fact: stream_channel 2.1.0 depends on async ^2.5.0
[   +1 ms] SLVR:                 selecting stream_channel 2.1.0
[   +1 ms] SLVR:                 fact: source_span 1.8.0 depends on charcode ^1.2.0
[   +5 ms] SLVR:                 fact: source_span 1.8.0 depends on collection ^1.15.0
[   +1 ms] SLVR:                 fact: source_span 1.8.0 depends on path ^1.8.0
[   +4 ms] SLVR:                 fact: source_span 1.8.0 depends on term_glyph ^1.2.0
[   +4 ms] SLVR:                   selecting source_span 1.8.0
[   +1 ms] SLVR:                     selecting meta 1.3.0
[   +6 ms] SLVR:                     fact: matcher 0.12.10 depends on stack_trace ^1.10.0
[   +3 ms] SLVR:                       selecting matcher 0.12.10
[   +4 ms] SLVR:                         selecting collection 1.15.0
[   +1 ms] SLVR:                           selecting charcode 1.2.0
[   +4 ms] SLVR:                             selecting characters 1.1.0
[   +1 ms] SLVR:                             fact: boolean_selector 2.1.0 depends on source_span ^1.8.0
[   +4 ms] SLVR:                             fact: boolean_selector 2.1.0 depends on string_scanner ^1.1.0
[   +2 ms] SLVR:                               selecting boolean_selector 2.1.0
[   +3 ms] SLVR:                               fact: async 2.5.0 depends on collection ^1.15.0
[   +6 ms] SLVR:                                 selecting async 2.5.0
[   +3 ms] SLVR:                                   selecting vector_math 2.1.0
[   +4 ms] SLVR:                                   fact: stack_trace 1.10.0 depends on path ^1.8.0
[   +1 ms] SLVR:                                     selecting stack_trace 1.10.0
[   +5 ms] SLVR:                                       selecting clock 1.1.0
[   +3 ms] SLVR:                                       fact: fake_async 1.2.0 depends on clock ^1.1.0
[   +1 ms] SLVR:                                       fact: fake_async 1.2.0 depends on collection ^1.15.0
[   +1 ms] SLVR:                                         selecting fake_async 1.2.0
[   +3 ms] SLVR:                                           selecting path 1.8.0
[   +2 ms] SLVR:                                           fact: test_api 0.2.19 depends on async ^2.5.0
[  +18 ms] SLVR:                                           fact: test_api 0.2.19 depends on boolean_selector ^2.1.0
[   +6 ms] SLVR:                                           fact: test_api 0.2.19 depends on collection ^1.15.0
[   +3 ms] SLVR:                                           fact: test_api 0.2.19 depends on meta ^1.3.0
[   +4 ms] SLVR:                                           fact: test_api 0.2.19 depends on path ^1.8.0
[   +4 ms] SLVR:                                           fact: test_api 0.2.19 depends on source_span ^1.8.0
[   +1 ms] SLVR:                                           fact: test_api 0.2.19 depends on stack_trace ^1.10.0
[   +4 ms] SLVR:                                           fact: test_api 0.2.19 depends on stream_channel ^2.1.0
[   +1 ms] SLVR:                                           fact: test_api 0.2.19 depends on string_scanner ^1.1.0
[   +3 ms] SLVR:                                           fact: test_api 0.2.19 depends on term_glyph ^1.2.0
[   +7 ms] SLVR:                                           fact: test_api 0.2.19 depends on matcher >=0.12.10 <0.12.11
[   +3 ms] SLVR:                                             selecting test_api 0.2.19
[   +4 ms] SLVR:                                               selecting sky_engine 0.0.99 from sdk
[ +192 ms] SLVR: Version solving took 0:00:00.655797 seconds.
[   +3 ms]     | Tried 1 solutions.
[   +4 ms] FINE: Resolving dependencies finished (0.698s).
[ +136 ms] MSG :   source_span 1.8.0 (1.8.1 available)
[  +95 ms] IO  : Writing 3445 characters to text file .\pubspec.lock.
[   +3 ms] FINE: Contents:
[   +3 ms]     | # Generated by pub
[   +1 ms]     | # See https://dart.dev/tools/pub/glossary#lockfile
[   +1 ms]     | packages:
[   +3 ms]     |   async:
[   +4 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +2 ms]     |       name: async
[   +3 ms]     |       url: "https://pub.dartlang.org"
[   +4 ms]     |     source: hosted
[   +1 ms]     |     version: "2.5.0"
[   +1 ms]     |   boolean_selector:
[   +3 ms]     |     dependency: transitive
[   +6 ms]     |     description:
[   +1 ms]     |       name: boolean_selector
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +2 ms]     |     source: hosted
[   +4 ms]     |     version: "2.1.0"
[   +1 ms]     |   characters:
[   +1 ms]     |     dependency: transitive
[   +4 ms]     |     description:
[   +4 ms]     |       name: characters
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +1 ms]     |     version: "1.1.0"
[   +2 ms]     |   charcode:
[   +5 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +2 ms]     |       name: charcode
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +4 ms]     |     source: hosted
[   +1 ms]     |     version: "1.2.0"
[   +1 ms]     |   clock:
[   +7 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +1 ms]     |       name: clock
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +3 ms]     |     version: "1.1.0"
[   +5 ms]     |   collection:
[   +1 ms]     |     dependency: transitive
[   +2 ms]     |     description:
[   +1 ms]     |       name: collection
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +1 ms]     |     version: "1.15.0"
[   +6 ms]     |   cupertino_icons:
[   +1 ms]     |     dependency: "direct main"
[   +1 ms]     |     description:
[   +1 ms]     |       name: cupertino_icons
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +1 ms]     |     version: "1.0.2"
[   +3 ms]     |   fake_async:
[   +5 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +1 ms]     |       name: fake_async
[   +2 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +4 ms]     |     version: "1.2.0"
[   +1 ms]     |   flutter:
[   +3 ms]     |     dependency: "direct main"
[   +1 ms]     |     description: flutter
[   +4 ms]     |     source: sdk
[   +1 ms]     |     version: "0.0.0"
[   +1 ms]     |   flutter_test:
[   +2 ms]     |     dependency: "direct dev"
[   +5 ms]     |     description: flutter
[   +1 ms]     |     source: sdk
[   +2 ms]     |     version: "0.0.0"
[   +4 ms]     |   matcher:
[   +1 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +2 ms]     |       name: matcher
[   +5 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +2 ms]     |     version: "0.12.10"
[   +1 ms]     |   meta:
[   +4 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +4 ms]     |       name: meta
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +4 ms]     |     source: hosted
[   +1 ms]     |     version: "1.3.0"
[   +1 ms]     |   path:
[   +2 ms]     |     dependency: transitive
[   +6 ms]     |     description:
[   +2 ms]     |       name: path
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +5 ms]     |     source: hosted
[   +1 ms]     |     version: "1.8.0"
[   +8 ms]     |   sky_engine:
[   +2 ms]     |     dependency: transitive
[   +4 ms]     |     description: flutter
[   +1 ms]     |     source: sdk
[   +6 ms]     |     version: "0.0.99"
[   +2 ms]     |   source_span:
[   +4 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +4 ms]     |       name: source_span
[   +5 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +3 ms]     |     version: "1.8.0"
[   +1 ms]     |   stack_trace:
[   +6 ms]     |     dependency: transitive
[   +2 ms]     |     description:
[   +1 ms]     |       name: stack_trace
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +1 ms]     |     version: "1.10.0"
[   +7 ms]     |   stream_channel:
[   +1 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +1 ms]     |       name: stream_channel
[   +1 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +1 ms]     |     version: "2.1.0"
[   +8 ms]     |   string_scanner:
[   +1 ms]     |     dependency: transitive
[   +2 ms]     |     description:
[   +1 ms]     |       name: string_scanner
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +4 ms]     |     version: "1.1.0"
[   +1 ms]     |   term_glyph:
[   +4 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +3 ms]     |       name: term_glyph
[   +5 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +2 ms]     |     version: "1.2.0"
[   +4 ms]     |   test_api:
[   +1 ms]     |     dependency: transitive
[   +5 ms]     |     description:
[   +1 ms]     |       name: test_api
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +3 ms]     |     version: "0.2.19"
[   +1 ms]     |   typed_data:
[   +6 ms]     |     dependency: transitive
[   +1 ms]     |     description:
[   +2 ms]     |       name: typed_data
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +1 ms]     |     source: hosted
[   +5 ms]     |     version: "1.3.0"
[   +1 ms]     |   vector_math:
[   +5 ms]     |     dependency: transitive
[   +8 ms]     |     description:
[   +7 ms]     |       name: vector_math
[   +4 ms]     |       url: "https://pub.dartlang.org"
[   +6 ms]     |     source: hosted
[   +2 ms]     |     version: "2.1.0"
[   +2 ms]     | sdks:
[   +1 ms]     |   dart: ">=2.12.0-0.0 <3.0.0"
[   +4 ms] IO  : Writing 2370 characters to text file .\.packages.
[   +6 ms] FINE: Contents:
[   +3 ms]     | # This file is deprecated. Tools should instead consume
[   +1 ms]     | # `.dart_tools/package_config.json`.
[   +5 ms]     | #
[   +5 ms]     | # For more info see: https://dart.dev/go/dot-packages-deprecation
[   +4 ms]     | #
[   +1 ms]     | # Generated by pub on 2021-02-23 15:32:13.639340.
[   +2 ms]     | async:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/async-2.5.0/lib/
[   +1 ms]     | boolean_selector:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/boolean_selector-2.1.0/lib/
[   +6 ms]     | characters:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/characters-1.1.0/lib/
[   +2 ms]     | charcode:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/charcode-1.2.0/lib/
[   +4 ms]     | clock:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/clock-1.1.0/lib/
[   +1 ms]     | collection:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/collection-1.15.0/lib/
[   +1 ms]     | cupertino_icons:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/cupertino_icons-1.0.2/lib/
[   +8 ms]     | fake_async:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/fake_async-1.2.0/lib/
[   +1 ms]     | flutter:file:///C:/flutter/packages/flutter/lib/
[        ]     | flutter_test:file:///C:/flutter/packages/flutter_test/lib/
[   +1 ms]     | matcher:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/matcher-0.12.10/lib/
[   +2 ms]     | meta:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/meta-1.3.0/lib/
[   +4 ms]     | path:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/path-1.8.0/lib/
[   +2 ms]     | sky_engine:file:///C:/flutter/bin/cache/pkg/sky_engine/lib/
[   +3 ms]     | source_span:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/source_span-1.8.0/lib/
[   +5 ms]     | stack_trace:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/stack_trace-1.10.0/lib/
[   +1 ms]     | stream_channel:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/stream_channel-2.1.0/lib/
[   +9 ms]     | string_scanner:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/string_scanner-1.1.0/lib/
[   +4 ms]     | term_glyph:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/term_glyph-1.2.0/lib/
[   +1 ms]     | test_api:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/test_api-0.2.19/lib/
[   +4 ms]     | typed_data:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/typed_data-1.3.0/lib/
[   +4 ms]     | vector_math:file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/vector_math-2.1.0/lib/
[   +1 ms]     | novel_crawler:lib/
[   +4 ms] IO  : Writing 4726 characters to text file .\.dart_tool\package_config.json.
[   +1 ms] FINE: Contents:
[   +3 ms]     | {
[   +6 ms]     |   "configVersion": 2,
[   +3 ms]     |   "packages": [
[   +4 ms]     |     {
[   +1 ms]     |       "name": "async",
[   +8 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/async-2.5.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.12"
[   +3 ms]     |     },
[   +1 ms]     |     {
[   +6 ms]     |       "name": "boolean_selector",
[   +3 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/boolean_selector-2.1.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +4 ms]     |       "languageVersion": "2.12"
[   +2 ms]     |     },
[   +5 ms]     |     {
[   +5 ms]     |       "name": "characters",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/characters-1.1.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +6 ms]     |       "languageVersion": "2.12"
[   +3 ms]     |     },
[   +4 ms]     |     {
[   +1 ms]     |       "name": "charcode",
[   +5 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/charcode-1.2.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +4 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +3 ms]     |     {
[   +6 ms]     |       "name": "clock",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/clock-1.1.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +4 ms]     |       "languageVersion": "2.12"
[   +4 ms]     |     },
[   +1 ms]     |     {
[   +4 ms]     |       "name": "collection",
[   +1 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/collection-1.15.0",
[   +3 ms]     |       "packageUri": "lib/",
[   +7 ms]     |       "languageVersion": "2.12"
[   +3 ms]     |     },
[   +1 ms]     |     {
[   +4 ms]     |       "name": "cupertino_icons",
[   +5 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/cupertino_icons-1.0.2",
[   +1 ms]     |       "packageUri": "lib/",
[   +5 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +3 ms]     |     {
[   +6 ms]     |       "name": "fake_async",
[   +3 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/fake_async-1.2.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +4 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +4 ms]     |     {
[   +4 ms]     |       "name": "flutter",
[   +3 ms]     |       "rootUri": "file:///C:/flutter/packages/flutter",
[   +1 ms]     |       "packageUri": "lib/",
[   +8 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +5 ms]     |     {
[   +1 ms]     |       "name": "flutter_test",
[   +8 ms] MSG : Got dependencies!
[   +1 ms]     |       "rootUri": "file:///C:/flutter/packages/flutter_test",
[   +1 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +9 ms]     |     {
[   +1 ms]     |       "name": "matcher",
[   +3 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/matcher-0.12.10",
[   +5 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.12"
[   +4 ms]     |     },
[   +4 ms]     |     {
[   +1 ms]     |       "name": "meta",
[   +3 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/meta-1.3.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +8 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +4 ms]     |     {
[   +1 ms]     |       "name": "path",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/path-1.8.0",
[   +4 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +3 ms]     |     {
[   +6 ms]     |       "name": "sky_engine",
[   +3 ms]     |       "rootUri": "file:///C:/flutter/bin/cache/pkg/sky_engine",
[   +1 ms]     |       "packageUri": "lib/",
[   +4 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +5 ms]     |     {
[   +2 ms]     |       "name": "source_span",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/source_span-1.8.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +3 ms]     |       "languageVersion": "2.12"
[   +6 ms]     |     },
[   +3 ms]     |     {
[   +1 ms]     |       "name": "stack_trace",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/stack_trace-1.10.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +5 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[  +20 ms]     |     {
[  +11 ms]     |       "name": "stream_channel",
[   +1 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/stream_channel-2.1.0",
[   +3 ms]     |       "packageUri": "lib/",
[   +5 ms]     |       "languageVersion": "2.12"
[   +5 ms]     |     },
[   +1 ms]     |     {
[   +4 ms]     |       "name": "string_scanner",
[   +1 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/string_scanner-1.1.0",
[   +3 ms]     |       "packageUri": "lib/",
[   +6 ms]     |       "languageVersion": "2.12"
[   +4 ms]     |     },
[   +4 ms]     |     {
[   +1 ms]     |       "name": "term_glyph",
[   +7 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/term_glyph-1.2.0",
[  +15 ms]     |       "packageUri": "lib/",
[   +3 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[  +10 ms]     |     {
[   +5 ms]     |       "name": "test_api",
[   +3 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/test_api-0.2.19",
[   +4 ms]     |       "packageUri": "lib/",
[   +6 ms]     |       "languageVersion": "2.12"
[   +6 ms]     |     },
[   +7 ms]     |     {
[   +3 ms]     |       "name": "typed_data",
[   +4 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/typed_data-1.3.0",
[   +1 ms]     |       "packageUri": "lib/",
[   +9 ms]     |       "languageVersion": "2.12"
[   +1 ms]     |     },
[   +4 ms]     |     {
[   +1 ms]     |       "name": "vector_math",
[   +5 ms]     |       "rootUri": "file:///C:/Users/admin/AppData/Local/Pub/Cache/hosted/pub.dartlang.org/vector_math-2.1.0",
[   +5 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.12"
[   +3 ms]     |     },
[   +1 ms]     |     {
[   +6 ms]     |       "name": "novel_crawler",
[   +3 ms]     |       "rootUri": "../",
[   +4 ms]     |       "packageUri": "lib/",
[   +1 ms]     |       "languageVersion": "2.7"
[   +5 ms]     |     }
[   +1 ms]     |   ],
[   +4 ms]     |   "generated": "2021-02-23T07:32:13.826827Z",
[   +1 ms]     |   "generator": "pub",
[   +3 ms]     |   "generatorVersion": "2.13.0-30.0.dev"
[   +6 ms]     | }
[  +61 ms] Running "flutter pub get" in novel_crawler... (completed in 3.3s)
[ +538 ms] Generating C:\Users\admin\AndroidStudioProjects\novel_crawler\android\app\src\main\java\io\flutter\plugins\GeneratedPluginRegistrant.java
[  +99 ms] ro.hardware = ranchu
[  +37 ms] Using hardware rendering with device sdk gphone x86 arm. If you notice graphics artifacts, consider enabling software rendering with "--enable-software-rendering".
[  +65 ms] Initializing file store
[  +33 ms] Skipping target: gen_localizations
[  +27 ms] complete
[  +12 ms] Launching lib\main.dart on sdk gphone x86 arm in debug mode...
[  +13 ms] C:\flutter\bin\cache\dart-sdk\bin\dart.exe --disable-dart-dev C:\flutter\bin\cache\artifacts\engine\windows-x64\frontend_server.dart.snapshot --sdk-root
C:\flutter\bin\cache\artifacts\engine\common\flutter_patched_sdk/ --incremental --target=flutter --debugger-module-names --experimental-emit-debug-metadata -DFLUTTER_WEB_AUTO_DETECT=true
--output-dill C:\Users\admin\AppData\Local\Temp\flutter_tools.5cf00e0a\flutter_tool.b3c862c9\app.dill --packages
C:\Users\admin\AndroidStudioProjects\novel_crawler\.dart_tool\package_config.json -Ddart.vm.profile=false -Ddart.vm.product=false --enable-asserts --track-widget-creation
--filesystem-scheme org-dartlang-root --initialize-from-dill build\c075001b96339384a97db4862b8ab8db.cache.dill.track.dill
[  +57 ms] executing: C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe -s emulator-5554 shell -x logcat -v time -t 1
[+1106 ms] <- compile package:novel_crawler/main.dart
[ +350 ms] --------- beginning of main
                    02-23 07:32:16.693 I/HotwordLSAdapter( 2572): stopListeningStatus result: 109
[  +29 ms] executing: C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe version
[  +71 ms] Android Debug Bridge version 1.0.41
           Version 30.0.5-6877874
           Installed as C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe
[   +8 ms] executing: C:\Users\admin\AppData\Local\Android\sdk\platform-tools\adb.exe start-server
[ +209 ms] Building APK
[  +36 ms] Running Gradle task 'assembleDebug'...
[  +16 ms] Using gradle from C:\Users\admin\AndroidStudioProjects\novel_crawler\android\gradlew.bat.
[   +3 ms] C:\Users\admin\AndroidStudioProjects\novel_crawler\android\gradlew.bat mode: 33279 rwxrwxrwx.
[  +20 ms] executing: C:\Program Files\Android\Android Studio\jre\bin\java -version
[ +282 ms] Exit code 0 from: C:\Program Files\Android\Android Studio\jre\bin\java -version
[   +3 ms] openjdk version "1.8.0_242-release"
           OpenJDK Runtime Environment (build 1.8.0_242-release-1644-b01)
           OpenJDK 64-Bit Server VM (build 25.242-b01, mixed mode)
[   +5 ms] executing: [C:\Users\admin\AndroidStudioProjects\novel_crawler\android/] C:\Users\admin\AndroidStudioProjects\novel_crawler\android\gradlew.bat -Pverbose=true
-Ptarget-platform=android-x86 -Ptarget=C:\Users\admin\AndroidStudioProjects\novel_crawler\lib\main.dart -Pdart-defines=RkxVVFRFUl9XRUJfQVVUT19ERVRFQ1Q9dHJ1ZQ== -Pdart-obfuscation=false
-Ptrack-widget-creation=true -Ptree-shake-icons=false -Pfilesystem-scheme=org-dartlang-root assembleDebug
[+1861 ms] Welcome to Gradle 6.7!
[  +14 ms] Here are the highlights of this release:
[  +12 ms]  - File system watching is ready for production use
[  +23 ms]  - Declare the version of Java your build requires
[  +10 ms]  - Java 15 support
[  +16 ms] For more details see https://docs.gradle.org/6.7/release-notes.html
[+1842 ms] Starting a Gradle Daemon, 6 stopped Daemons could not be reused, use --status for details
[+4541 ms] FAILURE: Build failed with an exception.
[   +3 ms] * What went wrong:
[   +5 ms] Could not receive a message from the daemon.
[   +6 ms] * Try:
[   +1 ms] Run with --stacktrace option to get the stack trace. Run with --info or --debug option to get more log output. Run with --scan to get full insights.
[   +8 ms] * Get more help at https://help.gradle.org
[  +51 ms] Running Gradle task 'assembleDebug'... (completed in 8.7s)
[+5619 ms] Exception: Gradle task assembleDebug failed with exit code 1
[   +5 ms] "flutter run" took 21,241ms.
[   +8 ms]
           #0      throwToolExit (package:flutter_tools/src/base/common.dart:12:3)
           #1      RunCommand.runCommand (package:flutter_tools/src/commands/run.dart:655:9)
           <asynchronous suspension>
           #2      FlutterCommand.verifyThenRunCommand (package:flutter_tools/src/runner/flutter_command.dart:1185:12)
           <asynchronous suspension>
           #3      FlutterCommand.run.<anonymous closure> (package:flutter_tools/src/runner/flutter_command.dart:1037:27)
           <asynchronous suspension>
           #4      AppContext.run.<anonymous closure> (package:flutter_tools/src/base/context.dart:152:19)
           <asynchronous suspension>
           #5      AppContext.run (package:flutter_tools/src/base/context.dart:151:12)
           <asynchronous suspension>
           #6      CommandRunner.runCommand (package:args/command_runner.dart:197:13)
           <asynchronous suspension>
           #7      FlutterCommandRunner.runCommand.<anonymous closure> (package:flutter_tools/src/runner/flutter_command_runner.dart:283:9)
           <asynchronous suspension>
           #8      AppContext.run.<anonymous closure> (package:flutter_tools/src/base/context.dart:152:19)
           <asynchronous suspension>
           #9      AppContext.run (package:flutter_tools/src/base/context.dart:151:12)
           <asynchronous suspension>
           #10     FlutterCommandRunner.runCommand (package:flutter_tools/src/runner/flutter_command_runner.dart:239:5)
           <asynchronous suspension>
           #11     run.<anonymous closure>.<anonymous closure> (package:flutter_tools/runner.dart:62:9)
           <asynchronous suspension>
           #12     run.<anonymous closure> (package:flutter_tools/runner.dart:60:12)
           <asynchronous suspension>
           #13     AppContext.run.<anonymous closure> (package:flutter_tools/src/base/context.dart:152:19)
           <asynchronous suspension>
           #14     AppContext.run (package:flutter_tools/src/base/context.dart:151:12)
           <asynchronous suspension>
           #15     runInContext (package:flutter_tools/src/context_runner.dart:77:10)
           <asynchronous suspension>
           #16     main (package:flutter_tools/executable.dart:92:3)
           <asynchronous suspension>


[  +41 ms] ensureAnalyticsSent: 32ms
[   +3 ms] Running shutdown hooks
[   +3 ms] Shutdown hook priority 4
[  +15 ms] Shutdown hooks complete
[   +3 ms] exiting with code 1
```

好吧我放棄了，先用Chrome來跑吧



### 規劃APP內容

#### App Bar

共用，就先放個上一頁，還有Title就好

#### Home Page

預計放小說網站的選擇，不過應該暫時'只會有一個

#### 選擇網站後

執行爬蟲，目標是放在這個頁面的熱門書籍連結，以及分類清單(放在Drawer裡面)

選擇清待後爬書名，選擇書名後爬章節目錄，選擇章節後爬內文



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

