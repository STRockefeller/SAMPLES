# Override

[MSDN:Override](https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/override)



細節常常忘記所以還是整理個筆記幫助記憶，順便比較new 和 override的使用時機

## MSDN範例

```C#
abstract class Shape
{
    public abstract int GetArea();
}

class Square : Shape
{
    int side;

    public Square(int n) => side = n;

    // GetArea method is required to avoid a compile-time error.
    public override int GetArea() => side * side;

    static void Main()
    {
        var sq = new Square(12);
        Console.WriteLine($"Area of the square = {sq.GetArea()}");
    }
}
// Output: Area of the square = 144
```

> `override` 方法提供繼承自基底類別的成員新實作。 `override` 宣告覆寫的方法稱之為覆寫基底方法。 覆寫基底方法必須和 `override` 方法有相同的簽章。 如需繼承的資訊，請參閱[繼承](https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/inheritance)。
>
> 您無法覆寫非虛擬或靜態方法。 覆寫基底方法必須是 `virtual`、`abstract` 或 `override`。
>
> `override` 宣告不能變更 `virtual` 方法的存取範圍。 `override` 方法和 `virtual` 方法都必須具有相同的[存取層級修飾詞](https://docs.microsoft.com/zh-tw/dotnet/csharp/language-reference/keywords/access-modifiers)。
>
> 您不能使用 `new`、`static` 或 `virtual` 修飾詞來修改 `override` 方法。
>
> 要覆寫的屬性宣告必須指定和繼承屬性完全相同的存取修飾詞、型別和名稱，而覆寫的屬性必須是 `virtual`、`abstract` 或 `override`。
>
> 如需如何使用關鍵字的詳細資訊 `override` ，請參閱使用 [Override 和 new 關鍵字進行版本控制](https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/versioning-with-the-override-and-new-keywords) ，以及 [瞭解使用 override 和 new 關鍵字](https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords)的時機。

## 範例及說明

先宣告一個父類別:噗尼

```C#
    public class Puni
    {
        public string attack() => "puni attack";
    }
```

宣告一個子類別繼承噗尼

```C#
    public class BluePuni : Puni { }
```

接下來的測試都基於以下物件宣告

```C#
            Puni puni = new Puni();
            BluePuni bluePuni = new BluePuni();
            Puni weirdPuni = new BluePuni();
```



`bluePuni.attack(); //"puni attack"`

此時藍噗尼的攻擊方式就是繼承父類別噗尼的攻擊方式也就是`"puni attack"`



### new

假如我要讓藍噗尼的攻擊有別於其他噗尼，修改子類別如下

```C#
    public class BluePuni : Puni
    {
        public string attack() => "blue puni attack";
    }
```

這時如果IDE是VS的話會警告將隱藏基底方法，並提示使用`new`關鍵字

加入`new`嚴謹寫法如下(即使沒有使用`new`關鍵字VS也會認定你想要隱藏基底類別的方法)

```C#
    public class BluePuni : Puni
    {
        public new string attack() => "blue puni attack";
    }
```

動作結果如下

```C#
            puni.attack();//"puni attack"
            bluePuni.attack();//"blue puni attack"
            weirdPuni.attack();//"puni attack"
```

以下為MSDN說明

> `new` 關鍵字會保留產生該輸出的關聯性，但是它會隱藏警告。 如果變數具有 `BaseClass` 類型，其會繼續存取 `BaseClass` 的成員，而含 `DerivedClass` 類型的變數則會繼續優先存取 `DerivedClass` 中的成員，然後才考慮繼承自 `BaseClass` 的成員。

由於此處的`weirdPuni`類型為`Puni`(值為`BluePuni`)所以會優先存取`Puni`的方法。



### override

接著改用override

改寫`BluePuni`

```C#
    public class BluePuni : Puni
    {
        public override string attack() => "blue puni attack";
    }
```

此時IDE會提示無法覆寫方法因為其未宣告為`virtual` `override` 或 `abstract`

照著提示修改父類別(加入`virtual`關鍵字)

```C#
    public class Puni
    {
        public virtual string attack() => "puni attack";
    }
```

結果如下

```C#
            puni.attack();//"puni attack"
            bluePuni.attack();//"blue puni attack"
            weirdPuni.attack();//"blue puni attack"
```



也可以照提示將父類別及其方法改為抽象

```C#
    public abstract class Puni
    {
        public abstract string attack();
    }
```

注意抽象類別**不能**實作，抽象方法也**不能**實作。

