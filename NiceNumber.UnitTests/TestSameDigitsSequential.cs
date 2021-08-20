using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestSameDigitsSequential
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DetectSameDigitsSequential()
        {
            const long number = 3222337;
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.Sequential,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 1,
                RegularityNumber = 3,
                Positions = new byte[]{1, 2, 3},
                SubNumberLengths = new byte[]{ 1, 1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 3,
                FirstPosition = 0,
                RegularityNumber = 3,
                Positions = new byte[] {0, 4, 5},
                SubNumberLengths = new byte[]{ 1, 1, 1}
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }

        [Test]
        public void Test_DetectSameDigitsSequential_NoResults()
        {
            
        }
        
        [Test]
        public void Test_DetectSameDigitsSequential_OneResult()
        {
            
        }
        
        [Test]
        public void Test_DetectSameDigitsSequential_OneResult_MinimalSet()
        {
            
        }
        
        [Test]
        public void Test_DetectSameDigitsSequential_SeveralResults_NotAllPositionsUsed()
        {
            
        }
        
        [Test]
        public void Test_DetectSameDigitsSequential_SeveralResults_AllPositionsUsed()
        {
            
        }
    }
}