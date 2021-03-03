# Novel Crawler

## 20210225

筆記寫太多了閱讀不易，分第二的檔案出來

### could not receive message from daemon

昨天還正常的專案今天打開又發生了第一次Debug失敗的那個錯誤，這次沒發現可以更新的SDK，換個關鍵字去Google就找到了這次問題的元兇

![](https://i.imgur.com/wec3wWk.png)

[StackOverFlow](https://github.com/gradle/gradle/issues/14094)

沒錯，只要把win10筆電的行動熱點關掉就行了orz

我在想前天遇到的狀況可能也是這個原因，而不是SDK未更新



>  事情太多了沒空寫，今天到此為止