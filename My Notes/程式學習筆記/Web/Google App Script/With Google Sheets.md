# GAS with Google Sheets

## Review with Questions

1. 請簡短描述從**GAS加入sheet服務**和**從google sheet設置指令碼**兩者的差異。

## Abstract

連動到google sheets在我看來應該算是GAS最實用的功能之一。

這次就從這個功能切入。

由於撰寫筆記的當下對於GAS仍不熟悉，所以先以學到哪記到哪的方式記錄，將來再找機會整理。



首先在GAS連動到google sheets的做法大致有兩種

* 先建立GAS，在服務中加入sheets，取得id後進行連結的做法
* 從google sheet上方選單中:`工具`-->`指令碼編輯器`建立GAS



以下都以我的AtelierLaDiDa資料為例，內容大概如下，含多個資料表格式都一樣

| No   | Name         | Attribute1 | Attribute2 | Attribute3 | Attribute4 | Attribute5 | Type1 | Type2  | Type3    | Type4 | Type5 | Type6 | Source1 | Source2 | Source3 | Source4 | Japanese     |
| ---- | ------------ | ---------- | ---------- | ---------- | ---------- | ---------- | ----- | ------ | -------- | ----- | ----- | ----- | ------- | ------- | ------- | ------- | ------------ |
| 1.0  | 尖刺果實     | 10         | -          | 15         | -          |            | 素材  | 植物類 | 尖刺果實 | 水果  |       |       |         |         |         |         | とげとげの実 |
| 2.0  | 綠色尖刺果實 | -          | 5          | 30         | -          |            | 素材  | 植物類 | 尖刺果實 | 水果  |       |       |         |         |         |         | 青とげの実   |
| 3.0  | 金色尖刺果實 | 10         | -          | 40         | -          |            | 素材  | 植物類 | 尖刺果實 | 水果  |       |       |         |         |         |         | 金とげの実   |



## 先建立GAS的做法

在服務中加入sheets，同意google的授權提示

然後取得要使用的sheet id

假如網址是`https://docs.google.com/spreadsheets/d/abcde/edit#gid=12345`

那id就是`abcde`

接著記錄下要取得的資訊(當然也可以全拿)

例如`"A2:R4"`代表從A2到R4的內容，在google sheet中如果範圍給`"A2:R"`R後面沒有數字代表有多少取多少

接著使用sheet api的get方法`Sheets.Spreadsheets.Values.get(spreadsheetId, rangeName)`取得資料

```js
function doGet(request) {
  var spreadsheetId = request.parameter.id;
  var rangeName = request.parameter.range;
  var data = Sheets.Spreadsheets.Values.get(spreadsheetId, rangeName);
  var values = data.values;
  
  if (!values) {
    Logger.log('No data found.');
  } else {
      //確定有資料後在這裡做運算
  }
  //把資料json序列化後回傳
  var myJSON = JSON.stringify(values)
  return ContentService.createTextOutput(myJSON).setMimeType(ContentService.MimeType.JSON);
}
function doTest(){
    //id複製sheet網址片段
  var req = {parameter:{id:"********",range:"A2:R"}};
  Logger.log(doGet(req).getContent());
}
```

debug查看上例的`data`可以看到`range`屬性的值為`"Ayesha!A2:R1000"`可以發現如果沒有指定工作表，google會很貼心的設定範圍在第一個工作表(google sheet的工作表會自動按照名稱排列)。





## 從Google sheet設置指令碼的方法

從google sheet上方選單中:`工具`-->`指令碼編輯器`建立GAS，一樣會有授權提問，不用加入服務就可以使用

取得google sheet的內容

```js
var sheet = SpreadsheetApp.getActive();
```

取得工作表的內容

```js
var totori = sheet.getSheetByName("Totori");
```

取得第i列第j行的內容

```js
var data = totori.getRange(i,j).getValue();
```



```js
function doGet(request) {
  var sheet = SpreadsheetApp.getActive();
  var totori = sheet.getSheetByName("Totori");
  for(var i =1;i<=386;i++)
  {
    for(var j=1;j<=18;j++)
    {
      Logger.log(totori.getRange(i,j).getValue());
    }
  }
}
```

