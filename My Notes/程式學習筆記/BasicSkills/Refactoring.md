# Refactoring

參考

https://ithelp.ithome.com.tw/users/20102562/ironman/1338



## Abstract

雖說是重構筆記，但內容會涵蓋，各式設計原則和技巧



減少技術債重點整理:

* 撰寫註解和文件
* 寫測試
* 預先做好設計規劃(如class diagram)



## Software Metric



使用工具進行程式碼分析:

1. 靜態分析:看source code就可以進行分析，例如:
   * 程式碼行數(IOC)
   * 類別耦合(Coupling)
2. 動態分析:執行程式進行分析，例如
   * 覆蓋率(code coverage)



工具大致找了一下

Visual Studio可以直接使用內建的分析功能

VSC則是安裝extension，雖然我看大多是js/ts的分析

PMD似乎也是不錯的選擇(跨語言) https://pmd.github.io/



## 手動修改的部分



### 考慮物件導向SOLID原則

- **S**ingle responsibility principle (SRP)
- **O**pen-Close principle (OCP)
- **L**iskov substitution principle (LSP)
- **I**nterface segregation principle (ISP)
- **D**ependency inversion principle (DIP)

老生常談了，細節就不記錄了



### Encapsulate Field

將僅限於本類別使用的變數重寫成私有（private）成員變數，並提供存取方法（accessor method）。這種重構方式可以將與外部呼叫者無關的變數隱藏起來，減少代碼的[耦合性](https://zh.wikipedia.org/wiki/耦合性_(計算機科學))，並減少意外出錯的概率。



### Extract Method

意思是將大段代碼中的一部分提取後，構成一個新方法。這種重構可以使整段程式的結構變得更清晰，從而增加可讀性。這也對[函式](https://zh.wikipedia.org/wiki/函式)（Function）通用。



### Generalize Type

將多個類別/函式共享的類型抽象出可以公用的基礎類別（base class），然後利用多型性追加每個類別/函式需要的特殊函式。這種重構可以讓結構更加清晰，同時可以增加代碼的可維護性。



### Pull Up

指的是方法從子類移動到父類別。



### Push Down

指的是方法從父類別移動到子類。



### Rename Method

將方法名稱以更好的表達它的用途。