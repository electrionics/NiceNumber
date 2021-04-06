using NiceNumber.Regularities;
using NiceNumber.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestSameDigitsWithFixedGap
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DetectSameDigitsWithFixedGap()
        {
            const long number = 2323233727;
            var regularity = new SameDigitsWithFixedGap();
            var supposed1 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.SameDigitsWithFixedGap,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 1,
                Gap = 1
            };
            var supposed2 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.SameDigitsWithFixedGap,
                Length = 3,
                FirstNumber = 3,
                FirstPosition = 1,
                RegularityNumber = 1,
                Gap = 1
            };
            var supposed3 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.SameDigitsWithFixedGap,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 3,
                Gap = 3
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 3);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.Pass();
        }
    }
}