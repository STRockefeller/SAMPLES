# Call Back Function

[Reference:CodingHive](https://medium.com/@codinghive.dev/how-to-implement-call-back-function-in-dart-66bdf8c8ca3c)

> ## Step: 1- Take function as a parameter
>
> ```dart
> void downloadProgress({Function(int) callback}) {............................}
> ```
>
> ## Step: 2- Set data to function, the data which have to send back from delegate class
>
> ```dart
> if(callback!=null)callback(progress);
> ```
>
> ## Step: 3- Pass function in parameters from calling class.
>
> ```dart
> dataProcessor.downloadProgress(callback: (int progress) {
>   print('download progress : $progress');
> });
> ```

---

[Reference:StackOverFlow](https://stackoverflow.com/questions/57282672/how-to-create-callback-function-in-dart-flutter)

> You can do like below. Note that you can specify parameter or avoid and I have added `Function`(You can use [ValueChange](https://api.flutter.dev/flutter/foundation/ValueChanged.html), [Voidcallback](https://api.flutter.dev/flutter/dart-ui/VoidCallback.html))
>
> ```dart
> myFunc({Function onTap}){
>    onTap();
> }
> 
> //invoke
> myFunc(onTap: () {});
> ```
>
> If you want to pass arguments:
>
> ```dart
> myFunc({Function onTap}){
>    onTap("hello");
> }
> 
> //invoke
> myFunc(onTap: (String text) {});
> ```