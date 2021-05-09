using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
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
            const long number = 135998642;
            var regularity = new ArithmeticProgression();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 0,
                RegularityNumber = 2,
                Positions = new byte[]{ 0, 1, 2},
                SubNumberLengths = new byte[]{ 1, 1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 4,
                FirstNumber = 8,
                FirstPosition = 5,
                RegularityNumber = -2,
                Positions = new byte[]{ 5, 6, 7, 8},
                SubNumberLengths = new byte[]{ 1, 1, 1, 1}
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count(x => x.SequenceType == SequenceType.Sequential), 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }

        [Test]
        public void Test_DetectArithmeticProgressionSequential_Numbers()
        {
            const long number = 2610149471115;
            var regularity = new ArithmeticProgression();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 4, // 6 digits
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 4,
                Positions = new byte[]{ 0, 1, 2, 4},
                SubNumberLengths = new byte[]{ 1, 1, 2, 2}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 3, // 4 digits
                FirstNumber = 14,
                FirstPosition = 4,
                RegularityNumber = -5,
                Positions = new byte[]{ 4, 6, 7},
                SubNumberLengths = new byte[]{ 2, 1, 1}
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 3, // 5 digits
                FirstNumber = 7,
                FirstPosition = 8,
                RegularityNumber = 4,
                Positions = new byte[]{ 8, 9, 11},
                SubNumberLengths = new byte[]{ 1, 2, 2}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count(x => x.SequenceType == SequenceType.Sequential), 3);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.Pass();
        }
    }
}