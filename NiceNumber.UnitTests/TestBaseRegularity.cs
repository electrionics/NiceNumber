using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core;
using NiceNumber.Core.Regularities;
using NiceNumber.Core.Results;
using NiceNumber.Helpers;
using NUnit.Framework;

namespace NiceNumber.UnitTests
{
    public class TestBaseRegularity
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConvertNumberToDigitsRepresentation_CommonCase()
        {
            var testable = new BaseRegularityTestable(3);
            const long number = 1234567;
            var supposed = new byte[] {1, 2, 3, 4, 5, 6, 7};
            
            var result = BaseRegularityTestable.ConvertNumberToDigitsRepresentation(number);
            
            Assert.NotNull(result);
            Assert.AreEqual(supposed.Length, result.Length);
            Assert.IsTrue(supposed.SequenceEqual(result));
            
            Assert.Pass();
        }
        
        [Test]
        public void TestSplitNumberToDigitSequences_CommonCase()
        {
            var testable = new BaseRegularityTestable(2);
            const long number = 1234567;
            var supposed = new byte[][] {
                new byte[]{1,2,3,4,5,6,7}, 
                new byte[]{1,2,3,4,5,6}, new byte[]{2,3,4,5,6,7}, 
                new byte[]{1,2,3,4,5}, new byte[]{2,3,4,5,6}, new byte[]{3,4,5,6,7}, 
                new byte[]{1,2,3,4}, new byte[]{2,3,4,5}, new byte[]{3,4,5,6}, new byte[]{4,5,6,7}, 
                new byte[]{1,2,3}, new byte[]{2,3,4}, new byte[]{3,4,5}, new byte[]{4,5,6}, new byte[]{5,6,7}, 
                new byte[]{1,2}, new byte[]{2,3}, new byte[]{3,4}, new byte[]{4,5}, new byte[]{5,6}, new byte[]{6,7}, 
            };
            var supposedPositions = new byte[]
            {
                0,
                0, 1,
                0, 1, 2,
                0, 1, 2, 3,
                0, 1, 2, 3, 4,
                0, 1, 2, 3, 4, 5
            };
            
            var converted = BaseRegularityTestable.ConvertNumberToDigitsRepresentation(number);
            var result = testable.SplitNumberToDigitSequences(converted, out var firstPositions);
            var sortedResult = result
                .OrderByDescending(x => x.Length)
                .ThenBy(x => x[0])
                .ToArray();

            Assert.NotNull(result);
            Assert.NotNull(firstPositions);
            Assert.AreEqual(supposed.Length, result.Length);
            Assert.AreEqual(supposed.Length, firstPositions.Length);

            for (var i = 0; i < supposed.Length; i++)
            {
                Assert.IsTrue(result[i].SequenceEqual(supposed[i]));
            }
            
            Assert.IsTrue(firstPositions.SequenceEqual(supposedPositions));
            
            Assert.Pass();
        }

        [Test]
        public void TestGetAllSubNumberLengths_CommonCase()
        {
            const int len = 6;
            const byte minLen = 3;
            var regularity = new BaseRegularityTestable(minLen);

            var lens = regularity.GetAllSubNumberLengths(len);

            var lensMatch = new[]
            {
                new byte[] {1, 1, 1, 1, 1, 1},
                
                new byte[] {2, 1, 1, 1, 1},
                new byte[] {1, 2, 1, 1, 1},
                new byte[] {1, 1, 2, 1, 1},
                new byte[] {1, 1, 1, 2, 1},
                new byte[] {1, 1, 1, 1, 2},
                
                new byte[] {2, 2, 1, 1},
                new byte[] {2, 1, 2, 1},
                new byte[] {2, 1, 1, 2},
                new byte[] {1, 2, 2, 1},
                new byte[] {1, 2, 1, 2},
                new byte[] {1, 1, 2, 2},
                
                new byte[] {3, 1, 1, 1},
                new byte[] {1, 3, 1, 1},
                new byte[] {1, 1, 3, 1},
                new byte[] {1, 1, 1, 3},
                
                new byte[] {2, 2, 2},
                
                new byte[] {3, 1, 2},
                new byte[] {3, 2, 1},
                new byte[] {2, 3, 1},
                new byte[] {2, 1, 3},
                new byte[] {1, 3, 2},
                new byte[] {1, 2, 3},
                
                new byte[] {4, 1, 1},
                new byte[] {1, 4, 1},
                new byte[] {1, 1, 4}
            };
            var lensNotMatch = new[]
            {
                new byte[] { 3, 3 },
                new byte[] { 4, 2},
                new byte[] { 2, 4},
                new byte[] { 1, 5},
                new byte[] { 5, 1},
                new byte[] { 6 }
            };
            foreach (var item in lens)
            {
                Assert.AreEqual(item.Sum(x => x), len);
                Assert.GreaterOrEqual(item.Length, minLen);
            }
            foreach (var item in lensMatch)
            {
                var res = lens.Contains(item, ByteArrayEqualityComparer.Comparer);
                Assert.IsTrue(res);
            }
            foreach (var item in lensNotMatch)
            {
                var res = lens.Contains(item, ByteArrayEqualityComparer.Comparer);
                Assert.IsFalse(res);
            }
        }

