# CodeWars:Going to the cinema:20210209:dart

[Reference](https://www.codewars.com/kata/562f91ff6a8b77dfe900006e)



## Question

My friend John likes to go to the cinema. He can choose between system A and system B.

```
System A : he buys a ticket (15 dollars) every time
System B : he buys a card (500 dollars) and a first ticket for 0.90 times the ticket price, 
then for each additional ticket he pays 0.90 times the price paid for the previous ticket.
```

#### Example:

If John goes to the cinema 3 times:

```
System A : 15 * 3 = 45
System B : 500 + 15 * 0.90 + (15 * 0.90) * 0.90 + (15 * 0.90 * 0.90) * 0.90 ( = 536.5849999999999, no rounding for each ticket)
```

John wants to know how many times he must go to the cinema so that the *final result* of System B, when rounded *up* to the next dollar, will be cheaper than System A.

The function `movie` has 3 parameters: `card` (price of the card), `ticket` (normal price of a ticket), `perc` (fraction of what he paid for the previous ticket) and returns the first `n` such that

```
ceil(price of System B) < price of System A.
```

More examples:

```
movie(500, 15, 0.9) should return 43 
    (with card the total price is 634, with tickets 645)
movie(100, 10, 0.95) should return 24 
    (with card the total price is 235, with tickets 240)
```

## My Solution

kyu 7 的簡單題目，剛學了dart來試試手

ANS1

```dart
int movie(int card, int ticket, double perc) 
{
  num pow(num x,num ex)
  {
    num res = 1;
    for(int i=0;i<ex;i++)
      res*=x;
    return res;
  }
  int systemA(int times) => ticket*times;
  double systemB(int times)
  {
    double res = 0.0 + card;
    for(int i=1; i<=times; i++)
    {
      res += ticket * pow(perc,i);
    }
    return res;
  }
  int times = 1;
  while(systemB(times).ceil()>=systemA(times))
    times++;
  return times;
}
```

未通過，進階測試timeout。

紀錄一下心得

1. 意外的C#的Lambda Expression行得通`int systemA(int times) => ticket*times;`
2. 方法必須寫在參考的前面才能被呼叫(我不確定是dart本來就該這樣寫還是dartPad才有這種限制)

顯然每次回圈都要重新計算systemB浪費太多時間了



---

ANS2

```dart
int movie(int card, int ticket, double perc)
{
  num pow(num x,num ex)
  {
    num res = 1;
    for(int i=0;i<ex;i++)
      res*=x;
    return res;
  }
  int times = 1;
  int systemA = 0;
  double systemB = 0.0+card;
  do
  {
    systemA += ticket;
    systemB += ticket*pow(perc,times);
    times++;
  }while(systemB.ceil()>=systemA);
  return times-1;
}
```

改成每次回圈進行累加，依然time out，kyu 7錯兩次有點丟臉啊。



---

試著先攤平`card`的金額再做計算

```dart
int movie(int card, int ticket, double perc)
{
  List<double> rate(int count,double total,double innerPerc)
  {
    if(count == 0){return [total,innerPerc];}
    return rate(count-1,total+innerPerc,innerPerc*perc);
  }
  int times = card~/ticket;
  int systemA = ticket*times;
  List<double> firstRate = rate(times,0,perc);
  double systemB = 0.0+card + ticket*firstRate[0];
  double currentPerc = firstRate[1];
  do
  {
    systemA += ticket;
    systemB += ticket*currentPerc;
    currentPerc*=perc;
    times++;
  }while(systemB.ceil()>=systemA);
  return times;
}
```

這次沒有time out 變成over flow

再進一步減少計算量，假設perc<0，第N次買票後票價會變得非常小甚至趨近於0，這時就沒有計算的必要了，我先設定<0.1就不計算好了。

```dart
void main()
{
  print(movie(2500, 20, 0.9));
}
int movie(int card, int ticket, double perc)
{
  List<double> rate(int count,double total,double innerPerc)
  {
    if(count == 0){return [total,innerPerc];}
    if(innerPerc < 0.001){return[total,0];}
    return rate(count-1,total+innerPerc,innerPerc*perc);
  }
  int times = card~/ticket;
  int systemA = ticket*times;
  List<double> firstRate = rate(times,0,perc);
  double systemB = 0.0+card + ticket*firstRate[0];
  double currentPerc = firstRate[1];
  do
  {
    systemA += ticket;
    systemB += ticket*currentPerc;
    currentPerc*=perc;
    times++;
  }while(systemB.ceil()>=systemA);
  return times;
}
```

最後是設定到0.001才夠精準。通關。

## Better Solutions

用dart解答的使用者只占1%而已，挺冷門的。

### Solution1

```dart
import 'dart:math';

int movie(int card, int ticket, double perc) {
  var n = 1;

  var price = card + ticket * perc;

  while (systemA(ticket, n) <= price.ceil()) {
    price += ticket * pow(perc, n++) * perc;
  }

  return n;
}

int systemA(int price, int count) => price * count;
```

`import 'dart:math'`之後就能使用`pow`方法了

但看來和我的第二次解法差不多，為啥不會time out?

