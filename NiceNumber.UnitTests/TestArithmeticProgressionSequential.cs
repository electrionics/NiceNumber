using NiceNumber.Regularities;
using NiceNumber.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestArithmeticProgressionSequential
    {
        [SetUp]
        public void SetUp()
        {
            
        }
        
        [Test]
        public void Test_DetectArithmeticProgressionSequential()
        {
            const long number = 123998642;
            var regularity = new ArithmeticProgressionSequential();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.AriphmeticProgressionSequential,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 0,
                RegularityNumber = 1
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.AriphmeticProgressionSequential,
                Length = 4,
                FirstNumber = 8,
                FirstPosition = 5,
                RegularityNumber = -2
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }

        [Test]
        public void Test_DetectArithmeticProgressionSequential_Numbers()
        {
            const long number = 2610149471115;
            var regularity = new ArithmeticProgressionSequential();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.AriphmeticProgressionSequential,
                Length = 4, // 6 digits
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 4
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.AriphmeticProgressionSequential,
                Length = 3, // 4 digits
                FirstNumber = 14,
                FirstPosition = 4,
                RegularityNumber = -5
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.AriphmeticProgressionSequential,
                Length = 3, // 5 digits
                FirstNumber = 7,
                FirstPosition = 5,
                RegularityNumber = 4
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