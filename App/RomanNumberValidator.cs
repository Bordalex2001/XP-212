using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumberValidator
    {
        public static void Validate(String input)
        {
            CheckSymbols(input);
            CheckZeroDigit(input);
            CheckDigitRatios(input);
            CheckSequence(input);
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

        public static void CheckZeroDigit(String input)
        {
            if (input.Contains('N') && input.Length > 1)
            {
                throw new FormatException();
            }
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
