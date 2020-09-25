# LeetCode:2. Add Two Numbers:20200925:C#

[Reference](https://leetcode.com/problems/add-two-numbers/)



## Question

You are given two **non-empty** linked lists representing two non-negative integers. The digits are stored in **reverse order** and each of their nodes contain a single digit. Add the two numbers and return it as a linked list.

You may assume the two numbers do not contain any leading zero, except the number 0 itself.

**Example:**

```
Input: (2 -> 4 -> 3) + (5 -> 6 -> 4)
Output: 7 -> 0 -> 8
Explanation: 342 + 465 = 807.
```

## My Solution

申請個帳號到LeetCode換個口味試試看

恰巧對linked list 不是很熟，這題應該會是不錯的練習機會

1. 兩數相加
2. 判斷進位
3. 若有任一方的next不是null，則呼叫next繼續相加
4. 兩方的next都是null則停止並輸出結果(更正，兩方的next都是null且沒有進位才停止)

```C#
/**
 * Definition for singly-linked list.
 * public class ListNode {
 *     public int val;
 *     public ListNode next;
 *     public ListNode(int val=0, ListNode next=null) {
 *         this.val = val;
 *         this.next = next;
 *     }
 * }
 */
public class Solution {
        public ListNode AddTwoNumbers(ListNode l1, ListNode l2) => subListNode(l1, l2, 0);
        private ListNode subListNode(ListNode l1, ListNode l2, int carry)
        {
            ListNode sum = new ListNode();
            if (l1 != null && l2 != null)
                sum.val = l1.val + l2.val;
            else if (l1 == null && l2 != null)
                sum.val = l2.val;
            else if (l2 == null && l1 != null)
                sum.val = l1.val;
            sum.val += carry;
            carry = sum.val / 10;
            sum.val %= 10;
            l1 = l1 == null ? new ListNode(0) : l1;
            l2 = l2 == null ? new ListNode(0) : l2;
            if (l1.next != null || l2.next != null || carry != 0)
                sum.next = subListNode(l1.next, l2.next, carry);
            return sum;
        }
}
```

 ![](https://i.imgur.com/YWHtVHJ.png)

前面的錯誤幾乎都是在處理null判定(例如99+1)，因為null沒有next可以呼叫

我以為我的作法是正攻法，但意外地看起來似乎落在C#解答的前段班，我真行。

## Better Solutions

不像code wars能直接顯示同語言的解答這點比較可惜，看完第一頁沒有人用C#解答。

不過這邊的討論倒是比code wars熱烈地多。

### Solution 1

```C++
class Solution {
public:
    ListNode* addTwoNumbers(ListNode* l1, ListNode* l2) {
        int sum=0;
        ListNode *l3=NULL;
        ListNode **node=&l3;
        while(l1!=NULL||l2!=NULL||sum>0)
        {
            if(l1!=NULL)
            {
                sum+=l1->val;
                l1=l1->next;
            }
            if(l2!=NULL)
            {
                sum+=l2->val;
                l2=l2->next;
            }
            (*node)=new ListNode(sum%10);
            sum/=10;
            node=&((*node)->next);
        }        
        return l3;
    }
};
```

C++迴圈解法



### Solution 2

```python
class Solution:
    def addTwoNumbers(self, l1, l2 ,c = 0):
        """
        :type l1: ListNode
        :type l2: ListNode
        :rtype: ListNode
        """
        val = l1.val + l2.val + c
        c = val // 10
        ret = ListNode(val % 10 ) 
        
        if (l1.next != None or l2.next != None or c != 0):
            if l1.next == None:
                l1.next = ListNode(0)
            if l2.next == None:
                l2.next = ListNode(0)
            ret.next = self.addTwoNumbers(l1.next,l2.next,c)
        return ret
```

python遞迴解，和我的寫法滿接近的，是說原來改輸入條件也可以通過，那我就不用特地多寫一個method了



### Solution 3

```python
# Definition for singly-linked list.
# class ListNode(object):
#     def __init__(self, x):
#         self.val = x
#         self.next = None

class Solution(object):
    def addTwoNumbers(self, l1, l2):
        """
        :type l1: ListNode
        :type l2: ListNode
        :rtype: ListNode
        """
        result = ListNode(0)
        result_tail = result
        carry = 0
                
        while l1 or l2 or carry:            
            val1  = (l1.val if l1 else 0)
            val2  = (l2.val if l2 else 0)
            carry, out = divmod(val1+val2 + carry, 10)    
                      
            result_tail.next = ListNode(out)
            result_tail = result_tail.next                      
            
            l1 = (l1.next if l1 else None)
            l2 = (l2.next if l2 else None)
               
        return result.next
```

python 63ms 解答，也太快了吧。

把l1,l2直接拿去作條件，看來python的條件式應該比較寬鬆(非null就是true?)

