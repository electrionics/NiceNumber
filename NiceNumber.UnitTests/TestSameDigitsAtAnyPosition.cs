using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
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
            var regularity = new SameNumbers();
            var supposed = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.Sequential,
                Length = 7,
                FirstNumber = 2,
                FirstPosition = 0,
                RegularityNumber = 7,
                Positions = new byte[]{ 0, 1, 2, 3, 4, 5, 6},
                SubNumberLengths = new byte[]{ 1, 1, 1, 1, 1, 1, 1}
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
            var regularity = new SameNumbers();
            var supposed = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.FixedGap,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 2,
                RegularityNumber = 3,
                Gap = 1,
                Positions = new byte[]{ 2, 4, 6},
                SubNumberLengths = new byte[]{ 1, 1, 1}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            //Assert.AreEqual(detected.Count, 1);
            Assert.IsTrue(detected.Contains(supposed));
            Assert.Pass();
        }
        
        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_OneResult()
        {
            const long number = 3412282;
            var regularity = new SameNumbers();
            var supposed = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 2,
                FirstPosition = 3,
                RegularityNumber = 3,
                Positions = new byte[]{ 3, 4, 6},
                SubNumberLengths = new byte[] { 1, 1, 1}
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
            var regularity = new SameNumbers();
            var supposed = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 7,
                FirstPosition = 3,
                RegularityNumber = 2,
                Positions = new byte[]{ 3, 6},
                SubNumberLengths = new byte[]{ 1, 1}
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
            var regularity = new SameNumbers();

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.IsFalse(detected.Any(x => x.Type == RegularityType.SameDigits || x.Type == RegularityType.SameNumbers));
            Assert.Pass();
        }

        [Test]
        public void Test_DetectSameNumbersAtAnyPosition_SeveralResults_NotAllPositionsUsed()
        {
            const long number = 377537;
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 37,
                FirstPosition = 0,
                RegularityNumber = 2,
                Positions = new byte[]{ 0, 4},
                SubNumberLengths = new byte[] { 2, 2}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 7,
                FirstPosition = 1,
                RegularityNumber = 3,
                Positions = new byte[]{ 1, 2, 5},
                SubNumberLengths = new byte[] { 1, 1, 1}
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
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.FixedGap,
                Length = 2,
                FirstNumber = 3,
                FirstPosition = 0,
                RegularityNumber = 2,
                Positions = new byte[]{ 0, 4},
                SubNumberLengths = new byte[]{ 1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.FixedGap,
                Length = 2,
                FirstNumber = 5,
                FirstPosition = 1,
                RegularityNumber = 2,
                Positions = new byte[]{ 1, 3},
                SubNumberLengths = new byte[]{ 1, 1}
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.FixedGap,
                Length = 2,
                FirstNumber = 7,
                FirstPosition = 2,
                RegularityNumber = 2,
                Positions = new byte[]{ 2, 5},
                SubNumberLengths = new byte[]{ 1, 1}
            };
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count(x => x.Type == RegularityType.SameDigits), 3);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.Pass();
        }

        [Test]
        public void Test_ManySameDigits_OneResult()
        {
            const long number = 444543524022;
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameDigits,
                SequenceType = SequenceType.General,
                Length = 5,
                FirstNumber = 4,
                FirstPosition = 0,
                RegularityNumber = 5,
                Positions = new byte[]{ 0, 1, 2, 4, 8 },
                SubNumberLengths = new byte[]{ 1, 1, 1, 1, 1}
            };
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.Pass();
        }

        [Test]
        public void Test_SameNumbers_NumberFromSameDigits()
        {
            const long number = 431148851128;
            var regularity = new SameNumbers();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.SameNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 11,
                FirstPosition = 2,
                RegularityNumber = 2,
                Positions = new byte[]{ 2, 8 },
                SubNumberLengths = new byte[]{ 2, 2}
            };
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.Pass();
        }
    }
}