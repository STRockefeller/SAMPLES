# Unit Testing

[Reference: MSDN](https://docs.microsoft.com/zh-tw/dotnet/core/testing/)

測試工具有`xUnit` `NUnit` `bUnit`等等，這篇筆記不會談到，有機會另外撰寫。

## 要點

### 良好單元測試的特性

- **快速**。 具有數千個單元測試的成熟專案並不罕見。 單元測試應只需要極少的時間執行。 毫秒。
- **隔離**。 單元測試是獨立的，可以單獨執行，並且對所有外部因素 (例如檔案系統或資料庫) 沒有任何相依性。
- 可 **重複**。 執行單元測試應該與其結果一致，亦即，如果您未變更回合之間的任何項目，會一律傳回相同的結果。
- **自我檢查**。 測試應該能夠自行偵測到通過或失敗，不需要任何人為互動。
- **及時**。 相較於所測試的程式碼，單元測試不應耗費不成比例的冗長時間來撰寫。 相較於撰寫程式碼，如果您發現測試程式碼花費大量時間，請考慮使用更易於測試的設計。



### 為測試命名

測試的名稱應該包含三個部分：

- 所測試的方法名稱。
- 用以測試的案例。
- 叫用案例時的預期行為。

例如

```c#
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add("0");

    Assert.Equal(0, actual);
}
```



### 排列測試

**排列、採取動作、判定** 是進行單元測試時的常見模式。 顧名思義，其中包含三個主要動作：

- 「排列」物件，並視需要建立和設定這些物件。
- 對物件 *採取行動*。
- 「判定」某個項目如預期般運作。



簡單的說就是**可讀性**要優先於**簡潔**

不好的例子

```C#
[Fact]
public void Add_EmptyString_ReturnsZero()
{
    // Arrange
    var stringCalculator = new StringCalculator();

    // Assert
    Assert.Equal(0, stringCalculator.Add(""));
}
```

好的例子

```C#
[Fact]
public void Add_EmptyString_ReturnsZero()
{
    // Arrange
    var stringCalculator = new StringCalculator();

    // Act
    var actual = stringCalculator.Add("");

    // Assert
    Assert.Equal(0, actual);
}
```



### 撰寫最低限度通過測試

要在單元測試中使用的輸入應該是最簡單的，才能驗證您目前正在測試的行為。

- 測試對程式碼基底的未來變更變得更具復原性。
- 更接近測試行為而不是實作。

如果測試包含的資訊比通過測試所需還要多，更有可能會將錯誤帶進測試中，且會讓測試的意圖較不清楚。 撰寫測試時，您想要專注于行為。 在模型上設定額外的屬性，或在不需要時使用非零值，只會減損您嘗試證明的項目。



不好的例子

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add("42");

    Assert.Equal(42, actual);
}
```

好的例子

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add("0");

    Assert.Equal(0, actual);
}
```



### 避免 magic strings

- 避免測試的讀者為了找出使該值變得特殊的原因而需要檢查生產環境程式碼。
- 明確地顯示您想要「證明」的項目，而不是嘗試「完成」的項目。

魔術字串可能會對測試的讀者造成混淆。 如果字串看起來不正常，讀者可能會納悶為什麼針對參數或傳回值選擇特定值。 這可能會導致他們過於仔細查看實作詳細資料，而不是專注於測試。

不好的例子

```csharp
[Fact]
public void Add_BigNumber_ThrowsException()
{
    var stringCalculator = new StringCalculator();

    Action actual = () => stringCalculator.Add("1001");

    Assert.Throws<OverflowException>(actual);
}
```

好的例子

```csharp
[Fact]
void Add_MaximumSumResult_ThrowsOverflowException()
{
    var stringCalculator = new StringCalculator();
    const string MAXIMUM_RESULT = "1001";

    Action actual = () => stringCalculator.Add(MAXIMUM_RESULT);

    Assert.Throws<OverflowException>(actual);
}
```



敘述比較難懂，但單就範例的程式碼看來，代表我們不應該沒頭沒尾的給一個`"1001"`，最好是能透過變數命名的方式來讓讀者知道`"1001"`代表什麼意思。



### 避免在測試中使用邏輯

撰寫您的單元測試時，請避免使用手動字串串連和邏輯條件，例如 `if`、`while`、`for`、`switch` 等等。



- 在測試內帶進 Bug 的機率更低。
- 著重最終結果，而不是實作詳細資料。



不好的例子

```csharp
[Fact]
public void Add_MultipleNumbers_ReturnsCorrectResults()
{
    var stringCalculator = new StringCalculator();
    var expected = 0;
    var testCases = new[]
    {
        "0,0,0",
        "0,1,2",
        "1,2,3"
    };

    foreach (var test in testCases)
    {
        Assert.Equal(expected, stringCalculator.Add(test));
        expected += 3;
    }
}
```

