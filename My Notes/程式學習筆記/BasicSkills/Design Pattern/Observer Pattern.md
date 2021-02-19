# Observer Pattern

[reference](https://medium.com/enjoy-life-enjoy-coding/design-pattern-%E5%8F%AA%E8%A6%81%E4%BD%A0%E6%83%B3%E7%9F%A5%E9%81%93-%E6%88%91%E5%B0%B1%E5%91%8A%E8%A8%B4%E4%BD%A0-%E8%A7%80%E5%AF%9F%E8%80%85%E6%A8%A1%E5%BC%8F-observer-pattern-feat-typescript-8c15dcb21622)

## 目的

> > 當多個 Class 都需要接收同一種資料的變化時，就適合使用 Observer Pattern
>
> 上方「多個 Class 」指的就是「觀察者」，而「同一種資料」指的就是觀察者們想了解的「主題」，因此
>
> > Observer Pattern 實作的原理就是把獲取資料的部分抽離出來，並在資料改變時，同步送給所有的觀察者。
> >
> > 且觀察者可以在任何時候決定是否要繼續接收資料。
>
> 以 Youtuber 來說，他們會在所有影片的最後或一開始告訴大家說：「如果喜歡我的影片，記得幫我按讚分享，別忘了開啟小鈴鐺訂閱我哦！」。
>
> 這個訂閱的功能就像「主題」與「觀察者」之間的關係，只要你開啟訂閱，系統就會把你加進「訂閱者」的清單，在新影片發佈時，會將通知送給清單內的人，當然如果有一天，訂閱者不再喜歡這些影片，也可以隨時取消訂閱，只要被移出「訂閱者」清單，就不會再收到通知。

## 概念

> ## Observer Pattern 實作流程
>
> 以下用幾個流程來完成一個簡單的 Observer Pattern 。
>
> ## 找出「主題」和「觀察者們」
>
> 就上方的例子來說，「 Youtuber 」就是主題，「喜歡影片而訂閱的人 」就是觀察者。
>
> ## 設計觀察者的 Interface
>
> 這裡先設計 Interface 有幾個原因：
>
> 1. 因為觀察者不只一個，為了讓所有的觀察者都能夠被主題「加入訂閱」和「移除訂閱」，它們就得擁有同一個 Interface ，當執行事件時，就只需要以 Interfacet 做共同型別代替 Class 傳入。
> 2. 觀察者需要被通知，用 Interface 可以確保所有 Class 都有接受通知的行為存在。
>
> 雖然感覺很複雜，但是實際上，在觀察者的 Interface 內，只需要確保有接受通知的行為就可以了：
>
> ```typescript
> interface IObserver {
>   update(videoName: string): void;
> }
> ```
>
> 
>
> ## 設計主題的 Interface
>
> 主題是 Youtuber ，它會需要三個行為，分別是「加入訂閱」、「移除訂閱」、「通知訂閱的人」：
>
> ```typescript
> interface IYoutuber {
>   registerObservers(o: IObserver): void;
>   removeObservers(o: IObserver): void;
>   notifyObserver(): void;
> }
> ```
>
> 
>
> 這裡就可以看見 `IObserver` 的用途了，如果所有的訂閱者沒有同一個 Interface ，那這裡就會變得很複雜，更詳細的可以參照 [這篇文章](https://medium.com/enjoy-life-enjoy-coding/typescript-從-ts-開始學習物件導向-interface-用法-77fd0959769f) 內提到關於 Interface 的應用。
>
> ## 以 IYoutuber 實作 Class
>
> 上方不斷地提到「訂閱」這個功能，但是究竟要把訂閱的人保存在哪裡呢？就直接放到 Class 的 Private 屬性中吧！當然這個屬性的型別是 Array 陣列，而 Array 內存放的型別就是 `IObserver` ：
>
> ```typescript
> class Youtuber implements IYoutuber {
>   private observers: Array<IObserver>
> 
>   constructor() {
>     this.observers = [];
>   }
> }
> ```
>
> 
>
> 接下來，關於「訂閱」和「取消訂閱」這兩個功能，就是將送進來的參數加入或從陣列移除，而負責送通知給訂閱者的 `notifyObserver` ，則是用 `for` 將 `observers` 內的所有訂閱者都取出來，執行 `update` ：
>
> ```typescript
> class Youtuber implements IYoutuber {
>   /* 其餘省略 */
>   registerObservers(o: IObserver): void {
>     this.observers.push(o);
>   }
> 
>   removeObservers(o: IObserver): void {
>     const targetIndex = this.observers.indexOf(o);
>     this.observers.splice(targetIndex, 1);
>   }
> 
>   notifyObserver(): void {
>     this.observers.forEach(observer => observer.update('有新影片了！'));
>   }
> }
> ```
>
> 
>
> 上方的 `notifyObserver` 也因為 `observers` 內的型別都是 `IObserver` ，所以一定會有 `update` 能執行， Interface 真的太猛了。
>
> 目前的 `Youtuber` 內有了幾個主要的行為，但這樣有點不夠真實，因此我再為他們加上了 `name` 和 `publishVideo` ，待會就可以創建各個不同的 Youtuber 然後透過 `publishVideo` 假裝發佈新影片，然後用 `notifyObserver` 做通知，完成後的 `Youtuber` 會長這樣：
>
> ```typescript
> class Youtuber implements IYoutuber {
>   private observers: Array<IObserver>
> 
>   public name: string
> 
>   constructor(name: string) {
>     this.name = name;
>     this.observers = [];
>   }
> 
>   registerObservers(o: IObserver): void {
>     this.observers.push(o);
>   }
> 
>   removeObservers(o: IObserver): void {
>     const targetIndex = this.observers.indexOf(o);
>     this.observers.splice(targetIndex, 1);
>   }
> 
>   notifyObserver(): void {
>     this.observers.forEach(observer => observer.update(`${this.name} 有新影片了！`));
>   }
> 
>   publishVideo() {
>     this.notifyObserver();
>   }
> }
> ```
>
> 
>
> ## 以 IObserver 實作 Observer
>
> 這裡比較輕鬆，只需要確認 `Observer` 能夠有 `update` 就好，其他事情則是在 `update` 處理：
>
> ```typescript
> 
> class Observer implements IObserver {
>   update(content: string): void {
>     console.log(content);
>   }
> }
> ```
>
> 
>
> 完成 `Youtuber` 和 `Observer` 後，就能來看看怎麼使用它們：
>
> ```typescript
> // == 建立 Youtuber 的實體 ==
> const aGa = new Youtuber('蔡阿嘎');
> const gao = new Youtuber('老高');
> 
> // == 建立 Observer 的實體 ==
> const observer = new Observer();
> 
> console.log('== 分別訂閱蔡阿嘎和老高 ==');
> aGa.registerObservers(observer);
> gao.registerObservers(observer);
> 
> console.log('== 當蔡阿嘎和老高發佈新影片 ==');
> aGa.publishVideo();
> gao.publishVideo();
> ```
>
> 
>
> 執行結果如下：
>
> ![Image for post](https://miro.medium.com/max/60/1*isAfzZhayRaMtVKP0DQOkw.png?q=20)
>
> ![Image for post](https://miro.medium.com/max/1090/1*isAfzZhayRaMtVKP0DQOkw.png)
>
> 如果取消訂閱，不再接收任何通知也非常簡單，只需調用 `removeObservers` ：
>
> ```typescript
> console.log('== 取消訂閱蔡阿嘎 ==');
> aGa.removeObservers(observer);
> 
> console.log('== 蔡阿嘎發佈新影片 ==');
> aGa.publishVideo();
> 
> console.log('== 再訂閱蔡阿嘎 ==');
> aGa.registerObservers(observer);
> 
> console.log('== 蔡阿嘎發佈新影片 ==');
> aGa.publishVideo();
> ```
>
> 
>
> 從執行結果可以看到取消訂閱後，當 `aGa` 再執行 `publishVideo` 時，`observer` 就不會得到通知，因為 `observer` 已經不在 `aGa` 內部的訂閱者清單中了：
>
> ![Image for post](https://miro.medium.com/max/60/1*jkXxf-T1HR-mmgipc1yhXQ.png?q=20)
>
> ![Image for post](https://miro.medium.com/max/1088/1*jkXxf-T1HR-mmgipc1yhXQ.png)
>
> ## 結論
>
> 1. Interface 真的很偉大。
> 2. 將負責處理資料的部分抽到 `Youtuber` ，當有新影片就主動通知所有訂閱的人，而不是由訂閱人一直問說有沒有新影片了。
> 3. 耦合度很低，因為只要是實作了 `IObserver` 接口都可以接收新影片，而不是特定某個 Class 或是 Object 做型別參數，傳入了才發現沒有 `update` 的行為。
> 4. `Youtuber` 不管誰收到通知後要幹嘛，因為它只負責通知，上方是以觀影者的角度接收通知，但其實不只觀影者，就算是官方負責統計資料的 Class ，只要它有實作 `IObserver` 接口，那就能和觀影者同步接收到新影片的通知，各自處理接到資料後的邏輯。
>
> 原本以為 Observer Pattern 會很容易說明，結果發現真的很困難啊 😭 ，尤其是在解說例子的部分，如果對文章有任何建議、問題、或是需要改進的地方再麻煩各位留言告訴我，謝謝大家！

## 實作

這次的觀念倒是很好理解，這個模式的敘述讓我很容易聯想到event，就使用event來實作些甚麼吧

懶得想例子了，直接學範例做個Youtube訂閱模式好了

### 基本款

類別圖

![](https://i.imgur.com/syLo54K.png)

內容

```C#
namespace ObserverPattern
{
    public interface IYoutuber
    {
        void addObserver(IObserver observer);

        void removeObserver(IObserver observer);

        void noticeObserver(string news);
    }

    public interface IObserver
    {
        void getNews(string content);
    }

    public class Youtuber : IYoutuber
    {
        private string name;

        public Youtuber(string name) => this.name = name;

        public List<IObserver> observers = new List<IObserver>();

        public void addObserver(IObserver observer) => observers.Add(observer);

        public void noticeObserver(string news)
        {
            Console.WriteLine($"{name} 發布新消息 {news}");
            foreach (IObserver observer in observers)
                observer.getNews(news);
        }

        public void removeObserver(IObserver observer) => observers.Remove(observer);
    }

    public class Observer : IObserver
    {
        private string name;

        public Observer(string name) => this.name = name;

        public void getNews(string content)
        {
            Console.WriteLine($"{name} 得知最新消息 {content}");
        }
    }
}
```

測試

```C#
        private void observerTest()
        {
            Youtuber alphaGo = new Youtuber("alphaGo");
            Youtuber aquaplus = new Youtuber("aquaplus");
            Youtuber alicesoft = new Youtuber("alicesoft");
            Observer lance = new Observer("lance");
            Observer aliceman = new Observer("aliceman");
            Observer shiru = new Observer("shiru");

            alphaGo.addObserver(aliceman);
            aquaplus.addObserver(shiru);
            aquaplus.addObserver(lance);
            alicesoft.addObserver(lance);
            alicesoft.addObserver(aliceman);
            alicesoft.addObserver(shiru);

            alphaGo.noticeObserver("今天又贏了一盤棋");
            alicesoft.noticeObserver("Lance10 is on sale.");
            aquaplus.noticeObserver("請期待波卡多");

            aquaplus.removeObserver(shiru);
            aquaplus.noticeObserver("好像有人退訂了");
        }
```

結果

> alphaGo 發布新消息 今天又贏了一盤棋
> aliceman 得知最新消息 今天又贏了一盤棋
> alicesoft 發布新消息 Lance10 is on sale.
> lance 得知最新消息 Lance10 is on sale.
> aliceman 得知最新消息 Lance10 is on sale.
> shiru 得知最新消息 Lance10 is on sale.
> aquaplus 發布新消息 請期待波卡多
> shiru 得知最新消息 請期待波卡多
> lance 得知最新消息 請期待波卡多
> aquaplus 發布新消息 好像有人退訂了
> lance 得知最新消息 好像有人退訂了



### 使用event

上面那個例子有個詭異的地方，由Youtuber來決定觀眾是否訂閱是不合邏輯的，應該由觀眾自己決定訂閱頻道與否

類別圖

![](https://i.imgur.com/poiEOR9.png)

內容

```C#
namespace ObserverPattern
{
    public interface IYoutuber
    {
        event EventHandler noticeObserver;
    }

    public interface IObserver
    {
        void getNews(object sender,EventArgs e);
    }

    public class Youtuber : IYoutuber
    {
        private string name;
        public Youtuber(string name) => this.name = name;

        public event EventHandler noticeObserver;
        public void updateNews(string news)
        {
            Console.WriteLine($"{name} 發布新消息 {news}");
            onUpdatingNews(this, new YoutubeNotification { content = news });
        }
        private void onUpdatingNews(object sender,YoutubeNotification notification)
        {
            noticeObserver?.Invoke(sender, notification);
        }
    }

    public class Observer : IObserver
    {
        private string name;
        public Observer(string name) => this.name = name;
        public void getNews(object sender, EventArgs e)
        {
            Youtuber youtuber = sender as Youtuber;
            YoutubeNotification notification = e as YoutubeNotification;
            Console.WriteLine($"{name} 得知最新消息 {notification.content}");
        }
    }

    public class YoutubeNotification:EventArgs
    {
        public string content { get; set; }
    }
}
```

測試

```C#
        private void observerTest()
        {
            Youtuber alphaGo = new Youtuber("alphaGo");
            Youtuber aquaplus = new Youtuber("aquaplus");
            Youtuber alicesoft = new Youtuber("alicesoft");
            Observer lance = new Observer("lance");
            Observer aliceman = new Observer("aliceman");
            Observer shiru = new Observer("shiru");

            alphaGo.noticeObserver += aliceman.getNews;
            aquaplus.noticeObserver += shiru.getNews;
            aquaplus.noticeObserver += lance.getNews;
            alicesoft.noticeObserver += lance.getNews;
            alicesoft.noticeObserver += aliceman.getNews;
            alicesoft.noticeObserver += shiru.getNews;

            alphaGo.updateNews("今天又贏了一盤棋");
            alicesoft.updateNews("Lance10 is on sale.");
            aquaplus.updateNews("請期待波卡多");

            aquaplus.noticeObserver -= shiru.getNews;
            aquaplus.updateNews("好像有人退訂了");
        }
```

結果

> alphaGo 發布新消息 今天又贏了一盤棋
> aliceman 得知最新消息 今天又贏了一盤棋
> alicesoft 發布新消息 Lance10 is on sale.
> lance 得知最新消息 Lance10 is on sale.
> aliceman 得知最新消息 Lance10 is on sale.
> shiru 得知最新消息 Lance10 is on sale.
> aquaplus 發布新消息 請期待波卡多
> shiru 得知最新消息 請期待波卡多
> lance 得知最新消息 請期待波卡多
> aquaplus 發布新消息 好像有人退訂了
> lance 得知最新消息 好像有人退訂了