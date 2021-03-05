# LeetCode:Intersection of Two Linked Lists:20210305:C#

[Reference](https://leetcode.com/explore/challenge/card/march-leetcoding-challenge-2021/588/week-1-march-1st-march-7th/3660/)



## Question

Write a program to find the node at which the intersection of two singly linked lists begins.

For example, the following two linked lists:

[![img](https://assets.leetcode.com/uploads/2018/12/13/160_statement.png)](https://assets.leetcode.com/uploads/2018/12/13/160_statement.png)

begin to intersect at node c1.

 

**Example 1:**

[![img](https://assets.leetcode.com/uploads/2020/06/29/160_example_1_1.png)](https://assets.leetcode.com/uploads/2020/06/29/160_example_1_1.png)

```
Input: intersectVal = 8, listA = [4,1,8,4,5], listB = [5,6,1,8,4,5], skipA = 2, skipB = 3
Output: Reference of the node with value = 8
Input Explanation: The intersected node's value is 8 (note that this must not be 0 if the two lists intersect). From the head of A, it reads as [4,1,8,4,5]. From the head of B, it reads as [5,6,1,8,4,5]. There are 2 nodes before the intersected node in A; There are 3 nodes before the intersected node in B.
```

 

**Example 2:**

[![img](https://assets.leetcode.com/uploads/2020/06/29/160_example_2.png)](https://assets.leetcode.com/uploads/2020/06/29/160_example_2.png)

```
Input: intersectVal = 2, listA = [1,9,1,2,4], listB = [3,2,4], skipA = 3, skipB = 1
Output: Reference of the node with value = 2
Input Explanation: The intersected node's value is 2 (note that this must not be 0 if the two lists intersect). From the head of A, it reads as [1,9,1,2,4]. From the head of B, it reads as [3,2,4]. There are 3 nodes before the intersected node in A; There are 1 node before the intersected node in B.
```

 

**Example 3:**

[![img](https://assets.leetcode.com/uploads/2018/12/13/160_example_3.png)](https://assets.leetcode.com/uploads/2018/12/13/160_example_3.png)

```
Input: intersectVal = 0, listA = [2,6,4], listB = [1,5], skipA = 3, skipB = 2
Output: null
Input Explanation: From the head of A, it reads as [2,6,4]. From the head of B, it reads as [1,5]. Since the two lists do not intersect, intersectVal must be 0, while skipA and skipB can be arbitrary values.
Explanation: The two lists do not intersect, so return null.
```

 

**Notes:**

- If the two linked lists have no intersection at all, return `null`.
- The linked lists must retain their original structure after the function returns.
- You may assume there are no cycles anywhere in the entire linked structure.
- Each value on each linked list is in the range `[1, 10^9]`.
- Your code should preferably run in O(n) time and use only O(1) memory.

## My Solution

C#給的初始程式碼

```C#
/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int x) { val = x; }
 * }
 */
public class Solution {
    public ListNode GetIntersectionNode(ListNode headA, ListNode headB) {
        
    }
}
```

題目的第一個範例讓我有點搞不懂

```
Input: intersectVal = 8, listA = [4,1,8,4,5], listB = [5,6,1,8,4,5], skipA = 2, skipB = 3
```

交接點是`8`而非`1`，以C或C++來想的話linked list結構中節點會指向下一個節點的address，所以可能兩個`1`是在不同的記憶體位置，但在C#中，就我的印象中是沒有指標的概念的。那又該如何區分呢？

先試著把題目的結構寫出來

```C#
            ListNode intersection = new ListNode(8)
            {
                next = new ListNode(4)
                {
                    next = new ListNode(5)
                }
            };
            ListNode headA = new ListNode(4) 
            { 
                next = new ListNode(1) 
                { 
                    next = intersection
                }
            };
            ListNode headB = new ListNode(5)
            {
                next = new ListNode(6)
                {
                    next = new ListNode(1)
                    {
                        next = intersection
                    }
                }
            };
```

不太肯定行不行，再寫個測試

```
var a = intersection;
var b = intersection;
Console.WriteLine(a == b ? "YES" : "NO");
var c = new ListNode(1) { next = intersection };
var d = new ListNode(1) { next = intersection };
Console.WriteLine(c == d ? "YES" : "NO");
```

結果

```
YES
NO
```

看來是沒問題，只要直接用`==`就能判斷是不是同一個node了

---

題目搞懂可以正式開始解題了

```
Your code should preferably run in O(n) time and use only O(1) memory.
```

這個條件我先無視，隨緣達成。

* 參數提供的兩個linked list都可以拆分成兩個部分，分別是交接前和交接後的部分(當然兩者都有可能為0)

* 由於交接的部分一定在後半段，若能從後方往前找是最輕鬆的做法，但linked list要這樣做實在不現實

* 由前方往後找的情況下，必須先對齊兩個linked list，不然無法正確比較節點
  * 對齊的方式就是把比較長的linked list的頭部忽略掉(因此需先計算長度)

實作

```C#
    public class Solution
    {
        public ListNode GetIntersectionNode(ListNode headA, ListNode headB)
        {
            int lengthA = listLength(0, headA);
            int lengthB = listLength(0, headB);
            ListNode compA = lengthA > lengthB ? subNode(lengthA - lengthB, headA) : headA;
            ListNode compB = lengthB > lengthA ? subNode(lengthB - lengthA, headB) : headB;
            return compare(compA,compB);
            int listLength(int count, ListNode node)
            {
                count++;
                if (node.next == null) { return count; }
                return listLength(count, node.next);
            }
            ListNode subNode(int skip, ListNode node)
            {
                if (skip == 0) { return node; }
                return subNode(skip - 1, node.next);
            }
            ListNode compare(ListNode nodeA, ListNode nodeB)
            {
                if (nodeA == nodeB) { return nodeA; }
                if (nodeA.next == null) { return null; }
                return compare(nodeA.next, nodeB.next);
            }
        }
    }
```

結果 runtime error

```
Runtime Error Message:
System.NullReferenceException: Object reference not set to an instance of an object
Line 21: Solution.<GetIntersectionNode>g__listLength|0_0 (System.Int32 count, ListNode node) in Solution.cs
Line 13: Solution.GetIntersectionNode (ListNode headA, ListNode headB) in Solution.cs
Line 61: __Driver__.Main (System.String[] args) in __Driver__.cs
[ERROR] FATAL UNHANDLED EXCEPTION: System.NullReferenceException: Object reference not set to an instance of an object
Line 21: Solution.<GetIntersectionNode>g__listLength|0_0 (System.Int32 count, ListNode node) in Solution.cs
Line 13: Solution.GetIntersectionNode (ListNode headA, ListNode headB) in Solution.cs
Line 61: __Driver__.Main (System.String[] args) in __Driver__.cs
Last executed input:
0
[]
[]
0
0
```

我猜應該是沒有考慮到`headA`或`headB`本身就是`null`的情況

修改如下

```C#
    public class Solution
    {
        public ListNode GetIntersectionNode(ListNode headA, ListNode headB)
        {
            if (headA == null || headB == null) { return null; }
            int lengthA = listLength(0, headA);
            int lengthB = listLength(0, headB);
            ListNode compA = lengthA > lengthB ? subNode(lengthA - lengthB, headA) : headA;
            ListNode compB = lengthB > lengthA ? subNode(lengthB - lengthA, headB) : headB;
            return compare(compA,compB);
            int listLength(int count, ListNode node)
            {
                count++;
                if (node.next == null) { return count; }
                return listLength(count, node.next);
            }
            ListNode subNode(int skip, ListNode node)
            {
                if (skip == 0) { return node; }
                return subNode(skip - 1, node.next);
            }
            ListNode compare(ListNode nodeA, ListNode nodeB)
            {
                if (nodeA == nodeB) { return nodeA; }
                if (nodeA.next == null) { return null; }
                return compare(nodeA.next, nodeB.next);
            }
        }
    }
```

Runtime: **128 ms** --贏過82.69%

Memory Usage: **36.7 MB** --贏過25.72%



時間方面，用了三個遞迴方法，但依然是O(n)，結果倒是意外的在前段班

空間方面我看應該有達成O(1)吧，不過宣告了一卡車的變數，結果這麼糟也不太意外了



## Better Solutions

