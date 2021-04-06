using System.Linq;
using NiceNumber.Regularities;
using NiceNumber.Results;
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
            var regularity = new SameDigitsSequential();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigitsSequential,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 1,
                RegularityNumber = 0
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigitsSequential,
                Length = 2,
                FirstNumber = 3,
                FirstPosition = 4,
                RegularityNumber = 0
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