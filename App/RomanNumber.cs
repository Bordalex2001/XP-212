using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public record RomanNumber(int Value)
    {
        public static RomanNumber Parse(string input) => RomanNumberParser.FromString(input);

        public override string? ToString()
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

            int value = this.Value;
            if (value == 0) 
            { 
                return "N"; 
            }

            StringBuilder result = new();

            foreach (var pair in ranges.OrderByDescending(kv => kv.Key))
            {
                while (value >= pair.Key)
                {
                    result.Append(pair.Value);
                    value -= pair.Key;
                }
            }
            
            return result.ToString();
        }

        public static bool CheckDigitRatio(int leftDigit, int rightDigit)
        {
            // цифри занадто "далекі" для віднімання цифр, що є "5"-ками
            return leftDigit >= rightDigit || !(leftDigit != 0 && rightDigit / leftDigit > 10 || leftDigit == 5 || leftDigit == 50 || leftDigit == 500);
        }


        
    }
}
