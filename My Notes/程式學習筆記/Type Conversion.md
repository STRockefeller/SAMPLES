# Type Conversion

## Abstract

雖說型別間的轉換是基礎中的基礎，但隨著學過的語言越來越多，時不時還是容易將其搞混，與其每次都Google不如還是自己整理一份文件來得輕鬆。

## Conversions

### To string

#### From int/ long/ float/ double/ number

---

C

C沒有string 所以目標變為char array

```C
int i = 1234;
char str[10];

sprintf(str, "%d", i);
```

---

C++

```C++
int i = 1234;
std::string str = to_string(i);
```

---

C#

```C#
int i = 1234;
string str = i.ToString();
```

---

TypeScript

```typescript
var i: number = 1234;
var str: string = i.toString();
```



#### From char array

---

C++

```C++
char arr[ ] = "Hi~";
std::string str = std::string(arr);
```

---

C#

```C#
char[] arr = new char[3] { 'H', 'i', '~' };
string str = new string(arr);
```
使用Join的場合
```C#
string str = string.Join(' ',arr);//str="H i ~"
```

---

TypeScript

```typescript
var arr: string[] = ['H', 'i', '~'];
var str: string = arr.join("");
```

注意:如果join沒有給參數的話

```typescript
var str: string = arr.join();//str="H,i,~"
```



### To int/ long/ float/ double/ number

#### From int/ long/ float/ double/ number

---

基本上`int直接拿去作為 long/ float/ double使用`或`float 作為 double使用`都不會有什麼問題，反過來才需要做型別轉換

---

C

```C
double d = 0.0;
int i =(int)d;
```

註:在C中`int i = d`是可行的但**不建議**使用，可能發生如下情況

```C
double a;
a = 3669.0;
int b;
b = a;//b==3668
```

因為系統認為a是`3668.99999999999954525264911353588104248046875`所以移到b的時候變為3668

---

C++

```C++
double d = 0.0;
int i =(int)d;
```

---

C#

```C#
double d = 0;
int i = (int)d;
```

```C#
int i = Convert.ToInt32(d);
```

---

TypeScript

在typescript中數字只有number型別所以應該不會有這類需求

#### From char

---

跟From string做區隔，這邊假設打算將目標轉為ascii code

如果是打算數字轉數字的話可以用 `i = c-'0';`

---

C

```C
char c = 'c';
int i = (int)(c);
```

---

C++

```C++
char c = 'c';
int i = (int)c;
```

---

C#

```C#
char c = 'c';
int i = c;
```

```C#
int i = (int)c;
```

```C#
int i = Convert.ToInt32(c);
```



#### From string

---

C

使用`sscanf`

```C
#include stdio.h
```

```C
char str[5] = { '1', '2', '3', '4','\0'};
int i;
sscanf(str, "%d", &i);
```

使用`atoi`

```C
char str[4] = {'1','2','3','4'};
int i = atoi(str);
```

---

C++

使用`stringstream`

```c++
#include <sstream>
using namespace std;
```

```C++
string str = "1234"
stringstream intValue(str);
int i = 0;
intValue >> i;
```

也可以使用`stoi()`

```C++
#include <iostream>
#include <string>
using namespace std;
```

```C++
string str = "1234"
int i = stoi(str);
```

---

C#

```C#
string str = "1234";
int i = Convert.ToInt32(str);
```

```C#
int i = Int32.Parse(str);
```

---

TypeScript

```typescript
var str: string = "1234";
var i: number = +str;
```

---

dart

```dart
String str = "1234";
int i = int.parse(str);
```



### To char

#### From ASCII

---

C

```C
int i = 99;
char c = a;//c=='c'
```

註:char* 格式的ASCII 可以用如下方法

```C
char c = atoi("99");
```

---

C++

```C++
int i = 99;
char c = (char)i;
```

---

C#

```C#
int i = 99;
char c = Convert.ToChar(i);
```

```C#
char c = (char)i;
```

---

TypeScript

```typescript
var i:number = 99;
var c:string = String.fromCharCode(i);
```

#### From string

---

(to char array)

---

C++

```C++
string str = "Hi~";
char cArr[1024];
strcpy(cArr, str.c_str());
```

```C++
string str = "Hi~";
char cArr[1024];
strncpy(cArr, str.c_str(), sizeof(cArr));
cArr[sizeof(cArr) - 1] = 0;
```

```C++
string str = "Hi~";
char * cArr = new char [str.length()+1];
strcpy (cArr, str.c_str());
```



---

C#

```C#
string str = "Hi~";
char[] cArr = str.ToCharArray();
```

```C#
string str = "Hi~";
char[] cArr = str.ToArray();
```

```C#
string str = "Hi~";
char[] cArr = str.Select(c=>c).ToArray();
```

---

TypeScript

```typescript
var str: string = "Hi~";
var cArr: string[] = str.split('');
```

