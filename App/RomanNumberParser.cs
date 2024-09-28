using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumberParser
    {
        public static RomanNumber FromString(String Value)
        {
            RomanNumberValidator.Validate(Value);

            int res = 0;
            int rightDigit = 0;
            foreach (char c in Value.Reverse())
            {
                int digit = DigitValue(c);
                res += digit < rightDigit ? -digit : digit;
                rightDigit = digit;
            }
            
            return new(res);
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
    }
}