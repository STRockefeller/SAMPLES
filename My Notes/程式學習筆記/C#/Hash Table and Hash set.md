# Hash Table and Hash set

[Reference:MSDN:HashSet](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.generic.hashset-1?view=netcore-3.1)

[Reference:MSDN:HashSet.UnionWith](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.generic.hashset-1.unionwith?view=netcore-3.1)

[Reference:ITRead:HashSet](https://www.itread01.com/content/1534098197.html)

[Reference:MSDN:Hashtable](https://docs.microsoft.com/zh-tw/dotnet/api/system.collections.hashtable?view=netcore-3.1)

[Reference:TechBridge:HashTable](https://blog.techbridge.cc/2017/01/21/simple-hash-table-intro/)

[Reference:StackOverFlow:HashSet](https://stackoverflow.com/questions/4558754/define-what-is-a-hashset)

[Reference:ITHelp:Hash](https://ithelp.ithome.com.tw/articles/10208884)

## Hash

> 很多人以為雜湊就是加密，但**雜湊不是加密！** **雜湊不是加密！** **雜湊不是加密！** 雜湊是因為他的特性很適合來做加密的運算，但真的不等同於加密！
>
> **雜湊 (Hash)**
>
> > 雜湊（英語：Hashing）是電腦科學中一種對資料的處理方法，通過某種特定的函式/演算法（稱為雜湊函式/演算法）將要檢索的項與用來檢索的索引（稱為雜湊，或者雜湊值）關聯起來，生成一種便於搜尋的資料結構（稱為雜湊表）。舊譯哈希（誤以為是人名而採用了音譯）。它也常用作一種資訊安全的實作方法，由一串資料中經過雜湊演算法（Hashing algorithms）計算出來的資料指紋（data fingerprint），經常用來識別檔案與資料是否有被竄改，以保證檔案與資料確實是由原創者所提供。
> >
> > 如今，雜湊演算法也被用來加密存在資料庫中的密碼（password）字串，由於雜湊演算法所計算出來的雜湊值（Hash Value）具有不可逆（無法逆向演算回原本的數值）的性質，因此可有效的保護密碼。
> > ---引自[〈維基百科〉](https://zh.wikipedia.org/wiki/散列)
>
> 這邊來統整一下。
>
> **雜湊函數 (Hash function)**
> 主要是將不定長度訊息的輸入，演算成固定長度雜湊值的輸出，且所計算出來的雜湊值必須符合兩個主要條件：
>
> - 由雜湊值是無法反推出原來的訊息
> - 雜湊值必須隨明文改變而改變。
>
> 舉例來說，雜湊函數就像一台果汁機，我們把蘋果香蕉你個芭樂 (資料) 都丟進去打一打、攪一攪，全部變得爛爛的很噁心對吧？！這時候出來的產物 (經過雜湊函數後的值)，是獨一無二的，沒有辦法反向組合成原來的水果 (資料)。倘若我們把蘋果改成紅龍果，出來的產物 (經過雜湊函數後的值) 就會跟著改變，變成桃紅色的，不再是原來的淡黃色。
>
> 承上述的例子，用紅龍果香蕉你個芭樂經過雜湊函數出來的顏色是桃紅色 (雜湊值)，那有沒有可能我用其他的水果也可以打出相同的顏色呢？但因為雜湊值的特性是無法反推的，所以如果真的打出相同的顏色的話，我們稱為**碰撞 (Collision)**。這就代表說這個雜湊值已經不安全，不再是獨一無二的了，需要更改雜湊函數。
>
> **雜湊表 (Hash table)**
> 在用雜湊函數運算出來的雜湊值，根據 **鍵 (key)** 來儲存在數據結構中。而存放這些記錄的數組就稱為 **雜湊表**。
>
> 舉例來說，我們有一筆資料用字典的形式表示，每個名字都搭配性別：
>
> ```
> {Joe:'M', Sue:'F', Dan:'M', Nell:'F', Ally:'F', Bob:'M'}
> ```
>
> 而我們將每個名字經過雜湊函數的運算。
>
> ```python
> (Key)                 (hash value)     (stored index)
> Joe  → (Hash function) →   4928   mod 5   =   3
> Sue  → (Hash function) →   7291   mod 5   =   1
> Dan  → (Hash function) →   1539   mod 5   =   4
> Nell → (Hash function) →   6276   mod 5   =   1
> Ally → (Hash function) →   9143   mod 5   =   3
> Bob  → (Hash function) →   5278   mod 5   =   3
> ```
>
> hash value 是獨一無二的，用 mod 5 來得到餘數並儲存才記憶體中。
>
> ```python
> 0： 
> 1： [ Sue, F ] → [ Nell, F ]
> 2： 
> 3： 
> 4： [ Joe, M ] → [ Ally, F ] → [ Bob, M ]
> 5： [ Dan, M ]
> ```
>
> 當我們要找資料的時候，例如 Ally 的性別，我們就把 Ally 丟到名為 Hash 的果汁機來得到 hash value，再用 mod 5 找到儲存在記憶體中的位置，但記憶體中的第一個位置並不是 Ally 是 Joe，我們根據 Joe 的鏈結找到下一個元素，直到找到答案。
>
> 另一個會讓人疑惑的點是為什麼用 mod 5？其實有很多種方式來選擇怎麼儲存在記憶體中，這邊只是範例。而會用 mod 5 是因為記憶體儲存空間假設為 5 。但儲存空間的大小也不是隨意設的，如果設得太大，資料沒那麼多，會造成空間浪費；若設得太小，會造成每一個空間的資料重疊，查找不易。所以設定空間大小也是一門學問。
>
> 總的來說，雜湊表其實很適合來存放不確定數量大小的資料，查找的時候也很快速、彈性。

## HashTable

基本上和Dictionary差不多(MSDN似乎更推薦使用Dictionary)，一個範例點到即止

```C#
class hashTableTest
{
   public void hashTable()
   {
            Hashtable ht = new Hashtable();
            ht.Add("boy", 24);
            ht.Add("girl", 25);            

            foreach(DictionaryEntry num in ht)
            {
                Console.Write(num.Key);
                Console.WriteLine(num.Value);
                Console.WriteLine();
            }
    }     
}

class Program
{
   static void Main(string[] args)
   {
      hashTableTest ht = new hashTableTest();
      ht.hashTable();
      Console.ReadLine();
   }
}
```

後面複習Data Structure

![](http://vhanda.in/blog/images/2012/07/19/normal-hash-table.png)

> Hash Table 是儲存 (key, value) 這種 mapping 關係的一種資料結構
>
> 為什麼他的時間複雜度這麼低呢? 舉例來說，如果我們有 n 個數字要儲存，一般大家常會用 array 來存。如果我們拿到另一個數字 A，要判斷這個數字 A 有沒有在 array 裡面，那我們勢必得跟 array 裡的元素一個個比較，時間複雜度是 O(n)。(先做過 sorting 的話，就可以用[二分搜尋法](http://blog.techbridge.cc/2016/09/24/binary-search-introduction/)比較快地找到，但還是需要 O(logn) 的時間複雜度)
>
> 但因為 hash function 的關係，如果先把 n 個數字儲存在 Hash Table 裡面，那如果要判斷這個數字 A 是不是已經被存在 Hash Table 裡面，只要先把這個數字丟進 hash function，就可以直接知道 A 對應到 Hash Table 中哪一格。所以其實是 hash function 幫我們省去了一個個比較的力氣。
>
> Hash Table 好不好用的關鍵跟這個神奇的 hash function 有很大的關係。讓我們想像一種情況，如果我們使用一個壞掉的 hash function，不管餵給這個 hash function 什麼內容他都會吐出同一個 index，那這樣的話就跟存一個 array 沒什麼兩樣。 搜尋的時間複雜度就會變成 O(n)。
>
> 以實用的角度出發，在簡單認識 Hash Table 的時候並不需要理解 hash function 要怎麼實作，但是我們要知道，hash function沒有完美的，有可能會把兩個不同的 key 指到同一個桶子，這就是所謂的 collision。當 collision 發生的時候，除了最直觀地增加 Hash Table 的桶子數，在每個桶子中用一個 linked list 來儲存 value、或是 linear probe 都是常用的方法，不過我們就先不細究下去。

## HashSet

以下兩段節錄自stackoverflow, itread對HashSet的介紹

> A `HashSet` holds a set of objects, but in a way that it allows you to easily and quickly determine whether an object is already in the set or not. It does so by internally managing an array and storing the object using an index which is calculated from the hashcode of the object.
>
> `HashSet` is an unordered collection containing unique elements. It has the standard collection operations Add, Remove, Contains, but since it uses a hash-based implementation, these operations are O(1). (As opposed to List for example, which is O(n) for Contains and Remove.) `HashSet` also provides standard set operations such as *union*, *intersection*, and *symmetric difference*.
>
> There are different implementations of Sets. Some make insertion and lookup operations super fast by hashing elements. However, that means that the order in which the elements were added is lost. Other implementations preserve the added order at the cost of slower running times.
>
> The `HashSet` class in C# goes for the first approach, thus **not** preserving the order of elements. It is much faster than a regular `List`. Some basic benchmarks showed that HashSet is decently faster when dealing with primary types (int, double, bool, etc.). It is a lot faster when working with class objects. So that point is that HashSet is fast.
>
> The only catch of `HashSet` is that there is no access by indices. To access elements you can either use an enumerator or use the built-in function to convert the `HashSet` into a `List` and iterate through that.



> 先來了解下`HashSet<T>`類，主要被設計用來存儲集合，做高性能**集運算**，例如兩個集合求交集、並集、差集等。從名稱可以看出，它是基於Hash的，可以簡單理解為沒有Value的Dictionary。
>
> `HashSet<T>`不能用索引訪問，不能存儲重復數據，元素T必須正確實現了`Equals`和`GetHashCode`。
>
> `HashSet<T>`的一些特性如下:
>
> 1. `HashSet<T>`中的值不能重復且沒有順序。
> 2. `HashSet<T>`的容量會按需自動添加。
>
> ## `HashSet<T>`的優勢和與`List<T>`的比較
>
> `HashSet<T>`最大的優勢是檢索的性能，簡單的說它的Contains方法的性能在大數據量時比`List<T>`好得多。曾經做過一個測試，將800W條int類型放在`List<int>`集合中，使用Contains判斷是否存在，速度巨慢，而放在`HashSet<int>`性能得到大幅提升。
>
> 在內部算法實現上，`HashSet<T>`的Contains方法復雜度是O(1)，`List<T>`的Contains方法復雜度是O(n)，後者數據量越大速度越慢，而`HashSet<T>`不受數據量的影響。
>
> 所以在集合的目的是為了檢索的情況下，我們應該使用`HashSet<T>`代替`List<T>`。比如一個存儲關鍵字的集合，運行的時候通過其Contains方法檢查輸入字符串是否關鍵字。
>
> 在3.5之前，想用哈希表來提高集合的查詢效率，只有Hashtable和Dictionary兩種選擇，而這兩種都是鍵-值方式的存儲。但有些時候，我們只需要其中一個值，例如一個Email集合，如果用泛型哈希表來存儲，往往要在Key和Value各保存一次，不可避免的要造成內存浪費。而HashSet只保存一個值，更加適合處理這種情況。
>
> 此外，HashSet的Add方法返回bool值，在添加數據時，如果發現集合中已經存在，則忽略這次操作，並返回false值。而Hashtable和Dictionary碰到重復添加的情況會直接拋出錯誤。
>
> 從使用上來看，HashSet和線性集合List更相似一些，但前者的查詢效率有著極大的優勢。假如，用戶註冊時輸入郵箱要檢查唯一性，而當前已註冊的郵箱數量達到10萬條，如果使用List進行查詢，需要遍歷一次列表，時間復雜度為O（n），而使用HashSet則不需要遍歷，通過哈希算法直接得到列表中是否已存在，時間復雜度為O（1），這是哈希表的查詢優勢。
>
> **和List的區別**
>
> HashSet是Set集合，它只實現了ICollection接口，在單獨元素訪問上，有很大的限制：
>
> 跟List相比，不能使用下標來訪問元素，如：list[1] 。
>
> 跟Dictionary相比，不能通過鍵值來訪問元素，例如：dic[key]，因為HashSet每條數據只保存一項，並不采用Key-Value的方式，換句話說，HashSet中的Key就是Value，假如已經知道了Key，也沒必要再查詢去獲取Value，需要做的只是檢查值是否已存在。
>
> 所以剩下的僅僅是開頭提到的集合操作，這是它的缺點，也是特點。
>
> **集合運算**
>
> **IntersectWith (IEnumerable other) （交集）**
>
> 
>
> **public \**void \*\*IntersectWithTest()\*\**\***
>
> {
>
> HashSet<**int> set1 = \**new HashSet<\*\*int>() { 1, 2, 3 };\*\**\***
>
> HashSet<**int> set2 = \**new HashSet<\*\*int>() { 2, 3, 4 };\*\**\***
>
> set1.IntersectWith(set2);
>
> **foreach (\**var item \*\*in set1)\*\**\***
>
> {
>
> Console.WriteLine(item);
>
> }
>
> 
>
> *//輸出：2,3*
>
> }
>
> **UnionWith (IEnumerable other) （並集）**
>
> public void UnionWithTest()
> {
> HashSet set1 = new HashSet() { 1, 2, 3 };
> HashSet set2 = new HashSet() { 2, 3, 4 };
>
> set1.UnionWith(set2);
>
> 
>
> **foreach (\**var item \*\*in set1)\*\**\***
>
> {
>
> Console.WriteLine(item);
>
> }
>
> 
>
> *//輸出：1,2,3,4*
>
> }
>
> **ExceptWith (IEnumerable other) （排除）**
>
> public void ExceptWithTest()
> {
> HashSet set1 = new HashSet() { 1, 2, 3 };
> HashSet set2 = new HashSet() { 2, 3, 4 };
>
> set1.ExceptWith(set2);
>
> 
>
> **foreach (\**var item \*\*in set1)\*\**\***
>
> {
>
> Console.WriteLine(item);
>
> }
>
> *//輸出：1*
>
> }



> In C#, HashSet is an unordered collection of unique elements. This collection is introduced in *.NET 3.5*. It supports the implementation of sets and uses the hash table for storage. This collection is of the generic type collection and it is defined under *System.Collections.Generic* namespace. It is generally used when we want to prevent duplicate elements from being placed in the collection. The performance of the HashSet is much better in comparison to the list.
>
> **Important Points:**
>
> - The HashSet class implements the *ICollection*, *IEnumerable*, *IReadOnlyCollection*, *ISet*, *IEnumerable*, *IDeserializationCallback*, and *ISerializable* interfaces.
> - In HashSet, the order of the element is not defined. You cannot sort the elements of HashSet.
> - In HashSet, the elements must be unique.
> - In HashSet, duplicate elements are not allowed.
> - Is provides many mathematical set operations, such as intersection, union, and difference.
> - The capacity of a HashSet is the number of elements it can hold.
> - A HashSet is a dynamic collection means the size of the HashSet is automatically increased when the new elements are added.
> - In HashSet, you can only store the same type of elements.

## 實作

先總結一下理解的部分，`HashSet`基本上算是`Hashtable`(或`Dictionary`)的簡化版本，不再以Key+Value的形式儲存資料，取而代之的只存一個資料，既是Key也是Value(所以不能重複)

先來看看分類的基準 Hash code，使用`GetHashCode`方法來獲取

```C#
            List<int> hashCode = new List<int>();
            hashCode.Add(12345.GetHashCode());
            hashCode.Add(0.12345.GetHashCode());
            hashCode.Add(-12345.GetHashCode());
            hashCode.Add('c'.GetHashCode());
            hashCode.Add("LaDiDa".GetHashCode());
            hashCode.Add(new int[3] { 1, 2, 3 }.GetHashCode());
            foreach (int h in hashCode)
                Console.WriteLine(h.ToString());
```

結果

> 12345
> 1863280663
> -12345
> 6488163
> -33055973
> 58225482

`int`型別看來是與其值相同，其他就看不太出規律了



再來試試傳說中的O(1)執行時間，與List做比較

測試程式如下

```C#
        private static void testHashSet(int[] source, int target)
        {
            HashSet<int> hset = new HashSet<int>(source);
            hset.Contains(target);
        }

        private static void testList(int[] source, int target)
        {
            List<int> list = new List<int>(source);
            list.Contains(target);
        }
        private static void hashRunTimeTest(int[] source, int target)
        {
            DateTime t1 = DateTime.Now;
            testHashSet(source, target);
            DateTime t2 = DateTime.Now;
            TimeSpan hashTime = t2 - t1;
            t1 = DateTime.Now;
            testList(source, target);
            t2 = DateTime.Now;
            TimeSpan listTime = t2 - t1;
            Console.WriteLine($"Array Length: {source.Length}\r\n" +
                $"Hash Set: {hashTime.ToString()}\r\n" +
                $"List: {listTime.ToString()}");
        }
```

測試條件

```C#
            var source = Enumerable.Range(1, 100).ToArray();
            int target = 100;
            hashRunTimeTest(source, target);
```

結果

> Array Length: 100
> Hash Set: 00:00:00.0576510
> List: 00:00:00.0031857

反而是List比較快，是因為建構式執行時間的關係嗎?

試試長一點的

測試條件

```C#
            var source = Enumerable.Range(1, 10000).ToArray();
            int target = 10000;
            hashRunTimeTest(source, target);
```

結果

> Array Length: 10000
> Hash Set: 00:00:00.0132252
> List: 00:00:00.0125367

變得相對接近了



測試條件

```C#
            var source = Enumerable.Range(1, 1000000).ToArray();
            int target = 1000000;
            hashRunTimeTest(source, target);
```

結果

> Array Length: 1000000
> Hash Set: 00:00:00.1081974
> List: 00:00:00.0122874

好吧看不懂了

修改測試程式，排除建構式執行時間的誤差

```C#
        private static void hashRunTimeTest(int[] source, int target)
        {
            HashSet<int> hset = new HashSet<int>(source);
            List<int> list = new List<int>(source);
            DateTime t1 = DateTime.Now;
            hset.Contains(target);
            DateTime t2 = DateTime.Now;
            TimeSpan hashTime = t2 - t1;
            t1 = DateTime.Now;
            list.Contains(target);
            t2 = DateTime.Now;
            TimeSpan listTime = t2 - t1;
            Console.WriteLine($"Array Length: {source.Length}\r\n" +
                $"Hash Set: {hashTime.ToString()}\r\n" +
                $"List: {listTime.ToString()}");
        }
```

再次測試

測試條件

```C#
            var source = Enumerable.Range(1, 1000000).ToArray();
            int target = 1000000;
            hashRunTimeTest(source, target);
```

結果

> Array Length: 1000000
> Hash Set: 00:00:00.0129811
> List: 00:00:00.0007912

還是List比較快，奇怪了

再試試更大的數據

> Array Length: 99999999
> Hash Set: 00:00:00.0076369
> List: 00:00:00.0380302

同樣條件，再一次

> Array Length: 99999999
> Hash Set: 00:00:00.0099490
> List: 00:00:00.0426985

可以看到，在大量數據的情況下Hash Set的檢索效率明顯比起List結構更加優異

---

來試試不同數據量的檢索時間

> Array Length: 1
> Hash Set: 00:00:00.0113654
> List: 00:00:00.0001390
> Array Length: 10
> Hash Set: 00:00:00.0000134
> List: 00:00:00.0000093
> Array Length: 100
> Hash Set: 00:00:00.0000048
> List: 00:00:00.0000031
> Array Length: 1000
> Hash Set: 00:00:00.0000010
> List: 00:00:00.0000181
> Array Length: 10000
> Hash Set: 00:00:00.0000010
> List: 00:00:00.0000091
> Array Length: 100000
> Hash Set: 00:00:00.0000030
> List: 00:00:00.0000356
> Array Length: 1000000
> Hash Set: 00:00:00.0000040
> List: 00:00:00.0003605
> Array Length: 10000000
> Hash Set: 00:00:00.0000125
> List: 00:00:00.0037719
> Array Length: 100000000
> Hash Set: 00:00:00.0000121
> List: 00:00:00.0420335

姑且不看第一筆不知道為啥花費時間特別多，從其他測試結果可以見得，List結構明顯隨著資料量越大檢索花費的時間也越多，反之，Hash Set就沒有這麼明顯的差別了