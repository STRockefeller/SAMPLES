# Delegate Note
## 具名委派
```C#
using System;

namespace DelegatePractice
{
    internal class Program
    {
        #region 宣告端

        /// <summary>
        /// 建立委派，委派相當於一個class，但限定只有一個方法
        /// cleanRoom是對room進行清掃的動作
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public delegate string cleanRoom(string room);

        #endregion 宣告端

        /// <summary>
        /// 房子類別
        /// </summary>
        public class House
        {
            /// <summary>
            /// 母親的命令，叫負責清掃的人(cleaner)去做委派的動作(cleanRoom)
            /// 但不會管你具體要怎麼做
            /// </summary>
            /// <param name="cleaner"></param>
            public void mothersOrder(cleanRoom cleaner)
            {
                Console.WriteLine(cleaner("bathroom"));
            }
        }

        #region 邏輯端

        /// <summary>
        /// 清掃者類別
        /// </summary>
        public class Cleaner
        {
            /// <summary>
            /// 具體行動
            /// </summary>
            /// <param name="room"></param>
            /// <returns></returns>
            public string wipe(string room)
            {
                return $"Wipe the floor in {room}";
            }
        }

        #endregion 邏輯端

        private static void Main(string[] args)
        {
            //我家
            House myHome = new House();

            //我
            Cleaner me = new Cleaner();

            #region 呼叫端

            //委派類別實作
            cleanRoom cleaner = new cleanRoom(me.wipe);
            //委派呼叫
            myHome.mothersOrder(cleaner);

            #endregion 呼叫端
        }
    }
}
```
## 匿名委派
承襲上方程式

其中呼叫端的部分可以簡化
1. 

```C#
cleanRoom cleaner = new cleanRoom(me.wipe);
```
簡化成
```C#
cleanRoom cleaner = me.wipe;
```

2. 
再進一步將
```C#
cleanRoom cleaner = me.wipe;
myHome.mothersOrder(cleaner);
```
簡化成
```C#
myHome.mothersOrder(me.wipe);
```
到這一步已經將cleanRoom實作物件的名稱都省略掉了，故稱匿名委派。

1. 
最後還能將邏輯端整個省略，變成如下
```C#
using System;
namespace DelegatePractice
{
    internal class Program
    {
        #region 宣告端

        /// <summary>
        /// 建立委派，委派相當於一個class，但限定只有一個方法
        /// cleanRoom是對room進行清掃的動作
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public delegate string cleanRoom(string room);

        #endregion 宣告端

        /// <summary>
        /// 房子類別
        /// </summary>
        public class House
        {
            /// <summary>
            /// 母親的命令，叫負責清掃的人(cleaner)去做委派的動作(cleanRoom)
            /// 但不會管你具體要怎麼做
            /// </summary>
            /// <param name="cleaner"></param>
            public void mothersOrder(cleanRoom cleaner)
            {
                Console.WriteLine(cleaner("bathroom"));
            }
        }

        private static void Main(string[] args)
        {
            //我家
            House myHome = new House();

            #region 呼叫端(包含邏輯)

            myHome.mothersOrder(delegate(string room) {
                return $"clean {room}";
            });

            #endregion 呼叫端(包含邏輯)
        }
    }
}
```

## 泛型委派

考慮到使用許多方法簽章雷同委派的情形，要重複命名委派名稱實屬麻煩，因此有了**泛型委派**這種東西。

泛型委派將重點方在方法簽章上，以較為抽象的名稱作為代表。以上例'來說，加入一個父親要求做飯的命令，實作如下

