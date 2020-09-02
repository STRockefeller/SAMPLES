# LINQ

[TOC]

## Reference

https://ithelp.ithome.com.tw/users/20009278/ironman/450?page=2

與SQLite連結的方法，參考LinqToSQLite.md

## 查詢運算式 Query syntax

MSDN說明

> 「查詢運算式」(Query Expression) 是以查詢語法表示的查詢。 查詢運算式是第一級的語言建構， 它就像其他任何運算式一樣，而且可以在 C# 運算式適用的任何內容中使用。 查詢運算式是由一組子句組成，而這些子句都是以類似 SQL 或 XQuery 的宣告式語法撰寫而成。 每個子句也會包含一個或多個 C# 運算式，而這些運算式可能本身就是查詢運算式，或是包含查詢運算式。
> 查詢運算式必須以 from 子句開頭，並以 select 或 group 子句結尾。 在第一個 from 子句和最後的 select 或 group 子句中間，也可以包含下列一個或多個子句：where、orderby、join、let，甚至是額外的 from 子句。 您也可以使用 into 關鍵字，讓 join 或 group 子句的結果變成同一個查詢運算式中其他查詢子句的來源。

> ![Img00](http://ithelp.ithome.com.tw/upload/images/20121008/201210081855285072b120b76c7_resize.png)

> ```C#
> IEnumerable<T> query-expression-identifier = 
>     from identifier in expression
>     let identifier = expression
>     where boolean-expression
>     join identifier in expression 
>         on expression equals expression 
>         into identifier
>     orderby ordering-clause ascending|descending
>     group expression by expression 
>         into identifier
>     select expression 
>         into identifier
> ```

最基礎的用法就是把SQL語法的Select ... From ... Where倒裝成from where select。

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var query =
                    from c in db.Sophie
                    where c.Type1 == "水"
                    select c;
                var qq = query.ToList();
            }
```

## 方法架構查詢 Method syntax

與Query Syntax有很大的部分重疊

> 1. 雖然方法語法（Method syntax）與查詢語法（Query syntax）之間並沒有語意上的差異，但是方法語法中可用的標準查詢運算子，有部分並未包含在查詢語法中，例如：Distinct、Take、Max 等等，若要使用這些運算子，就必須使用方法架構查詢的形式來撰寫。
> 2. System.Linq 命名空間（Namespace）中標準查詢運算子的相關參考文件，幾乎都是使用方法語法的格式做說明，因此，即使是撰寫 LINQ 查詢運算式，還是得了解並熟悉如何在兩種撰寫形式中，應用這些運算子。

以下為MSDN範例

````C#
class QueryVMethodSyntax
{
    static void Main()
    {
        int[] numbers = { 5, 10, 8, 3, 6, 12};

        //Query syntax:
        IEnumerable<int> numQuery1 =
            from num in numbers
            where num % 2 == 0
            orderby num
            select num;

        //Method syntax:
        IEnumerable<int> numQuery2 = numbers.Where(num => num % 2 == 0).OrderBy(n => n);

        foreach (int i in numQuery1)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine(System.Environment.NewLine);
        foreach (int i in numQuery2)
        {
            Console.Write(i + " ");
        }

        // Keep the console open in debug mode.
        Console.WriteLine(System.Environment.NewLine);
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}
/*
    Output:
    6 8 10 12
    6 8 10 12
 */
````

小練習

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                //query syntax
                var query =
                    from c in db.Sophie
                    where c.Type1 == "水"
                    select c;

                //method syntax
                var ms = db.Sophie.Where(c => c.Type1 == "水");
                
                foreach (Sophie s in ms)
                    Console.WriteLine(s.Name);
            }
```

另外上例中的`query`以及`ms`都是`IQueryable<Sophie>`物件(query應該要是IEnumerable才對?)

## 延後執行

依據查詢運算子回傳的是單一值或序列值會有執行時間的差異

### 回傳序列值的情況

```C#
            int[] arr = new int[] {1,2,3,4,5};
            Console.WriteLine("Original Value");
            foreach (int i in arr)
                Console.WriteLine(i);
            var qArr = from a in arr select a;
            Console.WriteLine("Fist Query");
            foreach (int i in qArr)
                Console.WriteLine(i);
            arr[3] = 0;
            Console.WriteLine("Second Query");
            foreach (int i in qArr)
                Console.WriteLine(i);
```

Console顯示如下

