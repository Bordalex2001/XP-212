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
             * �������� ���� ������ ���������� ������ �������� ��������������� ����-�����, �� ��������� ������������ ��� ������ � ��������� ���������� ���������: 
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

            /* ������� ������� � ���� ������� ������� ������ ������� 
             * ��� ���� ���������� ������� (������� � �����)
             */

            String tpl1 = "illegal symbol '%r1'";
            String tpl2 = "in position %r1";
            String tpl3 = "RomanNumber.Parse";
            String tpl4 = "illegal sequence: more than one smaller digits before '%r1'";
            String tpl5 = "illegal sequence: '%r1' before '%r2'";
            String[] all = [tpl3];

            testCases = [
                new( "W", [tpl1.R(["W"]), tpl2.R(["0"]), tpl3]),
                new( "CS", [tpl1.R(["S"]), tpl2.R(["1"]), tpl3]),
                new( "CX1", [tpl1.R(["1"]), tpl2.R(["2"]), tpl3]),
                // ����� ������ � ������� ����, ������ �� ��
                // !! ����� ���� ���� � ��������� ���������,
                // �������� ����������� ���������� ������ �������
                new( "IIX", [tpl4.R(["X"]), tpl2.R(["2"]), tpl3]),
                new( "VIX", [tpl4.R(["X"]), tpl2.R(["2"]), tpl3]),
                new( "XXC", [tpl4.R(["C"]), tpl2.R(["2"]), tpl3]),
                new( "IXC", [tpl4.R(["C"]), tpl2.R(["2"]), tpl3]),
                // "�������" �� ������� ��� �������:
                // ��������� ������ I, X, C ������� ��
                // ���� ������� ���� (I � �� V �� X, ...)
                new( "VX", [tpl5.R(["V", "X"]), tpl2.R(["0"]), tpl3]),
                new( "LC", [tpl5.R(["L", "C"]), tpl2.R(["0"]), tpl3]),
                new( "DM", [tpl5.R(["D", "M"]), tpl2.R(["0"]), tpl3]),
                new( "IC", [tpl5.R(["I", "C"]), tpl2.R(["0"]), tpl3]),
                new( "MIM", [tpl5.R(["I", "M"]), tpl2.R(["1"]), tpl3]),
                new( "MVM", [tpl5.R(["V", "M"]), tpl2.R(["1"]), tpl3]),
                new( "MXM", [tpl5.R(["X", "M"]), tpl2.R(["1"]), tpl3]),
                new( "CVC", [tpl5.R(["V", "C"]), tpl2.R(["1"]), tpl3]),
                new( "MCVC", [tpl5.R(["V", "C"]), tpl2.R(["2"]), tpl3]),
                new( "DCIC", [tpl5.R(["I", "C"]), tpl2.R(["2"]), tpl3]),
                new( "IM", [tpl5.R(["I", "M"]), tpl2.R(["0"]), tpl3]),
                // ����� ����� ���� ���� ���������
                new( "IXX", [tpl4.R(["I"]), tpl2.R(["0"]), tpl3]),
                new( "IXXX", [tpl4.R(["I"]), tpl2.R(["0"]), tpl3]),
                new( "XCC", [tpl4.R(["X"]), tpl2.R(["0"]), tpl3]),
                new( "XCCC", [tpl4.R(["X"]), tpl2.R(["0"]), tpl3]),
                new( "CXCC", [tpl4.R(["X"]), tpl2.R(["1"]), tpl3]),
                new( "CMM", [tpl4.R(["C"]), tpl2.R(["0"]), tpl3]),
                new( "CMMM", [tpl4.R(["C"]), tpl2.R(["0"]), tpl3]),
                new( "MCMM", [tpl4.R(["C"]), tpl2.R(["1"]), tpl3]),
                new( "LCC", [tpl4.R(["L"]), tpl2.R(["0"]), tpl3]),
                new( "ICCC", [tpl4.R(["I"]), tpl2.R(["0"]), tpl3]),
                // ����� N �� ���� ���� � ����, ����� ���� �� ���
                new( "NN", [tpl1.R(["0"]), tpl2.R(["1"]), tpl3]),
                new( "IN", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                new( "NX", [tpl1.R(["0"]), tpl2.R(["0"]), tpl3]),
                new( "NC", [tpl1.R(["0"]), tpl2.R(["1"]), tpl3]),
                new( "XNC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                new( "XVIN", [tpl1.R(["3"]), tpl2.R(["3"]), tpl3]),
                new( "XNNC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                new( "NMC", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
                new( "NIX", [tpl1.R(["1"]), tpl2.R(["1"]), tpl3]),
            ];
            
            foreach(var excCase in testCases) 
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase.Source),
                    $"RomanNumber.Parse(\"{excCase.Source}\") must throw FormatException"
                );

                /* ��������� ������ �� �����������:
                 * � �� ������ ��� ������, �� ���������� �� ������� 
                 * � �� ������ ������� ������� � �����
                 * � �� ������ ����� ������ �� �����*/
                 
                foreach (String part in excCase.ExMessageParts!)
                {
                    Assert.IsTrue(
                        ex.Message.Contains(part),
                        $"ex.Message must contain '{part}'; ex.Message: {ex.Message}"
                    );
                }
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

    record TestCase(String Source, int? Value, IEnumerable<String>? ExMessageParts = null)
    {
        public TestCase(String Source, IEnumerable<String> parts) : this(Source, null, parts) { }
    }

    public static class StringExtension
    {
        public static String F(this String Source, IEnumerable<String> olds, IEnumerable<String> news)
        {
            String res = Source;

            foreach (var item in olds.Zip(news))
            {
                res = res.Replace(item.First, item.Second);
            }

            return res;
        }

        public static String R(this String Source, IEnumerable<String> replaces)
        {
            String res = Source;
            int i = 0;

            foreach (var r in replaces)
            {
                ++i;
                res = res.Replace($"%r{i}", r);
            }

            return res;
        }
    }
}
/* �������� ����� �� ���������� �������� �������� �����:
 * � ���� ����� ���������� ������ ��������� ������
 * � ���� ����� ��������� �� � ������, � ������� Test
 * � ������ ����� ����� ���������� ������ ������������ ����� � ����� � ������� Test
 * 
 * ������ ����� ��������� ������� (Assert)
 * ���� ��������� ���������, ���� �� ���� ������� ������, ���������� � ���� ���� � ���� ����� ������
 */