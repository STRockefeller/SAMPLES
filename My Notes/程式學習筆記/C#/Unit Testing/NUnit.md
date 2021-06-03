#  NUnit

[Reference: MSDN](https://docs.microsoft.com/zh-tw/dotnet/core/testing/unit-testing-with-nunit)

MSDN寫的有點複雜，這裡把我理解的內容總結出來

## 前置作業

安裝這三個NuGet套件

* Microsoft.Net.Test.Sdk
* NUnit
* NUnit3TestAdapter

**重點說明一下**

`Microsoft.Net.Test.Sdk`這個套件**包含程式進入點**，所以如果是在現有的專案中撰寫測試程式的話，就會發生**同時存在多個程式進入點**的情況，在測試時必須先把原本的程式進入點註解掉才可以順利編譯。

剛踩完坑查看文件才知道有這回事，希望不要重複犯錯。

也因如此，情況允許的話，另外建立新專案進行測試可能是比較好的選擇。

2021/06/03 紀錄 : VS2019新建立的NUnit測試專案，三個套件都發生版本過舊的問題，需開啟NuGet套件管理進行更新後才可正常測試。



## 撰寫測試

引用`NUnit.Framework`

```C#
using NUnit.Framework;
```



在class上方加入`[TestFixture]`標籤

如果有多個測試程式要共用的物件或初始屬性可以建立帶有`[StartUp]`標籤的方法，這個方法會在執行測試之前先被呼叫。

測試程式上方加入`[Test]`標籤

然後按照3A原則撰寫測試。範例如下

```C#
[TestFixture]
public class TestClass
{
    [StartUp]
    public void StartUpFunction(){/*這個方法會在執行測試之前先被呼叫*/}
    [Test]
    public void TestFunction()
    {
        //arrange
        
        //設定初始條件
        //var input = "inptValue";
        
        //act
        
        //執行動作
        //var actual = myFunction(input);
        
        //assert
        
        //比較、輸出測試結果
        //var expected = "Correct Result";
        //Assert.AreEqual(expected, actual);
    }
}
```



當然實際上還有很多進階的用法，例如標籤後面可以再加入一些參數，`Assert`也不是只有`AreEqual`一個方法而已，不過這邊就不細說了，以後有興致再來補充。



## 執行測試

如果用的是VisualStudio，正常情況下寫完測試程式後，不論是測試方法還是被測試方法上方都會出現一個小圖示，點開可以進行測試。

或者對方法名稱按下滑鼠右鍵也會有執行測試的選項可以選擇。



### 執行測試後測試項目依然顯示"未測試"

先檢查三個套件是否都安裝完畢，即便僅安裝`NUnit.Framework`套件程式依然可以編譯過關，但無法正常測試。

再檢查三個套件版本是否是最新版，以及他們是否支援目前的專案類型及版本。