> Original Value
> 1
> 2
> 3
> 4
> 5
> Fist Query
> 1
> 2
> 3
> 4
> 5
> Second Query
> 1
> 2
> 3
> 0
> 5

在Second Query 的時候我並沒有重新撰寫查詢語法而是直接拿資料修改前的query來列印，但資料仍反映出了修改的內容

原因是在回傳**序列值**的情況下query會**延後執行**，在使用到該物件時才去進行查詢動作。

上例中在Debug模式下逐步執行也可以發現程式在執行到`foreach (int i in qArr)`的時候會往上跳到`var qArr = from a in arr select a;`執行

### 回傳單一值的情況

稍微修改上例程式

```C#
            int[] arr = new int[] { 1, 2, 3, 4, 5 };
            var qArr = (from a in arr select a).Sum();
            Console.WriteLine("Fist Query");
            Console.WriteLine(qArr);
            arr[3] = 0;
            Console.WriteLine("Second Query");
            Console.WriteLine(qArr);
            var qArr3 = (from a in arr select a).Sum();
            Console.WriteLine("Third Query");
            Console.WriteLine(qArr3);
```

回傳如下

> Fist Query
> 15
> Second Query
> 15
> Third Query
> 11

可以發現在回傳單一值的情況下查詢語句會**立即執行**

### 特殊情況

>  觸發立即執行作業還有一種情形，就是呼叫轉換運算子，也就是ToList<TSource>、ToArray<TSource>、ToLookup 和 ToDictionary。這些運算子雖然回傳的是值序列，但是因為有型別轉換的動作，所以會觸發立即執行作業。

## SelectMany

> SelectMany 運算子會先做和 Select 相同的事，也就是從資料來源序列中，根據我們所設定的條件，建立輸出序列，但是差異是，Select 運算子只要取得第一層的查詢結果就會放到輸出序列中，但是 SelectMany 若發現查詢結果回傳的是一個可以列舉的序列，則會再進一步把這個序列中的項目取出來，放到輸出序列中，也就是用 SelectMany 運算子，查詢出來的輸出序列之深度會比 Select 少一層（若 Select 結果是三層，用 SelectMany 會只有兩層）。

簡單來說就是SelectMany在Select得到列舉值後，若該資料可列舉則會再做一次Select

```C#
            List<string> testString = new List<string>();
            testString.AddRange(new string[] { "It", "Goes", "LaDiDa"});
            var querySelect = testString.Select(str => str);
            var quertSelectMany = testString.SelectMany(str => str);
```

`querySelect`會得到`"It", "Goes", "LaDiDa"`

`quertSelectMany`則是`'I','t','G','o'.....`因為字串本身可以再拆解成字元。

## OfType 運算子

根據型別篩選，屬於延後執行的方法

MSDN範例

```C#
System.Collections.ArrayList fruits = new System.Collections.ArrayList(4);
fruits.Add("Mango");
fruits.Add("Orange");
fruits.Add("Apple");
fruits.Add(3.0);
fruits.Add("Banana");

// Apply OfType() to the ArrayList.
IEnumerable<string> query1 = fruits.OfType<string>();

Console.WriteLine("Elements of type 'string' are:");
foreach (string fruit in query1)
{
    Console.WriteLine(fruit);
}

// The following query shows that the standard query operators such as
// Where() can be applied to the ArrayList type after calling OfType().
IEnumerable<string> query2 =
    fruits.OfType<string>().Where(fruit => fruit.ToLower().Contains("n"));

Console.WriteLine("\nThe following strings contain 'n':");
foreach (string fruit in query2)
{
    Console.WriteLine(fruit);
}

// This code produces the following output:
//
// Elements of type 'string' are:
// Mango
// Orange
// Apple
// Banana
//
// The following strings contain 'n':
// Mango
// Orange
// Banana
```

## 資料排序

共四種，延後執行

* OrderBy
* OrderByDescending
* ThenBy
* ThenByDescending

以下都用這個簡易測試用類別作排序範例

```C#
        public class Order
        {
            public string oderName { get; set; }
            public int o1 { get; set; }
            public int o2 { get; set; }
            public Order(string oderName, int o1,int o2)
            {
                this.oderName = oderName;
                this.o1 = o1;
                this.o2 = o2;
            }
        }
```

### OrderBy

遞增排序

