using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Helpers;
using NiceNumber.Regularities;
using NiceNumber.Results;
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
    }

    internal class BaseRegularityTestable:BaseRegularity<RegularityDetectResult>
    {
        #region Not for Test

        public BaseRegularityTestable(byte minLength) : base(minLength)
        {
        }
        
        public override RegularityType Type => throw new NotImplementedException();
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            throw new NotImplementedException();
        }

        protected override List<RegularityDetectResult> FilterOut(List<RegularityDetectResult> nonPrioritized)
        {
            throw new NotImplementedException();
        }
        
        #endregion

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
    }
}