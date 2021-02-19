# Singleton Pattern

[Reference](https://skyyen999.gitbooks.io/-study-design-pattern-in-java/content/singleton.html)

## 目的

保證一個類別只會產生一個物件，而且要提供存取該物件的統一方法

> 許多時候整個系統只需要擁有一個的全局對象，這樣有利於我們協調系統整體的行為。比如在某個伺服器程序中，該伺服器的配置信息存放在一個文件中，這些配置數據由一個單例對象統一讀取，然後服務進程中的其他對象再通過這個單例對象獲取這些配置信息。這種方式簡化了在複雜環境下的配置管理。

## 概念

> 單例模式是一個簡單易懂的模式，下面的程式碼很簡單的就達到這樣的需求： 一開始我們就直接new出這個類別的實體物件，並且將constructor宣告為private， 這樣其他程式就無法再new出新物件，如此一來就能保證這個類別只會存在一個實體物件， 這種寫法因為一開始就已經建立物件，因此也稱為貪婪單例模式(Greed Singleton)。
>
> ```
> public class SingletonGreed {
>     // 一開始就建立物件，這樣只要一直回傳這個物件就是簡單的singleton
>     private static SingletonGreed instance  = new SingletonGreed();
> 
>     // private constructor，這樣其他物件就沒辦法直接用new來取得新的實體
>     private SingletonGreed(){}
> 
>     // 因為constructor已經private，所以需要另外提供方法讓其他程式調用這個類別
>     public static SingletonGreed getInstance(){
>         return instance;
>     }
> }
> ```
>
> 假如建立這個物件需要耗費很多資源，可是程式運行中不一定會需要它，我們希望只有在第一次getInstance被呼叫的時候才花費資源來建立物件，code就要修改一下
>
> ```
> public class Singleton {
>     private static Singleton instance;
> 
>     private Singleton(){
>         // 這裡面跑很了多code，建立物件需要花費很多資源
>     }
> 
>     public static Singleton getInstance(){
>         // 第一次被呼叫的時候再建立物件
>         if(instance == null){
>             instance = new Singleton();
>         } 
>         return instance;
>     }
> }
> ```
>
> 以上程式看起來沒問題，不過如果是多執行緒的情況下呼叫getInstance，可能第一個執行緒跑到instance = new Singleton()時，將時間讓給第二個 執行緒，因此第二個執行緒也執行了instance = new Singleton()，造成同時new了兩個新的物件。
>
> ```
> /**
>  * 單例模式測試
>  */
> public class SingletonTest extends Thread {
>     String myId;
>     public SingletonTest(String id) {
>         myId = id;
>     }
> 
>     // 執行緒執行的時候就去呼叫Singleton.getInstance()
>     public void run() {
>         Singleton singleton = Singleton.getInstance();
>         if(singleton != null){
>             // 用hashCode判斷前後兩次取到的Singleton物件是否為同一個
>             System.out.println(myId+"產生 Singleton:" + singleton.hashCode());               
>         }
>     }
> 
>     public static void main(String[] argv) {
>         /*
>         // 單執行緒的時候，s1與s2確實為同一個物件
>         Singleton s1  =  Singleton.getInstance();
>         Singleton s2  =  Singleton.getInstance();
>         System.out.println("s1:"+s1.hashCode() + " s2:" + s2.hashCode());
>         System.out.println();
>         */
> 
>         // 兩個執行緒同時執行
>         Thread t1 = new SingletonTest("執行緒T1"); // 產生Thread物件
>         Thread t2 = new SingletonTest("執行緒T2"); // 產生Thread物件
>         t1.start(); // 開始執行t1.run()
>         t2.start();
>     }
> }
> ```
>
> 為了解決這樣的問題，可以用synchronized修飾來解決這個問題，讓getInstance方法被調用的時候被lock住，就不會同時產生兩個物件。
>
> ```
> public class Singleton {
>     private static Singleton instance;
> 
> 
>     private Singleton(){
>         // 這裡面跑很了多code，建立物件需要花費很多資源
>     }
> 
>     // 多執行緒時使用synchronized保證Singleton一定是單一的 
>     public static synchronized Singleton getInstance(){
>         if(instance == null){
>             instance = new Singleton();
>         } 
>         return instance;
>     }
> }
> ```
>
> 上面這樣的寫法，synchronized整個方法會造成執行效能會變差，實際上需要lock住的只有創造物件的過程，也就是new Singleton這段程式碼而已， 因此可以將synchronized搬到getInstance方法內來加快程式的效能。
>
> ```
> public class Singleton {
>     private static Singleton instance;
> 
> 
>     private Singleton(){
>         // 這裡面跑很了多code，建立物件需要花費很多資源
>     }
> 
>     // 多執行緒時，當物件需要被建立時才使用synchronized保證Singleton一定是單一的 ，增加程式校能
>     public static Singleton getInstance(){
>         if(instance == null){
>             synchronized(Singleton.class){
>                 if(instance == null){
>                     instance = new Singleton();
>                 }    
>             }
>         } 
>         return instance;
>     }
> }
> ```
>
> 由這個簡單的單例模式可以看到，一樣的設計模式在不同的情況也是會有不同的變化。 設計模式不會是一段固定的程式碼，而是一種如何解決特定問題的概念。

---

> 單例模式 (Singleton Pattern)，以下程式碼以 C# 為例
>
> **說明：**
> 讓一個類別只能有一個實例(Instance)的方法。
> 產生單一實例的方式：
> 懶漢方式(Lazy initialization)：第一次使用時，才產生實例。
> 餓漢方式(Eager initialization)：class 載入時就產生實例，不管後面會不會用到。
>
> **範例：**
> 對同一個類別，分別取得 a、b 實例，而 a == b。
>
> 希望達成如下的效果
>
> ```C#
> static void Main(string[] args)
> {
>     // 產生第一個實例
>     Singleton a = Singleton.GetInstance();
>     Console.WriteLine("a.test = {0}", a.test); // a.test = 1
>     a.test = 10; // 將第一個實例的 test 值，改為 10
>     Console.WriteLine("a.test = {0}", a.test); // a.test = 10
>  
>     // 產生第二個實例
>     Singleton b = Singleton.GetInstance();
>     Console.WriteLine("b.test = {0}", b.test); // b.test = 10
>  
>     Console.WriteLine(b == a); // True, b 跟 a 是同一個實例 
>  
>     Console.Read();
> }
> ```
>
> 執行結果： 
>
> ```
> a.test = 1
> a.test = 10
> b.test = 10
> True
> ```
>
> 
> 實現重點在於，確保 class 回傳的實例，都是同一個，有以下兩種做法。
>
> **懶漢方式：**第一次使用時，才產生實例
>
> ```C#
> class Singleton
> {
>     // 多執行緒，lock 使用
>     private static readonly object thisLock = new object();
>  
>     // 將唯一實例設為 private static
>     private static Singleton instance;
>  
>     public int test = 1;
>  
>     // 設為 private，外界不能 new
>     private Singleton()
>     {
>     }
>  
>     // 外界只能使用靜態方法取得實例
>     public static Singleton GetInstance()
>     {
>         // 先判斷目前有沒有實例，沒有的話才開始 lock，
>         // 此次的判斷，是避免在有實例的情況，也執行 lock ，影響效能
>         if (null == instance)
>         {
>             // 避免多執行緒可能會產生兩個以上的實例，所以 lock
>             lock (thisLock)
>             {
>                 // lock 後，再判斷一次目前有無實例
>                 // 此次的判斷，是避免多執行緒，同時通過前一次的 null == instance 判斷
>                 if (null == instance)
>                 {
>                     instance = new Singleton();
>                 }
>             }
>         }
>  
>         return instance;
>     }
> }
> ```
>
> 
> **餓漢方式：**class 載入時，即產生實例
>
> ```C#
> public sealed class Singleton
> {
>     // 設為 static，載入時，即 new 一個實例
>     private static readonly Singleton instance = new Singleton();
>  
>     public int test = 1;
>  
>     // 設為 private，外界不能 new
>     private Singleton()
>     {
>     }
>  
>     // 使用靜態方法取得實例，因為載入時就 new 一個實例，所以不用考慮多執行緒的問題
>     public static Singleton GetInstance()
>     {
>         return instance;
>     }
> }
> ```

---

> 定義：只有一個實例，而且自行實例化並向整個系統提供這個實例。
>
> > 屬於創建模式，
> >
> > 這個模式涉及到一個單一的類別，他必須要`創建自己的實例`，
> >
> > 並且`確保只有單一個對象被創建`。
> >
> > 這個類別`提供一個方法`訪問其被創建的唯一一個對象。
>
> ```
> 存取ＩＯ和資料庫等資源，這時候要考慮使用單例模式。
> ```
>
> ### UML
>
> ![ Singleton ](https://ithelp.ithome.com.tw/upload/images/20181114/20112528d85wEM7c50.png)
>
> - Singleton：很簡單的只有一個類別，其中提供存取自己物件的方法，確保整個系統只有實例化一個物件。
>
> ##### 有幾種方式可以實現單例模式
>
> 1. 懶散(Lazy)模式（線程不安全）
> 2. 懶散模式（線程安全）
> 3. 積極模式
> 4. 雙重鎖 (Double ChockLock)
> 5. 登記式（靜態內部類）
> 6. 枚舉 (enumeration)
>
> > 試著實現一個積極單例模式
>
> ```java
> public class SingleObject {
>  
>    //創建 SingleObject 的一個對象
>    private static SingleObject instance = new SingleObject();
>  
>    //讓構造函數為 private，這樣該類就不會被實例化
>    private SingleObject(){}
>  
>    //獲取唯一可用的對象
>    public static SingleObject getInstance(){
>       return instance;
>    }
>  
> }
> ```
>
> > 試著實現一個懶散單例模式
>
> 積極模式在宣告靜態物件的時候就已經初始化，
>
> 但是懶散模式(Lazy)在呼叫getInstance時才進行初始化。
>
> ```java
> public class Singleton{
>     private static Singleton instance;
>     //私有的建構式讓別人不能創造
>     private Singleton (){}
>     
>     //因為整個系統都要存取這個類別，很可能有多個process或thread同時存取
>     //為了讓線程安全添加synchronized在多線程下確保物件唯一性
>     public static synchronized Singleton getInstance(){
>         if (instance == null)
>         {
>             instance = new Singleton();
>         }
>         return instance;
>     }
> }
> ```
>
> 但是這個實現方式每次都需要進行同步，效率會很很低。
>
> > 試著用雙重鎖實現
>
> ```java
> public class Singleton {
>     public static Singleton instance;
> 
>     private Singleton(){}
> 
>     public static Singleton getInstance(){
> 
> //        第一層判斷為了避免不必要的同步
>         if(instance == null){
>             
>             synchronized (Singleton.class){
> //                第二層判斷為了在null的狀況下建立實例
>                 if(instance == null){
>                     instance = new Singleton();
>                 }
>             }
> 
>         }
> 
>         return instance;
>     }
> }
> ```
>
> 判斷兩次看起來有點奇怪，但其實這樣做是有原因的。
>
> ```java
> instance = new Singleton();
> ```
>
> 上面這段程式碼看起來只有一段，但其實他不是原子操作，這句程式碼會被編譯成多條組合指令，大致上他做了三件事：
>
> 1. 給Singleton的實例分配記憶體；
> 2. 呼叫Singleton的建構函數，初始化成員欄位；
> 3. 將instance物件指向分配的記憶體空間(此時instance不是null)。
>
> 但是由於Java編譯器允許失序執行，所以 2. 和 3. 的順序是無法保證的，有可能 1-2-3 也有可能 1-3-2 。如果在 3. 執行完畢、2. 還沒執行之前，切換到線程Ｂ，那instance已經不是null，此時Ｂ取走instance再使用就會出錯。
>
> > JDK1.5以後的版本，官方注意到問題，所以調整JMM具體化volatile關鍵字，所以只要把instance寫法改成`private volatile static Singleton instance = null;`就可以保證都從主記憶體讀取，並且以DCL寫法完成單例模式。
>
> 雖然偶爾會失效但是DCL還是運用最多的模式。
>
> > 試著用靜態內部類實現
>
> ```java
> public class StaticInnerClass {
>     private StaticInnerClass(){}
> 
>     public static StaticInnerClass getInstance(){
>         return StaticInnerClassHolder.instance;
>     }
> 
>     /**
>      * 靜態的內部類別
>      */
>     private static class StaticInnerClassHolder{
>         private static StaticInnerClass instance = new StaticInnerClass();
>     }
> }
> ```
>
> 可以確保線程安全，保證物件唯一性，並且延遲實例化，所以推薦使用。
>
> > 用列舉實現
>
> ```java
> public enum  EnumSingleton {
>     INSTANCE;
> 
>     public void doSomething(){
>         System.out.println("do do !");
>     }
> }
> ```
>
> 可以避免反實例化。
>
> > 前面的單例模式要避免反實例化要加入readResolve()方法
>
> ```java
> private Object readResolve() throws ObjectStreamException {
>     return instance;
> }
> ```
>
> 這是提供給開發人員控制物件的反序列化方法。
>
> #### 試著寫出SingletonFactory
>
> > 產品和工廠介面
>
> ```java
> public abstract class Product {
>     public String getName(){
>         return this.getClass().getSimpleName();
>     }
> }
> 
> public interface Factory {
>     public Product getProduct();
> }
> ```
>
> > 可樂和漢堡(繼承了getName方法)
>
> ```java
> public class Cola extends Product {
> }
> public class Humberger extends Product {
> }
> ```
>
> > SingletonFactory
>
> ```java
> public class SingletonFactory {
> 
>     public static Factory getColaFactory(){
>         return ColaFactory.colaFactory;
>     }
> 
>     public static Factory getHumbergerFactory(){
>         return HumbergerFactory.humbergerFactory;
>     }
> 
> 
>     private static class ColaFactory implements Factory{
> 
>         private static ColaFactory colaFactory = new ColaFactory();
> 
>         private ColaFactory(){}
> 
>         @Override
>         public Product getProduct() {
>             return new Cola();
>         }
>     }
> 
>     private static class HumbergerFactory implements Factory{
> 
>         private static HumbergerFactory humbergerFactory = new HumbergerFactory();
> 
>         private HumbergerFactory(){}
> 
>         @Override
>         public Product getProduct() {
>             return new Humberger();
>         }
>     }
>     
> }
> ```
>
> > 測試一下拿到可樂和漢堡產品
>
> ```java
> public class Test {
> 
>     public void test(){
> 
>         Cola cola = (Cola) SingletonFactory.getColaFactory().getProduct();
>         Humberger humberger =(Humberger) SingletonFactory.getHumbergerFactory().getProduct();
>         
>         System.out.println(cola.getName());
>         System.out.println(humberger.getName());
> 
>     }
> 
> }
> ```
>
> > 拿到的數值
>
> ```
> Cola
> Humberger
> ```

## 實作

實作起來沒甚麼難度，反倒是比較好奇這個模式可以應用在哪種場合

類別圖

![](https://i.imgur.com/2QWRV8G.png)

### 簡單實作(Lazy initialization)

```C#
namespace Singleton
{
    public class Singleton
    {
        private static Singleton instance;
        private Singleton() { }
        public static Singleton GetInstance()
        {
            if (instance == null)
                return instance = new Singleton();
            return instance;
        }
    }
}
```

#### 測試

為了測試給他一點內容好了

```C#
    public class Singleton
    {
        private static Singleton instance;
        public long randomNum;
        private Singleton() 
        {
            Random random = new Random();
            randomNum = random.Next();
        }
        public static Singleton GetInstance()
        {
            if (instance == null)
                return instance = new Singleton();
            return instance;
        }
    }
```

測試程式(可惜C#不方便取址，不然測試就很容易了)

```C#
        private void singletonTest()
        {
            Singleton.Singleton singleton1 = Singleton.Singleton.GetInstance();
            Singleton.Singleton singleton2 = Singleton.Singleton.GetInstance();

            tbxResult.Text = $"randomNum in singleton 1 is {singleton1.randomNum}\r\n";
            tbxResult.Text += $"randomNum in singleton 2 is {singleton2.randomNum}\r\n";
            tbxResult.Text += $"are they the same? {singleton1.randomNum == singleton2.randomNum}\r\n";
            tbxResult.Text += $"now let randomNum in singleton 1 equals 222\r\n";
            singleton1.randomNum = 222;
            tbxResult.Text += $"randomNum in singleton 1 is {singleton1.randomNum}\r\n";
            tbxResult.Text += $"randomNum in singleton 2 is {singleton2.randomNum}\r\n";
            tbxResult.Text += $"are they the same? {singleton1.randomNum == singleton2.randomNum}\r\n";
        }
```

結果

![](https://i.imgur.com/ttswocB.png)

#### 多執行緒測試

```C#
        private void singletonTest2()
        {
            Singleton.Singleton singleton1 = null;
            Singleton.Singleton singleton2 = null;
            Thread thread1 = new Thread(getSingleton1Instance);
            Thread thread2 = new Thread(getSingleton2Instance);
            thread1.Start();
            thread2.Start();
            void getSingleton1Instance() { singleton1 = Singleton.Singleton.GetInstance(); }
            void getSingleton2Instance() { singleton2 = Singleton.Singleton.GetInstance(); }
            while (singleton1 == null || singleton2 == null) { Task.Delay(100).Wait(); }
            tbxResult.Text = $"randomNum in singleton 1 is {singleton1.randomNum}\r\n";
            tbxResult.Text += $"randomNum in singleton 2 is {singleton2.randomNum}\r\n";
            tbxResult.Text += $"are they the same? {singleton1.randomNum == singleton2.randomNum}\r\n";
            tbxResult.Text += $"now let randomNum in singleton 1 equals 222\r\n";
            singleton1.randomNum = 222;
            tbxResult.Text += $"randomNum in singleton 1 is {singleton1.randomNum}\r\n";
            tbxResult.Text += $"randomNum in singleton 2 is {singleton2.randomNum}\r\n";
            tbxResult.Text += $"are they the same? {singleton1.randomNum == singleton2.randomNum}\r\n";
        }
```

結果(有時成功有時失敗)

![](https://i.imgur.com/86P1hbH.png)

### 雙重鎖實作(Lazy initialization)

```C#
    public class Singleton
    {
        private static Singleton instance;
        public long randomNum;
        private static object singletonLock = new object();

        private Singleton()
        {
            Random random = new Random();
            randomNum = random.Next();
        }

        public static Singleton GetInstance()
        {
            if (instance == null)
                lock (singletonLock)
                    if (instance == null)
                        return instance = new Singleton();
            return instance;
        }
    }
```

多執行緒測試通過，懶得截圖了



### 實作(Eager initialization)

```C#
    public class Singleton
    {
        private static Singleton instance = new Singleton();
        public long randomNum;

        private Singleton()
        {
            Random random = new Random();
            randomNum = random.Next();
        }

        public static Singleton GetInstance() => instance;
    }
```

多執行緒測試通過，懶得截圖了