using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumberValidator
    {
        public static void Validate(String Value)
        {
            CheckZeroDigit(Value);

            int result = 0;
            int rightDigit = 0;      // TODO: rename, 'prev' not semantics
            int pos = Value.Length;
            int maxDigit = 0;       // найбільша цифра, що пройдена
            int lessCounter = 0;    // кількість цифр, менших за неї
            int maxCounter = 1;     // кількість однакових найбільших цифр

            foreach (char c in Value.Reverse())
            {
                pos--;
                int digit = ParseDigit(c, pos, Value);
                ValidateDigitRatio(digit, rightDigit, c, Value, pos);
                UpdateMaxAndLessCounters(digit, ref maxDigit, ref lessCounter, ref maxCounter);
                if (IsInvalidLessCounter(lessCounter, maxCounter))
                {
                    throw new FormatException(
                        $"{nameof(RomanNumber)}.Parse() " +
                        $"illegal sequence: more than one smaller digits before '{Value[Value.Length - 1]}' in position {Value.Length - 1}");
                }
                result += digit < rightDigit ? -digit : digit;
                rightDigit = digit;
            }
        }

        private static void CheckZeroDigit(String input)
        {
            if (input.Contains('N') && input.Length > 1)
            {
                throw new FormatException();
            }
        }

        public static void CheckSequence(String input)
        {
            int maxDigit = 0;     // найбільша цифра, що пройдена
            int lessCounter = 0;  // кількість цифр, менших за неї
            int maxCounter = 1;   // кількість однакових найбільших цифр 
            for (int i = input.Length - 1; i >= 0; --i)
            {
                int digit = RomanNumberParser.DigitValue(input[i]);
                // рахуємо цифри, якщо вони менші за максимальну
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

                if (lessCounter > 1 || lessCounter > 0 && maxCounter > 1)
                {
                    throw new FormatException(
                        $"{nameof(RomanNumber)}.Parse('{input}') " +
                        $"illegal sequence: more than one smaller digits " +
                        $"before '{input[i + 2]}' in position {i + 2}");
                }
            }
        }

        public static bool CheckDigitRatio(int leftDigit, int rightDigit)
        {
            // цифри занадто "далекі" для віднімання цифр, що є "5"-ками
            return leftDigit >= rightDigit || !(leftDigit != 0 && rightDigit / leftDigit > 10 || leftDigit == 5 || leftDigit == 50 || leftDigit == 500);
        }

        public static void CheckDigitRatios(String input)
        {
            for (int i = 0; i < input.Length - 1; ++i)
            {
                int leftDigit = RomanNumberParser.DigitValue(input[i]);
                int rightDigit = RomanNumberParser.DigitValue(input[i + 1]);
                if (!(leftDigit >= rightDigit || !(
                    leftDigit != 0 && rightDigit / leftDigit > 10 ||
                    leftDigit == 5 || leftDigit == 50 || leftDigit == 500)))
                {
                    throw new FormatException(
                        $"{nameof(RomanNumber)}.Parse() " +
                        $"illegal sequence: '{input[i]}' before '{input[i + 1]}' " +
                        $"in position {i}");
                }
            }
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
                throw new FormatException($"{nameof(RomanNumber)}.Parse() found illegal symbol '{c}' in position {pos}");
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
                throw new FormatException($"{nameof(RomanNumber)}.Parse() illegal sequence: '{c}' before '{Value[pos + 1]}' in position {pos}");
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
                _ => throw new ArgumentException($"{nameof(RomanNumber)}.{nameof(DigitValue)}() illegal argument 'digit': '{digit}' not valid Roman digit.")
            };
        }

        private static void CheckSymbols(String input)
        {
            for (int i = 0; i < input.Length; ++i)
            {
                try { RomanNumberParser.DigitValue(input[i]); }
                catch
                {
                    throw new FormatException(
                        $"{nameof(RomanNumber)}.Parse()" +
                        $" found illegal symbol '{input[i]}' in position {i}");
                }
            }
        }
    }
}
