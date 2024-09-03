using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    public class RomanNumber(int value)
    {
        private readonly int _value = value;
        public int Value { get { return _value; } }

        public static RomanNumber Parse(string value)
        {
            return new(0);  
            /*int res = 0;
            int prevDigit = 0;
            
            foreach (char c in value.Reverse())
            {
                int digit = DigitValue(c);
                res += digit < prevDigit ? -digit : digit;
                prevDigit = digit;
            }
            return new(res);*/
        }

        /*public static int DigitValue(char digit) => digit switch 
        { 
            'N' => 0,
            'I' => 1,
            'V' => 5,
            'X' => 10,
            'L' => 50,
            'C' => 100,
            'D' => 500,
            _ => 1000
        };*/
    }
}