```C#
using System;

namespace DelegatePractice
{
    internal class Program
    {
        #region 宣告端
        /// <summary>
        /// 建立泛型委派，用於處理不確定的委派內容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="delegateParameter1"></param>
        /// <returns></returns>
        public delegate TResult myDelegate<T,TResult>(T delegateParameter1);

        #endregion 宣告端

        /// <summary>
        /// 房子類別
        /// </summary>
        public class House
        {
            /// <summary>
            /// 母親的命令，叫負責清掃的人(cleaner)去做委派的動作
            /// 但不會管你具體要怎麼做
            /// </summary>
            /// <param name="cleaner"></param>
            public void mothersOrder(myDelegate<string,string> cleaner)
            {
                Console.WriteLine(cleaner("bathroom"));
            }
            /// <summary>
            /// 使用同樣的泛型委派還能多做其他事情
            /// </summary>
            /// <param name="cheif"></param>
            public void fathersOrder(myDelegate<string,string> cheif)
            {
                Console.WriteLine(cheif("dinner"));
            }
        }

        private static void Main(string[] args)
        {
            //我家
            House myHome = new House();

            #region 呼叫端(包含邏輯)

            myHome.mothersOrder(delegate(string room) {
                return $"clean {room}";
            });

            myHome.fathersOrder(delegate (string meal)
            {
                return $"cook {meal}";
            });

            #endregion 呼叫端
        }
    }
}
```

既然相同的方法簽章要求都對應到相同的委派上，那麼委派的名稱已經不重要了。如此一來，我們便可以將各個方法簽章的委派宣告之後建置為dll，將來撰寫程式就可以省略委派的宣告動作了。

```C#
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDelegate
{
    public delegate TResult myDelegate<T, TResult>(T delegateParameter);
    public delegate TResult myDelegate<T1, T2, TResult>(T1 delegateParameter1, T2 delegateParameter2);
    public delegate TResult myDelegate<T1, T2, T3, TResult>(T1 delegateParameter1, T2 delegateParameter2, T3 delegateParameter3);
    public delegate TResult myDelegate<T1, T2, T3, T4, TResult>(T1 delegateParameter1, T2 delegateParameter2, T3 delegateParameter3, T4 delegateParameter4);
}
```

## Func委派及Action委派

既然泛型委派如此便利，那麼理所當然許多人都會採用這種方式來宣告委派，但這種情況下，每個人對泛型委派宣告名稱的不同，可能會造成程式整合或閱讀上的不易。因此，.Net 3.5之後在System命名空間之下提供了兩種泛型委派，分別是**有回傳值的Func**以及**沒有回傳值的Action**。概念如下

```C#
public delegate TResult Func<TResult>(); 
public delegate TResult Func<T, TResult>(T arg); 
public delegate TResult Func<T1, T2, TResult>(T1 arg1, T2 arg2, T3 arg3); 
public delegate TResult Func<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3); 
public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4); 
public delegate void Action(); 
public delegate void Action<T>(T obj); 
public delegate void Action<T1, T2>(T1 arg1, T2 arg2); 
public delegate void Action<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3); 
public delegate void Action<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4); 
```

> .Net 4.0版本之後兩者都提供到16個Argument了

最後試著將上例改為使用Func委派來撰寫吧

```C#
using System;

namespace DelegatePractice
{
    internal class Program
    {
        /// <summary>
        /// 房子類別
        /// </summary>
        public class House
        {
            /// <summary>
            /// 母親的命令，叫負責清掃的人(cleaner)去做委派的動作
            /// 但不會管你具體要怎麼做
            /// </summary>
            /// <param name="cleaner"></param>
            public void mothersOrder(Func<string,string> cleaner)
            {
                Console.WriteLine(cleaner("bathroom"));
            }
            /// <summary>
            /// 使用同樣的泛型委派還能多做其他事情
            /// </summary>
            /// <param name="cheif"></param>
            public void fathersOrder(Func<string,string> cheif)
            {
                Console.WriteLine(cheif("dinner"));
            }
        }

        private static void Main(string[] args)
        {
            //我家
            House myHome = new House();
            
            #region 呼叫端(包含邏輯)

            myHome.mothersOrder(delegate(string room) {
                return $"clean {room}";
            });

            myHome.fathersOrder(delegate (string meal)
            {
                return $"cook {meal}";
            });

            #endregion 呼叫端
        }
    }
}
```

如此一來，連委派的宣告都能省略了。

