# 使用Javascript

Framework: Blazor
Other Tags: Web Assembly, razor
Platform: Web
Programming Language: C#, Javascript

[Reference:ITHelp](https://ithelp.ithome.com.tw/articles/10249044)

雖然Blazor可以不用任何一行Javascript，就可以完成一個SPA應用程式，但某些情況使用Javascript反而更加合適。

## 使用方式

### 先將IJSRuntime注入到razor元件中

```html
@inject IJSRuntime js
```

如果將razor和cs分開寫的話，cs那邊還需要using namespace

```csharp
using Microsoft.JSInterop;
```

### 於Component中使用

IJSRuntime介面有兩個api可以使用：

- InvokeVoidAsync：執行的程式沒有回傳值，例如Console.log
- InvokeAsync`<T>`：要執行的程式有回傳值，可以將回傳的型別寫在`<T>`內

例如

```csharp
bool result = await js.InvokeAsync<bool>("confirm", $"確定要刪除{ItemName}?");
```

## ITHelp 範例練習

### 前置

先花點時間完成一個類似那個範例的Page，順便作為複習

子元件

ToDoItem.razor

```html
<tr>
    <td scope="row">
        <div class="form-check form-check-inline">
            <label class="form-check-label">
                <input class="form-check-input" type="checkbox" name="" id="" value="checkedValue"> Display value
            </label>
        </div>
    </td>
    <td>@Content</td>
    <td>
        <button type="button" class="btn btn-danger" @onclick="delete">Delete</button>
    </td>
</tr>
```

ToDoItem.razor.cs

```csharp
using Microsoft.AspNetCore.Components;

namespace BlazorAppWasm.Shared.ComponentItems
{
    public partial class ToDoItem
    {
        [Parameter]
        public string Content { get; set; }
        [Parameter]
        public int Index { get; set; }
        [Parameter]
        public EventCallback<int> EventCallback { get; set; }
        private void delete() => EventCallback.InvokeAsync(Index);
    }
}
```

父元件

ToDoList.razor

```html
@page "/todo"
@using BlazorAppWasm.Shared.ComponentItems

<h3>ToDoList</h3>

<table class="table">
    <thead>
        <tr>
            <th>Done</th>
            <th>Descript</th>
            <th>Delete</th>
        </tr>
    </thead>
    <tbody>
        @for(int i =0;i<items.Count;i++)
        {
            <ToDoItem Content="@items[i]" Index=@i EventCallback="deleteItemCallback"/>
        }
    </tbody>
</table>
<div class="form-group">
  <label for="">Add a new item</label>
  <input type="text"
    class="form-control" name="" id="" aria-describedby="helpId" placeholder="" @bind="@inputValue">
</div>
<button type="button" class="btn btn-primary" @onclick="addItem">ADD</button>
```

ToDoList.razor.cs

```csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace BlazorAppWasm.Pages
{
    public partial class ToDoList
    {
        private List<string> items;
        private string inputValue;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            items = new List<string>();
        }
        private void addItem() => items.Add(inputValue);
        private void deleteItemCallback(int index) => items.RemoveAt(index);
    }
}
```

### 加入JS

```html
@page "/todo"
@using BlazorAppWasm.Shared.ComponentItems
@inject IJSRuntime js;

<h3>ToDoList</h3>

<table class="table">
    <thead>
        <tr>
            <th>Done</th>
            <th>Descript</th>
            <th>Delete</th>
        </tr>
    </thead>
    <tbody>
        @for(int i =0;i<items.Count;i++)
        {
            <ToDoItem Content="@items[i]" Index=@i EventCallback="deleteItemCallbackAsync"/>
        }
    </tbody>
</table>
<div class="form-group">
  <label for="">Add a new item</label>
  <input type="text"
    class="form-control" name="" id="" aria-describedby="helpId" placeholder="" @bind="@inputValue">
</div>
<button type="button" class="btn btn-primary" @onclick="addItem">ADD</button>
```

```csharp
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorAppWasm.Pages
{
    public partial class ToDoList
    {

        private List<string> items;
        private string inputValue;
        protected override void OnInitialized()
        {
            base.OnInitialized();
            items = new List<string>();
        }
        private void addItem() => items.Add(inputValue);
        private async Task deleteItemCallbackAsync(int index)
        {
            bool confirm = await js.InvokeAsync<bool>("confirm", $"確定要刪除{items[index]}?");
            if (confirm)
                items.RemoveAt(index);
        }
    }
}
```



### 美化

> 在wwwroot -> 新增scripts資料夾 → 加入一個helper.js檔
在wwwroot/index.html加入sweet alert 2的cdn還有剛剛新增的helper.js

```html
<script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
<script src="scripts/helper.js"></script>
```

> 新增一個funtion，這邊先叫做SweetConfirm。

```jsx
function SweetConfirm(title, msg) {
    return new Promise((resolve) => {
        Swal.fire({
            title: title,
            text: msg,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: '刪除'
        }).then((result) => {
            if (result.isConfirmed) {
                result.value ? resolve(true) : resolve(false);
            }
        });
    });
}
```

> 因為SweetAlert的Swal.fire會回傳Promise，因此用Promise接收confirm結果再回傳
Todo子元件的Delete方法則改成如下：

```csharp
private async Task Delete()
{
    bool result = await js.InvokeAsync<bool>("SweetConfirm", "Delete", $"確定要刪除{ItemName}?");
    if (result)
    {
        await DeleteItem.InvokeAsync(ItemIndex);
    }
}
```