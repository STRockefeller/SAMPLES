# Crawler



refers to https://medium.com/@thepen0411/web-crawling-tutorial-in-c-48d921ef956a
範例中網頁的網址已變更，html中的tag也不盡相同，但仍依其步驟完成了簡易爬蟲功能



```C#
using System;
using System.Collections.Generic;
using System.Linq;
//NuGet套件 System.Net.Http以及HtmlAgilityPack
using System.Net.Http;
using HtmlAgilityPack;
using System.Text;
using System.Data.Odbc;
using System.Threading.Tasks;

namespace CrawlerTest20200813
{
    class Program
    {
        static void Main(string[] args)
        {
            startCrawlerAsync();
            Console.ReadLine();
        }
        private static async Task startCrawlerAsync()
        {
            //url -->目標網址
            string url = "https://www.automobile.tn/fr/neuf/bmw";
			
			#region htmlClient 功能同下二擇一即可，經測試發現部分網頁無法正常執行httpClient.GetStringAsync，原因不明。
            HttpClient httpClient = new HttpClient();
            //html --> 會得到目標網址的html全部內容(就像檢視網頁原始碼這樣)
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
			#endregion
			
			#region webclient 功能同上二擇一即可
            HtmlWeb webClient = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = webClient.Load(url);
            #endregion
			
            //divs = SELECT div FROM html WHERE class = 'versions-item';
            List<HtmlNode> divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("versions-item")).ToList();

            List<Car> cars = new List<Car>();
            foreach(HtmlNode div in divs)
            {
				//div.Descendants("h2").FirstOrDefault().InnerText找的位置如下
				//<div class = "versions-item">
				//...
				//	<h2>
				//		目標內容
				//	</h2>
				//...
				//</div>
				
				//也可以複製此處的Xpath 以div.DescendantNode.SelectNodes(XPath)來獲取資料---(待驗證)
				
                Car car = new Car(div.Descendants("h2").FirstOrDefault().InnerText, div.Descendants("div").FirstOrDefault().InnerText);
                cars.Add(car);
            }
        }
    }
	class Car
    {
        public string model { get; set; }
        public string price { get; set; }

        public Car(string model,string price)
        {
            this.model = model;
            this.price = price;
        }
    }
}

```

### 以下說明

首先，VS必須安裝NuGet套件 System.Net.Http以及HtmlAgilityPack

並引用之

```C#
using System.Net.Http;
using HtmlAgilityPack;
```

接著將主要內容寫入一個 Async Task 中

> 註：不這麼做也行但程式在執行爬蟲時會整個卡住。

接著必須想辦法獲得HtmlAgilityPack.HtmlDocument物件

可以透過以下兩種方式

#### HttpClient

```C#
HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
```

其中 html 會得到目標網址的html全部內容(就像檢視網頁原始碼這樣)

Lawal Abdulateef Olawale的教學文章中也是使用這種方法。

*但後來我自己在做嘗試的時候發現這個方法在小説家になろう等部分網站中，程式會停留在await httpClient.GetStringAsync(url)不動作，嘗試加入timeOut也一樣。目前還不清楚原因。*

#### WebClient

```C#
HtmlWeb webClient = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument htmlDocument = webClient.Load(url);
```

在網路上查到，另一個可以獲得HtmlAgilityPack.HtmlDocument物件的方法。

*可以適用在小説家になろう但沒有進一步嘗試，不確定是否所有網站都行的通。*



##### 編碼問題

20210303補充，在爬goodinfo的時候發現的狀況

測試程式回傳`System.InvalidOperationException: 'Sequence contains no elements'`

逐步執行檢查發現爬到的html中innerText是亂碼

![](https://i.imgur.com/VeiMGtr.png)

這個問題可以透過修改`HtmlWeb`物件的Encoding來解決，我改成UTF-8就能正確顯示內容了

```C#
webClient.OverrideEncoding = Encoding.UTF8;
```

結果如下www

![](https://i.imgur.com/XIP6TbV.png)

#### 獲取資料

成功得到HtmlAgilityPack.HtmlDocument物件後，接著就可以來找尋所要的資料了。

```C#
List<HtmlNode> divs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("versions-item")).ToList();
```

這段是把每台車的資料區段分別抓取後存入divs內。

```C#
htmlDocument.DocumentNode.Descendants("div")
```

這段指向文件中所有div tag

```C#
.Where(node => node.GetAttributeValue("class", "").Equals("versions-item"))
```

針對要指向的div tag再加入一些條件變為指向

```html
<div class="versions-item"></div>
```

並且裏頭偷藏了Lambda Expression

嘗試將其還原成匿名委派

```C#
.Where(delegate(HtmlNode node){
    return node.GetAttributeValue("class", "").Equals("versions-item");
})
```

可以查看where的方法簽章

```C#
where<HtmlNode>(Func<HtmlNode,bool> predicate)
```



最後是將符合條件的div以HtmlNode物件形式存入List中，如果只要第一個HtmlNode可以使用.FirstOrDefault()

接著看下一段

```C#
Car car = new Car(div.Descendants("h2").FirstOrDefault().InnerText, div.Descendants("div").FirstOrDefault().InnerText);
```

基本上格式跟上方差不多，比較值得一提的是InnerText這個屬性

在如下情況中

```html
<p></br></p>
```

會得到的字串為""而不是"</br>"

推測應該是獲取除了標籤以外的字串內容。