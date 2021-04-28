using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
using NiceNumber.Regularities;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestSameDigitsAtAnyPosition
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DetectSameNumbersSequence()
        {
            const long number = 2222222;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 7,
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 0,
                Positions = new byte[]{ 0, 1, 2, 3, 4, 5, 6} //TODO: арифметическая прогрессия, может тоже детектиться снаружи
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.AreEqual(detected.First(), supposed);
            Assert.Pass();
        }

        [Test]
        public void Test_DetectSameNumbersWithGapSequence()
        {
            const long number = 3421282;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 2,
                RegularityNumber = 0,
                Positions = new byte[]{ 2, 4, 6}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.AreEqual(detected.First(), supposed);
            Assert.Pass();
        }
        
        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_OneResult()
        {
            const long number = 3412282;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 3,
                RegularityNumber = 0,
                Positions = new byte[]{ 3, 4, 6}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.AreEqual(detected.First(), supposed);
            Assert.Pass();
        }
        
        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_OneResult_MinimalSet()
        {
            const long number = 3417587;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 2,
                FirstNumber = 7,
                FirstPosition = 3,
                RegularityNumber = 0,
                Positions = new byte[]{ 3, 6}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 1);
            Assert.AreEqual(detected.First(), supposed);
            Assert.Pass();
        }
        
        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_NoResults()
        {
            const long number = 3416587;
            var regularity = new SameDigitsAtAnyPosition();

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 0);
            Assert.Pass();
        }

        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_SeveralResults_NotAllPositionsUsed()
        {
            const long number = 377537;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed1 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 2,
                FirstNumber = 3,
                FirstPosition = 0,
                RegularityNumber = 0,
                Positions = new byte[]{ 0, 4}
            };
            var supposed2 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 3,
                FirstNumber = 7,
                FirstPosition = 1,
                RegularityNumber = 0,
                Positions = new byte[]{ 1, 2, 5}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }
        
        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_SeveralResults_AllPositionsUsed()
        {
            const long number = 357537;
            var regularity = new SameDigitsAtAnyPosition();
            var supposed1 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 2,
                FirstNumber = 3,
                FirstPosition = 0,
                RegularityNumber = 0,
                Positions = new byte[]{ 0, 4}
            };
            var supposed2 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 2,
                FirstNumber = 5,
                FirstPosition = 1,
                RegularityNumber = 0,
                Positions = new byte[]{ 1, 3}
            };
            var supposed3 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.SameDigitsAtAnyPosition,
                Length = 2,
                FirstNumber = 7,
                FirstPosition = 2,
                RegularityNumber = 0,
                Positions = new byte[]{ 2, 5}
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