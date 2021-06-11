# Flutter 右鍵選單

[Reference : StackOverFlow](https://stackoverflow.com/questions/62244113/can-i-change-right-click-action-in-flutter-web-application)



```dart
import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:universal_html/html.dart';

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      home: MyHomePage(),
    );
  }
}

class MyHomePage extends StatefulWidget {
  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  @override
  void initState() {
    super.initState();
    // Prevent default event handler
    document.onContextMenu.listen((event) => event.preventDefault());
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      resizeToAvoidBottomInset: false,
      body: Center(
        child: Listener(
          child: Icon(
            Icons.ac_unit,
            size: 48.0,
          ),
          onPointerDown: _onPointerDown,
        ),
      ),
    );
  }

  /// Callback when mouse clicked on `Listener` wrapped widget.
  Future<void> _onPointerDown(PointerDownEvent event) async {
    // Check if right mouse button clicked
    if (event.kind == PointerDeviceKind.mouse &&
        event.buttons == kSecondaryMouseButton) {
      final overlay =
          Overlay.of(context).context.findRenderObject() as RenderBox;
      final menuItem = await showMenu<int>(
          context: context,
          items: [
            PopupMenuItem(child: Text('Copy'), value: 1),
            PopupMenuItem(child: Text('Cut'), value: 2),
          ],
          position: RelativeRect.fromSize(
              event.position & Size(48.0, 48.0), overlay.size));
      // Check if menu item clicked
      switch (menuItem) {
        case 1:
          ScaffoldMessenger.of(context).showSnackBar(SnackBar(
            content: Text('Copy clicket'),
            behavior: SnackBarBehavior.floating,
          ));
          break;
        case 2:
          ScaffoldMessenger.of(context).showSnackBar(SnackBar(
              content: Text('Cut clicked'),
              behavior: SnackBarBehavior.floating));
          break;
        default:
      }
    }
  }
```

