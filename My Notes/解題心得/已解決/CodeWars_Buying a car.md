# CodeWars:Buying a car:20201007:TypeScript

[Reference](https://www.codewars.com/kata/554a44516729e4d80b000012)



## Question

Let us begin with an example:

A man has a rather old car being worth $2000. He saw a secondhand car being worth $8000. He wants to keep his old car until he can buy the secondhand one.

He thinks he can save $1000 each month but the prices of his old car and of the new one decrease of 1.5 percent per month. Furthermore this percent of loss increases of `0.5` percent at the end of every two months. Our man finds it difficult to make all these calculations.

**Can you help him?**

How many months will it take him to save up enough money to buy the car he wants, and how much money will he have left over?

**Parameters and return of function:**

```
parameter (positive int or float, guaranteed) startPriceOld (Old car price)
parameter (positive int or float, guaranteed) startPriceNew (New car price)
parameter (positive int or float, guaranteed) savingperMonth 
parameter (positive float or int, guaranteed) percentLossByMonth

nbMonths(2000, 8000, 1000, 1.5) should return [6, 766] or (6, 766)
```

### Detail of the above example:

```
end month 1: percentLoss 1.5 available -4910.0
end month 2: percentLoss 2.0 available -3791.7999...
end month 3: percentLoss 2.0 available -2675.964
end month 4: percentLoss 2.5 available -1534.06489...
end month 5: percentLoss 2.5 available -395.71327...
end month 6: percentLoss 3.0 available 766.158120825...
return [6, 766] or (6, 766)
```

where `6` is the number of months at **the end of which** he can buy the new car and `766` is the nearest integer to `766.158...` (rounding `766.158` gives `766`).

**Note:**

Selling, buying and saving are normally done at end of month. Calculations are processed at the end of each considered month but if, by chance from the start, the value of the old car is bigger than the value of the new one or equal there is no saving to be made, no need to wait so he can at the beginning of the month buy the new car:

```
nbMonths(12000, 8000, 1000, 1.5) should return [0, 4000]
nbMonths(8000, 8000, 1000, 1.5) should return [0, 0]
```

We don't take care of a deposit of savings in a bank:-)

## My Solution

整理一下題目

1. 存的錢是固定的，累加就好
2. 舊車和二手車折舊相同，所以直接用價差計算就好
3. 雖說每兩個月增加折舊率但範例第二個月就增加了
4. 剩下的錢要用四捨五入

```typescript
export function nbMonths(startPriceOld: number, startPriceNew: number, savingperMonth: number, percentLossByMonth: number): number[] {
    if (startPriceOld >= startPriceNew)
        return [0,startPriceOld-startPriceNew]
    var deposit: number = 0;
    var spread: number = startPriceNew - startPriceOld;
    for (var monthsCount: number = 1; ; monthsCount++) {
        if (monthsCount % 2 === 0)
            percentLossByMonth += 0.5;
        deposit += savingperMonth;
        spread *= (1 - percentLossByMonth * 0.01);
        if (deposit >= spread)
            return [monthsCount,Math.round(deposit-spread)];
    }
}
```

直接照著情況模擬就通關了，好像沒有練習到什麼TS

## Better Solutions



### Solution 1

```typescript
export function nbMonths(startPriceOld: number, startPriceNew: number, savingperMonth: number, percentLossByMonth: number): number[] {
  var months = 0, moneySaved = 0;
  while (startPriceNew > startPriceOld + moneySaved) {
    moneySaved += savingperMonth;
    startPriceOld -= (startPriceOld * (percentLossByMonth / 100));
    startPriceNew -= (startPriceNew * (percentLossByMonth / 100));
    months++;
    if (months % 2 == 1) {
      percentLossByMonth += .5;
    }
  }
  return [months, Math.round(startPriceOld + moneySaved - startPriceNew)];
}
```

也是模擬現實情況的解法，話說還有.5這種寫法喔