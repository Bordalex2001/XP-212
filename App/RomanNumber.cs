﻿using System;
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

        public RomanNumber Plus(RomanNumber second) => 
            second == null
            ? throw new ArgumentNullException(nameof(second)) 
            : this with { Value = Value + second.Value };

        public static explicit operator short(RomanNumber rn) => (short)rn.Value;
        public static explicit operator byte(RomanNumber rn) => (byte)rn.Value;
        public static explicit operator long(RomanNumber rn) => rn.Value;
        public static explicit operator int(RomanNumber rn) => rn.Value;
        public static explicit operator float(RomanNumber rn) => rn.Value;
        public static explicit operator double(RomanNumber rn) => rn.Value;

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
    }
}
