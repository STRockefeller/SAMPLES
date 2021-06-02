# Directives



Reference:[MSDN](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#attributes)

以下直接從MSDN節錄片段，再加上一些註解

## Directives

Razor directives are represented by implicit expressions with reserved keywords following the `@` symbol. A directive typically changes the way a view is parsed or enables different functionality.

Understanding how Razor generates code for a view makes it easier to understand how directives work.



```cshtml
@{
    var quote = "Getting old ain't for wimps! - Anonymous";
}

<div>Quote of the Day: @quote</div>
```

The code generates a class similar to the following:



```csharp
public class _Views_Something_cshtml : RazorPage<dynamic>
{
    public override async Task ExecuteAsync()
    {
        var output = "Getting old ain't for wimps! - Anonymous";

        WriteLiteral("/r/n<div>Quote of the Day: ");
        Write(output);
        WriteLiteral("</div>");
    }
}
```

Later in this article, the section [Inspect the Razor C# class generated for a view](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#inspect-the-razor-c-class-generated-for-a-view) explains how to view this generated class.

### `@attribute`

為頁面指定屬性標籤。

The `@attribute` directive adds the given attribute to the class of the generated page or view. The following example adds the `[Authorize]` attribute:



```cshtml
@attribute [Authorize]
```

### `@code`

基本上功能和`@function`完全一樣，但是在習慣用法上略有差別。

*This scenario only applies to Razor components (.razor).*

The `@code` block enables a [Razor component](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0) to add C# members (fields, properties, and methods) to a component:



```razor
@code {
    // C# members (fields, properties, and methods)
}
```

For Razor components, `@code` is an alias of [`@functions`](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#functions) and recommended over `@functions`. More than one `@code` block is permissible.

### `@functions`

基本上功能和`@code`完全一樣，但是在習慣用法上略有差別。

The `@functions` directive enables adding C# members (fields, properties, and methods) to the generated class:



```cshtml
@functions {
    // C# members (fields, properties, and methods)
}
```

In [Razor components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0), use `@code` over `@functions` to add C# members.

For example:



```cshtml
@functions {
    public string GetHello()
    {
        return "Hello";
    }
}

<div>From method: @GetHello()</div> 
```

The code generates the following HTML markup:



```html
<div>From method: Hello</div>
```

The following code is the generated Razor C# class:



```csharp
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor;

public class _Views_Home_Test_cshtml : RazorPage<dynamic>
{
    // Functions placed between here 
    public string GetHello()
    {
        return "Hello";
    }
    // And here.
#pragma warning disable 1998
    public override async Task ExecuteAsync()
    {
        WriteLiteral("\r\n<div>From method: ");
        Write(GetHello());
        WriteLiteral("</div>\r\n");
    }
#pragma warning restore 1998
```

`@functions` methods serve as templating methods when they have markup:



```cshtml
@{
    RenderName("Mahatma Gandhi");
    RenderName("Martin Luther King, Jr.");
}

@functions {
    private void RenderName(string name)
    {
        <p>Name: <strong>@name</strong></p>
    }
}
```

The code renders the following HTML:



```html
<p>Name: <strong>Mahatma Gandhi</strong></p>
<p>Name: <strong>Martin Luther King, Jr.</strong></p>
```

### `@implements`

強制實作介面。

The `@implements` directive implements an interface for the generated class.

The following example implements [System.IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable) so that the [Dispose](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable.dispose) method can be called:



```cshtml
@implements IDisposable

<h1>Example</h1>

@functions {
    private bool _isDisposed;

    ...

    public void Dispose() => _isDisposed = true;
}
```

### `@inherits`

The `@inherits` directive provides full control of the class the view inherits:



```cshtml
@inherits TypeNameOfClassToInheritFrom
```

The following code is a custom Razor page type:



```csharp
using Microsoft.AspNetCore.Mvc.Razor;

public abstract class CustomRazorPage<TModel> : RazorPage<TModel>
{
    public string CustomText { get; } = 
        "Gardyloo! - A Scottish warning yelled from a window before dumping" +
        "a slop bucket on the street below.";
}
```

The `CustomText` is displayed in a view:



```cshtml
@inherits CustomRazorPage<TModel>

<div>Custom text: @CustomText</div>
```

The code renders the following HTML:



```html
<div>
    Custom text: Gardyloo! - A Scottish warning yelled from a window before dumping
    a slop bucket on the street below.
</div>
```

`@model` and `@inherits` can be used in the same view. `@inherits` can be in a *_ViewImports.cshtml* file that the view imports:



```cshtml
@inherits CustomRazorPage<TModel>
```

The following code is an example of a strongly-typed view:



```cshtml
@inherits CustomRazorPage<TModel>

<div>The Login Email: @Model.Email</div>
<div>Custom text: @CustomText</div>
```

If "rick@contoso.com" is passed in the model, the view generates the following HTML markup:



```html
<div>The Login Email: rick@contoso.com</div>
<div>
    Custom text: Gardyloo! - A Scottish warning yelled from a window before dumping
    a slop bucket on the street below.
</div>
```

### `@inject`

DI的注入，詳細看我的DI筆記。

The `@inject` directive enables the Razor Page to inject a service from the [service container](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0) into a view. For more information, see [Dependency injection into views](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/dependency-injection?view=aspnetcore-5.0).

### `@layout`

*This scenario only applies to Razor components (.razor).*

The `@layout` directive specifies a layout for routable Razor components that have an [`@page`](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-5.0#page) directive. Layout components are used to avoid code duplication and inconsistency. For more information, see [ASP.NET Core Blazor layouts](https://docs.microsoft.com/en-us/aspnet/core/blazor/layouts?view=aspnetcore-5.0).

### `@model`

MVC專案適用，Blazor專案沒有這種東西。

*This scenario only applies to MVC views and Razor Pages (.cshtml).*

The `@model` directive specifies the type of the model passed to a view or page:



```cshtml
@model TypeNameOfModel
```

In an ASP.NET Core MVC or Razor Pages app created with individual user accounts, *Views/Account/Login.cshtml* contains the following model declaration:



```cshtml
@model LoginViewModel
```

The class generated inherits from `RazorPage<dynamic>`:



```csharp
public class _Views_Account_Login_cshtml : RazorPage<LoginViewModel>
```

Razor exposes a `Model` property for accessing the model passed to the view:



```cshtml
<div>The Login Email: @Model.Email</div>
```

The `@model` directive specifies the type of the `Model` property. The directive specifies the `T` in `RazorPage<T>` that the generated class that the view derives from. If the `@model` directive isn't specified, the `Model` property is of type `dynamic`. For more information, see [Strongly typed models and the @model keyword](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-5.0#strongly-typed-models-and-the--keyword).

### `@namespace`

The `@namespace` directive:

- Sets the namespace of the class of the generated Razor page, MVC view, or Razor component.
- Sets the root derived namespaces of a pages, views, or components classes from the closest imports file in the directory tree, *_ViewImports.cshtml* (views or pages) or *_Imports.razor* (Razor components).



```cshtml
@namespace Your.Namespace.Here
```

For the Razor Pages example shown in the following table:

- Each page imports *Pages/_ViewImports.cshtml*.
- *Pages/_ViewImports.cshtml* contains `@namespace Hello.World`.
- Each page has `Hello.World` as the root of it's namespace.

| Page                                        | Namespace                             |
| :------------------------------------------ | :------------------------------------ |
| *Pages/Index.cshtml*                        | `Hello.World`                         |
| *Pages/MorePages/Page.cshtml*               | `Hello.World.MorePages`               |
| *Pages/MorePages/EvenMorePages/Page.cshtml* | `Hello.World.MorePages.EvenMorePages` |

The preceding relationships apply to import files used with MVC views and Razor components.

When multiple import files have a `@namespace` directive, the file closest to the page, view, or component in the directory tree is used to set the root namespace.

If the *EvenMorePages* folder in the preceding example has an imports file with `@namespace Another.Planet` (or the *Pages/MorePages/EvenMorePages/Page.cshtml* file contains `@namespace Another.Planet`), the result is shown in the following table.

| Page                                        | Namespace               |
| :------------------------------------------ | :---------------------- |
| *Pages/Index.cshtml*                        | `Hello.World`           |
| *Pages/MorePages/Page.cshtml*               | `Hello.World.MorePages` |
| *Pages/MorePages/EvenMorePages/Page.cshtml* | `Another.Planet`        |

### `@page`

指定路由。

The `@page` directive has different effects depending on the type of the file where it appears. The directive:

- In a *.cshtml* file indicates that the file is a Razor Page. For more information, see [Custom routes](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-5.0#custom-routes) and [Introduction to Razor Pages in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-5.0).
- Specifies that a Razor component should handle requests directly. For more information, see [ASP.NET Core Blazor routing](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing?view=aspnetcore-5.0).

### `@preservewhitespace`

*This scenario only applies to Razor components (`.razor`).*

When set to `false` (default), whitespace in the rendered markup from Razor components (`.razor`) is removed if:

- Leading or trailing within an element.
- Leading or trailing within a `RenderFragment` parameter. For example, child content passed to another component.
- It precedes or follows a C# code block, such as `@if` or `@foreach`.

### `@section`

MVC專案插入片段內容用，詳見MVC View筆記，Blazor專案沒有這種東西。

*This scenario only applies to MVC views and Razor Pages (.cshtml).*

The `@section` directive is used in conjunction with [MVC and Razor Pages layouts](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-5.0) to enable views or pages to render content in different parts of the HTML page. For more information, see [Layout in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/layout?view=aspnetcore-5.0).

### `@using`

The `@using` directive adds the C# `using` directive to the generated view:



```cshtml
@using System.IO
@{
    var dir = Directory.GetCurrentDirectory();
}
<p>@dir</p>
```

In [Razor components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0), `@using` also controls which components are in scope.



## Directive attributes

和上面的區別是Directive attributes是寫在html上的

Razor directive attributes are represented by implicit expressions with reserved keywords following the `@` symbol. A directive attribute typically changes the way an element is parsed or enables different functionality.

### `@attributes`

透過`Dictionary<string, object>`型別指定html標籤屬性

```C#
@page "/splat"

<input id="useIndividualParams"
       maxlength="@maxlength"
       placeholder="@placeholder"
       required="@required"
       size="@size" />

<input id="useAttributesDict"
       @attributes="InputAttributes" />

@code {
    private string maxlength = "10";
    private string placeholder = "Input placeholder text";
    private string required = "required";
    private string size = "50";

    private Dictionary<string, object> InputAttributes { get; set; } =
        new()
        {
            { "maxlength", "10" },
            { "placeholder", "Input placeholder text" },
            { "required", "required" },
            { "size", "50" }
        };
}
```



*This scenario only applies to Razor components (.razor).*

`@attributes` allows a component to render non-declared attributes. For more information, see [ASP.NET Core Razor components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#attribute-splatting-and-arbitrary-parameters).

### `@bind`

詳細看我的Data Binding筆記

*This scenario only applies to Razor components (.razor).*

Data binding in components is accomplished with the `@bind` attribute. For more information, see [ASP.NET Core Blazor data binding](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/data-binding?view=aspnetcore-5.0).

### `@on{EVENT}`

針對標籤的各種事件作處理，例如滑鼠點擊、拖曳、複製貼上等等。詳細見[MSDN](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/event-handling?view=aspnetcore-5.0)，有機會可能另外撰寫一篇筆記做說明。

*This scenario only applies to Razor components (.razor).*

Razor provides event handling features for components. For more information, see [ASP.NET Core Blazor event handling](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/event-handling?view=aspnetcore-5.0).

### `@on{EVENT}:preventDefault`

用於取消HTML tag的預設動作

*This scenario only applies to Razor components (.razor).*

Prevents the default action for the event.

### `@on{EVENT}:stopPropagation`

用於停止事件的傳遞，由於DOM的事件冒泡機制，當子元素被操作時代表父元素也同時被做了相同操作，`@on{EVENT}:stopPropagation`就可以在這種情況下阻止父元素的事件被觸發。

*This scenario only applies to Razor components (.razor).*

Stops event propagation for the event.

### `@key`

*This scenario only applies to Razor components (.razor).*

The `@key` directive attribute causes the components diffing algorithm to guarantee preservation of elements or components based on the key's value. For more information, see [ASP.NET Core Razor components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#use-key-to-control-the-preservation-of-elements-and-components).

### `@ref`

*This scenario only applies to Razor components (.razor).*

Component references (`@ref`) provide a way to reference a component instance so that you can issue commands to that instance. For more information, see [ASP.NET Core Razor components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-5.0#capture-references-to-components).

### `@typeparam`

*This scenario only applies to Razor components (.razor).*

The `@typeparam` directive declares a generic type parameter for the generated component class. For more information, see [ASP.NET Core Blazor templated components](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/templated-components?view=aspnetcore-5.0).