# Decorator Pattern

[Reference](https://rongli.gitbooks.io/design-pattern/content/chapter8.html)

[Reference](https://dotblogs.com.tw/daniel/2018/04/09/102242)

[Reference](https://www.itread01.com/content/1547555617.html)

## 目的

通過使用修飾模式，可以在運行時擴充一個類的功能。原理是：增加一個修飾類包裹原來的類，包裹的方式一般是通過在將原來的對象作為修飾類的構造函數的參數。裝飾類實現新的功能，但是，在不需要用到新功能的地方，它可以直接調用原來的類中的方法。修飾類必須和原來的類有相同的接口。

修飾模式是類繼承的另外一種選擇。類繼承在編譯時候增加行為，而裝飾模式是在運行時增加行為。



優點

1. 提供比繼承更多的靈活性。
2. 使用不同的具體物件，及裝飾物件的排列組合，可以創造出許多不同的行為組合。

缺點

1. 進行系統設計的時候會產生很多小對象。
2. 靈活的特性，也代表比繼承容易出錯，除錯也比較困難。

## 概念

> 裝飾者模式是一個很精美且優雅的模式
>
> 本篇使用 文字內容->AES加密->Zip檔附加密碼->輸出儲存
>
> 向大家介紹這個優雅又精美的設計模式
>
> ### 情境
>
> 有個需求要做
>
> - 文字內容->壓縮zip(附上密碼)->輸出儲存
>
> 又改成...
>
> - 文字內容->AES加密->輸出儲存
>
> 需求又改成....
>
> - 文字內容->AES加密->Zip檔附加密碼->輸出儲存
>
> ###  
>
> 可發現需求一直在對於**文字內容**操作順序做變化,但他們核心離不開對於**文字內容****的操作**
>
>  
>
> ### 這種情境很適合來使用 **[裝飾者模式]**
>
>  
>
> ### 裝飾者模式 有兩個主要腳色 **被裝飾物件(Decorated) & 裝飾物件(Decorator)**
>
>  
>
>  
>
> 被裝飾物件(Decorated) 就像蛋糕的一樣, 裝飾物件(Decorator)就是上的水果,奶油,巧克力...等等裝飾物品
>
> 一般先有蛋糕被裝飾物件(Decorated),後再將裝飾物品加上去裝飾物件(Decorator)
>
>  
>
> 將物件有效的往上附加職責,不動到內部的程式碼, 在原來職責上附加額外的職責
>
>  
>
> 裝飾者模式運作就像 俄羅斯娃娃一樣 一層包一層
>
> 
>
> ## 第一步 先找尋他們共同裝飾東西,因為是讀寫檔案 所以我們可以對於`Byte` 起手
>
> ### 先可以開出 **讀 跟** **寫** 介面簽章當作裝飾動作的統一介面
>
> ```cs
> public interface IProcess
> {
>     byte[] Read(string path);
> 
>     void Write(string writePath, byte[] buffer);
> }
> ```
>
>  
>
> 在創建一個 `ProcessBase`給日後裝飾物品(Decorator)來繼承
>
> ```cs
> public abstract class ProcessBase : IProcess
> {
>     /// <summary>
>     /// 儲存被裝飾的物件
>     /// </summary>
>     protected IProcess _process;
> 
>     public abstract byte[] Read(string path);
> 
>     public abstract void Write(string writePath, byte[] buffer);
> 
>     public virtual void SetDecorated(IProcess process)
>     {
>         _process = process;
>     }
> }
> ```
>
>  
>
> 有兩點特別說明
>
> 1.  `protected IProcess _process;` 儲存被裝飾的物件
> 2. 由 `SetDecorated`方法來設置被裝飾的物件
>
> 就像俄羅斯娃娃只包裹一個娃娃,不管被包裹娃娃之前包含哪些娃娃
>
>  
>
>  
>
> ------
>
> ## 第二步 創建被裝飾物品(Decorated)
>
> 因為是檔案我們直接使用 
>
> 1. `File.ReadAllBytes ` 讀 檔案
> 2. `File.WriteAllBytes ` 寫 檔案
>
> ```cs
> /// <summary>
> /// 讀取檔案
> /// </summary>
> public class FileProcess : IProcess
> {
>     public byte[] Read(string path)
>     {
>         return File.ReadAllBytes(path);
>     }
> 
>     public void Write(string writePath, byte[] buffer)
>     {
>         File.WriteAllBytes(writePath, buffer);
>     }
> }
> ```
>
>  
>
> ------
>
> ## 第三步 創建裝飾物品(Decorator)
>
> 這次主要裝飾物品有兩個 
>
> 1. 加壓解壓ZIP檔
> 2. 加解密
>
>  
>
> **加密裝飾器**繼承ProcessBase並按照加解密重寫 `Write`和 `read`方法
>
> ```cs
> /// <summary>
> /// Aes 加密裝飾器
> /// </summary>
> public class AESCrypProcess : ProcessBase
> {
>     private AesCryptoServiceProvider aes;
> 
>     public string AESKey { get; set; } = "1776D8E110124E75";
>     public string AESIV { get; set; } = "B890E7F6BA01C273";
> 
>     public AESCrypProcess() 
>     {
>         aes = new AesCryptoServiceProvider();
>         aes.Key = Encoding.UTF8.GetBytes(AESKey);
>         aes.IV = Encoding.UTF8.GetBytes(AESIV);
>     }
> 
>     public override byte[] Read(string path)
>     {
>         byte[] encryptBytes = _process.Read(path);
>         return DecryptData(encryptBytes);
>     }
> 
>     /// <summary>
>     /// 進行解密
>     /// </summary>
>     /// <param name="encryptBytes"></param>
>     /// <returns></returns>
>     private byte[] DecryptData(byte[] encryptBytes)
>     {
>         byte[] outputBytes = null;
>         using (MemoryStream memoryStream = new MemoryStream(encryptBytes))
>         {
>             using (CryptoStream decryptStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
>             {
>                 MemoryStream outputStream = new MemoryStream();
>                 decryptStream.CopyTo(outputStream);
>                 outputBytes = outputStream.ToArray();
>             }
>         }
>         return outputBytes;
>     }
> 
>     /// <summary>
>     /// 裝飾者呼叫方法
>     /// </summary>
>     /// <param name="path"></param>
>     /// <param name="data"></param>
>     public override void Write(string path, byte[] data)
>     {
>         byte[] outputBytes = EncryptData(data);
>         _process.Write(path, outputBytes);
>     }
> 
>     private byte[] EncryptData(byte[] data)
>     {
>         byte[] outputBytes = null;
>         using (MemoryStream memoryStream = new MemoryStream())
>         {
>             using (CryptoStream encryptStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
>             {
>                 MemoryStream inputStream = new MemoryStream(data);
>                 inputStream.CopyTo(encryptStream);
>                 encryptStream.FlushFinalBlock();
>                 outputBytes = memoryStream.ToArray();
>             }
>         }
> 
>         return outputBytes;
>     }
> }
> ```
>
>  
>
> 這次讀寫zip使用[ SharpZipLib](https://github.com/icsharpcode/SharpZipLib) 開源第三方插件
>
>  **ZIP裝飾器**繼承ProcessBase並按照加解密重寫 `Write`和 `read`方法
>
> ```cs
> /// <summary>
> /// 讀取Zip使用
> /// </summary>
> public class ZipProcess : ProcessBase
> {
>     public string PassWord { get; set; }
>     public string FileName { get; set; }
> 
>     public override byte[] Read(string path)
>     {
>         byte[] buffer = _process.Read(path);
>         return ZipReader(path, buffer);
>     }
> 
>     public override void Write(string writePath, byte[] data)
>     {
>         byte[] buffer = ZipWriter(data);
>         _process.Write(writePath, buffer);
>     }
> 
>     private byte[] ZipWriter(byte[] buffer)
>     {
>         using (MemoryStream outputMemStream = new MemoryStream())
>         using (ZipOutputStream zipStream = new ZipOutputStream(outputMemStream))
>         using (MemoryStream memStreamIn = new MemoryStream(buffer))
>         {
>             zipStream.SetLevel(9); 
> 
>             ZipEntry newEntry = new ZipEntry(FileName);
>             newEntry.DateTime = DateTime.Now;
>             zipStream.Password = PassWord;
>             zipStream.PutNextEntry(newEntry);
> 
>             StreamUtils.Copy(memStreamIn, zipStream, new byte[4096]);//將zip流搬到memoryStream中
>             zipStream.CloseEntry();
> 
>             zipStream.IsStreamOwner = false;   
>             zipStream.Close();                
> 
>             return outputMemStream.ToArray();
>         }
>     }
> 
>     /// <summary>
>     /// 讀取zip檔
>     /// </summary>
>     /// <param name="buffer">zip檔案byte</param>
>     /// <returns></returns>
>     private byte[] ZipReader(string filePath, byte[] buffer)
>     {
>         byte[] zipBuffer = default(byte[]);
>         using (MemoryStream memoryStream = new MemoryStream(buffer))
>         {
>             memoryStream.Seek(0, SeekOrigin.Begin);
>             var zip = new ZipFile(memoryStream);
>             zip.Password = PassWord;
>             using (MemoryStream streamWriter = new MemoryStream())
>             {
>                 byte[] bufferReader = new byte[4096];
>                 var file = zip.GetEntry(FileName); //設置要去得的檔名
>                 //如果有檔案
>                 if (file != null)
>                 {
>                     var zipStream = zip.GetInputStream(file);
>                     StreamUtils.Copy(zipStream, streamWriter, bufferReader);
>                     zipBuffer = streamWriter.ToArray();
>                 }
>             }
>         }
>         return zipBuffer;
>     }
> }
> ```
>
> 上面就把我們要用的裝飾物品 (備料) 準備完成
>
> ------
>
> ## 第四步 創建使用(開始擺盤)
>
>  
>
> 創建一個` DecorateFactory` 來當生產 裝飾產品的工廠
>
> 建構子傳入一個 被裝飾的物件(FileProcess) 之後可依照喜好一直疊加 裝飾物品(ZipProcess,AESCrypProcess...)
>
> ```cs
> public class DecorateFactory
> {
>     IProcess _original;
> 
>     public DecorateFactory(IProcess original)
>     {
>         _original = original;
>     }
> 
>     public DecorateFactory SetProcess(ProcessBase process)
>     {
>         process.SetDecorated(_original);
>         _original = process;
>         return this;
>     }
> 
>     public IProcess GetProcess()
>     {
>         return _original;
>     }
> }
> ```
>
> 裝飾者模式順序是很重要的一個流程
>
>  
>
> 為了方便讀者閱讀 我使用小畫家畫出 讀寫順序
>
> 如下圖
>
> ![img](https://dotblogsfile.blob.core.windows.net/user/%E4%B9%9D%E6%A1%83/4bc56f03-a1e1-4504-bea3-7cb02a8aaa21/1522828936_73575.png)
>
> 使用就可很清晰來用
>
> 1. 利用DecorateFactory來創建裝飾流程
> 2. 使用 `factroy.GetProcess();`方法取得完成後的產品
> 3. 在簡單呼叫讀和寫方法
>
> ```cs
> string filePath = @"C:\Users\daniel.shih\Desktop\test.zip";
> string content = $"你好 123456 12@()!@ {Environment.NewLine} fsfd嘻嘻哈哈!!";
> 
> //設置初始化的被裝飾者
> DecorateFactory factroy = new DecorateFactory(new FileProcess());
> 
> //設置裝飾的順序
> factroy.SetProcess(new AESCrypProcess())
>         .SetProcess(new ZipProcess() { FileName = "1.txt",PassWord ="1234567"});
> 
> IProcess process = factroy.GetProcess();
> 
> byte[] data_buffer = Encoding.UTF8.GetBytes(content);
> process.Write(filePath, data_buffer);
> 
> byte[] buffer = process.Read(filePath);
> Console.WriteLine(Encoding.UTF8.GetString(buffer));
> ```
>
> 日後不管需求是改成
>
> - 文字內容->壓縮zip(附上密碼)->輸出儲存
> - 文字內容->AES加密->輸出儲存
> - 文字內容->AES加密->Zip檔附加密碼->輸出儲存
> - 還是.....
>
> 我們都不怕 因為我們把各種操作封裝和多態
>
> 各個模組間都是獨立的很好映證 高內聚低耦合 的設計原則
>
>  
>
>  
>
> ## 小結:
>
> 裝飾者模式是一個很精美且優雅的模式 希望這篇文章可讓讀者對於此模式有更加了解

---

> 在軟體開發中，我們經常想要對一類物件新增不同的功能，例如要給手機新增貼膜、掛件、外殼等。如果此時使用繼承來實現的話，我們就需要定義無數的類，這樣會導致“子類爆炸”的問題。為了解決這個問題，可以使用裝飾者模式來動態地給一個物件新增額外的職責。
>
> ### 裝飾者模式的詳細介紹
>
> **定義**
>
> 裝飾者模式以對客戶透明的方式動態地給一個物件附加上更多的職責，裝飾者模式相比生成子類可以更靈活地增加功能。
>
> **具體實現**
>
> ```cpp
> using System;
> using System.Collections.Generic;
> using System.Linq;
> using System.Text;
> using System.Threading.Tasks;
> 
> namespace _23DecoratorPatternDemo
> {
>     /// <summary>
>     /// 手機抽象類，即裝飾者模式中的抽象元件類
>     /// </summary>
>     public abstract class Phone
>     {
>         public abstract void Print();
>     }
>     /// <summary>
>     /// 手機，即裝飾者模式中的具體元件類
>     /// </summary>
>     public class ApplePhone : Phone
>     {
>         public override void Print()
>         {
>             Console.WriteLine("開始執行具體的物件————蘋果手機");
>         }
>     }
>     /// <summary>
>     /// 裝飾抽象類，要讓裝飾完全取代抽象元件，必須繼承自Phone
>     /// </summary>
>     public abstract class Decorator :Phone
>     {
>         private Phone phone;
>         public Decorator(Phone p)
>         {
>             this.phone = p;
>         }
>         public override void Print()
>         {
>             if(phone!=null)
>             {
>                 phone.Print();
>             }
>         }
>     }
>     /// <summary>
>     /// 貼膜，即具體裝飾者
>     /// </summary>
>     public class Sticker:Decorator
>     {
>         public Sticker(Phone p):base(p)
>         {
> 
>         }
>         public override void Print()
>         {
>             base.Print();
>             //新增新行為
>             AddSticker();
>         }
>         /// <summary>
>         /// 新的行為方法
>         /// </summary>
>         public void AddSticker()
>         {
>             Console.WriteLine("現在蘋果手機有膜了");
>         }
>     }
>     /// <summary>
>     /// 手機掛件
>     /// </summary>
>     public class Accessories : Decorator
>     {
>         public Accessories(Phone p) : base(p)
>         {
>         }
>         public override void Print()
>         {
>             base.Print();
>             AddAccessories();
>         }
>         public void AddAccessories()
>         {
>             Console.WriteLine("現在蘋果手機有掛件了");
>         }
>     }
> 
>     class Program
>     {
>         static void Main(string[] args)
>         {
>             // 我買了個蘋果手機
>             Phone phone = new ApplePhone();
> 
>             // 現在想貼膜了
>             Decorator applePhoneWithSticker = new Sticker(phone);
>             // 擴充套件貼膜行為
>             applePhoneWithSticker.Print();
>             Console.WriteLine("----------------------\n");
> 
>             // 現在我想有掛件了
>             Decorator applePhoneWithAccessories = new Accessories(phone);
>             // 擴充套件手機掛件行為
>             applePhoneWithAccessories.Print();
>             Console.WriteLine("----------------------\n");
> 
>             // 現在我同時有貼膜和手機掛件了
>             Sticker sticker = new Sticker(phone);
>             Accessories applePhoneWithAccessoriesAndSticker = new Accessories(sticker);
>             applePhoneWithAccessoriesAndSticker.Print();
>             Console.WriteLine();
>         }
>     }
> }
> ```
>
> 從上面的Program類程式碼可以看出，客戶端可以動態地將手機配件加到手機上，如果需要新增另外的配件時，只需要新增一個繼承自Decorator類的對應類就行了。可以看出，裝飾者模式的擴充套件性是非常好的。
>
> ### 裝飾者模式中的角色
>
> **抽象構件角色（phone）:**給出一個抽象介面，以規範準備接受附加責任的物件
>
> **具體構件角色（ApplePhone）：**定義一個將要接受附加責任的類
>
> **裝飾角色（Decorator）:**持有一個構件物件的例項，並定義一個與抽象構件介面一致的介面
>
> **具體裝飾角色（Sticker和Accessories）：**負責給構件物件新增附加的責任
>
> ### 裝飾者模式的優缺點
>
> **優點：**
>
> 1. **裝飾者模式和繼承都是擴充套件物件的功能，但裝飾者模式比繼承更靈活**
> 2. **通過使用不同的具體裝飾類以及這些類的排列組合，設計師可以創造出很多不同行為的組合**
> 3. **裝飾者模式有很好的擴充套件性**
>
> **缺點:**
>
> - 裝飾者模式會導致設計中出現許多小物件，如果過度使用，會讓程式變的更復雜。並且更多的物件會是的差錯變得困難，特別是這些物件看上去都很像。
>
> ### 使用場景
>
> 1. 需要擴充套件一個類的功能或給一個類增加附加責任。
> 2. 需要動態地給一個物件增加功能，這些功能可以再動態地撤銷。
> 3. 需要增加由一些基本功能的排列組合而產生的非常大量的功能
>
> ### 總結
>
> 裝飾者模式採用物件組合而非繼承的方式實現了在執行時動態地擴充套件物件的功能的能力，而且可以根據需要擴充套件多個功能，避免了單獨使用繼承帶來的“靈活性差”和“多子類衍生”問題。同時它很好地符合面向物件設計原則中“優先使用物件組合而非繼承”和“開放-封閉”原則。

## 實作

看了不少文章還是似懂非懂，剩下就用實作來進一步了解吧

![](https://i.imgur.com/FtjCPIc.png)

套了剛學的Singleton Pattern把`MyBill`變成單例

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecoratorPattern
{
    public interface IBill
    {
        void Show();
    }

    public class MyBill : IBill
    {
        private static readonly MyBill instance = new MyBill();
        public List<string> items;
        public int price;
        public string toPrint = "";

        private MyBill()
        {
            items = new List<string>();
            price = 0;
        }

        public static MyBill GetInstance() => instance;

        public void Show()
        {
            toPrint = "Bill Details\r\n";
            foreach (string s in items)
                toPrint += s + "\r\n";
            toPrint += $"Total: {price} NTD";
            Console.WriteLine(toPrint);
        }
    }

    public class Decorator : IBill
    {
        private MyBill bill = MyBill.GetInstance();

        protected void addItem(string item, int price)
        {
            bill.items.Add(item);
            bill.price += price;
        }

        public void Show() => bill.Show();
    }

    public class Toast : Decorator
    {
        public Toast() => this.addItem("toast", 15);
    }

    public class Egg : Decorator
    {
        public Egg() => this.addItem("egg", 10);
    }
}
```

測試

        private void decoratorTest()
        {
            MyBill myBill = MyBill.GetInstance();
            Decorator decorator1 = new Toast();
            Decorator decorator2 = new Egg();
            myBill.Show();
            tbxResult.Text = myBill.toPrint;
        }
結果

> Bill Details
> toast
> egg
> Total: 25 NTD



算是寫出來了，但不知道夠不夠標準，寫完後還是沒感受到這個模式的好處，改天再試試