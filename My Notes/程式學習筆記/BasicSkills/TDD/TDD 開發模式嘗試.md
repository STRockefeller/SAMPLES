# TDD開發模式嘗試



[Reference:Alpha Camp](https://tw.alphacamp.co/blog/tdd-test-driven-development-example)

TDD本身沒有太多的內容給我筆記，所以就直接按著流程練習幾遍。



## 流程

最開頭還是先把整個TDD的開發流程記錄下來



首先是TDD的宗旨:**先寫測試再開發**

接著是步驟(節錄自AlphaCamp)

> ### 步驟一：選定一個功能，新增測試案例
>
> - 重點在於思考希望怎麼去使用目標程式，定義出更容易呼叫的 API 介面。
>
> - 這個步驟會寫好測試案例的程式，同時決定產品程式的 API 介面。
>
> - **但尚未實作 API 實際內容**。
>
>   
>
> ### 步驟二：執行測試，得到 Failed（紅燈）
>
> - 由於還沒撰寫 API 實際內容，執行測試的結果自然是 failed。
> - 確保測試程式可執行，沒有語法錯誤等等。
>
> ### 步驟三：實作「夠用」的產品程式
>
> - 這個階段力求快速實作出功能邏輯，用「**最低限度**」通過測試案例即可。
> - 不求將程式碼優化一步到位。
>
> ### 步驟四：再次執行測試，得到 Passed（綠燈）
>
> - 確保產品程式的功能邏輯已經正確地得到實作。
> - **到此步驟，將完成一個可運作且正確的程式版本，包含產品程式和測試程式。**
>
> ### 步驟五：重構程式
>
> - 優化程式碼，包含產品程式和測試程式（測試程式也是專案需維護的一部份）。
> - 提升程式的可讀性、可維護性、擴充性。
> - 同時確保每次修改後，執行測試皆能通過。



## 練習

總之先挑個題目，因為主要目的是練習TDD而非解題，所以會挑個簡單的題目試試

[CodeWars 7kyu Odd or Even?](https://www.codewars.com/kata/5949481f86420f59480000e7)

解題和測試的程式碼也會以我相對較於熟悉的C#撰寫

### 題目內容

#### Task:

Given a list of integers, determine whether the sum of its elements is odd or even.

Give your answer as a string matching `"odd"` or `"even"`.

If the input array is empty consider it as: `[0]` (array with a zero).

#### Examples:

```
Input: [0]
Output: "even"

Input: [0, 1, 4]
Output: "odd"

Input: [0, -1, -5]
Output: "even"
```

Have fun!



### 解題

#### 選定一個功能，新增測試案例

也就一個功能沒啥好選的

總之邏輯端先不實作

```C#
public string oddOrEven(int[] input) => null;
```

測試端

```C#
using NUnit.Framework;

namespace ConsoleApp1
{
    [TestFixture]
    class OODTest
    {
        private OOD oOD;
        [SetUp]
        public void SetUp()
        {
            oOD = new OOD();
        }
        [Test]
        public void OddOrEven_InputCaseOdd_ReturnStringOdd()
        {
            //arrange
            
            int[] input = new int[] { 1 };
            //act
            string actual = oOD.OddOrEven(input);
            //assert
            string expected = "odd";
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void OddOrEven_InputCaseEven_ReturnStringEven()
        {
            //arrange
            
            int[] input = new int[] { 1, 2, 3 };
            //act
            string actual = oOD.OddOrEven(input);
            //assert
            string expected = "even";
            Assert.AreEqual(expected, actual);
        }
    }
}
```



#### 執行測試，得到 Failed（紅燈）

理所當然拿到兩顆紅燈，就不截圖了

#### 實作「夠用」的產品程式

邏輯端

```C#
public string OddOrEven(int[] input) => Math.Abs(input.Sum()) % 2 == 1 ? "odd" : "even";
```



#### 再次執行測試，得到 Passed（綠燈）

順利得到綠燈。

#### 重構程式

例子太簡單沒什麼好重構的