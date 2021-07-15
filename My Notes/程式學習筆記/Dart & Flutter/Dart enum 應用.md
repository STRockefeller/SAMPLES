



## Enum 賦值



Dart沒辦法像下方C#的作法一樣給enum數值

```C#
    public enum EnumSeriesName
    {
        A11_Atelier_Rorona = 11,

        A12_Atelier_Totori = 12,

        A14_Atelier_Ayesha = 14,

        A15_Atelier_EschaLogy = 15,

        A17_Atelier_Sophie = 17,

        A18_Atelier_Firis = 18,

        A19_Atelier_LydieSuelle = 19,

        A20_Atelier_Lulua = 20,

        A21_Atelier_Ryza = 21
    }
}
```

不過我們可以透過`extension`達成類似的效果

```dart
enum EnumSeriesName {

  A11_Atelier_Rorona,

  A12_Atelier_Totori,

  A14_Atelier_Ayesha,

  A15_Atelier_EschaLogy,

  A17_Atelier_Sophie,

  A18_Atelier_Firis,

  A19_Atelier_LydieSuelle,

  A20_Atelier_Lulua,

  A21_Atelier_Ryza
}

extension EnumSeriesNameExtension on EnumSeriesName {
  String get name {
    switch (this) {
      case EnumSeriesName.A11_Atelier_Rorona:
        return "Rorona";
      case EnumSeriesName.A12_Atelier_Totori:
        return "Totori";
      case EnumSeriesName.A14_Atelier_Ayesha:
        return "Ayesha";
      case EnumSeriesName.A15_Atelier_EschaLogy:
        return "EschaLogy";
      case EnumSeriesName.A17_Atelier_Sophie:
        return "Sophie";
      case EnumSeriesName.A18_Atelier_Firis:
        return "Firis";
      case EnumSeriesName.A19_Atelier_LydieSuelle:
        return "LydieSuelle";
      case EnumSeriesName.A20_Atelier_Lulua:
        return "Lulua";
      case EnumSeriesName.A21_Atelier_Ryza:
        return "Ryza";
      default:
        return "";
    }
  }
}
```

使用的時候就像

```dart
EnumSeriesName series = series.EnumSeriesName.A20_Atelier_Lulua;
String name = series.name;
```



## 未整理

參考https://stackoverflow.com/questions/64887178/how-to-loop-over-an-enum-in-dart

Given an enum like so,

```dart
enum MyEnum {
  horse,
  cow,
  camel,
  sheep,
  goat,
}
```

## Looping over the values of an enum

You can iterate over the values of the enum by using the `values` property:

```dart
for (var value in MyEnum.values) {
  print(value);
}

// MyEnum.horse
// MyEnum.cow
// MyEnum.camel
// MyEnum.sheep
// MyEnum.goat
```

## Converting between a value and its index

Each value has an index:

```dart
int index = MyEnum.horse.index; // 0
```

And you can convert an index back to an enum using subscript notation:

```dart
MyEnum value = MyEnum.values[0]; // MyEnum.horse
```

## Finding the next enum value

Combining these facts, you can loop over the values of an enum to find the next value like so:

```dart
MyEnum nextEnum(MyEnum value) {
  final nextIndex = (value.index + 1) % MyEnum.values.length;
  return MyEnum.values[nextIndex];
}
```

Using the modulo operator handles even when the index is at the end of the enum value list:

```dart
MyEnum nextValue = nextEnum(MyEnum.goat); // MyEnum.horse
```
