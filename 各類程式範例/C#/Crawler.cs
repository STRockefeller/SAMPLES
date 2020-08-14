//refers to https://medium.com/@thepen0411/web-crawling-tutorial-in-c-48d921ef956a
//範例中網頁的網址已變更，html中的tag也不進相同，但仍依其步驟完成了簡易爬蟲功能

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
