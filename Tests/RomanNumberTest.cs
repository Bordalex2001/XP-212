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
                { "DCCCC", 900 },
                { "XIXIIII", 23 },
                { "XXXXX", 50 },
                { "DDMD", 1500 }
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
                    ex.Message.Contains($"illegal symbols '{excCase[1]}'"),
                    $"ex.Message must contain symbols which cause error: '{excCase[1]}', ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"in position {excCase[2]}"),
                    $"ex.Message must contain error symbol positions, ex.Message: {ex.Message}"
                );
                Assert.IsTrue(
                    ex.Message.Contains(nameof(RomanNumber))
                    && ex.Message.Contains(nameof(RomanNumber.Parse)),
                    $"ex.Message must contain names of class and method, ex.Message: {ex.Message}"
                );
            }

            /* “ести на неправильну композиц≥ю числа (вс≥ цифри правильн≥, але неправильна њх посл≥довн≥сть)
             * VV, VX, IIX, ...
             */
            Object[][] excCases2 = [
                ["VX", 'V', 'X', 0], // ---
                ["LC", 'L', 'C', 0], // "в≥дстань" м≥ж цифрами при в≥дн≥манн≥:
                ["DM", 'D', 'M', 0], // в≥дн≥матись можуть I, X, C причому в≥д
                ["IC", 'I', 'C', 0], // двох сус≥дних цифр (I Ц в≥д V та X, ...)
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

            Object[][] excCases3 = [
                ["IIX", 'X', 2], // ѕеред цифрою Ї дек≥лька цифр, менших за нењ
                ["VIX", 'X', 2], // !! кожна пара цифр Ч правильна комб≥нац≥€,
                ["XXC", 'C', 2], // проблема створюЇтьс€ щонайменше трьома цифрами
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

            Object[][] excCases4 = [
                ["IXX", 'I', 0],   // ћенша цифра п≥сл€ двох однакових
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

            Object[][] excCases5 = [
                ["NN", '0', 1],   // ÷ифра N не може бути у числ≥, т≥льки сама по соб≥
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

        [TestMethod]
        public void DigitValueTest()
        {
            Dictionary<char, int> testCases = new()
            {
                { 'N', 0 },
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 },
            };
            foreach (var testCase in testCases)
            {
                Assert.AreEqual(
                    testCase.Value,
                    RomanNumber.DigitValue(testCase.Key),
                    $"{testCase.Key} => {testCase.Value}"
                );
            }

            char[] excCases = { '0', '1', 'x', 'i', '&' };

            foreach (var testCase in excCases)
            {
                var ex = Assert.ThrowsException<ArgumentException>(
                    () => RomanNumber.DigitValue(testCase),
                    $"DigitValue('{testCase}') must throw ArgumentException"
                );
                Assert.IsTrue(
                    ex.Message.Contains($"'{testCase}'"),
                    $"DigitValue ex.Message should contain a symbol which cause exception: symbol: '{testCase}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains($"{nameof(RomanNumber)}") 
                    && ex.Message.Contains($"{nameof(RomanNumber.DigitValue)}"),
                    $"DigitValue ex.Message should contain a name of class and a name of method which cause exception: symbol: '{testCase}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains("'digit'") 
                    && ex.Message.Contains("argument"),
                    $"DigitValue ex.Message should contain a name of the argument ('digit') and a word 'argument' which cause exception: symbol: '{testCase}', ex.Message: '{ex.Message}'."
                );
                Assert.IsTrue(
                    ex.Message.Contains("not valid Roman digit"),
                    $"DigitValue ex.Message should contain a text 'not valid Roman digit' which cause exception: symbol: '{testCase}', ex.Message: '{ex.Message}'."
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