```C#
            List<Order> orders = new List<Order>();
            orders.Add(new Order("order1", 2, 3));
            orders.Add(new Order("order2", 7, 1));
            orders.Add(new Order("order3", 5, 0));
            orders.Add(new Order("order4", 12, 6));
            orders.Add(new Order("order5", 3, 8));

            Console.WriteLine("Order by o1");
            var query = orders.OrderBy(o => o.o1);
            foreach(Order o in query)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");

            Console.WriteLine("Order by o2");
            var query2 = from o in orders orderby o.o2 select o;
            foreach (Order o in query2)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");

            Console.WriteLine("Order by o1 again");
            orders[4].o1 = 88;
            foreach (Order o in query)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");
```

結果如下

> Order by o1
> order1 2 3
> order5 3 8
> order3 5 0
> order2 7 1
> order4 12 6
> Order by o2
> order3 5 0
> order2 7 1
> order1 2 3
> order4 12 6
> order5 3 8
> Order by o1 again
> order1 2 3
> order3 5 0
> order2 7 1
> order4 12 6
> order5 88 8

### OrderByDescending

遞減排序，注意query syntax將descending關鍵字放在後方

```C#
            List<Order> orders = new List<Order>();
            orders.Add(new Order("order1", 2, 3));
            orders.Add(new Order("order2", 7, 1));
            orders.Add(new Order("order3", 5, 0));
            orders.Add(new Order("order4", 12, 6));
            orders.Add(new Order("order5", 3, 8));

            Console.WriteLine("Order by o1");
            var query = orders.OrderByDescending(o => o.o1);
            foreach(Order o in query)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");

            Console.WriteLine("Order by o2");
            var query2 = from o in orders orderby o.o2 descending select o;
            foreach (Order o in query2)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");

            Console.WriteLine("Order by o1 again");
            orders[4].o1 = 88;
            foreach (Order o in query)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");
```

> Order by o1
> order4 12 6
> order2 7 1
> order3 5 0
> order5 3 8
> order1 2 3
> Order by o2
> order5 3 8
> order4 12 6
> order1 2 3
> order2 7 1
> order3 5 0
> Order by o1 again
> order5 88 8
> order4 12 6
> order2 7 1
> order3 5 0
> order1 2 3

### ThenBy / ThenByDescending

接續在OrderBy後的排序條件

```C#
            List<Order> orders = new List<Order>();
            orders.Add(new Order("order1", 2, 3));
            orders.Add(new Order("order2", 7, 1));
            orders.Add(new Order("order3", 5, 0));
            orders.Add(new Order("order4", 12, 6));
            orders.Add(new Order("order5", 3, 8));
            orders.Add(new Order("order5", 3, 6));
            orders.Add(new Order("order5", 3, 9));
            orders.Add(new Order("order5", 3, 7));

            Console.WriteLine("Order by o1 then by o2");
            var query = orders.OrderByDescending(o => o.o1).ThenBy(o => o.o2);
            foreach(Order o in query)
                Console.WriteLine($"{o.oderName} {o.o1} {o.o2}");
```

輸出

> Order by o1 then by o2
> order4 12 6
> order2 7 1
> order3 5 0
> order5 3 6
> order5 3 7
> order5 3 8
> order5 3 9
> order1 2 3



## 取得單一項目

> 在 LINQ 技術中，有設計一組專門用來取出來源序列中單一項目的運算子，也就是 First、Last、ElementAt、Single，它們同時也都搭配了一組取不到資料就輸出來源資料型別預設值的運算子：FirstOrDefault、LastOrDefault、ElementAtOrDefault、SingleOrDefault

這八個標準查詢運算子都**不支援查詢語法**（Query syntax），必須用方法語法（Method syntax）才能使用，而且這八個運算子都是**立即執行**（Immediately execution）查詢的運算子

後方有OrDefault的方法在找不到目標時會回傳該型別的預設物件

回到最早的範例

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.Where(c => c.Type1 == "水");
                foreach (Sophie s in ms)
                    Console.WriteLine(s.Name);
            }
```

Output

> 井戸水
> おいしい水
> 夜光水
> 妖しい液体
> プニの体液
> 虹プニの体液
> 竜の血晶
> 聖水
> ピュアウォーター
> 精霊の涙



以下修改

### First / FirstOrDefault

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.FirstOrDefault(c => c.Type1 == "水");
                Console.WriteLine(ms.Name);
            }
```

> 井戸水



### Last / LastOrDefault

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.FirstOrDefault(c => c.Type1 == "水");
                Console.WriteLine(ms.Name);
            }