## Lambda Expression

MSDN的定義

> 「Lambda 運算式」(Lambda Expression) 是一種匿名函式，它可以包含運算式和陳述式 (Statement)，而且可以用來建立委派 (Delegate) 或運算式樹狀架構型別。
> 所有的 Lambda 運算式都會使用 Lambda 運算子 =>，意思為「移至」。 Lambda 運算子的左邊會指定輸入參數 (如果存在)，右邊則包含運算式或陳述式區塊。 Lambda 運算式 x => x * x 的意思是「x 移至 x 乘以 x」。

直接將上例做改寫比較明瞭

請看呼叫端

```C#
 			myHome.mothersOrder(delegate(string room) {
                return $"clean {room}";
            });

            myHome.fathersOrder(delegate (string meal)
            {
                return $"cook {meal}";
            });
```

改寫成

```C#
 			myHome.mothersOrder(room =>
            {
                return $"clean {room}";
            });

            myHome.fathersOrder(meal =>
            {
                return $"cook {meal}";
            });
```

可以注意到，Lambda 運算式，除了語法的更精簡之外，實務上還有一個特色：Lambda 運算式的型別推斷很強悍，大多數情況下，都可以省略傳入參數的型別。

MSDN中將Lambda分為運算式和陳述式兩種

> 1. 運算式 Lambda（Expression Lambda）：委派的邏輯只需要一行就可以寫完，可採用此方式 (input parameters) => expression。這種寫法的特別就是，不需要寫 return 關鍵字，也不需要用大括號把邏輯包起來。常見的有下面四種寫法：
>
>    (int x, string s) => s.Length > x; //明確指定傳入參數的型別，適用在無法型別推斷的時候。
>    (a, b) => a + b; //讓編譯器使用型別推斷省去撰寫傳入參數型別的寫法。
>    a => a * a; //只有一個傳入參數時，可以省略圓括號。
>    () => "L" + "I" + "N" + "Q"; //沒有傳入參數時，必須用空的圓括號。
>
> 2. 陳述式 Lambda（Statement Lambda）：委派的邏輯必須用兩行以上程式碼才能完成，就必須選用此方式 (input parameters) => {statement;}。這種寫法和匿名委派相比較，其實就是把 delegate 關鍵字省略成 「=>」運算子而已。所以了解匿名委派的寫法，那使用陳述式 Lambda 應當是毫無問題，常見寫法和運算式寫法雷同，其實也就是加上大括號和 return 關鍵字而已：
>
>    (int x, string s) => {x = x * 2; return s.Length > x;}
>    (a, b) => {a = a + b; return a * b;}

根據以上，可以發現先前寫的範例符合運算式的特點，因此可以再進行簡化。變成如下

```C#
            myHome.mothersOrder(room =>$"clean {room}");

            myHome.fathersOrder(meal =>$"cook {meal}");
```

最後，附上完整的程式碼

```C#
using System;

namespace DelegatePractice
{
    internal class Program
    {
        /// <summary>
        /// 房子類別
        /// </summary>
        public class House
        {
            /// <summary>
            /// 母親的命令，叫負責清掃的人(cleaner)去做委派的動作
            /// 但不會管你具體要怎麼做
            /// </summary>
            /// <param name="cleaner"></param>
            public void mothersOrder(Func<string, string> cleaner)
            {
                Console.WriteLine(cleaner("bathroom"));
            }

            /// <summary>
            /// 使用同樣的泛型委派還能多做其他事情
            /// </summary>
            /// <param name="cheif"></param>
            public void fathersOrder(Func<string, string> cheif)
            {
                Console.WriteLine(cheif("dinner"));
            }
        }

        private static void Main(string[] args)
        {
            //我家
            House myHome = new House();

            #region 呼叫端(包含邏輯)

            myHome.mothersOrder(room =>$"clean {room}");

            myHome.fathersOrder(meal =>$"cook {meal}");

            #endregion 呼叫端(包含邏輯)
        }
    }
}
```