        [Test]
        public void TestGetSubNumbers_CommonCase()
        {
            const byte minLen = 3;
            var regularity = new BaseRegularityTestable(minLen);

            var number = new byte[] {1, 2, 3, 4, 5, 6, 7, 8};
            var lens = new byte[] {1, 3, 1, 2, 1};

            var res = regularity.GetSubNumbers(number, lens);
            
            Assert.AreEqual(lens.Length, res.Length);
            
            Assert.AreEqual(1, res[0]);
            Assert.AreEqual(234, res[1]);
            Assert.AreEqual(5, res[2]);
            Assert.AreEqual(67, res[3]);
            Assert.AreEqual(8, res[4]);
        }

        [Test]
        public void TestGetSubNumberPositions_CommonCase()
        {
            const byte minLen = 3;
            var regularity = new BaseRegularityTestable(minLen);
            
            var lens = new byte[] {1, 3, 1, 2, 1};

            var expectedPoss = new byte[] {0, 1, 4, 5, 7};
            var poss = regularity.GetSubNumberPositions(lens);
            
            Assert.NotNull(poss);
            Assert.AreEqual(expectedPoss.Length, poss.Length);
            
            Assert.AreEqual(expectedPoss[0], poss[0]);
            Assert.AreEqual(expectedPoss[1], poss[1]);
            Assert.AreEqual(expectedPoss[2], poss[2]);
            Assert.AreEqual(expectedPoss[3], poss[3]);
            Assert.AreEqual(expectedPoss[4], poss[4]);
        }

        [Test]
        public void TestSetTypes_CommonCase()
        {
            var result1 = new RegularityDetectResult
            {
                Positions = new byte[] {1, 2, 3},
                SubNumberLengths = new byte[] {1, 1, 2}
            }; // sequential, gap = 0
            var result2 = new RegularityDetectResult
            {
                Positions = new byte[] {2, 4, 5},
                SubNumberLengths = new byte[] {2, 1, 1}
            }; // sequential, gap = 0
            var result3 = new RegularityDetectResult
            {
                Positions = new byte[] {1, 3, 5},
                SubNumberLengths = new byte[]{1, 1, 2}
            }; // fixed gap, gap = 1
            var result4 = new RegularityDetectResult
            {
                Positions = new byte[] {1, 5, 7},
                SubNumberLengths = new byte[]{ 2, 1, 3}
            }; // general, gap = 0
            
            var regularity = new BaseRegularityTestable(3);
            
            
            regularity.SetTypesForTest(result1);
            regularity.SetTypesForTest(result2);
            regularity.SetTypesForTest(result3);
            regularity.SetTypesForTest(result4);
            
            
            Assert.AreEqual(result1.SequenceType, SequenceType.Sequential);
            Assert.AreEqual(result2.SequenceType, SequenceType.Sequential);
            Assert.AreEqual(result3.SequenceType, SequenceType.FixedGap);
            Assert.AreEqual(result4.SequenceType, SequenceType.General);
            
            Assert.AreEqual(result1.Gap, 0);
            Assert.AreEqual(result2.Gap, 0);
            Assert.AreEqual(result3.Gap, 1);
            Assert.AreEqual(result4.Gap, 0);
            
            Assert.Pass();
        }
    }

    internal class BaseRegularityTestable:BaseRegularity<RegularityDetectResult>
    {
        #region Not for Test

        public BaseRegularityTestable(byte minLength) : base(minLength)
        {
        }
        
        public override RegularityType MainType => RegularityType.ArithmeticProgression;
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number, byte[] lengths)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> FilterOut(List<RegularityDetectResult> nonPrioritized)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        public new int[] GetSubNumbers(byte[] number, byte[] lengths)
        {
            return base.GetSubNumbers(number, lengths);
        }

        public new byte[] GetSubNumberPositions(byte[] lengths)
        {
            return base.GetSubNumberPositions(lengths);
        }

        public new byte[][] SplitNumberToDigitSequences(byte[] number, out byte[] firstPositions)
        {
            return base.SplitNumberToDigitSequences(number, out firstPositions);
        }

        public new static byte[] ConvertNumberToDigitsRepresentation(long number)
        {
            return BaseRegularity<RegularityDetectResult>.ConvertNumberToDigitsRepresentation(number);
        }

        public new byte[][] GetAllSubNumberLengths(int length)
        {
            return base.GetAllSubNumberLengths(length);
        }

        public void SetTypesForTest(RegularityDetectResult result)
        {
            base.SetTypes(result);
        }
    }
}