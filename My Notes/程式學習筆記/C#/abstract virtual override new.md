# abstract vs virtual

總是搞混的內容，每次查完都覺得很簡單，下次再用到又不確定這樣對不對，還是認命做一次筆記吧

---

**使用場合**

* `abstract` `virtual` 用於父類別
* `override` `new`用於子類別

---

**重點說明**

* `abstract method`是空方法，無實體，子類別**必須**`override`此方法 (就跟繼承`interface`一樣)。
* `abstract method`只存在於`abstract class`中，`abstract class`無法實例化 (因為含有沒實體的方法)
* `abstract method`不能用`private`修飾 (廢話)。

* `virtual method`**必須**宣告主體
* 用`override`複寫`virtual method`的場合，即使將物件宣告為父類別，依然會執行子類別的方法
* 用`new`複寫`virtual method`的場合，若將物件宣告為父類別，則會執行父類別的方法
* 沒寫`override`和`new`的場合，會當作`new`



整理成表格如下(都是用以父類別宣告`new`子類別的建構式的觀點看的)

| sub class \ super class |      abstract       |         virtual         |
| :---------------------: | :-----------------: | :---------------------: |
|        override         | O(父類別不能實例化) | O(父類別用子類別的方法) |
|           new           |          X          | O(父類別用父類別的方法) |

---

## 範例

>  我感覺我在Design Pattern的筆記中寫過類似的東西

---

**abstract and override**

```c#
    abstract class Puni
    {
        public string Color;
        protected abstract void Punipuni();
    }
    class BluePuni:Puni
    {
        protected override void Punipuni() => this.Color = "blue";
    }
```

沒什麼好說的

---

**virtual, override and new**

```C#
    class Puni
    {
        public string Color;
        public virtual void Punipuni() => this.Color="I don't know.";
    }
    class BluePuni:Puni
    {

    }
    class GreenPuni:Puni
    {
        public new void Punipuni() => this.Color = "Green";
    }
    class RedPuni:Puni
    {
        public override void Punipuni() => this.Color = "Red";
    }
```

測試方法，直接把結果寫在後面

```C#
        static void test()
        {
            List<string> puniColors = new List<string>();
            Puni puniA = new Puni();          //I don't know
            Puni puniB = new BluePuni();      //I don't know
            Puni puniC = new GreenPuni();     //I don't know
            Puni puniD = new RedPuni();       //Red
            BluePuni puniE = new BluePuni();  //I don't know
            GreenPuni puniF = new GreenPuni();//Green
            RedPuni puniG = new RedPuni();    //Red
            puniA.Punipuni();
            puniB.Punipuni();
            puniC.Punipuni();
            puniD.Punipuni();
            puniE.Punipuni();
            puniF.Punipuni();
            puniG.Punipuni();
            puniColors.Add(puniA.Color);
            puniColors.Add(puniB.Color);
            puniColors.Add(puniC.Color);
            puniColors.Add(puniD.Color);
            puniColors.Add(puniE.Color);
            puniColors.Add(puniF.Color);
            puniColors.Add(puniG.Color);
            foreach (string str in puniColors)
                Console.WriteLine(str);
        }
```

