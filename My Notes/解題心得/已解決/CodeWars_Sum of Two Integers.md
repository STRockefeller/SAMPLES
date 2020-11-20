# CodeWars:Sum of Two Integers:20201120:C

[Reference](https://www.codewars.com/kata/5a9c35e9ba1bb5c54a0001ac/c)



## Question

> # Task
>
> ***Given\*** *Two integers a , b* , ***find\*** *The sum of them* , ***BUT\*** ***You are not allowed to use the operators + and -\***
>
> ------
>
> # Notes
>
> - ***The numbers\*** *(a,b) may be positive , negative values or zeros* .
> - ***Returning value\*** will be an ***integer\*** .
>
> ------
>
> # Input >> Output Examples
>
> ```cpp
> 1- Add (5,19) ==> return (24) 
> 
> 2- Add (-27,18) ==> return (-9)
> 
> 3- Add (-14,-16) ==> return (-30)
> ```

## My Solution

看完題目後想到的作法有二

* 使用類似邏輯電路中`加法器`的方式完成，先將輸入轉為Binary再使用邏輯運算得到結果
* 窮舉個位數的加法運算結果

第二個做法太低端了，不考慮



舉個例子來分析一下加法過程吧

1100(12)+0101(5)

​	1100

​	0101

  10001

0+0=0

0+1=1 1+0=1

1+1=0 carry 1

不考慮進位的話就是XOR

進位只發生在1+1的情況下

```C
int add(int x, int y)
{
    int carry = (x & y) << 1;
    int res = x ^ y;
    return carry == 0 ? res : add(res, carry); 
}
```

查了一下C的運算子發現XOR可以直接用`^`完成，簡單方便。



## Better Solutions



### Solution 1

```C
int add(int x, int y)
{
    while (y != 0)
    {
        int carry = x & y;  
        x = x ^ y; 
        y = carry << 1;
    }
    return x;
}
```

基本上是一樣的做法，只是把遞迴換成迴圈



---

簡單的問題炸出一群妖魔鬼怪來解答，下面三個答案我基本看不懂orz

### Solution 2

```C
int add(int x, int y)
{
    const char* const p = x;
    return &p[y];
}
```

> It uses pointer arthmatic.
> When you have pointer p.
> &p[n] is just equal to p + n*sizeof(p[0]).
> p is pointer to char, so sizeof(p[0]) is 1

> In other words you point `p` to the value of `x` as an address (it's not forbidden to do it until you do use this address)
> And then using the index operation you get the new address added by the value of `y` and because the size of `char` is one you got exactly what you need...
> **Great**!
> But under the hood the forbidden addition is still used...
> So it's a hack...
> (cool)



### Solution 3

```C
const unsigned add = 0xC337048D;
```

> The solution and test code are in separate compilation units. So the type of `add` in the solution does not matter for a successful compilation. The only required thing is that something called `add` is in the compiled solution and `add` is executable. Constants are placed in the executable memory by clang. The `add` constant contains machine codes corresponding to the following assembly program:
>
> ```
> lea eax, [rdi + rsi]  ; 8d 04 37
> ret                   ; c3
> ```
>
> (The input arguments are in `edi` and `esi`, the result is returned in `eax`.)



### Solution 4

```C
int add(int x, int y) {
  __asm__("add %2, %0" : "=r"(x) : "0"(x), "r"(y));
  return x;
}
```

