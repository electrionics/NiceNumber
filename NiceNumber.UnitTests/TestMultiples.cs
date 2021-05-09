using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestMultiples
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Test_Multiples_CommonCase()
        {
            const long number = 2024359;
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 3},
                SubNumberLengths = new byte[] {1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.Sequential,
                Length = 2,
                FirstNumber = 20,
                RegularityNumber = 10,
                FirstPosition = 0,
                Positions = new byte[] {0, 2},
                SubNumberLengths = new byte[] {2, 1}
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 20,
                RegularityNumber = 5,
                FirstPosition = 0,
                Positions = new byte[] {0, 3},
                SubNumberLengths = new byte[] {2, 1}
            };
            var supposed4 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 20,
                RegularityNumber = 4,
                FirstPosition = 0,
                Positions = new byte[] {0, 5},
                SubNumberLengths = new byte[] {2, 1}
            };
            var supposed5 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 3,
                RegularityNumber = 3,
                FirstPosition = 4,
                Positions = new byte[] {4, 6},
                SubNumberLengths = new byte[] {1, 1}
            };
            var supposed6 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.Sequential,
                Length = 2,
                FirstNumber = 24,
                RegularityNumber = 8,
                FirstPosition = 2,
                Positions = new byte[] {2, 4},
                SubNumberLengths = new byte[] {2, 1}
            };
            var supposed7 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.General,
                Length = 2,
                FirstNumber = 243,
                RegularityNumber = 27,
                FirstPosition = 2,
                Positions = new byte[] {2, 6},
                SubNumberLengths = new byte[] {3, 1}
            };
            var supposed8 = new RegularityDetectResult
            {
                Type = RegularityType.MultiplesNumbers,
                SequenceType = SequenceType.Sequential,
                Length = 2,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 2,
                Positions = new byte[] {2, 3},
                SubNumberLengths = new byte[] {1, 1}
            };

            var regularity = new Multiples(1);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 8);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.IsTrue(detected.Contains(supposed4));
            Assert.IsTrue(detected.Contains(supposed5));
            Assert.IsTrue(detected.Contains(supposed6));
            Assert.IsTrue(detected.Contains(supposed7));
            Assert.IsTrue(detected.Contains(supposed8));
            Assert.Pass();
        }
    }
}