

# Pointer and Address

[reference](https://openhome.cc/Gossip/CppGossip/Pointer.html)

## Address-of

變數提供一個有名稱的記憶體儲存空間，變數可包含的資訊包含變數資料型態、變數記憶體位址與變數儲存值。

如果想知道變數的位址為何，可以使用 `&` 取址運算子（Address-of operator），例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    int n = 10; 

    cout << "n 的值：" << n << endl
         << "n 位址：" << &n << endl; 

    return 0; 
}
```

執行結果：

```
n 的值：10
n 位址：0x61febc
```

這個程式中，宣告了一個 `int` 整數變數 `n`，`n` 的位址是 `0x61febc`，使用 16 進位表示法，若 `int` 長度為 4 個位元組，從 `0x61febc` 後 4 個位元組都是配置給 `n` 的空間，現在這個空間中儲存值為 10。

## Pointer

直接存取變數會對分配到的空間作存取，指標（Pointer）是一種變數，指標可儲存特定的記憶體位址，要宣告指標，使用以下的語法：

```c++
type *ptr;
```

`ptr` 可儲存位址，而 `type` 為該位址儲存值的型態，實際宣告的方式如下：

```c++
int *n;
float *s;
char *c;
```

雖然宣告指標時，C++ 習慣將 `*` 前置在變數名稱前，不過 `n` 的型態是 `int*`，`s` 的型態是 `float*`，而 `c` 的型態是 `char*`，指標的型態決定了位址上的資料如何解釋，以及如何進行指標運算（Pointer arithmetic）。

可以使用 `&` 運算子取得變數位址並指定給指標，例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    int n = 10; 
    int *p = &n ; 

    cout << "n 變數的位址：" << &n << endl
         << "p 儲存的位址：" << p << endl; 

    return 0; 
}
```

執行結果：

```
n 變數的位址：0x61feb8
p 儲存的位址：0x61feb8
```

以上的程式使用 `&` 來取得變數 `n` 儲存的位址，然後指定給指標 `p`，因此 `p` 儲存的值就與 `&n` 取出的值相同。

## Dereference

可以使用提取 （Dereference）運算子 `*` 來提取指標儲存位址處的物件。例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    int n = 10; 
    int *p = &n;

    cout << "指標 p 儲存的位址：" << p << endl
         << "提取 p 儲存位址處的物件：" << *p << endl;

    return 0; 
}
```

`*p` 提取了 `p` 儲存的位址處之物件，這個物件就是 `n` 變數，因此執行結果如下：

```
指標 p 儲存的位址：0x61feb8
提取 p 儲存位址處的物件：10
```

`*p` 提取了變數 `n`，將值指定給 `*p` 時，就是指定給變數 `n`，例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    int n = 10; 
    int *p = &n ; 

    cout << "n = " << n << endl
         << "*p = " << *p << endl; 

    *p = 20; 

    cout << "n = " << n << endl
         << "*p = " << *p << endl;

    return 0; 
}
```

執行結果：

```
n = 10
*p = 10
n = 20
*p = 20
```

## Initialize pointer

如果宣告指標但不指定初值，指標儲存的位址是未知的，存取未知位址的記憶體內容是危險的，例如：

```c++
int *p;
*p = 10;
```

這會造成不可預知的結果，最好為指標設定初值，如果指標一開始不儲存任何位址，可設定初值為 0，或者是使用 `nullptr`，例如：

```c++
int *p = nullptr;
```

## Declare pointer

在指標宣告時，可以靠在變數旁邊，也可以靠在型態關鍵字旁邊，或者是置中，例如：

```c++
int *p1;
int* p2;
int * p3;
```

這三個宣告方式都是可允許的，C++ 開發者傾向用第一個，因為可以避免以下的錯誤：

```c++
int* p1, p2;
```

這樣的宣告方式，初學者可能以為 `p2` 也是指標，但事實上並不是，以下的宣告 `p1` 與 `p2` 才都是指標：

```c++
int *p1, *p2;
```

有時只希望儲存位址而不關心型態，可以使用 `void*` 來宣告指標，例如：

```c++
void* p;
```

### reinterpret_cast

由於 `void*` 型態的指標沒有任何型態資訊，只用來持有位址，不可以使用 `*` 運算子對 `void*` 型態指標提取值，編譯器也不會允許將 `void*` 指標直接指定給具有型態資訊的指標，必須使用 `reinterpret_cast` 明確告知編譯器，這個動作是你允許的，例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    int n = 10; 
    void *p = &n ; 

    int *iptr = reinterpret_cast<int*>(p);
    cout << *iptr << endl; // 顯示 10

    return 0; 
}
```

`reinterpret_cast` 用於指標，它告訴編譯器，你就是要以指定型態重新解釋 `p` 位址處的資料。

被 `const` 宣告的變數指定值後，就不能再改變變數值，也無法對該變數取址：

```c++
const int n = 10;
int *p = &n; // error,  invalid conversion from `const int*' to `int*'
```

用 `const` 宣告的變數，必須使用對應的 `const` 型態指標才可以：

```c++
const int n = 10;
const int *p = &n;
```

同樣地，也就不能如下試圖改變位址處的資料：

```c++
*p = 20; // error: assignment of read-only location '* p'
```

被 `const` 宣告的變數指定值後，就不能再改變變數值，也無法對該變數取址，編譯會不通過，不過必要時，可以用 `const_cast` 叫編譯器住嘴：

```c++
const int n = 10;
int *p = const_cast<int*>(&n); 
```

 `const_cast`，這只是叫編譯器住嘴罷了，後續程式碼也是別對 pi 位址處的資料做變動，以避免執行時期不可預期的結果。

要留意的是，`const int *p` 宣告的 `p` 並不是常數，可以儲存不同的位址。例如：

```c++
#include <iostream> 
using namespace std; 

int main() { 
    const int n = 10;
    const int m = 20;

    const int *p = &n;
    cout << p << endl;

    p = &m;
    cout << p << endl;

    return 0; 
}
```

執行結果：

```
0x61feb8
0x61feb4
```

如果想令指標儲存的值無法變動，必須建立指標常數，先來看看來源變數沒有 `const` 的情況：

```c++
int n = 10;
int m = 20;

int* const p = &n;
p = &m;  //  error: assignment of read-only variable 'p'
```

如果 `n`、`m` 被 `const` 修飾，那麼就必須如下建立指標常數：

```c++
const int n = 10;
const int m = 20;

const int* const p = &n;
p = &m; // error: assignment of read-only variable 'p'
```