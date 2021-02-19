# Linked list

[reference](http://alrightchiu.github.io/SecondRound/linked-list-introjian-jie.html)



## Abstract：Linked list

Linked list(連結串列)是一種常見的資料結構，其使用**node(節點)**來記錄、表示、儲存資料(data)，並利用每個node中的**pointer**指向下一個node，藉此將多個node串連起來，形成Linked list，並以`NULL`來代表Linked list的終點，見圖一(a)。



![cc](https://github.com/alrightchiu/SecondRound/blob/master/content/Algorithms%20and%20Data%20Structures/BasicDataStructures/LinkedList/Intro/f1.png?raw=true)



**圖一(a)。**





![cc](https://github.com/alrightchiu/SecondRound/blob/master/content/Algorithms%20and%20Data%20Structures/BasicDataStructures/LinkedList/Intro/f2.png?raw=true)



**圖一(b)。**



若實際打開每個node的內部，至少會包含(1)**data**來代表資料，與(2)**pointer**指向下一個node，見圖一(b)：

- Node1：

  - 以`int`的data，記錄正整數77。
  - 本身的記憶體位置為`0x1001042f0`。
  - 以「node之**pointer**」，記錄(指向)下一個node之記憶體位置(`0x100104300`)。

- Node2：

  - 以`int`的data，記錄正整數33。

  - 本身的記憶體位置為

    ```
    0x100104300
    ```

    。

    - 由於在Node1中的「node之**pointer**」指向了Node2之記憶體位置，因此，便能夠經由Node1「找到」Node2。

  - 以「node之**pointer**」，記錄(指向)下一個node之記憶體位置(`0x100104310`)。

- Node3：

  - 以`int`的data，記錄正整數1414。

  - 本身的記憶體位置為

    ```
    0x100104310
    ```

    。

    - 由於在Node2中的「node之**pointer**」指向了Node3之記憶體位置，因此，便能夠經由Node2「找到」Node3。

  - 以「node之**pointer**」，記錄(指向)`NULL`，表示Linked list的最後一個node。

通常在面對一個Linked list時，能夠公開存取(access)的node只有「第一個node」，以`ListNode *first`表示，不過因為node中有**pointer**記錄下一個node的記憶體位置，便能夠讀取下一個node的**data**與**pointer**，換句話說，有了node中的**pointer**就可以在Linked list中「移動(**traversal**)」，更進一步，便能進行諸如「新增節點」、「刪除節點」、「印出Linked list」等等的資料處理。





### Realized Linked list by 2 classes

若以C++的**class(類別)**來實作Linked list，可以利用兩個class，使得node的資料不會被任意更動(也就是封裝(Encapsulation)的概念)。
使用`struct`來代表node也是常見的做法，差別在於，`struct`的資料成員(data member)一定是**公開的(public)**。

`class ListNode`的private data有兩項，一項代表著資料項目(在此以`int`示範)，一項是「指向型別(type)為ListNode之指標」，以`ListNode *next`表示，用來記錄「下一個node」的記憶體位置。

在`class LinkedList`的private data中，最基本一定會有代表「第一個node」的`ListNode *first`，其餘資料項目可以視情況增加，像是`int size`用來記錄Linked list的長度等等。

```c++
// C++ code
#include <iostream>
class LinkedList;    // 為了將class LinkedList設成class ListNode的friend,
                     // 需要先宣告
class ListNode{
private:
    int data;
    ListNode *next;
public:
    ListNode():data(0),next(0){};
    ListNode(int a):data(a),next(0){};

    friend class LinkedList;
};

class LinkedList{
private:
    // int size;                // size是用來記錄Linked list的長度, 非必要
    ListNode *first;            // list的第一個node
public:
    LinkedList():first(0){};
    void PrintList();           // 印出list的所有資料
    void Push_front(int x);     // 在list的開頭新增node
    void Push_back(int x);      // 在list的尾巴新增node
    void Delete(int x);         // 刪除list中的 int x
    void Clear();               // 把整串list刪除
    void Reverse();             // 將list反轉: 7->3->14 => 14->3->7
};
```





## Compare：Array & Linked list

Array(矩陣)也是常見的用來記錄一連串「具有相同形態的資料」的資料結構，以下便列舉Array與Linked list各自的優缺點，以及各自適合的問題情境。





### Array

**優點**：

- random access：只要利用**index**即可在O(11)時間對Array的資料做存取。

- 較Linked list為節省記憶體空間：因為Linked list需要多一個

  pointer

  來記錄下一個node的記憶體位置。

  - 假設node之**data**項目為11byte的`char`，但是**pointer**項目卻要44bytes，這樣的資料結構就多花了4倍的記憶體空間在與真正要處理的資料無關的部分上，是個沒有效率的做法。

**缺點**：

- 新增/刪除資料很麻煩：若要在第一個位置新增資料，就需要O(NN)時間把矩陣中所有元素往後移動。同理，若要刪除第一個位置的資料，也需要O(NN)時間把矩陣中剩餘的元素往前移動。
- 若資料數量時常在改變，要時常調整矩陣的大小，會花費O(NN)的時間在搬動資料(把資料從舊的矩陣移動到新的矩陣)。

**適用時機**：

- 希望能夠快速存取資料。
- 已知欲處理的資料數量，便能確認矩陣的大小。
- 要求記憶體空間的使用越少越好。





### Linked list

**優點**：

- 新增/刪除資料較Array簡單，只要對O(11)個node(所有與欲新增/刪除的node有pointer相連的node)調整pointer即可，不需要如同Array般搬動其餘元素。

  - 若是在Linked list的「開頭」新增node，只要O(11)。
- 若要刪除特定node，或者在特定位置新增node，有可能需要先執行O(NN)的「搜尋」。
  
- Linked list的資料數量可以是動態的，不像Array會有**resize**的問題。

**缺點**：

- 因為Linked list沒有**index**，若要找到特定node，需要從頭(`ListNode *first`)開始找起，搜尋的時間複雜度為O(NN)。
- 需要額外的記憶體空間來儲存**pointer**。

**適用時機**：

- 無法預期資料數量時，使用Linked list就沒有**resize**的問題。
- 需要頻繁地新增/刪除資料時。
- 不需要快速查詢資料。
