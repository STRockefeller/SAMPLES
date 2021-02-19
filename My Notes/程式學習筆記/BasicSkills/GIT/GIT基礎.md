# Git 基礎

[Reference](https://progressbar.tw/posts/1)

這個在github持續更新的筆記已經寫了半年多了(2020下半年到現在2021二月中旬)，才在寫GIT的筆記倒是頗奇怪。

其原因還得歸咎於自己的健忘，當初粗略的學完git後一直都在使用git kraken這種第三方工具，以至於現在原本的git怎麼操作都忘的差不多了。現在是時候從頭撿起來，並做好筆記了。



## 安裝

[https://git-scm.com/download/win](https://git-scm.com/download/win)

安裝中有很多設定，反正就按照需求勾選，就算全部都直接NEXT也沒問題。



## 指令

### 一般指令

就基本上和 Command line 類似，但也有些許不同

一樣可以透過help查看

> $ help
> GNU bash, version 4.4.23(1)-release (x86_64-pc-msys)
> These shell commands are defined internally.  Type `help' to see this list.
> Type `help name' to find out more about the function `name'.
> Use `info bash' to find out more about the shell in general.
> Use `man -k' or `info' to find out more about commands not in this list.
>
> A star (*) next to a name means that the command is disabled.
>
>  job_spec [&]                                 history [-c] [-d offset] [n] or history ->
>  (( expression ))                             if COMMANDS; then COMMANDS; [ elif COMMAN>
>  . filename [arguments]                       jobs [-lnprs] [jobspec ...] or jobs -x co>
>  :                                            kill [-s sigspec | -n signum | -sigspec] >
>  [ arg... ]                                   let arg [arg ...]
>  [[ expression ]]                             local [option] name[=value] ...
>  alias [-p] [name[=value] ... ]               logout [n]
>  bg [job_spec ...]                            mapfile [-d delim] [-n count] [-O origin]>
>  bind [-lpsvPSVX] [-m keymap] [-f filename]>  popd [-n] [+N | -N]
>  break [n]                                    printf [-v var] format [arguments]
>  builtin [shell-builtin [arg ...]]            pushd [-n] [+N | -N | dir]
>  caller [expr]                                pwd [-LPW]
>  case WORD in [PATTERN [| PATTERN]...) COMM>  read [-ers] [-a array] [-d delim] [-i tex>
>  cd [-L|[-P [-e]] [-@]] [dir]                 readarray [-n count] [-O origin] [-s coun>
>  command [-pVv] command [arg ...]             readonly [-aAf] [name[=value] ...] or rea>
>  compgen [-abcdefgjksuv] [-o option] [-A ac>  return [n]
>  complete [-abcdefgjksuv] [-pr] [-DE] [-o o>  select NAME [in WORDS ... ;] do COMMANDS;>
>  compopt [-o|+o option] [-DE] [name ...]      set [-abefhkmnptuvxBCHP] [-o option-name]>
>  continue [n]                                 shift [n]
>  coproc [NAME] command [redirections]         shopt [-pqsu] [-o] [optname ...]
>  declare [-aAfFgilnrtux] [-p] [name[=value]>  source filename [arguments]
>  dirs [-clpv] [+N] [-N]                       suspend [-f]
>  disown [-h] [-ar] [jobspec ... | pid ...]    test [expr]
>  echo [-neE] [arg ...]                        time [-p] pipeline
>  enable [-a] [-dnps] [-f filename] [name ..>  times
>  eval [arg ...]                               trap [-lp] [[arg] signal_spec ...]
>  exec [-cl] [-a name] [command [arguments .>  true
>  exit [n]                                     type [-afptP] name [name ...]
>  export [-fn] [name[=value] ...] or export >  typeset [-aAfFgilnrtux] [-p] name[=value]>
>  false                                        ulimit [-SHabcdefiklmnpqrstuvxPT] [limit]
>  fc [-e ename] [-lnr] [first] [last] or fc >  umask [-p] [-S] [mode]
>  fg [job_spec]                                unalias [-a] name [name ...]
>  for NAME [in WORDS ... ] ; do COMMANDS; do>  unset [-f] [-v] [-n] [name ...]
>  for (( exp1; exp2; exp3 )); do COMMANDS; d>  until COMMANDS; do COMMANDS; done
>  function name { COMMANDS ; } or name () { >  variables - Names and meanings of some sh>
>  getopts optstring name [arg]                 wait [-n] [id ...]
>  hash [-lr] [-p pathname] [-dt] [name ...]    while COMMANDS; do COMMANDS; done
>  help [-dms] [pattern ...]                    { COMMANDS ; }

下面列一些代表

#### pwd

顯示當前位置

> $ pwd
> /c/Users/admin/desktop/TESTGIT

補充: 在CMD 可以透過`cd ,`查詢

> C:\Users\admin\Desktop\TESTGIT>cd ,
> C:\Users\admin\Desktop\TESTGIT

#### cd

基本上和CMD一樣

不過有一些方便的小技巧，比如輸入到中途按下`tab`可以自動帶入指令(如果多於一個符合的指令無效)，按兩下`tab`會顯示開頭符合的所有指令。

比如我輸入 cd d接著按想下`tab`

> admin@NBP0572 MINGW64 ~
> $ cd D
> Desktop/   Documents/ Downloads/

試著導到路徑

> admin@NBP0572 MINGW64 ~
> $ pwd
> /c/Users/admin
>
> admin@NBP0572 MINGW64 ~
> $ cd Desktop/
>
> admin@NBP0572 MINGW64 ~/Desktop
> $ cd testgit
>
> admin@NBP0572 MINGW64 ~/Desktop/testgit (master)

在CMD中如下

> C:\Users\admin>cd desktop
>
> C:\Users\admin\Desktop>cd testgit
>
> C:\Users\admin\Desktop\TESTGIT>



現在有個小麻煩，我不知道怎麼回退，輸入絕對路徑和相對路徑都不行

> admin@NBP0572 MINGW64 ~
> $ cd Desktop/
>
> admin@NBP0572 MINGW64 ~/Desktop
> $ cd TESTGIT/
>
> admin@NBP0572 MINGW64 ~/Desktop/TESTGIT (master)
> $ cd ..\..\
>
> > pwd
> > bash: cd: ....pwd: No such file or directory
>
> admin@NBP0572 MINGW64 ~/Desktop/TESTGIT (master)
> $ cd c:\
> > pwd
> > bash: cd: c:pwd: No such file or directory
>
> admin@NBP0572 MINGW64 ~/Desktop/TESTGIT (master)
> $ cd c
> bash: cd: c: No such file or directory
>
> admin@NBP0572 MINGW64 ~/Desktop/TESTGIT (master)



#### ls

顯示當前目錄下的檔案 (Command Line 沒有)

> ls
> Readme.txt  ewqf

相當於CMD 的 `dir`

> C:\Users\admin\Desktop\TESTGIT>dir
>  磁碟區 C 中的磁碟是 WIN10
>  磁碟區序號:  5E6B-3299
>
>  C:\Users\admin\Desktop\TESTGIT 的目錄
>
> 2021/02/17  上午 10:59    <DIR>          .
> 2021/02/17  上午 10:59    <DIR>          ..
> 2021/02/17  上午 10:44    <DIR>          ewqf
> 2021/02/17  上午 09:54                12 Readme.txt
>                1 個檔案              12 位元組
>                3 個目錄  118,027,829,248 位元組可用



#### clear

清空文字，沒啥好演示的



### git 指令

一步步按順序來

建立測試用資料夾和檔案

TESTGIT資料夾，內新增ReadMe.txt，內容+initialization

#### git init

初始化

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT
> $ git init
> Initialized empty Git repository in C:/Users/admin/Desktop/TESTGIT/.git/

這個步驟完成後會產生`.git`資料夾(HIDDEN)

然後MINGW上的路徑會多出(Master)

像這樣

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $

#### git add

加入版本管理

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git add ReadMe.txt

也可以用`$ git add -f --all `將目錄底下的檔案全部加入



#### git commit

如果按找上方步驟來的話，這時候git還沒存入任何資訊

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git log
> fatal: your current branch 'master' does not have any commits yet

必須先commit一次把最初的資料記錄起來。(git kraken也是這樣做的)



試著輸入`git commit`

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git commit
> hint: Waiting for your editor to close the file... "C:\Users\admin\AppData\Local\Programs\Microsoft VS Code\Code.exe" --wait: C:\Users\admin\AppData\Local\Programs\Microsoft VS Code\Code.exe: No such file or directory
> error: There was a problem with the editor '"C:\Users\admin\AppData\Local\Programs\Microsoft VS Code\Code.exe" --wait'.
> Please supply the message using either -m or -F option.

總之會被提醒需要附帶message(VSCODE 是我安裝時選擇的EDITOR 預設我記得是VIM)

再一次，成功了

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git commit  -m "initialization"
> [master (root-commit) a608c43] initialization
>  1 file changed, 1 insertion(+)
>  create mode 100644 ReadMe.txt

註:如果出現訊息`***Please tell me who you are.`，則必須去設定基本資料

`$ git config --global user.email "<你的Email信箱>"`

`$ git config --global user.name "<你的名稱>"`

我這邊好像直接幫我使用了我在git kraken中設定的資料，所以沒有出現這個情況

commit後再看log就有資訊了(Email被我改過了)

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git log
> commit a608c43481f7427b970e5a0facf03ddce11cf78f (HEAD -> master)
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:40:15 2021 +0800



試著修改ReadMe.txt再次commit

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git add -f --all
>
> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git commit -m "first change"
> [master c48eb70] first change
>  1 file changed, 1 insertion(+), 1 deletion(-)
>
> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git log
> commit c48eb7078faa5d49eec51c5623121746dde8f64e (HEAD -> master)
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:50:49 2021 +0800
>
>     first change
>
> commit a608c43481f7427b970e5a0facf03ddce11cf78f
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:40:15 2021 +0800
>
>     initialization



#### git log

顯示修改紀錄(過去的)



#### git show

顯示變更內容譬如

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git log
> commit c48eb7078faa5d49eec51c5623121746dde8f64e (HEAD -> master)
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:50:49 2021 +0800
>
>     first change
>
> commit a608c43481f7427b970e5a0facf03ddce11cf78f
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:40:15 2021 +0800
>
>     initialization
>
> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git show c48e
> commit c48eb7078faa5d49eec51c5623121746dde8f64e (HEAD -> master)
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:50:49 2021 +0800
>
>     first change
>
> diff --git a/ReadMe.txt b/ReadMe.txt
> index cf49e83..3c26ed4 100644
> --- a/ReadMe.txt
> +++ b/ReadMe.txt
> @@ -1 +1 @@
> -initialization
> \ No newline at end of file
> +first change
> \ No newline at end of file

NOTICE:版本號至少要輸入前四碼(可以更多)



#### git reset

回朔到之前的版本

一般會加上參數`--hard`，否則只會回朔版本而資料保持原樣，如果忘記加`--hard`就再下一次指令就好。

>admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
>$ git reset a608
>Unstaged changes after reset:
>M       ReadMe.txt
>
>admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
>$ git reset --hard a608
>HEAD is now at a608c43 initialization



#### git reflog

查看往後的紀錄

上一步驟做完(經過reset)會發現log變少了

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git log
> commit a608c43481f7427b970e5a0facf03ddce11cf78f (HEAD -> master)
> Author: STRockefeller <testGit@gmail.com>
> Date:   Wed Feb 17 11:40:15 2021 +0800
>
>     initialization

原因是`git log`只能查到之前的紀錄。

這時就可以使用`git reflog`來查詢

> admin@NBP0572 MINGW64 /c/users/admin/Desktop/TESTGIT (master)
> $ git reflog
> a608c43 (HEAD -> master) HEAD@{0}: reset: moving to a608
> a608c43 (HEAD -> master) HEAD@{1}: reset: moving to a608
> c48eb70 HEAD@{2}: commit: first change
> a608c43 (HEAD -> master) HEAD@{3}: commit (initial): initialization

就可以知道少掉的是c48e.....



#### 更多 git 指令

[Reference](https://hellojs-tw.github.io/git-101/cheat-sheet.html)

[Reference](https://www.freecodecamp.org/news/10-important-git-commands-that-every-developer-should-know/)