好的例子

```csharp
[Theory]
[InlineData("0,0,0", 0)]
[InlineData("0,1,2", 3)]
[InlineData("1,2,3", 6)]
public void Add_MultipleNumbers_ReturnsSumOfNumbers(string input, int expected)
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add(input);

    Assert.Equal(expected, actual);
}
```



### 避免多個判定

在撰寫測試時，請嘗試於每個測試只包含一個判定。 只使用一個判定的常見方法包括：

- 為每個判定建立個別測試。
- 使用參數化測試。

Why?

- 如果某個判定失敗，將不會評估後續的判定。
- 確保您不會在測試中判定多個案例。
- 為您提供測試失敗原因的全貌。

不好的例子

```csharp
[Fact]
public void Add_EdgeCases_ThrowsArgumentExceptions()
{
    Assert.Throws<ArgumentException>(() => stringCalculator.Add(null));
    Assert.Throws<ArgumentException>(() => stringCalculator.Add("a"));
}
```

好的例子

```csharp
[Theory]
[InlineData(null)]
[InlineData("a")]
public void Add_InputNullOrAlphabetic_ThrowsArgumentException(string input)
{
    var stringCalculator = new StringCalculator();

    Action actual = () => stringCalculator.Add(input);

    Assert.Throws<ArgumentException>(actual);
}
```



### 透過單元測試的公用方法驗證私用方法

在大部分情況下，應該不需要測試私用方法。 私用方法是實作詳細資料。 您可以這樣來看：私用方法永遠不會單獨存在。 在某個時間點，將有一個公眾對應方法呼叫私用方法作為其實作的一部分。 您應該關注的是呼叫私用方法的公用方法最終結果。



### 虛設 (Stub) 靜態參考

單元測試的其中一個準則是，其必須具有待測系統的完整控制權。 當實際執行的程式碼包含對靜態 (參考的呼叫時，這可能會造成問題，例如 `DateTime.Now`) 。 請考慮下列程式碼



```csharp
public int GetDiscountedPrice(int price)
{
    if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
    {
        return price / 2;
    }
    else
    {
        return price;
    }
}
```

如何對此程式碼進行單元測試？ 您可以嘗試一種方法，例如



```csharp
public void GetDiscountedPrice_NotTuesday_ReturnsFullPrice()
{
    var priceCalculator = new PriceCalculator();

    var actual = priceCalculator.GetDiscountedPrice(2);

    Assert.Equals(2, actual)
}

public void GetDiscountedPrice_OnTuesday_ReturnsHalfPrice()
{
    var priceCalculator = new PriceCalculator();

    var actual = priceCalculator.GetDiscountedPrice(2);

    Assert.Equals(1, actual);
}
```

不幸的是，您很快就會發現測試有幾個問題。

- 如果測試套件是在星期二執行，第二項測試會通過，但第一項測試會失敗。
- 如果測試套件是在其他天執行，第一項測試會通過，但第二項測試會失敗。

若要解決這些問題，您需要將「接合線」帶進生產環境程式碼。 其中一個方法是在介面中包裝您需要控制的程式碼，並使生產環境程式碼相依於該介面。



```csharp
public interface IDateTimeProvider
{
    DayOfWeek DayOfWeek();
}

public int GetDiscountedPrice(int price, IDateTimeProvider dateTimeProvider)
{
    if (dateTimeProvider.DayOfWeek() == DayOfWeek.Tuesday)
    {
        return price / 2;
    }
    else
    {
        return price;
    }
}
```

您的測試套件現在已變成



```csharp
public void GetDiscountedPrice_NotTuesday_ReturnsFullPrice()
{
    var priceCalculator = new PriceCalculator();
    var dateTimeProviderStub = new Mock<IDateTimeProvider>();
    dateTimeProviderStub.Setup(dtp => dtp.DayOfWeek()).Returns(DayOfWeek.Monday);

    var actual = priceCalculator.GetDiscountedPrice(2, dateTimeProviderStub);

    Assert.Equals(2, actual);
}

public void GetDiscountedPrice_OnTuesday_ReturnsHalfPrice()
{
    var priceCalculator = new PriceCalculator();
    var dateTimeProviderStub = new Mock<IDateTimeProvider>();
    dateTimeProviderStub.Setup(dtp => dtp.DayOfWeek()).Returns(DayOfWeek.Tuesday);

    var actual = priceCalculator.GetDiscountedPrice(2, dateTimeProviderStub);

    Assert.Equals(1, actual);
}
```



這段說的比較複雜，簡單的說就是必須設法避免方法依賴於外在的條件。