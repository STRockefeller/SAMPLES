# Navigator

這個`Navigator`指的不是常出現在網頁上方或左方的那種Navigator，而是Flutter中的一個`Class`，但是他們功能是相近的，就是把使用者導到正確的頁面。

[官方API](https://api.flutter.dev/flutter/widgets/Navigator-class.html)

在APP中打開許多頁面的情況下，當我們返回的時候，它會先後退到上一個打開的頁面，然後一層一層後退，這就是一個`stack`。而在Flutter中，則是由`Navigator`來負責管理維護這些頁面`stack`。

通常我們我們在構建APP的時候並沒有手動去創建一個`Navigator`，也能進行頁面導航，這又是為什麼呢。

這個`Navigator`正是`MaterialApp`為我們提供的。但是如果`home`，`routes`，`onGenerateRoute`和`onUnknownRoute`都為null，並且`builder`不為null，`MaterialApp`則不會創建任何`Navigator`。

`Navigator`呈現一個`stack`的結構，它透過對`route`的`pop`和`push`來完成頁面的切換

這裡的`route`和網頁的`route`又是不太一樣的概念

根據[官方說法](https://flutter.dev/docs/cookbook/navigation/navigation-basics)

```
In Flutter, screens and pages are called routes. The remainder of this recipe refers to routes.
```

`route`其實也不完全代表頁面，更準確來說是包含頁面在內的資源銜接(例如一些原生切換動畫等)。後面會做解釋。

---

事實上，`pop`和`push`都屬於`NavigatorState`的方法，而非`Navigator`，我們通常透過靜態方法`Navigator.of()`取得`NavigatorState`物件。



例如，若我想要將畫面移至`SecondPage`這個`Widget`，會寫成如下

```dart
              Navigator.of(context)
                  .push(new MaterialPageRoute(builder: (context) => SecondPage()));
```

分析它

* 為了取得`NavigatorState`物件所以使用了` Navigator.of(context)`方法

* `NavigatorState`物件的`push()`方法，把`route`放到`stack`最上方

* 被`push`的`route`是一個`MaterialPageRoute`物件

  * 在這個新被實例化的`MaterialPageRoute`物件中設定`builder`屬性傳入的方法

    ```dart
    (context) => SecondPage()
    ```

    這邊類似C#的`Lambda Expression`，前面的`(context)`代表`BuilderContext`型別的參數

    當然不一定要寫成`context`，可以隨意做自己喜歡的命名如`A` `b` `_`等等，反正dart會知道要傳神麼進去

    