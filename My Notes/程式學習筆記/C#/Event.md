# Event

[TOC]



> refer to https://ithelp.ithome.com.tw/articles/10228852

> 　　平常我們撰寫程式時，都是自己主動使用某些物件提供的方法來得到結果。相反地，若物件的結果發生時間是隨機不固定的，我們可能不想自己一直主動詢問物件或一直等待，希望物件結果出來後主動通知我們，而物件結果出來這一件事情就是一種事件、一種改變、一種狀態發生，以上描述的這些算是一種事件驅動的程式設計。在WebForm中，每一個按鈕、控制項都是靠使用者點擊後觸發事件來執行後續程式。

## Publisher-Subscriber pattern

> Enable an application to announce events to multiple interested consumers asynchronously, without coupling the senders to the receivers.

## Basic Example

```C#
using System;

namespace EventTest
{
    /// <summary>
    /// 訂閱者
    /// </summary>
    class subscriber
    {
        public string name { get; set; }
        public void tellMe(string message)
        {
            Console.WriteLine($"{name} has received the message:{message}");
        }
    }
	/// <summary>
    /// 宣告委派
    /// </summary>
    /// <param name="message"></param>
    public delegate void tellWho(string message);
    /// <summary>
    /// 發佈者
    /// </summary>
    class publisher
    {

        public tellWho lastNews;

        public void updateNews(string news)
        {
            //觸發事件
            lastNews.Invoke(news);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            subscriber farmer = new subscriber() { name = "farmer" };
            subscriber chief = new subscriber() { name = "chief" };
            subscriber knight = new subscriber() { name = "knight" };

            publisher kingdomDaily = new publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;

            while (true)
            {
                Console.WriteLine("input news：");

                string message = Console.ReadLine();

                kingdomDaily.updateNews(message);
            }

        }
    }
}

```

### 解析

* 訂閱者

  具體執行的內容

  ```C#
  public void tellMe(string message)
          {
              Console.WriteLine($"{name} has received the message:{message}");
          }
  ```

* 委派

  傳遞方法

  ```C#
  public delegate void tellWho(string message);
  ```

  

* 發佈者

  委派宣告

  ```C#
  public tellWho lastNews;
  ```

  觸發事件

  ```C#
  public void updateNews(string news)
          {
              lastNews.Invoke(news);
          }
  ```

* 訂閱動作

  當委派被invoke時做什麼

  ```C#
  			kingdomDaily.lastNews += farmer.tellMe;
              kingdomDaily.lastNews += chief.tellMe;
              kingdomDaily.lastNews += knight.tellMe;
  ```

  

## 加入Event

承上例，委派必須提供給外部訂閱，勢必能被外部使用，因此有被外部使用後產生非預期結果的可能性

```C#
			subscriber farmer = new subscriber() { name = "farmer" };
            subscriber chief = new subscriber() { name = "chief" };
            subscriber knight = new subscriber() { name = "knight" };

            publisher kingdomDaily = new publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;
			
			kingdomDaily.lastNews.Invoke("Fake news");
```

所以在C#中，提供了event關鍵字來解決這個問題，讓委派能被外部加入方法，又能禁止被外部執行。



只要將Publisher中的

```C#
public tellWho lastNews;
```

改成

```C#
public event tellWho lastNews;
```

即可



## 委派會被多次呼叫的情形

若者個委派可能在內部多個地方都有可能觸發到，那可以**包裝成一個方法**，在C#與.NET中習慣將觸發事件方法前面加上`On`

```C#
class publisher
    {
        public event tellWho lastNews;

        public void updateNews(string news)
        {
            onGettingNews(news);
        }
        public void onGettingNews(string news)
        {
            lastNews.Invoke(news);
        }
    }
```

另外在無人訂閱的情況下我們不動作

```C#
class publisher
    {
        public event tellWho lastNews;

        public void updateNews(string news)
        {
            onGettingNews(news);
        }
        public void onGettingNews(string news)
        {
            if(lastNews!=null)
                lastNews.Invoke(news);
        }
    }
```

C#6.0之後可簡化成

```C#
                lastNews?.Invoke(news);
```

## 多個參數的情形

比如參數變為新聞標題及新聞內文兩個，以以下方式實現。

### 更變委派的參數量

```C#
using System;

namespace EventTest
{
    /// <summary>
    /// 訂閱者
    /// </summary>
    class subscriber
    {
        public string name { get; set; }
        public void tellMe(string t,string message)
        {
            Console.WriteLine($"{name} has received the message:{t}\t{message}");
        }
    }
    /// <summary>
    /// 宣告委派
    /// </summary>
    /// <param name="message"></param>
    public delegate void tellWho(string title,string message);
    /// <summary>
    /// 發佈者
    /// </summary>
    class publisher
    {

        public event tellWho lastNews;

        public void updateNews(string t,string news)
        {
            onGettingNews(t,news);
        }
        public void onGettingNews(string t,string news)
        {
                lastNews?.Invoke(t,news);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            subscriber farmer = new subscriber() { name = "farmer" };
            subscriber chief = new subscriber() { name = "chief" };
            subscriber knight = new subscriber() { name = "knight" };

            publisher kingdomDaily = new publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;

            while (true)
            {
                Console.WriteLine("input news：");

                string title = Console.ReadLine();
                string message = Console.ReadLine();

                kingdomDaily.updateNews(title, message);
            }

        }
    }
}

```

相當直覺的作法

### 宣告一個Class

