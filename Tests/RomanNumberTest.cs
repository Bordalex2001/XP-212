using App;

namespace Tests
{
    [TestClass]
    public class RomanNumberTest
    {
        [TestMethod]
        public void ParseTest()
        {
            RomanNumber rn = RomanNumber.Parse("M");
            //rn = null!;
            Assert.IsNotNull(rn, "Parse result is not null");
            Assert.AreEqual(0, rn.Value, "Zero testing");
  /*Dictionary<String, int> testCases = new()
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
                { "XI", 11 },
                { "IX", 9 },
                { "MM", 2000 },
                { "MCM", 1900 },
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
            }*/
        }
        /*[TestMethod]
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
        }*/
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