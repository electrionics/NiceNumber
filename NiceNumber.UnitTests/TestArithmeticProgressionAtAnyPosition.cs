using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Results;
using NiceNumber.Regularities;
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
            const long number = 2446468;
            var supposed1 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 6},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed2 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 4, 5, 6},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed3 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 5, 6},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed4 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 1, 3, 6},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed5 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 1, 5, 6},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed6 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 3,
                FirstNumber = 24,
                RegularityNumber = 22,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 5},
                SubNumberLengths = new byte[] {2, 2, 2}
            };
            var supposed7 = new RegularityDetectResultWithPositions
            {
                Type = RegularityType.ArithmeticProgressionAtAnyPosition,
                Length = 3,
                FirstNumber = 4,
                RegularityNumber = 0,
                FirstPosition = 1,
                Positions = new byte[] {1, 2, 4},
                SubNumberLengths = new byte[] {1, 1, 1}
            };
            
            var regularity = new ArithmeticProgressionAtAnyPosition(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 7);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.IsTrue(detected.Contains(supposed4));
            Assert.IsTrue(detected.Contains(supposed5));
            Assert.IsTrue(detected.Contains(supposed6));
            Assert.IsTrue(detected.Contains(supposed7));
            Assert.Pass();
        }

        //[Test]
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