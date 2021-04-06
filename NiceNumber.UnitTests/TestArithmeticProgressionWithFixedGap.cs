using NiceNumber.Regularities;
using NiceNumber.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestArithmeticProgressionWithFixedGap
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test_ArithmeticProgressionWithFixedGap()
        {
            const long number = 23256739;
            var regularity = new ArithmeticProgressionWithFixedGap();
            var supposed1 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.ArithmeticProgressionWithFixedGap,
                Length = 4,
                FirstNumber = 3,
                FirstPosition = 1,
                RegularityNumber = 2,
                Gap = 1
            };
            var supposed2 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.ArithmeticProgressionWithFixedGap,
                Length = 3,
                FirstNumber = 3,
                FirstPosition = 1,
                RegularityNumber = 3,
                Gap = 2
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }

        [Test]
        public void Test_IncludeFiltering()
        {
            const long number = 12131415161;
            var regularity = new ArithmeticProgressionWithFixedGap();
            var supposed1 = new RegularityDetectResultWithGap
            {
                Type = RegularityType.ArithmeticProgressionWithFixedGap,
                Length = 5, // 2 3 4 5 6
                FirstNumber = 2,
                FirstPosition = 1,
                RegularityNumber = 1,
                Gap = 1
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.Pass();
        }
    }
}