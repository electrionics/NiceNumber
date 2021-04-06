using System.Linq;
using NiceNumber.Regularities;
using NiceNumber.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestArithmeticProgressionAtAnyPosition
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Test_ArithmeticProgressionAtAnyPosition_CommonCase()
        {
            const long number = 2146468;
            var supposed1 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 6}
            };
            var supposed2 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 4, 5, 6}
            };
            var supposed3 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 5, 6}
            };
            
            var regularity = new ArithmeticProgressionAtAnyPosition(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 3);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.Pass();
        }

        [Test]
        public void Test_ArithmeticProgressionAtAnyPosition_SequentialGrowth()
        {
            const long number = 1103030305050777;
            /*                  0123456789012345
             *
             * 0 3 9 13
             * 0 3 9 14
             * 0 3 9 15
             * 0 3 11 13
             * 0 3 11 14
             * 0 3 11 15
             * 0 5 9 13
             * 0 5 9 ...
             * 0 5 11 ...
             * 0 7 ...
             * 1 ... ...
             */
            
            var supposedPositionGroups = new[]
            {
                new byte[] {0, 1},
                new byte[] {3, 5, 7},
                new byte[] {9, 11},
                new byte[] {13, 14, 15}
            };
            var total = supposedPositionGroups.Aggregate(1, (res, bytes) => res * bytes.Length);
            var supposedPrototype = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = supposedPositionGroups.Length,
                FirstNumber = 1,
                RegularityNumber = 2
            };
            
            var regularity = new ArithmeticProgressionAtAnyPosition(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, total);
            foreach (var detectResult in detected)
            {
                Assert.AreEqual(supposedPrototype.Type, detectResult.Type);
                Assert.AreEqual(supposedPrototype.Length, detectResult.Length);
                Assert.AreEqual(supposedPrototype.FirstNumber, detectResult.FirstNumber);
                Assert.AreEqual(supposedPrototype.RegularityNumber, detectResult.RegularityNumber);
                
                Assert.IsTrue(supposedPositionGroups.All(supposedPositions => 
                    ((RegularityDetectResultWithPositions)detectResult).Positions.Any(supposedPositions.Contains)));
            }

            Assert.Pass();
        }
    }
}