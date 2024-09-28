using App;

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
        public void TestToShort()
        {
            RomanNumber rn = new(123);
            short result = (short)rn;
            Assert.AreEqual(123, result);
        }

        [TestMethod]
        public void TestToByte()
        {
            RomanNumber rn = new(10);
            byte result = (byte)rn;
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void TestToInt()
        {
            RomanNumber rn = new(497);
            int result = (int)rn;
            Assert.AreEqual(497, result);
        }

        [TestMethod]
        public void TestToFloat()
        {
            RomanNumber rn = new(28);
            float result = (float)rn;
            Assert.AreEqual(28f, result);
        }

        [TestMethod]
        public void TestToDouble()
        {
            RomanNumber rn = new(352);
            double result = (double)rn;
            Assert.AreEqual(352d, result);
        }

        [TestMethod]
        public void TestParseToInt()
        {
            RomanNumber rn = RomanNumber.Parse("CXXIII"); //123
            int result = (int)rn;
            Assert.AreEqual(123, result);
        }

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
            TestCase[] testCases =
            [
                new( "N", 0 ),
                new( "I", 1 ),
                new( "II", 2 ),
                new( "III", 3 ),
                new( "IIII", 4 ),
                new( "V", 5 ),
                new( "X", 10 ),
                new( "D", 500 ),
                new( "IV", 4 ),
                new( "VI", 6 ),
                new( "VII", 7 ),
                new( "VIII", 8 ),
                new( "XI", 11 ),
                new( "XII", 12 ),
                new( "XIII", 13 ),
                new( "IX", 9 ),
                new( "MM", 2000 ),
                new( "MCM", 1900 ),
                new( "XL", 40 ),
                new( "XC", 90 ),
                new( "CD", 400 ),
                new( "CMII", 902 ),
                new( "DCCCC", 900 ),
                new( "XXXXX", 50 ),
                new( "CCCC", 400)
            ];
      
            foreach (var testCase in testCases)
            {
                RomanNumber rn = RomanNumber.Parse(testCase.Source);
                Assert.IsNotNull(rn, $"Parse result of '{testCase.Source}' is not null");
                Assert.AreEqual(
                    testCase.Value, 
                    rn.Value, 
                    $"Parse '{testCase.Source}' => {testCase.Value}"
                );
            }

            /* ¬ин€ток парсера Ч окр≥м причини вин€тку м≥стить в≥домост≥ 
             * про м≥сце виникненн€ помилки (позиц≥€ у р€дку)
             */

            String src = "RomanNumber.Parse('%r1') error: illegal";
            String pos = "in position";
            String illegalSymbolTemplate = $"{src} symbol '%r2' {pos} %r3";
            String tpl2 = $"{src} sequence: more than one smaller digits before '%r2' {pos} %r3";
            String tpl3 = $"{src} sequence: '%r2' before '%r3' {pos} %r4";

            testCases = [
                new( ["W", 'W', 0, illegalSymbolTemplate] ),
                new( ["CS", 'S', 1, illegalSymbolTemplate] ),
                new( ["CX1", '1', 2, illegalSymbolTemplate] ),
                // ѕеред цифрою Ї дек≥лька цифр, менших за нењ
                // !! кожна пара цифр Ч правильна комб≥нац≥€,
                // проблема створюЇтьс€ щонайменше трьома цифрами
                new( ["IIX", 'X', 2, tpl2] ),
                new( ["VIX", 'X', 2, tpl2] ),
                new( ["XXC", 'C', 2, tpl2] ),
                new( ["IXC", 'C', 2, tpl2] ),
                // "в≥дстань" м≥ж цифрами при в≥дн≥манн≥:
                // в≥дн≥матись можуть I, X, C причому в≥д
                // двох сус≥дних цифр (I Ц в≥д V та X, ...)
                new( ["VX", 'V', 'X', 0, tpl3] ),
                new( ["LC", 'L', 'C', 0, tpl3] ),
                new( ["DM", 'D', 'M', 0, tpl3] ),
                new( ["IC", 'I', 'C', 0, tpl3] ),
                new( ["MIM", 'I', 'M', 1, tpl3] ),
                new( ["MVM", 'V', 'M', 1, tpl3] ),
                new( ["MXM", 'X', 'M', 1, tpl3] ),
                new( ["CVC", 'V', 'C', 1, tpl3] ),
                new( ["MCVC", 'V', 'C', 2, tpl3] ),
                new( ["DCIC", 'I', 'C', 2, tpl3] ),
                new( ["IM", 'I', 'M', 0, tpl3] ),
                // ћенша цифра п≥сл€ двох однакових
                /*new( "IXX", [tpl6.R(["I"]) ]),
                new( "IXXX", [tpl6.R(["I"]) ]),
                new( "XCC", [tpl6.R(["X"]) ]),
                new( "XCCC", [tpl6.R(["X"]) ]),
                new( "CXCC", [tpl6.R(["X"]) ]),
                new( "CMM", [tpl6.R(["C"]) ]),
                new( "CMMM", [tpl6.R(["C"]) ]),
                new( "MCMM", [tpl6.R(["C"]) ]),
                new( "LCC", [tpl6.R(["L"]) ]),
                new( "ICCC", [tpl6.R(["I"]) ]),*/
                // ÷ифра N не може бути у числ≥, т≥льки сама по соб≥
                //new( "NN", [tpl1.R(["0"]), tpl2.R(["1"]), tpl3]),
                //new( "IN", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                //new( "NX", [tpl1.R(["0"]), tpl2.R(["0"]), tpl3]),
                //new( "NC", [tpl1.R(["0"]), tpl2.R(["1"]), tpl3]),
                //new( "XNC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                //new( "XVIN", [tpl1.R(["3"]), tpl2.R(["3"]), tpl3]),
                //new( "XNNC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                //new( "NMC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                //new( "NIX", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
            ];
            
            foreach(var excCase in testCases) 
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase.Source),
                    $"RomanNumber.Parse(\"{excCase.Source}\") must throw FormatException"
                );
                Assert.AreEqual(
                    excCase.ExMessage,
                    ex.Message,
                    $"ex.Message must contain '{excCase.ExMessage}'; ex.Message: {ex.Message}"
                );

                /* ЌакладаЇмо вимоги на пов≥домленн€:
                 * Ч маЇ м≥стити сам символ, що призводить до вин€тку 
                 * Ч маЇ м≥стити позиц≥ю символу в р€дку
                 * Ч маЇ м≥стити назву методу та класу*/

                //foreach (String part in exCase.ExMessageParts!)
                //{
                //    Assert.IsTrue(
                //    ex.Message.Contains(part),
                //    $"ex.Message must contain '{part}'; ex.Message: {ex.Message}"
                //    );
                //}
            }
        }

        [TestMethod]
        public void DigitValueTest()
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

        [TestMethod]
        public void PlusTest()
        {
            RomanNumber rn1 = new(1), rn2 = new(2);
            RomanNumber res = rn1.Plus(rn2);
            Assert.IsInstanceOfType<RomanNumber>(res);
            Assert.AreNotEqual(rn1, res);
            Assert.AreNotEqual(rn2, res);
            // вимога: результат Plus не Ї ан≥ rn1, ан≥ rn2
            // забезпечити доц≥льн≥сть правидьного р≥шенн€ (множину тест≥в)
            for (var i = 0; i < 100; ++i)
            {
                Assert.AreEqual(
                    i + rn1.Value,
                    rn1.Plus(new(i)).Value,
                    $"1 + {i} --> {1 + i}"
                );
            }
            // вимога: маЇ бути вин€ток при передач≥ Null
            Assert.ThrowsException<ArgumentNullException>(
                () => rn1.Plus(null!)
            );
            // RomanNumber.Plus(RomanNumber.Plus(rn1, rn2), rn2);
            // RomanNumber.Plus(rn1, rn2, rn2);
        }
    }

    record TestCase(String Source, int? Value, String? ExMessage = null)
    {
        public TestCase(String Source, String? ExMessage) : this(Source, null, ExMessage) { }

        public TestCase(List<Object> data) : 
            this(
                data.First().ToString()!, 
                null, 
                data.Last().ToString()!.R(data[..^1])
            ) { }
    }

    public static class StringExtension
    {
        public static String R(this String Source, List<Object> replaces)
        {
            String res = Source;
            int i = 0;

            foreach (var r in replaces)
            {
                ++i;
                res = res.Replace($"%r{i}", r.ToString());
            }

            return res;
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