```

不會有輸出，因為會跳Exception

原因為Last / LastOrDefault不支援 LinQ to SQL 以及 LINQ to Entities

不過用法是與First相同

### Single / SingleOrDefault

傳回序列的唯一一個項目，如果序列是空白，則為預設值，如果序列中有一個以上的項目，這個方法就會擲回例外狀況。

> Net 3.5 的 LINQ to Entities、LINQ to SQL 都不支援，但是 .Net 4.0 以上就支援了。

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.SingleOrDefault(item => item.Source1 == "日輪の雫");
                Console.WriteLine(ms.Name);
            }
```

> 真理の鍵



## 分頁方法

* Take
* Skip
* TakeWhile
* SkipWhile

取得前面數來多少個資料/省略前面數來多少個資料/取得資料直到無法滿足條件/省略資料直到無法滿足條件

後兩者都**不支援** LINQ to Entities 和 LINQ to SQL 



### Take

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.Where(c => c.Type1 == "水").Take(3);
                foreach (Sophie s in ms)
                    Console.WriteLine(s.Name);
            }
```

> 井戸水
> おいしい水
> 夜光水

### Skip

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.Where(c => c.Type1 == "水").Skip(3);
                foreach (Sophie s in ms)
                    Console.WriteLine(s.Name);
            }
```

> 妖しい液体
> プニの体液
> 虹プニの体液
> 竜の血晶
> 聖水
> ピュアウォーター
> 精霊の涙

### TakeWhile

```C#
            using (var db = new DataModels.AtelierLaDiDaA17JPSqliteDB())
            {
                var ms = db.Sophie.Where(c => c.Type1 == "水").TakeWhile(c => c.Attribute1 == "藍");
                foreach (Sophie s in ms)
                    Console.WriteLine(s.Name);
            }
```

跳Exception 因為不支援Linq to SQL

### SkipWhile

同上。



## 判斷資料是否存在或包含特定的項目

* Any
* All
* Contains 

這三個運算子都是**立即執行**查詢的運算子

### Any

> Any 運算子用來判斷序列中，是否至少有一個項目，或者是否有任何項目符合指定的條件。

> 第一個多載方法，不用傳入任何參數，可直接在來源序列上調用 Any 運算子，只要序列不是空的（至少有一個項目），就會回傳 true，反之回傳 false。請注意，若來源序列是 null，則執行時會拋回 ArgumentNullException 例外
>
> ```C#
> var list = new List<string>() {"ASUS","Acer","BenQ", "Toshiba","IBM","HP","Dell"}; 
> var emptyList = new List<string>() {}; 
> Console.WriteLine(list.Any()); 
> Console.WriteLine(emptyList.Any());
> /* 輸出：
> True 
> False
> */
> ```

> 第二個多載方法則是可以讓我們傳入一個委派 predicate，自行決定序列中的項目要符合什麼樣的邏輯才為真（true）
>
> ```C#
> var list = new List<string>() {"ASUS","Acer","BenQ", "Toshiba","IBM","HP","Dell"}; 
> Console.WriteLine(list.Any (l => l == "Acer")); 
> Console.WriteLine(list.Any (l => l.Length > 10)); 
> /* 輸出：
> True 
> False 
> */
> ```

### All

> All 運算子要求我們必須傳入一個委派 predicate，自行設定條件，然後來源序列中所有項目都必須符合才為真（true）
>
> ```C#
> var list = new List<string>() {"ASUS","Acer","BenQ", "Toshiba", "Dell"}; 
> Console.WriteLine(list.All(l => l.Length > 3)); 
> Console.WriteLine(list.All(l => l.Contains("A")));
> /* 輸出：
> True 
> False 
> */ 
> ```

> All 運算子有一個特性請大家特別注意，如果來源序列是空的（沒有任何項目），則不管設定的條件為何，回傳都是真（true）

### Contains

>  Contains 運算子，它用來判斷序列中是否包含指定的項目。請注意，.Net 3.5 的 LINQ to Entities、LINQ to SQL 完全不支援 Contains 運算子！但是在 .Net 4.0 版本，則支援第一個多載方法

方法簽章

```C#
public static bool Contains<TSource>( 
    this IEnumerable<TSource> source, 
    TSource value 
)
public static bool Contains<TSource>( 
    this IEnumerable<TSource> source, 
    TSource value, 
    IEqualityComparer<TSource> comparer 
)
```

> 兩個多載方法差別就是第一個方法使用型別（TSource）預設的相等比較子，第二個方法則是傳入自訂的 IEqualityComparer<TSource> 來判斷序列中是否包含指定項目

