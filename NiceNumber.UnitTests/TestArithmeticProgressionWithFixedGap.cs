using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Regularities.Deprecated;
using NiceNumber.Core.Results;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestArithmeticProgressionWithFixedGap
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void Test_ArithmeticProgressionWithFixedGap()
        {
            const long number = 23256739;
            var regularity = new ArithmeticProgression();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.FixedGap,
                Length = 4,
                FirstNumber = 3,
                FirstPosition = 1,
                RegularityNumber = 2,
                Gap = 1,
                Positions = new byte[] {1, 3, 5, 7},
                SubNumberLengths = new byte[] {1, 1, 1, 1}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.FixedGap,
                Length = 3,
                FirstNumber = 3,
                FirstPosition = 1,
                RegularityNumber = 3,
                Gap = 2,
                Positions = new byte[] { 1, 4, 7},
                SubNumberLengths = new byte[] {1, 1, 1}
            };

            var detected = regularity.Process(number);
            
            Assert.NotNull(detected);
            Assert.AreEqual(detected.Count(x => x.SequenceType == SequenceType.FixedGap), 2);
            Assert.IsTrue(detected.Contains(supposed1));
            Assert.IsTrue(detected.Contains(supposed2));
            Assert.Pass();
        }

        [Test]
        public void Test_IncludeFiltering()
        {
            const long number = 12131415161;
            var regularity = new ArithmeticProgression();
            var supposed1 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 5, // 12 13 14 15 16
                FirstNumber = 12,
                FirstPosition = 0,
                RegularityNumber = 1,
                Gap = 0,
                Positions = new byte[]{0, 2, 4, 6, 8},
                SubNumberLengths = new byte[]{ 2, 2, 2, 2, 2}
            };
            var supposed2 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.Sequential,
                Length = 5,
                FirstNumber = 21,
                FirstPosition = 1,
                RegularityNumber = 10,
                Gap = 0,
                Positions = new byte[] {1, 3, 5, 7, 9},
                SubNumberLengths = new byte[] {2, 2, 2, 2, 2}
            };
            var supposed3 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.FixedGap,
                Length = 3,
                FirstNumber = 121,
                FirstPosition = 0,
                RegularityNumber = 20,
                Gap = 1,
                Positions = new byte[] {0, 4, 8},
                SubNumberLengths = new byte[] {3, 3, 3}
            };
            var supposed4 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 0,
                RegularityNumber = 30,
                Gap = 0,
                Positions = new byte[] {0, 3, 9},
                SubNumberLengths = new byte[] {1, 2, 2}
            };
            var supposed5 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 2,
                RegularityNumber = 30,
                Gap = 0,
                Positions = new byte[] {2, 3, 9},
                SubNumberLengths = new byte[] {1, 2, 2}
            };
            var supposed6 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 0,
                RegularityNumber = 2,
                Gap = 0,
                Positions = new byte[] {0, 3, 7},
                SubNumberLengths = new byte[] {1, 1, 1}
            };
            var supposed7 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 1,
                FirstPosition = 2,
                RegularityNumber = 2,
                Gap = 0,
                Positions = new byte[] {2, 3, 7},
                SubNumberLengths = new byte[] {1, 1, 1}
            };
            var supposed8 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.General,
                Length = 3,
                FirstNumber = 31,
                FirstPosition = 3,
                RegularityNumber = -15,
                Gap = 0,
                Positions = new byte[] {3, 8, 10},
                SubNumberLengths = new byte[] {2, 2, 1} 
            };
            var supposed9 = new RegularityDetectResult
            {
                Type = RegularityType.ArithmeticProgression,
                SequenceType = SequenceType.FixedGap,
                Length = 3,
                FirstNumber = 12,
                FirstPosition = 0,
                RegularityNumber = 2,
                Gap = 2,
                Positions = new byte[] {0, 4, 8},
                SubNumberLengths = new byte[] {2, 2, 2}
            };
            
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