using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestGeometricProgressionAtAnyPosition
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Test_GeometricProgressionAtAnyPosition_CommonCase()
        {
            const long number = 2721943816;
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.GeometricProgression,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 27,
                RegularityNumber = (double) 1 / 3,
                FirstPosition = 0,
                Positions = new byte[] {0, 4, 6, 8},
                SubNumberLengths = new byte[] {2, 1, 1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.GeometricProgression,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 5, 7, 8},
                SubNumberLengths = new byte[] {1, 1, 1, 2}
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.GeometricProgression,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 2,
                RegularityNumber = 2,
                FirstPosition = 2,
                Positions = new byte[] {2, 5, 7, 8},
                SubNumberLengths = new byte[] {1, 1, 1, 2}
            };
            var supposed4 = new RegularityDetectResult
            {
                Type = RegularityType.GeometricProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                RegularityNumber = 4,
                FirstPosition = 3,
                Positions = new byte[] {3, 5, 8},
                SubNumberLengths = new byte[] {1, 1, 2}
            };
            var supposed5 = new RegularityDetectResult
            {
                Type = RegularityType.GeometricProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                RegularityNumber = 9,
                FirstPosition = 3,
                Positions = new byte[] {3, 4, 7},
                SubNumberLengths = new byte[] {1, 1, 2}
            };
            
            var regularity = new GeometricProgression(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 5);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.IsTrue(detected.Contains(supposed4));
            Assert.IsTrue(detected.Contains(supposed5));
            Assert.Pass();
        }

        [Test]
        public void Test_GeometricProgressionAtAnyPosition_NotCountDoubleQ()
        {
            const long number = 2718128; // q = 2/3
            var regularity = new GeometricProgression(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 0);
            Assert.Pass();
        }

        [Test]
        public void Test_GeometricProgressionAtAnyPosition_NotCountZeroRegularityNumber()
        {
            const long number = 476670057680;
            var regularity = new GeometricProgression(3);
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 0);
            Assert.Pass();
        }
    }
}