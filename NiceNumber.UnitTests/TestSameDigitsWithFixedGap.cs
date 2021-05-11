using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
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
            const long number = 23923723;
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameNumbers,
                SequenceType = SequenceType.Sequential,
                Length = 3,
                FirstNumber = 23,
                FirstPosition = 0,
                RegularityNumber = 0,
                Gap = 1,
                Positions = new byte[]{0, 3, 6},
                SubNumberLengths = new byte[] {2, 2, 2}
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.Pass();
        }
    }
}