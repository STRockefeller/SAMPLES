# Builder Pattern

[Reference](http://corrupt003-design-pattern.blogspot.com/2017/01/builder-pattern.html)

[Reference](https://medium.com/wenchin-rolls-around/%E8%A8%AD%E8%A8%88%E6%A8%A1%E5%BC%8F-%E5%BB%BA%E9%80%A0%E8%80%85%E6%A8%A1%E5%BC%8F-builder-design-pattern-7c8eac7c9a7)

## 目的

建造者模式可以解決的問題

> 1. 拖太長的建構子 (telescoping constructor)
> 2. 建構子內不需要的參數必須放 null

---

這個例子說明為甚麼我們需要Builder Pattern

> ## 蓋一間房子
>
> 假設我要創造一個房子的物件，那最簡單粗暴的方法就是定義一個物件 `House`、賦予它房子該有的屬性 (property)：窗戶數、門數、房間數，並使用建構子 (constructor) 來帶入這些參數。
>
> 以 pseudocode 表示：
>
> ```
> House(doorNum, windowNum, roomNum)
> ```
>
> ## 蓋許多間不同的房子
>
> 今天假設我想要創建不同的房子：有花園的房子、有泳池的房子、有車庫的房子等等呢？
>
> 簡單，我們都知道可以把 `House` 變成 base class，再讓其他子類別繼承它，打造不同的房子。
>
> 這時為了記錄這些房子多出來的功能，我的 `House` 物件的屬性就要擴增成：窗戶數、門數、房間數、有無花園、有無泳池、有無車庫。
>
> ```
> House(doorNum, windowNum, roomNum, hasGarden, hasPool, hasGarage)
> ```
>
> 那假設我這個 `House` 物件要給更多房子用：有雕像的房子、有健身房的房子、有爐火的房子等等，那這物件的參數會拖得很長。這顯然不是一個好的解法。
>
> ```
> House(doorNum, windowNum, roomNum, hasGarden, hasPool, hasGarage, hasStatue, hasGym, hasFireStove)
> ```
>
> 因為當我要創造一個房子時，我很難一目瞭然每個建構子代表的意義：
>
> ```
> new House(2, 4, 3, true, true, true, false, false, false)
> ```
>
> 而且如果今天我可能有一系列的房子沒有爐火管線、也禁止蓋健身房，那可能對於這些房子我都要把某些參數設為 null，增加更多的混亂：
>
> ```
> new House(2, 4, 3, true, true, true, false, null, null)
> ```

---

> Builder Pattern（建造者模式）屬於設計模式中Creational Pattern（創建模式）。當物件(object)的建構過程比較複雜或建構的物件有多種樣貌時，可利用Builder Pattern來設計。
>
> 
>
> Builder Pattern的定義如下，「把一個複雜物件的建構與樣貌分離，如此相同的建構過程可以產生不同樣貌的物件」。
>
> > Separate the construction of a complex object from its representation so that the same construction process can create different representations.
>
> 「物件的樣貌(object's representation)」的意思是指物件目前持有屬性的狀態。
>
> 「不同樣貌的物件(different representations)」的意思是，一個類別的屬性（成員變數）有多個，而該類別的實例為各屬性不同排列組合所建構的物件。

## 概念

> 典型的Builder Pattern包含下列角色。
>
> - Product：最終要被建構物件的類別。
> - Builder：用來定義建構物件過程中各必要步驟（方法）的介面。
> - ConcreteBuilder：實作Builder介面，實際用來建構物件的類別
> - Director：負責指揮ConcreteBuilder該如何建構物件。

---

> ## Builder: 客製化蓋房子的種種零件
>
> 建造者模式用一種 configuration 的方式，拆解每個元件建造的過程，避免有拖得很長的建構子，使建構的意圖清晰許多。
>
> 因此剛剛那些房子，實現建造者模式的程式碼可能會長得像這樣：
>
> ```
> HouseBuilder.SetDoorNum(2);            
> HouseBuilder.SetWindowNum(4);            
> HouseBuilder.SetRoomNum(3);        
> HouseBuilder.SetHasGarden(true);
> HouseBuilder.SetHasPool(true);       
> ...    
> HouseBuilder.GetHouse();
> ```
>
> 或是這樣：
>
> ```
> new HouseBuilder().SetDoorNum(2)
>                   .SetWindowNum(4)
>                   .SetRoomNum(3)
>                   .SetHasGarden(true)
>                   .SetHasPool(true)  
>                   ...
>                   .Build();
> ```
>
> 以下我們用另一個例子詳細說明，並在 C# 實現這兩種方式。
>
> # 怎麼實現建造者模式？
>
> ## 開一間水上活動工作室
>
> 假設今天你擁有 SUP、獨木舟、浮潛、衝浪……的器材和專業，打算開一個水上活動工作室，設計不同的活動來銷售。
>
> 藉由建造者模式，我們可以開始設計不同的課程。
>
> 使用建造者模式的方法有兩種：
>
> ## 方式一：透*過 Client、Director、Builder 和 Product 形成的建造者模式*
>
> ![Image for post](https://miro.medium.com/max/575/0*xAiM-17lirZ0h0aF.png)
>
> Source: [Refactoring Guru — Design Patterns: Builder](https://refactoring.guru/design-patterns/builder)
>
> **Builder**: 創建抽象建造者 `ITripBuilder`，僅宣告打造水上活動行程的步驟，不實作這些方法
>
> ```C#
> public interface ITripBuilder
> {
>     void Reset();
>     void SetDestination(string destination);
>     void SetPrice();
>     void SetDifficulty();
>     void SetDurationHours();
>     void SetMaxParticipants();
>     void SetDescription();
>     void SetSalesContext();
>     Trip GetTrip();
> }
> ```
>
> **Concrete Builder**: 建立實體建造者 `KayakTripBuilder`, `SupTripBuilder`，實現 `ITripBuilder` 的各式設定
>
> 這邊以 `SupTripBuilder` 舉例，我們暫且假設 SUP 行程的價格都是 $3,000、行程時間都是 5 小時……，僅有地點是變動的：
>
> ```C#
> public class SupTripBuilder : ITripBuilder
> {
>     private Trip _trip;
>     private int _price = 3000;
>     private int _difficulty = 3;
>     private int _durationHours = 5;
>     private int _maxParticipants = 10;
>     private string _description = "立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水域，從事探索、激流及救生等多方面的活動。";
>     private string _note = "7/1-8/31 SUP 行程 85 折";
>     public SupTripBuilder()
>     {
>         Reset();
>     }
>     public void Reset()
>     {
>         _trip = new Trip();
>     }
>     public void SetDestination(string destination)
>     {
>         _trip.AddDetail($"地點: {destination}");
>     }
>     public void SetPrice()
>     {
>         _trip.AddDetail($"每人價格: NTD {_price}");
>     }
>     public void SetDifficulty()
>     {
>         _trip.AddDetail($"困難度: {_difficulty}/5");
>     }
>     public void SetDurationHours()
>     {
>         _trip.AddDetail($"時間: {_durationHours}");
>     }
>     public void SetMaxParticipants()
>     {
>         _trip.AddDetail($"每團人數限制: {_maxParticipants} 人");
>     }
>     public void SetDescription()
>     {
>         _trip.AddDetail($"SUP 活動敘述: {_description}");
>     }
>     public void SetSalesContext()
>     {
>         _trip.AddDetail($"【{_note}】");
>     }
>     public Trip GetTrip()
>     {
>         return _trip;
>     }
> }
> ```
>
> 
>
> **Director**: 建立 `TripDirector`，讓它呼叫 `KayakTripBuilder`, `SupTripBuilder` 以建立行程的各個部分，並宣告一套流程按順序 (從 SetSalesContext, SetDestination, SetPrice… 接下去) 來建造複雜物件
>
> ```C#
> public class TripDirector
> {
>     public Trip CreateTrip(ITripBuilder tripBuilder, string destination)
>     {
>         tripBuilder.SetSalesContext();
>         tripBuilder.SetDestination(destination);
>         tripBuilder.SetPrice();
>         tripBuilder.SetDifficulty();
>         tripBuilder.SetDurationHours();
>         tripBuilder.SetMaxParticipants();
>         tripBuilder.SetDescription();
>         return tripBuilder.GetTrip();
>     }
> }
> ```
>
> 
>
> **Product**: 建立一個物件 `Trip` 收整行程內容
>
> 這邊簡化的用 List 紀錄行程內容的字串，並直接 yield return 這些內容：
>
> ```C#
> public class Trip
> {
>     private IList<string> _tripDetail = new List<string>();
>     public void AddDetail(string detail)
>     {
>         _tripDetail.Add(detail);
>     }
>     public IEnumerable<string> GetDetail()
>     {
>         foreach (var item in _tripDetail)
>         {
>             yield return item;
>         }
>     }
> }
> ```
>
> **Client**: 建構完以上物件，我們可以在`Program.cs`呼叫 `TripDirector` 開始打造行程
>
> ```C#
> class Program
> {
>     static void PrintTripDetail(IEnumerable<object> tripDetail)
>     {
>         foreach (var item in tripDetail)
>         {
>             Console.WriteLine(item);
>         }
>         Console.WriteLine("---");
>     }
>     static void Main(string[] args)
>     {
>         var tripDirector = new TripDirector();
>         Console.WriteLine("SUP 象鼻岩行程:");
>         var trip = tripDirector.CreateTrip(new SupTripBuilder(), "深澳象鼻岩");
>         var tripDetail = trip.GetDetail();
>         PrintTripDetail(tripDetail);
> 
>         Console.WriteLine("SUP 龜山島行程:");
>         var trip1 = tripDirector.CreateTrip(new SupTripBuilder(), "龜山島牛奶湖");
>         var tripDetail1 = trip1.GetDetail();
>         PrintTripDetail(tripDetail1);
> 
>         Console.WriteLine("獨木舟 東澳行程:");
>         var trip2 = tripDirector.CreateTrip(new SupTripBuilder(), "東澳海蝕洞");
>         var tripDetail2 = trip2.GetDetail();
>         PrintTripDetail(tripDetail2);
>     }
> }
> ```
>
> 
>
> Console output:
>
> ```
> SUP 象鼻岩行程:
> 【7/1-8/31 SUP 行程 85 折】
> 地點: 深澳象鼻岩
> 每人價格: NTD 3000
> 困難度: 3/5
> 時間: 5
> 每團人數限制: 10 人
> SUP 活動敘述: 立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結
> 合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水 
> 域，從事探索、激流及救生等多方面的活動。
> ---
> SUP 龜山島行程:
> 【7/1-8/31 SUP 行程 85 折】
> 地點: 龜山島牛奶湖
> 每人價格: NTD 3000
> 困難度: 3/5
> 時間: 5
> 每團人數限制: 10 人
> SUP 活動敘述: 立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結
> 合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水 
> 域，從事探索、激流及救生等多方面的活動。
> ---
> 獨木舟 東澳行程:
> 【7/1-8/31 SUP 行程 85 折】
> 地點: 東澳海蝕洞
> 每人價格: NTD 3000
> 困難度: 3/5
> 時間: 5
> 每團人數限制: 10 人
> SUP 活動敘述: 立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結
> 合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水 
> 域，從事探索、激流及救生等多方面的活動。
> ---
> ```
>
> ## 方式二：透*過靜態內部類方式實現建造者模式*
>
> 上面的假設是 SUP 行程價格、困難度、時間……都相同，僅地點有差異，所以 `TripDirector` 的參數只有一個。
>
> 然而今天可能行程一多，針對同樣都是 SUP（或同樣都是獨木舟）的行程，不同地點我們希望也做不同價格、困難度、時間……的設定。
>
> 這時候用第二種實現方法，會更加的彈性。
>
> > 這種方式使用更加靈活，更符合定義。內部有複雜物件的預設實現，使用時可以根據使用者需求自由定義更改內容，並且無需改變具體的構造方式。就可以生產出不同複雜產品
>
> 在我們的例子，就是讓 `Trip` 本身可以直接設定傳入的參數，並且每個 Builder 方法都回傳 `this`，以達到 chaining 的效果。
>
> **Builder**: 創建抽象建造者 `ITripBuilder`，僅宣告打造水上活動行程的步驟，注意回傳型別也都是 `ITripBuilder`
>
> ```
> public interface ITripBuilder
> {
>     ITripBuilder SetDestination(string destination);
>     ITripBuilder SetPrice(int price);
>     ITripBuilder SetDifficulty(int difficulty);
>     ITripBuilder SetDurationHours(int hours);
>     ITripBuilder SetMaxParticipants(int maxParticipants);
>     ITripBuilder SetDescription();
>     ITripBuilder SetSalesContext();
>     Trip Build();
> }
> ```
>
> **Concrete Builder**: 建立實體建造者 `KayakTripBuilder`, `SupTripBuilder`，實現 `ITripBuilder` 的方法
>
> 這邊以 `SupTripBuilder`舉例，我們把 SetDestination, SetPrice… 等步驟都交給 `Trip` 這個最後要完工的物件去進行：
>
> ```C#
> public class SupTripBuilder : ITripBuilder
> {
>     private Trip _trip;
>     private string _description = "立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水域，從事探索、激流及救生等多方面的活動。";
>     private string _salesContext = "7/1-8/31 SUP 行程 85 折";
>     public SupTripBuilder()
>     {
>         _trip = new Trip();
>     }
>     public ITripBuilder SetDestination(string destination)
>     {
>         _trip.SetDestination(destination);
>         return this;
>     }
>     public ITripBuilder SetPrice(int price)
>     {
>         _trip.SetPrice(price);
>         return this;
>     }
>     public ITripBuilder SetDifficulty(int difficulty)
>     {
>         _trip.SetDifficulty(difficulty);
>         return this;
>     }
>     public ITripBuilder SetDurationHours(int hours)
>     {
>         _trip.SetDurationHours(hours);
>         return this;
>     }
>     public ITripBuilder SetMaxParticipants(int maxParticipants)
>     {
>         _trip.SetMaxParticipants(maxParticipants);
>         return this;
>     }
>     public ITripBuilder SetDescription()
>     {
>         _trip.SetDescription(_description);
>         return this;
>     }
>     public ITripBuilder SetSalesContext()
>     {
>         _trip.SetSalesContext(_salesContext);
>         return this;
>     }
>     public Trip Build()
>     {
>         return _trip;
>     }
> }
> ```
>
> 
>
> **Product**: 建立一個物件 `Trip` 收整行程內容
>
> 和第一個方法不同，這個方法直接在物件中做 SetDestination, SetPrice… 等步驟。最後一樣全部整理成字串丟到一個 List 裡面：
>
> ```C#
> public class Trip
> {
>     private string _destination;
>     private int _price;
>     private int _difficulty;
>     private int _hours;
>     private int _maxParticipants;
>     private string _description;
>     private string _salesContext;
>     private IList<string> _tripDetail = new List<string>();
>     public IEnumerable<string> GetDetail()
>     {
>         foreach (var item in _tripDetail)
>         {
>             yield return item;
>         }
>     }
>     // Getters to be added
>     public void SetDestination(string destination)
>     {
>         _destination = destination;
>         _tripDetail.Add($"地點: {destination}");
>     }
>     public void SetPrice(int price)
>     {
>         _price = price;
>         _tripDetail.Add($"每人價格: NTD {price}");
>     }
>     public void SetDifficulty(int difficulty)
>     {
>         _difficulty = difficulty;
>         _tripDetail.Add($"困難度: {difficulty}/5");
>     }
>     public void SetDurationHours(int hours)
>     {
>         _hours = hours;
>         _tripDetail.Add($"時間: {hours} 小時");
>     }
>     public void SetMaxParticipants(int maxParticipants)
>     {
>         _maxParticipants = maxParticipants;
>         _tripDetail.Add($"每團人數限制: {maxParticipants} 人");
>     }
>     public void SetDescription(string description)
>     {
>         _description = description;
>         _tripDetail.Add($"SUP 活動敘述: {_description}");
>     }
>     public void SetSalesContext(string salesContext)
>     {
>         _salesContext = salesContext;
>         _tripDetail.Add($"【{_salesContext}】");
>     }
> }
> ```
>
> 
>
> **Client**: 建構完以上物件，我們可以在`Program.cs`呼叫 Builder 並用 chaining 的方式打造行程
>
> 用這個方法可以更彈性就是因為順序（先設定地點還先設定價格？）、屬性多寡（要不要這個行程暫不設定行銷內容跟活動敘述？）都可以直接在建造 Trip 時決定。並且因為 `TripBuilder` 的方法都是回傳該 `TripBuilder` (this)，所以可以一路 chain 下去：
>
> ```C#
> class Program
> {
>     static void PrintTripDetail(Trip trip)
>     {
>         var tripDetail = trip.GetDetail();
>         foreach (var item in tripDetail)
>         {
>             Console.WriteLine(item);
>         }
>         Console.WriteLine("---");
>     }
>     static void Main(string[] args)
>     {
>         Console.WriteLine("Trip 1. SUP 象鼻岩行程");
>         Trip trip = new SupTripBuilder()
>                     .SetSalesContext()
>                     .SetDestination("深澳象鼻岩")
>                     .SetPrice(2000)
>                     .SetDifficulty(2)
>                     .SetDurationHours(4)
>                     .SetMaxParticipants(15)
>                     .SetDescription()
>                     .Build();
>         PrintTripDetail(trip);
> 
>         Console.WriteLine("Trip 2. SUP 龜山島行程");
>         Trip trip1 = new SupTripBuilder()
>                     .SetSalesContext()
>                     .SetDestination("龜山島牛奶湖")
>                     .SetPrice(5000)
>                     .SetDifficulty(4)
>                     .SetDurationHours(6)
>                     .SetMaxParticipants(10)
>                     .SetDescription()
>                     .Build();
>         PrintTripDetail(trip1);
> 
>         Console.WriteLine("Trip 3. 獨木舟 小琉球行程");
>         Trip trip2 = new KayakTripBuilder()
>                     .SetSalesContext()
>                     .SetDestination("小琉球")
>                     .SetPrice(3500)
>                     .SetDifficulty(2)
>                     .SetDurationHours(5)
>                     .SetMaxParticipants(20)
>                     .SetDescription()
>                     .Build();
>         PrintTripDetail(trip2);
> 
>         Console.WriteLine("Trip 4. 獨木舟 東澳行程");
>         Trip trip3 = new KayakTripBuilder()
>                     .SetSalesContext()
>                     .SetDestination("東澳海蝕洞")
>                     .SetPrice(1500)
>                     .SetDifficulty(3)
>                     .SetDurationHours(4)
>                     .Build();
>         PrintTripDetail(trip3);
> 
>         Console.WriteLine("Trip 5. 獨木舟 龍洞行程");
>         Trip trip4 = new KayakTripBuilder()
>                     .SetSalesContext()
>                     .SetDestination("龍洞")
>                     .SetPrice(1500)
>                     .Build();
>         PrintTripDetail(trip4);
>     }
> }
> ```
>
> 
>
> Console output:
>
> ```
> Trip 1. SUP 象鼻岩行程
> 【7/1-8/31 SUP 行程 85 折】
> 地點: 深澳象鼻岩
> 每人價格: NTD 2000
> 困難度: 2/5
> 時間: 4 小時
> 每團人數限制: 15 人
> SUP 活動敘述: 立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結
> 合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水 
> 域，從事探索、激流及救生等多方面的活動。
> ---
> Trip 2. SUP 龜山島行程
> 【7/1-8/31 SUP 行程 85 折】
> 地點: 龜山島牛奶湖
> 每人價格: NTD 5000
> 困難度: 4/5
> 時間: 6 小時
> 每團人數限制: 10 人
> SUP 活動敘述: 立式槳板運動（英語：Stand Up Paddle, SUP)，也俗稱「槳板」， 是起源於夏威夷的一種運動，由衝浪與傳統的手划槳板（Paddleboard）結
> 合而成。活動器材係由槳板（類似大型衝浪板）加上一支高於身高的單槳所組成。運用於衝浪時又稱立式單槳衝浪（簡稱立槳衝浪），也可在湖泊及河流等水 
> 域，從事探索、激流及救生等多方面的活動。
> ---
> Trip 3. 獨木舟 小琉球行程
> 【獨木舟振興券優惠套餐 開跑囉】
> 地點: 小琉球
> 每人價格: NTD 3500
> 困難度: 2/5
> 時間: 5 小時
> 每團人數限制: 20 人
> SUP 活動敘述: 獨木舟是一種用單根樹幹挖成的划艇，需要藉助槳驅動。獨木舟的優點在於由一根樹幹製成，製作簡單，不易有漏水，散架的風險。它可以說
> 是人類最古老的水域交通工具之一。
> ---
> Trip 4. 獨木舟 東澳行程
> 【獨木舟振興券優惠套餐 開跑囉】
> 地點: 東澳海蝕洞
> 每人價格: NTD 1500
> 困難度: 3/5
> 時間: 4 小時
> ---
> Trip 5. 獨木舟 龍洞行程
> 【獨木舟振興券優惠套餐 開跑囉】
> 地點: 龍洞
> 每人價格: NTD 1500
> ---
> ```

> ### 建立者模式 (Builder Pattern)
>
> ​    今天你要為一個遊樂園設計程式，讓客人可以自由選擇旅館及各種門票、餐廳訂位、或是其他特殊活動來建立自己的假期計畫，但你可能會遇到一些問題：每個客人的**假期計畫都不一樣**，如旅行天數不同，或是想住的旅館會不同，而且為了要滿足客製化假期內容的要求，你的設計可能會有**很多建構子**才能滿足需求：
>
> 
>
> ```
> public class Vocation {
> 
>     // 客人只要求天數，其他隨便排
>     public Vocation(Date begin, Date end)
>     {
>         // ...
>     }
> 
>     // 客人要求天數，還指定飯店
>     public Vocation(Date begin, Date end, Hotel hotel)
>     {
>         // ...
>     }
> 
>     // 客人指定天數，飯店，還有餐廳
>     public Vocation(Date begin, Date end, Hotel hotel,
>                     Restaurant restaurant)
>     {
>         // ...
>     }
> 
>     // 可能還有其他可以讓客人選擇的建構子
> }
> ```
>
> ​    這樣對於客戶而言，他需要先知道全部的建構子有哪些，才能知道要選用哪個來建立適合自己的假期物件。但有時候我們提供的的建構子可能還是不符合客戶的要求，因此我們需要有一個有彈性的結構，可以把**整個假期物件的產生過程封裝起來**，而且客戶還可以不影響物件建立的步驟，建立出自己想要的假期物件，這就是這次要介紹的建立者模式！
>
> ​    我們先來看看建立者模式的定義及類別圖吧
>
> **將一個複雜對象的建構和表現分離，使得同樣的建構過程可以產生出不同的表現**
> **Separate the construction of a complex object from its representation so that the same construction process can create different representations.**
>
> [![img](https://2.bp.blogspot.com/-hqQjkCLqtOw/WHH5uNE944I/AAAAAAAAH_4/gTbxmZ4FeTkKKb3LCi2XYsmgXkzqnCxRQCLcB/s640/builder%2Bpattern.png)](https://2.bp.blogspot.com/-hqQjkCLqtOw/WHH5uNE944I/AAAAAAAAH_4/gTbxmZ4FeTkKKb3LCi2XYsmgXkzqnCxRQCLcB/s1600/builder%2Bpattern.png)
>
> ​    從類別圖及定義可以知道，Builder 就是定義物件產生的一個介面，通常會定義一系列的方法，通常是 setXXX / addXXX，可以讓使用者來更改想要的物件行為。**物件建構的過程是隱藏的**，使用者是碰不到的。因為上述兩點，才符合定義說的相同建構過程可以有不同表現。Director 就是實際上調用這個介面來產生物件的中介者，主要的工作是指導 Builder 產生出特定的物件，client 要自己調用 Builder 來取得物件當然也可以。ConcreteBuilder 就是 Builder 介面的實作，可以產生真正產品的類別。
>
> ​    接著就來看看上述的範例使用了建立者模式的程式碼吧
>
> 
>
> ```
> // Builder 介面
> // 實務上 Builder 會搭配 Fluent interface
> // 讓程式更有可讀性
> public interface VocationBuilder {
> 
>     // 指定假期開始時間
>     public VocationBuilder setBeginDate(String date);
> 
>     // 指定假期結束時間
>     public VocationBuilder setEndDate(String date);
> 
>     // 指定住哪間飯店
>     public VocationBuilder setHotel(Hotel hotel);
> 
>     // 指定吃哪間餐廳
>     public VocationBuilder setRestaurant(Restaurant restaurant);
> 
>     // 指定要玩哪些景點的門票
>     public VocationBuilder setTicket(List tickets);
> 
>     // 要提供一個方法讓使用者能取得假期物件
>     public Vocation create();
> }
> 
> // 能產生三天假期規劃的 Builder, 實作上面的 Builder
> // 這裡只是範例, 實際上可以依需求有不同實作
> public class ThreeDayVocationBuilder implements VocationBuilder {
> 
>     // 保留使用者想要的客製化,
>     // 就是使用者想要的 "表現"
>     private String mBeginDate;
>     private String mEndDate;
>     private Hotel mHotel;
>     private Restaurant mRestaurent;
>     private List mTickets;
> 
>     @Override
>     public VocationBuilder setBeginDate(String date);
>     {
>         mBeginDate = date;
>         return this;
>     }
> 
>     @Override
>     public VocationBuilder setEndDate(String date)
>     {
>         // 這邊可以加上些判斷, 確認使用者傳入的參數
>         // 確實是三天後，或是自動幫使用者調整
>         mEndDate = date;
>         return this;
>     }
> 
>     @Override
>     public VocationBuilder setHotel(Hotel hotel)
>     {
>         mHotel = hotel;
>         return this;
>     }
> 
>     @Override
>     public VocationBuilder setRestaurant(Restaurant restaurant)
>     {
>         mRestaurant = restaurant;
>         return this;
>     }
> 
>     @Override
>     public VocationBuilder setTicket(List tickets);
>     {
>         mTickets = tickets;
>         return this;
>     }
> 
>     @Override
>     public Vocation create()
>     {
>         // 回傳真正的假期物件給使用者
>         // 省略了 Vocation 的程式碼
>         // 這邊只是範例, 實務上可能物件的產生很繁瑣
>         return new Vocation(mBeginDate, mEndDate, mHotel,
>                             mRestaurent, mTickets);
>     }
> }
> 
> // 使用上可以這樣使用 Builder
> VocationBuilder builder = new ThreeDayVocationBuilder();
> builder.setBeginDate("2018/01/01");
> builder.setEndDate("2018/01/03");
> builder.setHotel(someHotel); // 懶得寫 hotel 物件
> Vocation vocation = builder.build() // 這樣就能取得 Vocation
>     
> // 也能搭配使用 Fluent interface
> Vocation vocation = new ThreeDayVocationBuilder()
>                         .setBeginDate("2018/01/01")
>                         .setEndDate("2018/01/03")
>                         .setHotel(someHotel)
>                         .create()
> ```
>
> ​    看到這裡，可能有些人會覺得，同樣都是建立物件，那直接 [Abstract Factory](http://corrupt003-design-pattern.blogspot.tw/2016/06/abstract-factory-pattern.html) 來建立物件不就好了？這裡要注意的是， Builder 著重在**隱藏****複雜****物件生成的步驟**，**且生成的物件(通常是複雜物件) 彼此會有「部份」（Part of）的概念**。而 Abstract Factory 則是著重在**管理有關聯性的物件，但這些物件不會有****「部份」（Part of）的概念**。實務上這個模式還滿常被使用到的，如 JAVA SDK 裡的 [StringBuilder](http://docs.oracle.com/javase/8/docs/api/java/lang/StringBuilder.html)，[StringBuffer](http://docs.oracle.com/javase/8/docs/api/java/lang/StringBuffer.html)，以及 Android SDK 裡的 [AlertDialog.Builder](https://developer.android.com/reference/android/app/AlertDialog.Builder.html)，有興趣的人可以參考看看。
>
> ​    最後來說說總結吧，建立者模式的特點就是將複雜物件的產生過程隱藏起來，使用者無法碰到，且允許物件用多個步驟建立 (跟 [Factory Method](http://corrupt003-design-pattern.blogspot.tw/2016/05/factory-method-pattern.html) 只有一個步驟不同)，因為它的特性，因此經常用來建立合成結構。但對於使用者而言，要是不知道有哪些 setXXX() 方法可以用，也無法建立出想要的物件，這是要注意的地方。



## 實作之前

不知不覺筆記的前兩個部份就變成僅節錄網路或書籍資料的內容，心得部分都移到實作了，這次把這個部分獨立出來試試。

### Builder Pattern 與其他 Design Pattern 的比較

關於這個`Builder Pattern`，第一眼得到的印象是**似曾相識**，目的是生成物件這點讓我聯想到`Factory Pattern`，將整體拆成細部再一一組起的概念類似於`Decorator Pattern`，把這些設計模式弄混顯然有礙於我學習並理解這次的主角`Builder Pattern`。

所以第一要務就是比較並釐清這些設計模式的差別所在

#### VS Factory Pattern

`Factory Pattern` 著重於"在執行階段生成物件"，生成的物件根據傳入的參數來自相應的類別(但會繼承自相同的父類別或介面)

`Builder Pattern` 則是在**同一個類別**中減少建構式多載的情形

#### VS Decorator Pattern

同樣將物件拆分，`Decorator Pattern`使用多個Decorator類別去影響目標的屬性。在`Builder Pattern`中，我們則是透過Builder class 去一步步完善Product物件。

## 實作

既然是Builder Pattern那就以蓋房子為例吧，簡單的選項如幾扇門、幾扇窗、幾層樓、有沒有後院等等。

### 實作一

先用標準結構實作一次

Class Diagram

![](https://i.imgur.com/EwiBd9K.png)

Building (就是Product)

```C#
    public class Building
    {
        private List<string> describes = new List<string>();
        public Building() => addDescribe("房子的詳細規格:");
        public void addDescribe(string describe) => describes.Add(describe);
        public IEnumerable<string> GetDetail()
        {
            foreach (string des in describes)
                yield return des;
        }
    }
```

IBuilder

```C#
    public interface IBuilder
    {
        void reset();
        void setWindowNum(int num);
        void setDoorNum(int num);
        void setFloorNum(int num);
        void setBackYard();
        Building GetBuilding();
    }
```

Builder

```C#
    public class Builder : IBuilder
    {
        private Building building;
        public Builder() => reset();

        public Building GetBuilding() => building;

        public void reset() => building = new Building();

        public void setBackYard() => building.addDescribe("含後院");

        public void setDoorNum(int num) => building.addDescribe(num.ToString()+"扇門");
        public void setFloorNum(int num) => building.addDescribe(num.ToString() + "層樓");

        public void setWindowNum(int num) => building.addDescribe(num.ToString() + "扇窗");
    }
```

Director

```C#
    public class Director
    {
        public Building GetOrdinaryBuilding()
        {
            Builder builder = new Builder();
            builder.setFloorNum(3);
            builder.setWindowNum(8);
            builder.setDoorNum(3);
            builder.setBackYard();
            return builder.GetBuilding();
        }
    }
```



測試程式

```C#
        private void builderTest()
        {
            Director director = new Director();
            Building building = director.GetOrdinaryBuilding();
            IEnumerable<string> buildingDetail = building.GetDetail();
            foreach (string detail in buildingDetail)
                Console.WriteLine(detail);
        }
```

結果

> 房子的詳細規格:
> 3層樓
> 8扇窗
> 3扇門
> 含後院



### 實作二

用另一種架構實作看看

Class Diagram

![](https://i.imgur.com/tIeTDdR.png)

Building

```C#
    public class Building
    {
        public int? DoorNum { get; set; }
        public int? WindowNum { get; set; }
        public int? FloorNum { get; set; }
        public bool HasBackYard { get; set; }
        public Building() => HasBackYard = false;
        public List<string> Describe()
        {
            List<string> res = new List<string>();
            res.Add("房子的詳細規格:");
            if (DoorNum != null) { res.Add(DoorNum.ToString() + "扇門"); }
            if (WindowNum != null) { res.Add(WindowNum.ToString() + "扇窗"); }
            if (FloorNum != null) { res.Add(FloorNum.ToString() + "層樓"); }
            if (HasBackYard) { res.Add("含後院"); }
            return res;
        }
    }
```

`?`是為了做null判斷 [Reference](https://medium.com/@mybaseball52/handling-null-in-csharp-60eabafe8e22)

IBuilder

```C#
    public interface IBuilder
    {
        IBuilder reset();
        IBuilder setWindowNum(int num);
        IBuilder setDoorNum(int num);
        IBuilder setFloorNum(int num);
        IBuilder setBackYard();
        Building GetBuilding();
    }
```

Builder

```C#
    public class Builder : IBuilder
    {
        private Building building;
        public Builder() => building = new Building();
        public Building GetBuilding() => building;

        public IBuilder reset()
        {
            building = new Building();
            return this;
        }

        public IBuilder setBackYard()
        {
            building.HasBackYard = true;
            return this;
        }

        public IBuilder setDoorNum(int num)
        {
            building.DoorNum = num;
            return this;
        }

        public IBuilder setFloorNum(int num)
        {
            building.FloorNum = num;
            return this;
        }

        public IBuilder setWindowNum(int num)
        {
            building.WindowNum = num;
            return this;
        }
    }
```



測試程式

```C#
        private void builderTest2()
        {
            Builder2.Building building = new Builder2.Builder()
                .setDoorNum(3)
                .setFloorNum(3)
                .setWindowNum(8)
                .setBackYard()
                .GetBuilding();
            foreach (string s in building.Describe())
                Console.WriteLine(s);
        }
```

結果

> 房子的詳細規格:
> 3扇門
> 8扇窗
> 3層樓
> 含後院