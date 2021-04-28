﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NiceNumber.Helpers;
using NiceNumber.Results;

namespace NiceNumber.Core.Regularities
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

        protected virtual bool UseSubNumbers => false;

        protected virtual byte[][] FilterLengths(byte[][] lengths)
        {
            return lengths;
        }
        
        /// <summary>
        /// алгоритм 1 разбить 7-значное число на 5 3-х значных, 4 4-х значных, 3 5-ти значных, и т.д. и проверить каждое на то, является ли оно закономерностью данного типа
        /// (проще и нагляднее реализация логики)
        /// </summary>
        /// <param name="number">digit representation of sub-number</param>
        /// <param name="firstPosition">start position of sub-number in source number</param>
        /// <returns>null, if no regularity of specified type</returns>
        protected abstract List<TResult> Detect(byte[] number, byte firstPosition = 0);

        protected abstract List<TResult> Detect(byte[] number, byte[] lengths, byte firstPosition);

        /// <summary>
        /// алгоритм 2: найти все закономерности данного типа в числе
        /// (оптимальнее по потребляемой памяти с скорости)
        /// </summary>
        /// <param name="number">digit representation of number</param>
        /// <returns>null if not supported, all regularities of specified type</returns>
        protected abstract List<TResult> DetectAll(byte[] number);

        protected abstract List<TResult> DetectAll(byte[] number, byte[] lengths);

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
            List<TResult> result;
            if (UseSubNumbers)
            {
                result = new List<TResult>();
                var resCollection = new ConcurrentBag<TResult>();
                
                // var sw = new Stopwatch();
                // sw.Start();
                var allLengths = GetAllSubNumberLengths(converted.Length);
                // sw.Stop();
                // Console.WriteLine($"{((double)sw.ElapsedMilliseconds) / 1000}");
                
                Parallel.ForEach(allLengths, new ParallelOptions { MaxDegreeOfParallelism = 24 }, lengths =>
                {
                    var subResult = DetectAll(converted, lengths);
                    if (subResult == null) // detect all not supported
                    {
                        var split = SplitNumberToDigitSequences(converted, out var firstPositions);

                        for (var i = 0; i < split.Length; i++)
                        {
                            var detected = Detect(split[i], lengths, firstPositions[i]);
                            if (detected != null)
                            {
                                foreach (var item in detected)
                                {
                                    resCollection.Add(item);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in subResult)
                        {
                            resCollection.Add(item);
                        }
                    }
                });
                
                result.AddRange(resCollection);
            }
            else
            {
                result = DetectAll(converted);
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

        protected int[] GetSubNumbers(byte[] number, byte[] lengths)
        {
            var result = new int[lengths.Length];
            var index = 0;
            for (var i = 0; i < lengths.Length; i++)
            {
                var length = lengths[i];
                for (var j = 0; j < length; j++)
                {
                    result[i] = result[i] * 10 + number[index];
                    index++;
                }
            }

            return result;
        }
        
        protected byte[][] GetAllSubNumberLengths(int len)
        {
            var start = new byte[len];
            for (var i = 0; i < len; i++)
            {
                start[i] = 1; // every sub-number initially is digit
            }
            
            var result = FillMergedVariations(start, MinLength);
            return result.Distinct().ToArray();
        }

        protected byte[] GetSubNumberPositions(byte[] lengths)
        {
            var result = new byte[lengths.Length];
            result[0] = 0;
            for (var i = 1; i < lengths.Length; i++)
            {
                result[i] = (byte)(result[i - 1] + lengths[i - 1]);
            }

            return result;
        }

        private static HashSet<byte[]> FillMergedVariations(byte[] lengths, byte minLength)
        {
            var result = new HashSet<byte[]>(ByteArrayEqualityComparer.Comparer);
            var mergedVariations = new HashSet<byte[]>(ByteArrayEqualityComparer.Comparer);

            result.Add(lengths);
            mergedVariations.Add(lengths);

            for (var len = lengths.Length; len > minLength; len--)
            {
                var variationsList = mergedVariations.ToList();
                mergedVariations.Clear();
                
                for (var i = 0; i < variationsList.Count; i++)
                {
                    for (var j = 1; j < len; j++)
                    {
                        var newLengths = new byte[len - 1];
                        newLengths[j - 1] = (byte)(variationsList[i][j - 1] + variationsList[i][j]);
                    
                        if (j - 1 > 0)
                        {
                            Array.Copy(variationsList[i], 0, newLengths, 0, j - 1);
                        }
                        if (len - j - 1 > 0)
                        {
                            Array.Copy(variationsList[i], j + 1, newLengths, j, len - j - 1);
                        }

                        mergedVariations.Add(newLengths);
                    }
                }

                foreach (var mergedVariation in mergedVariations)
                {
                    result.Add(mergedVariation);
                }
            }

            return result;
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