# Strategy Pattern

[Reference:ITHelp](https://ithelp.ithome.com.tw/articles/10202506)

[Reference](https://medium.com/enjoy-life-enjoy-coding/design-pattern-%E5%BE%9E%E5%BE%A9%E4%BB%87%E8%80%85%E7%9C%8B%E7%AD%96%E7%95%A5%E6%A8%A1%E5%BC%8F-strategy-pattern-feat-typescript-8623989c5e46)

[Reference:ITRead](https://www.itread01.com/content/1510588597.html)

## 目的

在執行時期選擇演算法

## 概念

跟簡單工廠差不多

#### 優點：

1. 將**物件本身的職責**與**算法的職責**分離。
2. 新增/修改算法時，不會影響既有程式碼。
3. 具體算法組合交給客戶端，能在**執行時**才被決定

#### 缺點：

1. 會做出許多小類別。



> 一般來說，當一個動作有多種實現方法，在實際使用時，需要根據不同情況選擇某個方法執行動作，就可以考慮使用策略模式。
>
> 把動作抽象成接口，比如把玩球抽象成接口。代碼如下：
>
> ```
> public interface IBall
> {
>     void Play();
> }
> ```
>
> 
>
> 有可能是玩足球、籃球、排球等，把這些球類抽象成實現接口的類。分別如下：
>
> ```
> public class Football : IBall
> {
>     public void Play()
>     {
>       Console.WriteLine("我喜歡足球");
>     }
> }
> public class Basketball : IBall
> {
>     public void Play()
>     {
>       Console.WriteLine("我喜歡籃球");
>     }
> }
> public class Volleyball : IBall
> {
>     public void Play()
>     {
>       Console.WriteLine("我喜歡排球");
>     }
> }
> ```
>
> 
>
> 還有一個類專門用來選擇哪種球類，並執行接口方法：
>
> ```
> public class SportsMan
> {
>     private IBall ball;
>     public void SetHobby(IBall myBall)
>     {
>       ball = myBall;
>     }
>     public void StartPlay()
>     {
>       ball.Play();
>     }
> }
> ```
>
> 
>
> 客戶端需要讓用戶作出選擇，根據不同的選擇實例化具體類：
>
> ```
> class Program
> {
>     static void Main(string[] args)
>     {
>       IBall ball = null;
>       SportsMan man = new SportsMan();
>       while (true)
>       {
>         Console.WriteLine("選擇你喜歡的球類項目(1=足球， 2=籃球，3=排球)");
>         string input = Console.ReadLine();
>         switch (input)
>         {
>           case "1":
>             ball = new Football();
>             break;
>           case "2":
>             ball = new Basketball();
>             break;
>           case "3":
>             ball = new Volleyball();
>             break;
>         }
>         man.SetHobby(ball);
>         man.StartPlay();
>       }
>     }
> }
> ```

## 實作

感覺C#的class diagram也沒有很適合表達(資訊量有點少)

![](https://i.imgur.com/f9sogOG.png)

```C#
namespace StrategyPattern
{
    public interface IStrategy
    {
        string describe();
    }

    public class ByTrain : IStrategy
    {
        public string describe() => "Go by train";
    }
    public class ByCar : IStrategy
    {
        public string describe() => "Go by car";
    }
    public class ByBike : IStrategy
    {
        public string describe() => "Go by bike";
    }

    public class PickStrategy
    {
        public IStrategy pick(string context)
        {
            switch(context)
            {
                case "closed":
                    return new ByBike();
                case "far":
                    return new ByTrain();
                default:
                    return new ByCar();
            }
        }
    }
}
```

真心覺得寫起來就跟簡單工廠差不多