```C#
using System;

namespace EventTest
{
    /// <summary>
    /// 新聞類型
    /// </summary>
    public class News
    {
        public string title { get; set; }
        public string content { get; set; }
    }
    /// <summary>
    /// 訂閱者
    /// </summary>
    class Subscriber
    {
        public string name { get; set; }
        public void tellMe(News news)
        {
            Console.WriteLine($"{name} has received the message:{news.title}\t{news.content}");
        }
    }
    /// <summary>
    /// 宣告委派
    /// </summary>
    /// <param name="message"></param>
    public delegate void tellWho(News news);
    /// <summary>
    /// 發佈者
    /// </summary>
    class Publisher
    {
        public event tellWho lastNews;

        public void updateNews(News news)
        {
            onGettingNews(news);
        }
        public void onGettingNews(News news)
        {
                lastNews?.Invoke(news);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Subscriber farmer = new Subscriber() { name = "farmer" };
            Subscriber chief = new Subscriber() { name = "chief" };
            Subscriber knight = new Subscriber() { name = "knight" };

            Publisher kingdomDaily = new Publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;

            while (true)
            {
                Console.WriteLine("input news：");
                News news = new News();
                news.title = Console.ReadLine();
                news.content = Console.ReadLine();

                kingdomDaily.updateNews(news);
            }
        }
    }
}

```

同樣很直覺的做法，比較乾淨點

### EventHandler

使用.net提供已經定義好的委派

```C#
public delegate void EventHandler(object sender, EventArgs eventArgs);
```

記得參數類別要繼承EventArgs

```C#
public class News : EventArgs
    {
        public string title { get; set; }
        public string content { get; set; }
    }
```



完整如下

```C#
using System;

namespace EventTest
{
    /// <summary>
    /// 新聞類型
    /// </summary>
    public class News : EventArgs
    {
        public string title { get; set; }
        public string content { get; set; }
    }

    /// <summary>
    /// 訂閱者
    /// </summary>
    internal class Subscriber
    {
        public string name { get; set; }

        public void tellMe(object sender, EventArgs eventArgs)
        {
            Publisher publisher = sender as Publisher;
            News news = eventArgs as News;
            Console.WriteLine($"{name} has received the message:{news.title}\t{news.content}");
        }
    }

    /// <summary>
    /// 宣告委派
    /// </summary>
    /// <param name="message"></param>
    public delegate void EventHandler(object sender, EventArgs eventArgs);

    /// <summary>
    /// 發佈者
    /// </summary>
    internal class Publisher
    {
        public event EventHandler lastNews;

        public void updateNews(News news)
        {
            onGettingNews(this,news);
        }

        public void onGettingNews(Publisher publisher,News news)
        {
            lastNews?.Invoke(publisher, news);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Subscriber farmer = new Subscriber() { name = "farmer" };
            Subscriber chief = new Subscriber() { name = "chief" };
            Subscriber knight = new Subscriber() { name = "knight" };

            Publisher kingdomDaily = new Publisher();
            Publisher kingdomWeekly = new Publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;
            kingdomWeekly.lastNews += farmer.tellMe;

            while (true)
            {
                Console.WriteLine("input news：");
                News news = new News();
                news.title = Console.ReadLine();
                news.content = Console.ReadLine();

                kingdomDaily.updateNews(news);
                kingdomWeekly.updateNews(news);
            }
        }
    }
}
```

委派宣告可以省略，因為System命名空間內也有了，實例化的時候也可以連帶指定EventArg

```C#
public event EventHandler<News> lastNews;
```



```C#
using System;

namespace EventTest
{
    /// <summary>
    /// 新聞類型
    /// </summary>
    public class News : EventArgs
    {
        public string title { get; set; }
        public string content { get; set; }
    }

    /// <summary>
    /// 訂閱者
    /// </summary>
    internal class Subscriber
    {
        public string name { get; set; }

        public void tellMe(object sender, EventArgs eventArgs)
        {
            Publisher publisher = sender as Publisher;
            News news = eventArgs as News;
            Console.WriteLine($"{name} has received the message:{news.title}\t{news.content}");
        }
    }

    /// <summary>
    /// 發佈者
    /// </summary>
    internal class Publisher
    {
        public event EventHandler<News> lastNews;

        public void updateNews(News news)
        {
            onGettingNews(this,news);
        }

        public void onGettingNews(Publisher publisher,News news)
        {
            lastNews?.Invoke(publisher, news);
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            Subscriber farmer = new Subscriber() { name = "farmer" };
            Subscriber chief = new Subscriber() { name = "chief" };
            Subscriber knight = new Subscriber() { name = "knight" };

            Publisher kingdomDaily = new Publisher();
            Publisher kingdomWeekly = new Publisher();
            //訂閱
            kingdomDaily.lastNews += farmer.tellMe;
            kingdomDaily.lastNews += chief.tellMe;
            kingdomDaily.lastNews += knight.tellMe;
            kingdomWeekly.lastNews += farmer.tellMe;

            while (true)
            {
                Console.WriteLine("input news：");
                News news = new News();
                news.title = Console.ReadLine();
                news.content = Console.ReadLine();

                kingdomDaily.updateNews(news);
                kingdomWeekly.updateNews(news);
            }
        }
    }
}
```

## 補充

### 關於委派物件的null判別

經實驗，似乎不判別也不會有影響，但大家都這麼寫，所以還是跟著寫吧。

### 關於取消訂閱

在確定不動作時(例如物件消滅或連線斷線等)最好記得將訂閱取消，以免發生預期外的狀況。