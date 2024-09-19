using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public record RomanNumber(int value)
    {
        private readonly int _value = value;
        public int Value { get { return _value; } }

        public static RomanNumber Parse(string Value)
        {
            CheckZeroDigit(Value);
           
            int res = 0;
            int rightDigit = 0; // TODO: rename, 'prev' not semantics
            int pos = Value.Length;
            int maxDigit = 0; // найбільша цифра, що пройдена
            int lessCounter = 0; // кількість цифр, менших за неї
            int maxCounter = 1; // кількість однакових найбільших цифр 

            foreach (char c in Value.Reverse())
            {
                pos--;
                int digit = ParseDigit(c, pos, Value);
                ValidateDigitRatio(digit, rightDigit, c, Value, pos);
                UpdateMaxAndLessCounters(digit, ref maxDigit, ref lessCounter, ref maxCounter);

                if (IsInvalidLessCounter(lessCounter, maxCounter))
                {
                    throw new FormatException($"{nameof(RomanNumber)}.{nameof(Parse)}() illegal sequence: more than one smaller digits before '{Value[Value.Length - 1]}' in position '{Value.Length - 1}'");
                }

                res += digit < rightDigit ? -digit : digit;
                rightDigit = digit;
            }

            return new(res);
        }

        private static void CheckZeroDigit(String input)
        {
            if (input.Contains('N') && input.Length > 1)
            {
                throw new FormatException();
            }
        }

        // цифри занадто "далекі" для віднімання цифр, що є "5"-ками
        public static bool CheckDigitRatio(int leftDigit, int rightDigit)
        {
            return leftDigit >= rightDigit || !(leftDigit != 0 && rightDigit / leftDigit > 10 || leftDigit == 5 || leftDigit == 50 || leftDigit == 500);
        }

        private static int ParseDigit(char c, int pos, string Value)
        {
            int digit;
            
            try
            {
                digit = DigitValue(c);
            }
            catch (ArgumentException)
            {
                throw new FormatException($"{nameof(RomanNumber)}.{nameof(Parse)}() found illegal symbol '{c}' in position {pos}.");
            }

            return digit;
        }

        private static bool IsInvalidLessCounter(int lessCounter, int maxCounter)
        {
            return lessCounter > 1 || (lessCounter > 0 && maxCounter > 1);
        }

        private static void UpdateMaxAndLessCounters(int digit, ref int maxDigit, ref int lessCounter, ref int maxCounter)
        {
            if (digit > maxDigit)
            {
                maxDigit = digit;
                lessCounter = 0;
                maxCounter = 1;
            }
            else if (digit == maxDigit)
            {
                maxCounter += 1;
                lessCounter = 0;
            }
            else
            {
                lessCounter += 1;
            }
        }

        private static void ValidateDigitRatio(int digit, int rightDigit, char c, string Value, int pos)
        {
            if (!CheckDigitRatio(digit, rightDigit))
            {
                throw new FormatException($"{nameof(RomanNumber)}.{nameof(Parse)}() illegal sequence: '{c}' before '{Value[pos + 1]}' in position {pos}");
            }
        }

        public static int DigitValue(char digit) 
        {
            if (!"NIVXLCDM".Contains(digit))
            {
                throw new ArgumentException($"RomanNumber.DigitValue() illegal argument 'digit': '{digit}' not valid Roman digit");
            }

            return digit switch
            {
                'N' => 0,
                'I' => 1,
                'V' => 5,
                'X' => 10,
                'L' => 50,
                'C' => 100,
                'D' => 500,
                'M' => 1000,
                _ => throw new ArgumentException($"RomanNumber.DigitValue() illegal argument 'digit': '{digit}' not valid Roman digit")
            };
        }

        /*public override string? ToString()
        {
            Dictionary<int, String> ranges = new()
            {
                { 1, "I" },
                { 4, "IV" },
                { 5, "V" },
                { 9, "IX" },
                { 10, "X" },
                { 40, "XL" },
                { 50, "L" },
                { 90, "XC" },
                { 100, "C" },
                { 400, "CD" },
                { 500, "D" },
                { 900, "CM" },
                { 1000, "M" },
            };

            if (_value == 0) { return "N"; }

            int number = _value;
            StringBuilder result = new();

            foreach (var range in ranges.Reverse())
            {
                while (number >= range.Key)
                {
                    result.Append(range.Value);
                    number -= range.Key;
                }
            }
            
            return result.ToString();
        }*/
    }
}
