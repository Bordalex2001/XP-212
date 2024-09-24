using App;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class RomanNumberValidatorTest
    {
        private TestCase[] casesCheckSymbols = [
            new( "W", ["W", "0"]),
            new( "CS", ["S", "1"]),
            new( "CX1", ["1", "2"])
        ];
        
        private TestCase[] casesCheckZeroDigitTest = [
            new("NN", ["0"]),
            new("IN", ["1"]),
            new("NX", ["0"]),
            new("NC", ["0"]),
            new("XNC", ["1"]),
            new("XVIN", ["3"]),
            new("XNNN", ["1"])
        ];

        [TestMethod]
        public void ValidateTest()
        {
            foreach (var excCase in casesCheckZeroDigitTest.Concat(casesCheckSymbols))
            {
                var ex = Assert.ThrowsException<FormatException>(
                  () => RomanNumberValidator.Validate(excCase.Source),
                  $"RomanNumberValidator.Validate(\"{excCase.Source}\") must throw FormatException"
                );
            }
        }
        
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
                ["XCC", 'X', 0],   // 
                ["XCCC", 'X', 0],  //
                ["CXCC", 'X', 1],
                ["CMM", 'C', 0],
                ["CMMM", 'C', 0],
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
                ["NC", '0', 1],   // 
                ["XNC", '1', 1],
                ["XVIN", '3', 3],
                ["XNNC", '1', 1],
                ["NMC", '1', 1],
                ["NIX", '1', 1]
            ];
            // Тестування приватних методів — задача потрібна, але вимагає особливого підходу
            // Об'єктна рефлексія — робота з типами даних
            Type rnvType = typeof(RomanNumberValidator);
            String methodName = "CheckZeroDigit";
            MethodInfo? method = rnvType.GetMethod( // шукаєио у типі метод за 
                "CheckZeroDigit",                   // назвою, та такий, що 
                BindingFlags.Static |               // є статичним та
                BindingFlags.NonPublic);            // не public

            Assert.IsNotNull(method, $"Method '{methodName}' must be in type");

            foreach (var excCase in casesCheckZeroDigitTest)
            {
                // !! Виконання методів з винятками через рефлексію
                // призводить до появи окремого винятку:
                // TargetInvocationException замість будь-якого винятку,
                // що викидається у самому методі
                // Також зауважимо, що Assert перевіряє типи суворо, тобто 
                // зазначити загальний Exception також буде помилкою
                var ex = Assert.ThrowsException<TargetInvocationException>(
                   () => //RomanNumber.Parse(excCase[0].ToString()!),
                         method.Invoke(
                             null,
                             [excCase.Source]),
                   $"RomanNumber.Parse(\"{excCase.Source}\") must throw TargetInvocationException"
                );
                var innerEx = ex.InnerException;
                Assert.AreEqual(
                    "FormatException",
                    innerEx?.GetType().Name,
                    $"RomanNumberValidator.{methodName}(\"{excCase.Source}\") must throw FormatException"
                );
            }
        }
    }
}
/*
 * звичайний виклик                 | через рефлексію
 * obj.theMethod(params)            | type = obj.GetType() / typeof(TheType)
 *                                  | method = type.GetMethod("theMethod")
 * obj.theMethod(<this>, params)    |
 *                                  | method.Invoke(obj, [params])
 */