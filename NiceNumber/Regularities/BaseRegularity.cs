using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Helpers;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public abstract class BaseRegularity<TResult>:IRegularity
        where TResult: RegularityDetectResult
    {
        #region Base Members
        
        public abstract RegularityType Type { get; }

        protected BaseRegularity()
        {
            MinLength = 3;
        }

        protected BaseRegularity(byte minLength)
        {
            MinLength = minLength;
        }

        protected byte MinLength { get; }
        
        /// <summary>
        /// алгоритм 1 разбить 7-значное число на 5 3-х значных, 4 4-х значных, 3 5-ти значных, и т.д. и проверить каждое на то, является ли оно закономерностью данного типа
        /// (проще и нагляднее реализация логики)
        /// </summary>
        /// <param name="number">digit representation of sub-number</param>
        /// <param name="firstPosition">start position of sub-number in source number</param>
        /// <returns>null, if no regularity of specified type</returns>
        protected abstract List<TResult> Detect(byte[] number, byte firstPosition = 0);

        /// <summary>
        /// алгоритм 2: найти все закономерности данного типа в числе
        /// (оптимальнее по потребляемой памяти с скорости)
        /// </summary>
        /// <param name="number">digit representation of number</param>
        /// <returns>null if not supported, all regularities of specified type</returns>
        protected abstract List<TResult> DetectAll(byte[] number);

        /// <summary>
        /// filter out result of detecting all regularities from regularities which included in other or equals, so only "largest" regularities left
        /// </summary>
        /// <param name="nonPrioritized">not filtered regularities</param>
        /// <returns></returns>
        protected virtual List<TResult> FilterOut(List<TResult> nonPrioritized)
        {
            var result = nonPrioritized
                .OrderByDescending(x => x.Length)
                .ToList();

            foreach (var detectResult in nonPrioritized)
            {
                result.RemoveAll(x => Include(detectResult, x) && !detectResult.Equals(x));
            }

            return result
                .Distinct(Comparer)
                .ToList();
        }

        protected virtual bool Include(TResult first, TResult second)
        {
            return false;
        }

        protected virtual IEqualityComparer<TResult> Comparer => RegularityDetectResult.Comparer;

        #endregion
        
        public List<RegularityDetectResult> Process(long number)
        {
            var converted = ConvertNumberToDigitsRepresentation(number);
            
            var result = DetectAll(converted);
            if (result == null) // detect all not supported
            {
                var split = SplitNumberToDigitSequences(converted, out var firstPositions);
                result = new List<TResult>();
                
                for (var i = 0; i < split.Length; i++)
                {
                    var detected = Detect(split[i], firstPositions[i]);
                    if (detected != null)
                    {
                        result.AddRange(detected);
                    }
                }
            }
            
            foreach (var detected in result)
            {
                detected.Type = Type;
            }

            return FilterOut(result).Cast<RegularityDetectResult>().ToList();
        }

        #region Helper Methods
        
        protected byte[][] SplitNumberToDigitSequences(byte[] number, out byte[] firstPositions)
        {
            var n = number.Length - MinLength + 1;
            const int a1 = 1;
            var an = n;
            var resSize = n * (a1 + an) / 2;
            var res = new byte[resSize][];
            var firstPositionsRes = new byte[resSize];
            
            var index = 0;
            for (var len = number.Length; len >= MinLength; len--)
            {
                for (var pos = 0; pos < number.Length - len + 1; pos++)
                {
                    res[index] = new byte[len];
                    Array.Copy(number, pos, res[index], 0, len);
                    firstPositionsRes[index] = (byte)pos;
                    index++;
                }
            }

            firstPositions = firstPositionsRes;
            return res;
        }

        protected static byte[] ConvertNumberToDigitsRepresentation(long number)
        {
            var res = new List<byte>();
            do
            {
                res.Add((byte) (number % 10));
                number /= 10;
            } while (number > 0); 

            res.Reverse();
            return res.ToArray();
        }
        
        #region Merge Digits To Numbers
        
        protected byte[][] GetAllSubNumberLengths(int len)
        {
            var start = new byte[len];
            for (var i = 0; i < len; i++)
            {
                start[i] = 1; // every sub-number initially is digit
            }
            
            var result = new HashSet<byte[]>(ByteArrayEqualityComparer.Comparer);
            FillMergedVariations(start, MinLength, result);
            return result.Distinct().ToArray();
        }

        private static void FillMergedVariations(byte[] lengths, byte minLength, HashSet<byte[]> result)
        {
            var len = lengths.Length;
            var mergedVariations = new List<byte[]>();
            
            result.Add(lengths);

            if (len <= minLength)
            {
                return;
            }
            
            for (var i = 1; i < len; i++)
            {
                var newLengths = new byte[len - 1];
                newLengths[i - 1] = (byte)(lengths[i - 1] + lengths[i]);
                
                if (i - 1 > 0)
                {
                    Array.Copy(lengths, 0, newLengths, 0, i - 1);
                }
                if (len - i - 1 > 0)
                {
                    Array.Copy(lengths, i + 1, newLengths, i, len - i - 1);
                }

                mergedVariations.Add(newLengths);
            }

            foreach (var mergedVariation in mergedVariations)
            {
                FillMergedVariations(mergedVariation, minLength, result);
            }
        }
        
        #endregion

        #endregion
    }

    public interface IRegularity
    {
        RegularityType Type { get; }
        List<RegularityDetectResult> Process(long number);
    }
}