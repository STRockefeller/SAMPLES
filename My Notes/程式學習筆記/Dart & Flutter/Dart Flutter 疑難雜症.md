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