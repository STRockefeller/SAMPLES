

```C#
public static class StringExt {
    static private Regex regexNumber = new Regex("\\d+");
    static public bool IsNumber(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        return regexNumber.IsMatch(input);
    }
}
```

