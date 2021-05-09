using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestMirrorDigits
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void Test_MirrorDigitsAtAnyPosition_CommonCase()
        {
            var number = 1024595542219;
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 1,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 4, 6, 8, 9, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 4, 7, 8, 9, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 1,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 4, 6, 8, 10, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed4 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 2,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 4, 7, 8, 10, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed5 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 0,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 6, 7, 8, 10, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed6 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 8,
                FirstNumber = 1,
                RegularityNumber = 0,
                FirstPosition = 0,
                Positions = new byte[] {0, 2, 3, 6, 7, 8, 9, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1, 1, 1, 1, 1}
            }; //+
            var supposed7 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 9,
                RegularityNumber = 0,
                FirstPosition = 5,
                Positions = new byte[] {5, 6, 7, 12},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            }; //+
            var supposed8 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 9,
                RegularityNumber = 0,
                FirstPosition = 5,
                Positions = new byte[] {5, 9, 10, 12},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            }; //+
            var supposed9 = new RegularityDetectResult
            {
                Type = RegularityType.MirrorDigits,
                SequenceType = SequenceType.General,
                Length = 4,
                FirstNumber = 1,
                RegularityNumber = 0,
                FirstPosition = 0,
                Positions = new byte[] {0, 9, 10, 11},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            }; //+
            
            var regularity = new MirrorDigits();
            
            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count, 9);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.IsTrue(detected.Contains(supposed3));
            Assert.IsTrue(detected.Contains(supposed4));
            Assert.IsTrue(detected.Contains(supposed5));
            Assert.IsTrue(detected.Contains(supposed6));
            Assert.IsTrue(detected.Contains(supposed7));
            Assert.IsTrue(detected.Contains(supposed8));
            Assert.IsTrue(detected.Contains(supposed9));
            Assert.Pass();
        }
    }
}