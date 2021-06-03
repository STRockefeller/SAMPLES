# 關於`scanf()`

[Reference: 紅燒小站](https://sites.google.com/site/9braised/fan-si/c1)



Sample1

```C
#include <stdio.h>
int main()
{
char s[200];
scanf("%[abc]",s);
printf("%s\n",s);
return 0;
}
```

其中"`[ ]` 為掃描字符集合
那上述主程式的主要功能為
輸入的每個字元 會掃描[]中的字元
若掃描到輸入的字元屬於[]中的某個字元 就提取該字元
若掃描到不屬於[]中的字元就結束提取

範例輸入: abc123
範例輸出: abc



Sample2

```C
#include <stdio.h>
int main()
{
char s[200];
scanf("%[^abc]",s);
printf("%s\n",s);
return 0;
}
```

再來scanf中 多了一個" ^ " 的符號
用字元^可以說明補集 把^字元放在[]中的第一字元時,構成其他字元組成的命令的補集合 指示scanf指接受位說明的其他字元
那該程式的功能 就跟上一個相反
輸入的每個字元 會掃描[]中的字元
若掃描到輸入的字元不屬於[]中的某個字元 就提取該字元
若掃描到屬於[]中的字元就結束提取

範例輸入: 123abc
範例輸出: 123

那有了這個概念
也可以做輸入空白會提早結束讀取的錯誤



Sample3

```C
#include <stdio.h>
int main()
{
char s[200];
scanf("%[^\n]",s);
printf("%s\n",s);
return 0;
}
```

該程式值到我輸入換行的功能 也就是enter鍵才會結束讀取
就不會碰到空白提早結束

那[]掃描字符集合 接受這樣的寫法 [A-Z]
意思是掃描A~Z的字元

要注意的是 掃描字元集合裡 方括號兩邊不能有空格 如[ abc ] or [ ^abc ] 這些是錯的寫法 不然空格也會算再裡面 導致輸入碰到空格一樣會提早結束提取的問題