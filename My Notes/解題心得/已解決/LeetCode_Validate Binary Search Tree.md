# LeetCode:Validate Binary Search Tree:20201217:C#

[Reference](https://leetcode.com/explore/featured/card/december-leetcoding-challenge/571/week-3-december-15th-december-21st/3568/)



## Question

Given the `root` of a binary tree, *determine if it is a valid binary search tree (BST)*.

A **valid BST** is defined as follows:

- The left subtree of a node contains only nodes with keys **less than** the node's key.
- The right subtree of a node contains only nodes with keys **greater than** the node's key.
- Both the left and right subtrees must also be binary search trees.

 

**Example 1:**

![img](https://assets.leetcode.com/uploads/2020/12/01/tree1.jpg)

```
Input: root = [2,1,3]
Output: true
```

**Example 2:**

![img](https://assets.leetcode.com/uploads/2020/12/01/tree2.jpg)

```
Input: root = [5,1,4,null,null,3,6]
Output: false
Explanation: The root node's value is 5 but its right child's value is 4.
```

 

**Constraints:**

- The number of nodes in the tree is in the range `[1, 104]`.
- `-231 <= Node.val <= 231 - 1`

## My Solution



```C#
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public class Solution {
    public bool IsValidBST(TreeNode root)
    {
        if (root.left == null && root.right == null)
            return true;
        if(root.left != null)
            if(root.val <= root.left.val){return false;}
        if(root.right != null)
            if(root.val >= root.right.val){return false;}
        if(root.left != null && root.right != null)
            return IsValidBST(root.left) && IsValidBST(root.right);
        if(root.left == null){return IsValidBST(root.right);}
        return IsValidBST(root.left);
    }
}
```

第二階段測試失敗**[5,4,6,null,null,3,7]**回傳true(正確答案是false)，看來是搞錯題意了，binary tree 右方的**所有**節點的值都必須比該節點大才對，左邊亦然。

---

修改加入上層的最大/最小值

```C#
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public class Solution {
    public bool IsValidBST(TreeNode root)
    {
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
            if (root.val <= root.left.val) { return false; }
        if (root.right != null)
            if (root.val >= root.right.val) { return false; }
        if (root.left != null && root.right != null)
            return IsValidBSTLeft(root.left, root.val) && IsValidBSTRight(root.right, root.val);
        if (root.left == null) { return IsValidBSTRight(root.right, root.val); }
        return IsValidBSTLeft(root.left, root.val);
    }
    private bool IsValidBSTLeft(TreeNode root, int max)
    {
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
            if (root.val <= root.left.val) { return false; }
        if (root.right != null)
            if (root.val >= root.right.val || root.right.val >= max) { return false; }
        if (root.left != null && root.right != null)
            return IsValidBSTLeft(root.left, root.val) && IsValidBSTRight(root.right, root.val);
        if (root.left == null) { return IsValidBSTRight(root.right, root.val); }
        return IsValidBSTLeft(root.left, root.val);
    }
    private bool IsValidBSTRight(TreeNode root, int min)
    {
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
            if (root.val <= root.left.val || root.left.val <= min) { return false; }
        if (root.right != null)
            if (root.val >= root.right.val) { return false; }
        if (root.left != null && root.right != null)
            return IsValidBSTLeft(root.left, root.val) && IsValidBSTRight(root.right, root.val);
           if (root.left == null) { return IsValidBSTRight(root.right, root.val); }
        return IsValidBSTLeft(root.left, root.val);
    }
}
```

依然測試失敗，**[120,70,140,50,100,130,160,20,55,75,110,119,135,150,200]**

回傳true(正確答案是false)

120往右140往左130往左119(小於最上層的120所以應該是false)

出錯的原因是只考慮到上層的限制而沒有再往上考慮

---

再次修正，將每一層的限制記錄下來

```C#
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public class Solution {
    public bool IsValidBST(TreeNode root, List<int> maxValues = null, List<int> minValues = null)
    {
        maxValues = maxValues == null ? new List<int>() : maxValues;
        minValues = minValues == null ? new List<int>() : minValues;
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
            if (root.val <= root.left.val) { return false; }
            else if (minValues.Count >= 1)
                if (root.left.val <= minValues.Max()) { return false; }
        if (root.right != null)
            if (root.val >= root.right.val) { return false; }
            else if (maxValues.Count >= 1)
                if (root.right.val >= maxValues.Min()) { return false; }
        List<int> transMin = new List<int>(minValues);
        transMin.Add(root.val);
        List<int> transMax = new List<int>(maxValues);
        transMax.Add(root.val);
        if (root.left != null && root.right != null)
            return IsValidBST(root.left, transMax, minValues) && IsValidBST(root.right, maxValues, transMin);
        if (root.left == null) { return IsValidBST(root.right, maxValues, transMin); }
        return IsValidBST(root.left, transMax, minValues);
    }
}
```

過關。



Runtime: **104 ms**

Memory Usage: **29.8 MB**

執行時間跟記憶體使用量都是後段班



---

我感覺記憶體的部分可以再加強下，畢竟使用了大量的List，而且記錄下的資料有許多不會被用到的

```C#
public class Solution {
    public bool IsValidBST(TreeNode root, int max = Int32.MaxValue, int min = Int32.MinValue)
    {
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
            if (root.val <= root.left.val || root.left.val <= min) { return false; }
        if (root.right != null)
            if (root.val >= root.right.val || root.right.val >= max) { return false; }
        if (root.left != null && root.right != null)
            return IsValidBST(root.left, Math.Min(root.val, max), min) && IsValidBST(root.right, max, Math.Max(root.val, min));
        if (root.left == null) { return IsValidBST(root.right, max, Math.Max(root.val, min)); }
            return IsValidBST(root.left, Math.Min(root.val, max), min);
    }
```

測試未通過**[-2147483648,null,2147483647]**回傳FALSE(正確:TRUE)

這兩個節點值分別是INT32的最小和最大值，所以執行到`root.right.val >= max`就符合條件回傳false了

---

再改

```C#
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public class Solution {
    public bool IsValidBST(TreeNode root, int max = Int32.MaxValue, int min = Int32.MinValue)
    {
        if (root.left == null && root.right == null)
            return true;
        if (root.left != null)
        {
            bool secondCondition = min == Int32.MinValue ? false : root.left.val <= min;
            if (root.val <= root.left.val || secondCondition) { return false; }
        }
        if (root.right != null)
        {
            bool secondCondition = max == Int32.MaxValue ? false : root.right.val >= max;
            if (root.val >= root.right.val || secondCondition) { return false; }
        }
        if (root.left != null && root.right != null)
            return IsValidBST(root.left, Math.Min(root.val, max), min) && IsValidBST(root.right, max, Math.Max(root.val, min));
        if (root.left == null) { return IsValidBST(root.right, max, Math.Max(root.val, min)); }
            return IsValidBST(root.left, Math.Min(root.val, max), min);
    }
}
```

還是沒過，**[-2147483648,null,2147483647,-2147483648]**

看來只能區分是否為第一次進入方法了

---

再次修改

```C#
/**
 * Definition for a binary tree node.
 * public class TreeNode {
 *     public int val;
 *     public TreeNode left;
 *     public TreeNode right;
 *     public TreeNode(int val=0, TreeNode left=null, TreeNode right=null) {
 *         this.val = val;
 *         this.left = left;
 *         this.right = right;
 *     }
 * }
 */
public class Solution {
    public bool IsValidBST(TreeNode root, int max = Int32.MaxValue, int min = Int32.MinValue,
            bool maxInit = true, bool minInit = true)
        {
            if (root.left == null && root.right == null)
                return true;
            if (root.left != null)
            {
                bool secondCondition = minInit ? false : root.left.val <= min;
                if (root.val <= root.left.val || secondCondition) { return false; }
            }

            if (root.right != null)
            {
                bool secondCondition = maxInit ? false : root.right.val >= max;
                if (root.val >= root.right.val || secondCondition) { return false; }
            }

            if (root.left != null && root.right != null)
                return IsValidBST(root.left, Math.Min(root.val, max), min, false, minInit) &&
                    IsValidBST(root.right, max, Math.Max(root.val, min), maxInit, false);
            if (root.left == null) { return IsValidBST(root.right, max, Math.Max(root.val, min), maxInit, false); }
            return IsValidBST(root.left, Math.Min(root.val, max), min, false, minInit);
        }
}
```

通關

Runtime: **96 ms**

Memory Usage: **26.5 MB**



執行時間前25%

記憶體使用量前3%

~滿意~

## Better Solutions

