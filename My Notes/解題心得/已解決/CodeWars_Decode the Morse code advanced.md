# CodeWars:Decode the Morse code, advanced:202008XX:C#

[Reference](https://www.codewars.com/kata/decode-the-morse-code-advanced)



## Question

Part of Series 2/3

This kata is part of a series on the Morse code. Make sure you solve the [previous part](https://www.codewars.com/kata/decode-the-morse-code) before you try this one. After you solve this kata, you may move to the [next one](https://www.codewars.com/kata/decode-the-morse-code-for-real).


In this kata you have to write a [Morse code](https://en.wikipedia.org/wiki/Morse_code) decoder for [wired electrical telegraph](https://en.wikipedia.org/wiki/Electrical_telegraph).

Electric telegraph is operated on a 2-wire line with a key that, when pressed, connects the wires together, which can be detected on a remote station. The Morse code encodes every character being transmitted as a sequence of "dots" (short presses on the key) and "dashes" (long presses on the key).

When transmitting the Morse code, the international standard specifies that:

- "Dot" – is 1 time unit long.
- "Dash" – is 3 time units long.
- Pause between dots and dashes in a character – is 1 time unit long.
- Pause between characters inside a word – is 3 time units long.
- Pause between words – is 7 time units long.

However, the standard does not specify how long that "time unit" is. And in fact different operators would transmit at different speed. An amateur person may need a few seconds to transmit a single character, a skilled professional can transmit 60 words per minute, and robotic transmitters may go way faster.

For this kata we assume the message receiving is performed automatically by the hardware that checks the line periodically, and if the line is connected (the key at the remote station is down), `1` is recorded, and if the line is not connected (remote key is up), `0` is recorded. After the message is fully received, it gets to you for decoding as a string containing only symbols `0` and `1`.

For example, the message `HEY JUDE`, that is `···· · −·−−  ·−−− ··− −·· ·` may be received as follows:

```
1100110011001100000011000000111111001100111111001111110000000000000011001111110011111100111111000000110011001111110000001111110011001100000011
```

As you may see, this transmission is perfectly accurate according to the standard, and the hardware sampled the line exactly two times per "dot".

That said, your task is to implement two functions:

1. Function `decodeBits(bits)`, that should find out the transmission rate of the message, correctly decode the message to dots `.`, dashes `-` and spaces (one between characters, three between words) and return those as a string. Note that some extra `0`'s may naturally occur at the beginning and the end of a message, make sure to ignore them. Also if you have trouble discerning if the particular sequence of `1`'s is a dot or a dash, assume it's a dot.

\2. Function `decodeMorse(morseCode)`, that would take the output of the previous function and return a human-readable string.

**NOTE:** For coding purposes you have to use ASCII characters `.` and `-`, not Unicode characters.

The Morse code table is preloaded for you (see the solution setup, to get its identifier in your language).

```scala
Eg:
  morseCodes(".--") //to access the morse translation of ".--"
```

All the test strings would be valid to the point that they could be reliably decoded as described above, so you may skip checking for errors and exceptions, just do your best in figuring out what the message is!

Good luck!

After you master this kata, you may try to [Decode the Morse code, for real](http://www.codewars.com/kata/decode-the-morse-code-for-real).

## My Solution

```C#
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

public class MorseCodeDecoder
{
        public static string DecodeBits(string bits)
        {
            bits = bits.Trim('0');
            bits = "~" + bits + "~";
            for (int i = 30; i > 0; i--)
            {
                bool bSpace = bits.Contains("1"+strCounts("0000000",i)+"1");
                bool bDotDash = matchBit(bits, strCounts("1", i)) && matchBit(bits, strCounts("1", 3 * i));
                bool bDash = matchBit(bits, strCounts("1", 3 * i)) && matchBitZ(bits, strCounts("0", i));
                bool bDot = matchBit(bits, strCounts("1", i)) && !matchBit(bits, strCounts("1", i / 3));
                bool bZcheck = matchBitZ(bits, strCounts("0", i / 3));
                if (bSpace || bDotDash || (bDot && !bZcheck) || bDash)
                {
                    bits = bits.Replace("~", "");
                    bits = bits.Replace(strCounts("0000000", i), "   ");
                    bits = bits.Replace(strCounts("000", i), " ");
                    bits = bits.Replace(strCounts("111", i), "-");
                    bits = bits.Replace(strCounts("0", i), "");
                    bits = bits.Replace(strCounts("1", i), ".");
                    bits = bits.Replace("0", "");
                    bits = bits.Replace("1", ".");
                    return bits;
                }
            }
            for (int i = 30; i > 0; i--)
            {
                bool bDot = matchBit(bits, strCounts("1", i));
                if (bDot)
                {
                    bits = bits.Replace("~", "");
                    bits = bits.Replace(strCounts("000", i), " ");
                    bits = bits.Replace(strCounts("0", i), "");
                    bits = bits.Replace(strCounts("1", i), ".");
                    bits = bits.Replace("0", "");
                    return bits;
                }
            }
            return bits;
        }

        private static bool matchBit(string bits, string matchBits) => bits.Contains("0" + matchBits + "0") || bits.Contains("~" + matchBits + "0") || bits.Contains("0" + matchBits + "~") || bits.Contains("~" + matchBits + "~");

        private static bool matchBitZ(string bits, string matchBits) => bits.Contains("1" + matchBits + "1") || bits.Contains("~" + matchBits + "1") || bits.Contains("1" + matchBits + "~") || bits.Contains("~" + matchBits + "~");

       private static string strCounts(string str , int counts)
        {
            string rstr = "";
            for (int i = 0; i < counts; i++)
                rstr += str;
            return rstr;
        }
        public static string DecodeMorse(string morseCode)
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
                    result += characterDecode(c, out ex);
                if (i != words.Count - 1 && !ex)
                    result += " ";
            }
            return result;
        }

        private static string characterDecode(string morseChar, out bool ex)
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



接續上一題，就算不算自己多寫的字元Decode方法也很長

印象中遇到的三個最難的坎:

一是時間單位的判斷

二是像 I:".." / M:"--" 這類字源的判別(E:"." / T:"-" 也是)

三是無訊號時間，這個主要是測試一直不通過但沒想到是因為要考慮到開頭結尾無訊號時間



## Better Solutions



```C#
using System;
using System.Collections.Generic;
using System.Linq;

public class MorseCodeDecoder
{
  public static string DecodeBits(string bits)
  {
    var cleanedBits = bits.Trim('0');
    var rate = GetRate();
    return cleanedBits
      .Replace(GetDelimiter(7, "0"), "   ")
      .Replace(GetDelimiter(3, "0"), " ")
      .Replace(GetDelimiter(3, "1"), "-")
      .Replace(GetDelimiter(1, "1"), ".")
      .Replace(GetDelimiter(1, "0"), "");
        
    string GetDelimiter(int len, string c) => Enumerable.Range(0, len * rate).Aggregate("", (acc, _) => acc + c);
    int GetRate() => GetLengths("0").Union(GetLengths("1")).Min();
    IEnumerable<int> GetLengths(string del) => cleanedBits.Split(del, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Length);
  }

  public static string DecodeMorse(string morseCode)
  {
    return morseCode
      .Split("   ")
      .Aggregate("", (res, word) => $"{res}{ConvertWord(word)} ")
      .Trim();
      
    string ConvertWord(string word) => word.Split(' ').Aggregate("", (wordRes, c) => wordRes + MorseCode.Get(c));
  }
}
```

