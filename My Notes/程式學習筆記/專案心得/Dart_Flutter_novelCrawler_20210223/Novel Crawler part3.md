# Novel Crawler

## 20210416

### Loading Widget

發現我的APP因為要等待非同步的爬蟲程式運作的原因Loading還算頻繁，乾脆寫一個東西填入，避免畫面一片空白。

先簡單放個文字就好了，以後要改圖片或者動圖都可以直接修改這段

```dart
static Widget loadingWidget()=>new Container(child: myText("Loading...",Colors.black,50),alignment: Alignment.center,);
```

### Chapter List Page

姑且是寫出來了，讀取時間有一點點略久，不過也不是不能接受(章節數少的約一秒內，多的可能超過三秒)

![](https://i.imgur.com/h5eLBtx.png)

接著就是閱讀頁面的部份了。

 

### Article read

雖然很敷衍不過還是搞出來了

![](https://i.imgur.com/MmVtKnp.png)

姑且算到一個段落好了，之後再慢慢優化。

