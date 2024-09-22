using App;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class RomanNumberTest
    {
        private Dictionary<char, int> digits = new()
        {
            { 'N', 0 },
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        [TestMethod]
        public void ToStringTest()
        {
            var testCases = new Dictionary<int, String>()
            {
                { 4, "IV" },
                { 6, "VI" },
                { 19, "XIX" },
                { 49, "XLIX" },
                { 95, "XCV" },
                { 444, "CDXLIV" },
                { 946, "CMXLVI" },
                { 3333, "MMMCCCXXXIII" }
            }
            .Concat(digits.Select(d => new KeyValuePair<int, string>(d.Value, d.Key.ToString()))
            ).ToDictionary();

            foreach (var testCase in testCases)
            {
                RomanNumber rn = new(testCase.Key);
                var value = rn.ToString();
                Assert.IsNotNull(value);
                Assert.AreEqual(testCase.Value, value);
            }
        }

        [TestMethod]
        public void CrossTest_Parse_ToString()
        {
            /*
             * Ќа€вн≥сть двох метод≥в протилежноњ роботи дозвол€Ї використовувати крос-тести, €к≥ посл≥довно застосовують два методи ≥ одержують початковий результат: 
             * "XIX" -Parse-> 19 -ToString-> "XIX" v 
             * "IIII" -Parse-> 4 -ToString-> "IV" x 
             * 4 -ToString-> "IV" -Parse-> 4 v
             */
            for (int i = 0; i <= 1000; i++)
            {
                int c = RomanNumberParser.FromString(new RomanNumber(i).ToString()!).Value;
                Assert.AreEqual(i, c, $"Cross test for {i}: {new RomanNumber(i)} -> {c}");
            }
        }

        [TestMethod]
        public void ParseTest()
        {
            Dictionary<String, int> testCases = new()
            {
                { "N", 0 },
                { "I", 1 },
                { "II", 2 },
                { "III", 3 },
                { "IIII", 4 },
                { "V", 5 },
                { "X", 10 },
                { "D", 500 },
                { "IV", 4 },
                { "VI", 6 },
                { "VII", 7 },
                { "VIII", 8 },
                { "XI", 11 },
                { "XII", 12 },
                { "XIII", 13 },
                { "IX", 9 },
                { "MM", 2000 },
                { "MCM", 1900 },
                { "XL", 40 },
                { "XC", 90 },
                { "CD", 400 },
                { "CMII", 902 },
                //{ "DCCCC", 900 },
                //{ "XIXIIII", 23 },
                //{ "XXXXX", 50 },
                //{ "DDMD", 1500 }
            };
      
            foreach (var testCase in testCases)
            {
                RomanNumber rn = RomanNumber.Parse(testCase.Key);
                //rn = null!;
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Key}' is not null");
                Assert.AreEqual(
                    testCase.Value, 
                    rn.Value, 
                    $"Parse '{testCase.Key}' => {testCase.Value}"
                );
            }

            /* ¬ин€ток парсера Ч окр≥м причини вин€тку м≥стить в≥домост≥ 
             * про м≥сце виникненн€ помилки (позиц≥€ у р€дку)
             */
            Object[][] excCases = [
                ["W", 'W', 0 ],
                ["CS", 'S', 1 ],
                ["CX1", '1', 2 ],
            ];
            
            foreach(var excCase in excCases) 
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{excCase[0]}\") must throw FormatException"
                );

                /* ЌакладаЇмо вимоги на пов≥домленн€:
                 * Ч маЇ м≥стити сам символ, що призводить до вин€тку 
                 * Ч маЇ м≥стити позиц≥ю символу в р€дку
                 * Ч маЇ м≥стити назву методу та класу*/
                 
                Assert.IsTrue(
                    ex.Message.Contains($"illegal symbol '{excCase[1]}'"),
                    $"ex.Message must contain symbol which cause error: '{excCase[1]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {excCase[2]}"),
                    $"ex.Message must contain error symbol position"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber))
                    && ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, ex.Message: {ex.Message}"
                );
            }
        }

        [TestMethod]
        public void DigitValueChar()
        {
            //foreach (var testCase in testCases)
            //{
            //    Assert.AreEqual(
            //        testCase.Value,
            //        RomanNumber.DigitValue(testCase.Key),
            //        $"{testCase.Key} => {testCase.Value}"
            //    );
            //}

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var digit in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNumberParser.DigitValue(digit),
                    $"DigitValue('{digit}') must throw ArgumentException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"'{digit}'"),
                    $"DigitValue ex.Message should contain a symbol which cause exception: symbol: '{digit}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains($"{nameof(RomanNumber)}")
                    && ex.Message.Contains($"DigitValue"),
                    $"DigitValue ex.Message should contain a name of class and a name of method which cause exception: symbol: '{digit}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains("'digit'")
                    && ex.Message.Contains("argument"),
                    $"DigitValue ex.Message should contain a name of the argument ('digit') and a word 'argument' which cause exception: symbol: '{digit}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains("not valid Roman digit"),
                    $"DigitValue ex.Message should contain a text 'not valid Roman digit' which cause exception: symbol: '{digit}', ex.Message: '{ex.Message}'."
                );
            }
        }
    }
}
/* “естовий проЇкт за структурою в≥дтворюЇ основний проЇкт:
 * Ч його папки в≥дпов≥дають папкам основного проЇкту
 * Ч його класи називають €к ≥ проЇктн≥, з дописом Test
 * Ч методи клас≥в також в≥дтворюють методи випробуванн€ клас≥в ≥ також з дописом Test
 * 
 * ќснову тест≥в складають вислови (Assert)
 * “ест вважаЇтьс€ пройденим, €кщо вс≥ його вислови ≥стинн≥, проваленим Ч €кщо хоча б один висл≥в хибний
 */