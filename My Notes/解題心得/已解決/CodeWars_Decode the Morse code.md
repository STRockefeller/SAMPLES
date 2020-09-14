# CodeWars:Decode the Morse code Part1/3:202008XX:C#

[Reference](https://www.codewars.com/kata/54b724efac3d5402db00065e)



## Question

Part of Series 1/3

This kata is part of a series on the Morse code. After you solve this kata, you may move to the [next one](https://www.codewars.com/kata/decode-the-morse-code-advanced).


In this kata you have to write a simple [Morse code](https://en.wikipedia.org/wiki/Morse_code) decoder. While the Morse code is now mostly superseded by voice and digital data communication channels, it still has its use in some applications around the world.

The Morse code encodes every character as a sequence of "dots" and "dashes". For example, the letter `A` is coded as `·−`, letter `Q` is coded as `−−·−`, and digit `1` is coded as `·−−−−`. The Morse code is case-insensitive, traditionally capital letters are used. When the message is written in Morse code, a single space is used to separate the character codes and 3 spaces are used to separate words. For example, the message `HEY JUDE` in Morse code is `···· · −·−−  ·−−− ··− −·· ·`.

**NOTE:** Extra spaces before or after the code have no meaning and should be ignored.

In addition to letters, digits and some punctuation, there are some special service codes, the most notorious of those is the international distress signal [SOS](https://en.wikipedia.org/wiki/SOS) (that was first issued by [Titanic](https://en.wikipedia.org/wiki/RMS_Titanic)), that is coded as `···−−−···`. These special codes are treated as single special characters, and usually are transmitted as separate words.

Your task is to implement a function that would take the morse code as input and return a decoded human-readable string.

For example:

```csharp
MorseCodeDecoder.Decode(".... . -.--   .--- ..- -.. .")
//should return "HEY JUDE"
```

**NOTE:** For coding purposes you have to use ASCII characters `.` and `-`, not Unicode characters.

The Morse code table is preloaded for you as a dictionary, feel free to use it:

- Coffeescript/C++/Go/JavaScript/Julia/PHP/Python/Ruby/TypeScript: `MORSE_CODE['.--']`
- C#: `MorseCode.Get(".--")` (returns `string`)
- Elixir: `@morse_codes` variable (from `use MorseCode.Constants`). Ignore the unused variable warning for `morse_codes` because it's no longer used and kept only for old solutions.
- Elm: `MorseCodes.get : Dict String String`
- Haskell: `morseCodes ! ".--"` (Codes are in a `Map String String`)
- Java: `MorseCode.get(".--")`
- Kotlin: `MorseCode[".--"] ?: ""` or `MorseCode.getOrDefault(".--", "")`
- Rust: `self.morse_code`
- Scala: `morseCodes(".--")`

- C: provides parallel arrays, i.e. `morse[2] == "-.-"` for `ascii[2] == "C"`

All the test strings would contain valid Morse code, so you may skip checking for errors and exceptions. In C#, tests will fail if the solution code throws an exception, please keep that in mind. This is mostly because otherwise the engine would simply ignore the tests, resulting in a "valid" solution.

Good luck!

After you complete this kata, you may try yourself at [Decode the Morse code, advanced](http://www.codewars.com/kata/decode-the-morse-code-advanced).



## My Solution

```C#
    using System;
    using System.Collections.Generic;
   internal class MorseCodeDecoder
    {
        public static string Decode(string morseCode)
        {
            List<string> words = new List<string>();
            words.AddRange(morseCode.Split("   "));
            string result = "";
            for (int i = 0; i < words.Count; i++)
            {
                bool ex = false;
                List<string> chara = new List<string>();
                chara.AddRange(words[i].Split(" "));
                foreach (string c in chara)
                    result += characterDecode(c,out ex);
                if (i != words.Count - 1 && !ex)
                    result += " ";
            }
            return result;
        }

        private static string characterDecode(string morseChar ,out bool ex)
        {
            ex = false;
            switch (morseChar)
            {
                case ".-":
                    return "A";

                case "-...":
                    return "B";

                case "-.-.":
                    return "C";

                case "-..":
                    return "D";

                case ".":
                    return "E";

                case "..-.":
                    return "F";

                case "--.":
                    return "G";

                case "....":
                    return "H";

                case "..":
                    return "I";

                case ".---":
                    return "J";

                case "-.-":
                    return "K";

                case ".-..":
                    return "L";

                case "--":
                    return "M";

                case "-.":
                    return "N";

                case "---":
                    return "O";

                case ".--.":
                    return "P";

                case "--.-":
                    return "Q";

                case ".-.":
                    return "R";

                case "...":
                    return "S";

                case "-":
                    return "T";

                case "..-":
                    return "U";

                case "...-":
                    return "V";

                case ".--":
                    return "W";

                case "-..-":
                    return "X";

                case "-.--":
                    return "Y";

                case "--..":
                    return "Z";

                case "-----":
                    return "0";

                case ".----":
                    return "1";

                case "..---":
                    return "2";

                case "...--":
                    return "3";

                case "....-":
                    return "4";

                case ".....":
                    return "5";

                case "-....":
                    return "6";

                case "--...":
                    return "7";

                case "---..":
                    return "8";

                case "----.":
                    return "9";
                
                case "...---...":
                    return "SOS";
                
                case "-.-.--":
                    return "!";
                
                case ".-.-.-":
                    return ".";
                
                default:
                    ex = true;
                    return "";
            }
        }
    }
```

系列題目第一題，當初花不少時間在這個系列上，過了好一段時間已記不清當初的解題思路了，印象比較深的是題目沒看清楚還自己寫方法將dot dash轉換成字元orz

## Better Solutions



```C#
using System;
using System.Linq;

class MorseCodeDecoder
{
  public static string Decode(string morseCode)
  {
    var words = morseCode.Trim().Split(new[] {"   "}, StringSplitOptions.None);
    var translatedWords = words.Select(word => word.Split(' ')).Select(letters => string.Join("", letters.Select(MorseCode.Get))).ToList();
    return string.Join(" ", translatedWords);
  }
}
```

認識兩個很方便的字串處理方法，另外MorseCode.Get是題目提供的字元解碼方法

[MSDN:String.Join](https://docs.microsoft.com/zh-tw/dotnet/api/system.string.join?view=netcore-3.1)

[MSDN:String.Trim](https://docs.microsoft.com/zh-tw/dotnet/api/system.string.trim?view=netcore-3.1)