# KeyBoard Event

[Reference](https://johnson560.pixnet.net/blog/post/313121832-c%23-%E8%A6%96%E7%AA%97%E7%A8%8B%E5%BC%8F%E7%AF%84%E4%BE%8B--%E9%8D%B5%E7%9B%A4%E4%BA%8B%E4%BB%B6)

C# 視窗程式範例--鍵盤事件

當輸入一個字元，則此三個事件發生的順序為：
KeyDown 事件 ---> KeyPress 事件 ---> KeyUp 事件

**一、KeyPress 事件
**● 在指定物件上收到由鍵盤按鍵的字元。僅能回應案件動作，無法判斷目前按鍵是否按住或放開。
● 所按的鍵必須是具有 KeyAscii 碼的按鍵，才會觸動執行此事件。

| 有效的按鍵          | KeyAscii 碼 |
| ------------------- | ----------- |
| BackSpace (倒退鍵)  | 8           |
| Enter               | 13          |
| (空白鍵)            | 32          |
| 可顯示的鍵盤字元    | 33 ~ 126    |
| Ctrl + A ~ Ctrl + Z | 1 ~ 26      |
| Ctrl + Enter        | 10          |
| Ctrl + BackSpace    | 127         |

● 在此事件中，所按的字元可由 **e.KeyChar** 取得其鍵值。
● 在此事件中可以利用 **e.Handled** 來設定控制輸入的資料。例如：e.Handled = True 表示按下的資料不會輸入，即不會觸發 TextChanged 事件。

 ' 建立 txtInput 控制項的過濾字元 (僅能輸入數字) Private Sub txtInput_**KeyPress**(......) Handles txtInput.KeyPress   If (e.KeyChar < "0" Or e.KeyChar > "9") And (e.KeyChar <> vbBack) Then     **e.Handled = True**   End If End Sub


**二、KeyDown 事件**
● 在指定物件上偵測到鍵盤有鍵被按住。
● 可以處理 KeyPress 事件無法處理的按鍵 (例如：功能鍵、編輯鍵及組合鍵)。
  \1. **e.Alt** = True / False (判斷是否按下 Alt 鍵)
  \2. **e.Shift** = True / False (判斷是否按下 Shift 鍵)
  \3. **e.Control** = True / False (判斷是否按下 Ctrl 鍵) 
  \4. **e.KeyCode** (e.KeyCode.ToString 可輸出描述)



| 按鍵             | KeyCode 碼                                                  |
| ---------------- | ----------------------------------------------------------- |
| BackSpace        | 8                                                           |
| Enter            | 13                                                          |
| Shift            | 16                                                          |
| Ctrl             | 17                                                          |
| Alt              | 18                                                          |
| CapsLock         | 20                                                          |
| Esc              | 27                                                          |
| (空白鍵)         | 32                                                          |
| PageUp、PageDown | 33、34                                                      |
| End              | 35                                                          |
| Home             | 36                                                          |
| 左、上、右、下   | 37、38、39、40  (Keys.Left、Keys.Up、Keys.Right、Keys.Down) |
| Insert           | 45                                                          |
| Delete           | 46                                                          |
| 0 ~ 9            | 48 ~ 57                                                     |
| A ~ Z            | 65 ~ 90                                                     |
| F1 ~ F12         | 112 ~ 123                                                   |



**三、KeyUp 事件**
● 在指定物件上偵測到鍵盤上被按住的鍵已放開。
● 可以處理 KeyPress 事件無法處理的按鍵 (例如：功能鍵、編輯鍵及組合鍵)。
  (同 KeyDown 事件)