> ```C#
> var list = new List<string>() {"ASUS","Acer","BenQ", "Toshiba", "Dell"}; 
> Console.WriteLine(list.Contains("Acer")); 
> Console.WriteLine(list.Contains("A"));
> /* 輸出：
> True 
> False 
> */
> ```

值得注意的是，與`String.Contains`不同，`IEnumerable<TSource>.Contains `運算子檢查是兩個物件必須完全相同

第二個多載範例

> ```C#
> void Main() 
> { 
>     var list = new List<string>() {"ASUS","Acer","BenQ", "Toshiba", "Dell"}; 
>     var comparer = new IgnoreCaseSensitive(); 
>     Console.WriteLine(list.Contains("asus")); 
>     Console.WriteLine(list.Contains("asus", comparer)); 
> } 
> //自訂忽略大小寫的相等比較子 
> public class IgnoreCaseSensitive : IEqualityComparer<string> 
> { 
>     public bool Equals(string s1, string s2) 
>     { 
>         return (s1.ToUpper().Equals(s2.ToUpper())); 
>     } 
>     public int GetHashCode(string str) 
>     { 
>         return str.ToUpper().GetHashCode(); 
>     } 
> }
> /* 輸出：
> False 
> True
> */ 
> ```



## 設定方法

* Range
* Repeat
* Empty
* DefaultIfEmpty
* Distinct 

### Range

Range 運算子可以讓我們產生指定範圍內的「整數」序列

方法簽章

```C#
public static IEnumerable<int> Range( 
    int start, 
    int count 
)
```

引數為起始值和數量(非終值)

> ```C#
> var squares = Enumerable.Range(2, 4).Select(i => i * i); 
> foreach (var num in squares) 
> { 
>     Console.WriteLine(num); 
> } 
> Console.WriteLine(squares.GetType());
> 
> /* 輸出：
> 4 
> 9 
> 16 
> 25
> typeof (IEnumerable<Int32>)
> */
> ```

### Repeat

Repeat 運算子可以讓我們產生一個序列，包含指定數量的元素（或稱之為項目）

方法簽章

```C#
public static IEnumerable<TResult> Repeat<TResult>( 
    TResult element, 
    int count 
)
```

> ```C#
> var echoes = Enumerable.Repeat("Hello~ ", 3); 
> foreach (var s in echoes) 
> { 
>     Console.WriteLine(s); 
> } 
> Console.WriteLine(echoes.GetType());
> /* 輸出：
> Hello~ 
> Hello~ 
> Hello~ 
> typeof (IEnumerable<String>) 
> */
> ```

### Empty

Empty 運算子可以讓我們建立一個包含指定型別的空序列

方法簽章

```C#
public static IEnumerable<TResult> Empty<TResult>()
```

> ```C#
> IEnumerable<Decimal> pays = Enumerable.Empty<Decimal>(); 
> Console.WriteLine(pays.GetType());
> // 輸出：typeof (Decimal[])
> ```

### DefaultIfEmpty 

> DefaultIfEmpty 運算子是 IEnumerable<TSource> 的擴充方法，不須傳入任何參數，在來源序列上呼叫時，若來源序列是空的，則會回傳 TSource 型別預設值

方法簽章

```C#
public static IEnumerable<TSource> DefaultIfEmpty<TSource>(
    this IEnumerable<TSource> source
)
```

> ```C#
> IEnumerable<Decimal> pays = Enumerable.Empty<Decimal>();
> foreach (var e in pays)
> {
>     Console.WriteLine(e);
> }
> Console.WriteLine("----------------");
> Console.WriteLine(pays.DefaultIfEmpty());
> /* 輸出：
> ----------------
> 0
> */
> ```

### Distinct

依序把來源序列中的每一個項目，拿來和輸出序列中的項目做比較，若已存在於來源序列中，則排除，反之，則加入準備回傳的輸出序列裡。

在 C# 中，Distinct 運算子只能應用在方法架構查詢

方法簽章

```C#
public static IEnumerable<TSource> Distinct<TSource>( 
    this IEnumerable<TSource> source 
)
public static IEnumerable<TSource> Distinct<TSource>( 
    this IEnumerable<TSource> source, 
    IEqualityComparer<TSource> comparer 
) 
```

> 第一個多載方法不用傳入任何參數，系統會使用 TSource 型別預設的相等比較子，來判斷項目是否相等；第二個多載方法則是傳入自訂的相等比較子（IEqualityComparer<TSource> comparer），來比較項目是否相等

