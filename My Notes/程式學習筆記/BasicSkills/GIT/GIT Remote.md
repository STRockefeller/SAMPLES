# Git Remote

## github

只是在github上建立新的reposit或上傳現有資料基本上沒啥太大問題，空的reposit點到code tab github 會直接手把手教可以很順利的完成。

### 容量問題

push 時會遇到的問題

像是

```
fatal: The remote end hung up unexpectedly
```

或者

```
remote: error: GH001: Large files detected. You may want to try Git Large File Storage
```



網路上有提供調整git config的作法，參考[這篇文章](https://pingnote.blogspot.com/2020/03/git-push-fatal-remote-end-hung-up-unexpectedly.html)

```
git config --local http.postBuffer 524288000
```

我試過沒有作用就是了



最後有成功解決的作法是

error跳出來的時候會附帶通知哪個/哪些檔案超出容量，把那個/那些檔案刪除之後，再次push會發現--依然跳出一樣的錯誤

我推測原因是push時會把每一個本地的log都一起丟上去(本地因為沒有容量限制所以都可以成功commit)

最後我把`.git`資料夾整個刪除，然後再次做`git init`重做一次就OK了。