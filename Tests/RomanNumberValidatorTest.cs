using App;

namespace Tests
{
    [TestClass]
    public class RomanNumberValidatorTest
    {
        [TestMethod]
        public void DigitValueTest()
        {
            //Dictionary<char, int> testCases = new()
            //{
            //    { 'N', 0 },
            //    { 'I', 1 },
            //    { 'V', 5 },
            //    { 'X', 10 },
            //    { 'L', 50 },
            //    { 'C', 100 },
            //    { 'D', 500 },
            //    { 'M', 1000 }
            //};
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
                    () => RomanNumberValidator.DigitValue(digit),
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
        public void DigitRatiosTest()
        {
            /* Тести на неправильну композицію числа (всі цифри правильні, але неправильна їх послідовність)
             * VV, VX, IIX, ...
             */
            Object[][] excCases2 = [
                ["VX", 'V', 'X', 0], // ---
                ["LC", 'L', 'C', 0], // "відстань" між цифрами при відніманні:
                ["DM", 'D', 'M', 0], // відніматись можуть I, X, C причому від
                ["IC", 'I', 'C', 0], // двох сусідних цифр (I – від V та X, ...)
                ["MIM", 'I', 'M', 1],
                ["MVM", 'V', 'M', 1],
                ["MXM", 'X', 'M', 1],
                ["CVC", 'V', 'C', 1],
                ["MCVC", 'V', 'C', 2],
                ["DCIC", 'I', 'C', 2],
                ["IM", 'I', 'M', 0] // ---
            ];
            foreach (var excCase in excCases2)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{excCase[0]}\") must throw FormatException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: '{excCase[1]}' before '{excCase[2]}'"),
                    $"ex.Message must contain symbols '{excCase[1]}' and '{excCase[2]}' which cause error: testCase: '{excCase[0]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {excCase[3]}"),
                    $"ex.Message must contain error symbol position, testCase: '{excCase[0]}'"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber))
                    && ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, testCase: '{excCase[0]}', ex.Message: {ex.Message}"
                );
            }
        }

        [TestMethod]
        public void InvalidLessCounterTest()
        {
            Object[][] excCases3 = [
                ["IIX", 'X', 2], // Перед цифрою є декілька цифр, менших за неї
                ["VIX", 'X', 2], // !! кожна пара цифр — правильна комбінація,
                ["XXC", 'C', 2], // проблема створюється щонайменше трьома цифрами
                ["IXC", 'C', 2],
                ["IIXL", 'L', 3],
                ["VIXL", 'L', 3]
            ];
            foreach (var excCase in excCases3)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{excCase[0]}\") must throw FormatException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"illegal sequence: more than one smaller digits before '{excCase[1]}'"),
                    $"ex.Message must contain symbol before error: '{excCase[1]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {excCase[2]}"),
                    $"ex.Message must contain error symbol position, testCase: '{excCase[0]}'"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber))
                    && ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, testCase: '{excCase[0]}', ex.Message: {ex.Message}"
                );
            }
        }

        [TestMethod]
        public void MaxAndLessCountersTest()
        {
            Object[][] excCases4 = [
                ["IXX", 'I', 0],   // Менша цифра після двох однакових
                ["IXXX", 'I', 0],  //
                ["IXIX", 'I', 0],  //
                ["XCC", 'X', 0],   // 
                ["XCXC", 'X', 0],
                ["IVIV", 'I'],
                ["XCCC", 'X', 0],
                ["CXCC", 'X', 1],
                ["CMM", 'C', 0],
                ["CMMM", 'C', 0],
                ["CMCM", 'C', 0],
                ["MCMM", 'C', 1],
                ["LCC", 'L', 0],
                ["ICCC", 'I', 0]
            ];
            foreach (var excCase in excCases4)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{excCase[0]}\") must throw FormatException"
                );
            }
        }

        [TestMethod]
        public void ChechZeroDigitTest()
        {
            Object[][] excCases5 = [
                ["NN", '0', 1],   // Цифра N не може бути у числі, тільки сама по собі
                ["IN", '1', 1],   //
                ["NX", '0', 0],   //
                ["NC", '0', 0],   // 
                ["XNC", '1', 1],
                ["IVIN", '3', 3],
                ["XNNC", '1', 1],
                ["NMC", '0', 0],
                ["NIX", '0', 0]
            ];
            foreach (var excCase in excCases5)
            {
                var ex = Assert.ThrowsException<FormatException>(
                    () => RomanNumber.Parse(excCase[0].ToString()!),
                    $"RomanNumber.Parse(\"{excCase[0]}\") must throw FormatException"
                );
            }
        }
    }
}