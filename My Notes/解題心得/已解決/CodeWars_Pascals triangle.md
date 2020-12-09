# CodeWars:Pascal's Triangle:20201124:Python

[Reference](https://www.codewars.com/kata/5226eb40316b56c8d500030f/python)



## Question

In mathematics, Pascal's triangle is a triangular array of the binomial coefficients expressed with formula `(n k) = n!/(n-k)!`, where `n` denotes a row of the triangle, and `k` is a position of a term in the row.

![Pascal's Triangle](http://upload.wikimedia.org/wikipedia/commons/0/0d/PascalTriangleAnimated2.gif)

You can read [Wikipedia article on Pascal's Triangle](http://en.wikipedia.org/wiki/Pascal's_triangle) for more information.

### Task

Write a function that, given a depth `n`, returns `n` top rows of Pascal's Triangle flattened into a one-dimensional list/array.

`n` guarantees that terms of the Triangle won't overflow.

### Example:

```
n = 1: [1]
n = 2: [1,  1, 1]
n = 4: [1,  1, 1,  1, 2, 1,  1, 3, 3, 1]
```

## My Solution

python 測試程式如下;

```python
Test.assert_equals(pascals_triangle(1), [1],"1 level triangle incorrect");
Test.assert_equals(pascals_triangle(2), [1,1,1],"2 level triangle incorrect");
Test.assert_equals(pascals_triangle(3), [1,1,1,1,2,1],"3 level triangle incorrect");
```



目前的想法是將帕斯卡三角形的每一層分開計算出來，最後整合。

查了下Python版的`addRange()`方法 [W3School:Python - Add List Items](https://www.w3schools.com/python/python_lists_add.asp)

應該是使用`extend()`

```python
thislist = ["apple", "banana", "cherry"]
tropical = ["mango", "pineapple", "papaya"]
thislist.extend(tropical)
print(thislist)
```



```python
def pascals_triangle(n):
    res = []
    for i in range(n):
        res.extend(pasGenerate(i+1))
    return res
    
def pasGenerate(n):
    if n == 1:
        return [1]
    if n == 2:
        return [1,1]
    res = []
    prePas = pasGenerate(n-1)
    for i in range(len(prePas)-1):
        res.append(prePas[i]+prePas[i+1])
    res = [1] + res + [1]
    return res
```

過關，錯誤一次第四行的`res.extend(pasGenerate(i+1))`一開始是寫成`res.append(pasGenerate(i+1))`輸出格式會變成數字陣列的陣列而非數字的陣列不符合要求。

有想過使用公式解出第n層的內容物，但是反正都要輸出1~n層的內容了，乾脆用最好理解的方式一層層算下來。(而且公式也早就記不清楚了)

## Better Solutions



### Solution 1

```python
def pascals_triangle(n):
  if n == 1:
      return [1]
  prev = pascals_triangle(n - 1)
  return prev + [1 if i == 0 or i == n -1 else prev[-i] + prev[-(i + 1)] 
              for i in range(n)]
```



### Solution 2

```Python
from scipy.special import comb
def pascals_triangle(n):
    return [comb(a, b, exact=True) for a in range(n) for b in range(a + 1)]   
```

