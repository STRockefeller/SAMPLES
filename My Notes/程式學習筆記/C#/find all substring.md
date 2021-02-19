

[Source](https://www.csharpstar.com/csharp-program-to-find-all-substrings-in-a-string/)

```C#
static void Main()
        {
            string value = "rstuvwxyz";
            // Avoid full length.
            for (int length = 1; length < value.Length; length++)
            {
                // End index is tricky.
                for (int start = 0; start <= value.Length - length; start++)
                {
                    string substring = value.Substring(start, length);
                    Console.WriteLine(substring);
                }
            }
        }
```

稍微改一下，作為通用方法。

```C#
public List<string> findAllSubstring(string str)
{
    List<string> res = new List<string>();
    for(int length = 1; length < str.Length; length++)
        for(int start = 0; start <= str.Length - length; start++)
            res.Add(str.Substring(start, length));
    return res;
}
```

限定長度的版本

```C#
        private List<string> findAllSubstringWithLength(string str, int length)
        {
            List<string> res = new List<string>();
            for (int start = 0; start <= str.Length - length; start++)
                res.Add(str.Substring(start, length));
            return res;
        }
```



