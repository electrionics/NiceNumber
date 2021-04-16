using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class ArithmeticProgressionAtAnyPosition:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public ArithmeticProgressionAtAnyPosition(byte minLength = 3):base(minLength)
        {
        }
        
        public override RegularityType Type => RegularityType.ArithmeticProgressionAtAnyPosition;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte firstPosition = 0)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number)
        {
            var result = new List<RegularityDetectResultWithPositions>();
            
            var startCounts = new Dictionary<byte, byte>(); // start number - count
            var dCounts = new Dictionary<sbyte, byte>(); // d - count
            
            var poss = new Dictionary<Tuple<sbyte, byte>, byte[][]>(); // d, startIndex - positions
            var lens = new Dictionary<Tuple<sbyte, byte>, byte[]>(); // d, startIndex - len
            var pIndexes = new Dictionary<Tuple<sbyte, byte>, byte>();  // d, startIndex - pIndex

            InitializeWorkingCollections(poss, lens, pIndexes, startCounts, number, ref dCounts, out var ds);

            for (byte currIndex = 0; currIndex <= number.Length - MinLength; currIndex++)
            {
                var currNumber = number[currIndex];
                if (startCounts[currNumber] > 0)
                {
                    startCounts[currNumber]--;

                    foreach (var currD in ds)
                    {
                        var dCount = dCounts[currD];
                        if (dCount == 0)
                        {
                            continue;
                        }
                        
                        var currKey = new Tuple<sbyte, byte>(currD, currIndex);

                        InitializeProgressions(poss, lens, currKey, dCount, currIndex);

                        SearchFromCurrent(poss, lens, pIndexes, dCounts, number, currKey, currIndex);

                        SetupFoundResults(poss, lens, number, result, currKey);
                    }
                }
            }

            return result;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number, byte[] lengths)
        {
            return null;
        }

        protected override bool Include(RegularityDetectResultWithPositions first, RegularityDetectResultWithPositions second)
        {
            return second.Positions.All(pos => first.Positions.Contains(pos));
        }

        protected override IEqualityComparer<RegularityDetectResultWithPositions> Comparer =>
            RegularityDetectResultWithPositions.Comparer;

        private void InitializeWorkingCollections(
            IDictionary<Tuple<sbyte, byte>, byte[][]> poss,
            IDictionary<Tuple<sbyte, byte>, byte[]> lens,
            IDictionary<Tuple<sbyte, byte>, byte> pIndexes,
            IDictionary<byte, byte> startCounts,
            byte[] number,
            ref Dictionary<sbyte, byte> dCounts,
            out sbyte[] ds)
        {
            for (var i = 0; i < number.Length - 1; i++)
            {
                if (i <= number.Length - MinLength)
                {
                    if (startCounts.ContainsKey(number[i]))
                    {
                        startCounts[number[i]]++;
                    }
                    else 
                    {
                        startCounts[number[i]] = 1;
                    }
                }
                
                for (var j = i + 1; j < number.Length ; j++)
                {
                    var d = (sbyte) (number[j] - number[i]);
                    if (dCounts.ContainsKey(d))
                    {
                        dCounts[d]++;
                    }
                    else if (i <= number.Length - MinLength && j <= number.Length - MinLength + 1)
                    {
                        dCounts[d] = 1;
                    }
                }
            }
            
            dCounts = dCounts
                .Where(x => x.Value >= MinLength - 1 && x.Key != 0)
                .ToDictionary(x => x.Key, x => x.Value);
            ds = dCounts.Keys.ToArray();
            
            for (byte startIndex = 0; startIndex <= number.Length - MinLength; startIndex++)
            {
                foreach (var d in dCounts.Keys)
                {
                    var key = new Tuple<sbyte, byte>(d, startIndex);
                    poss[key] = new byte[dCounts[d]][];
                    lens[key] = new byte[dCounts[d]];
                    pIndexes[key] = 0;
                }
            }
        }

        private static void InitializeProgressions(
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[][]> poss, 
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[]> lens,
            Tuple<sbyte, byte> currKey,
            int dCount,
            byte currIndex)
        {
            var pIndex = dCount - 1;
            while (pIndex >= 0)
            {
                if (poss[currKey][pIndex] == null)
                {
                    poss[currKey][pIndex] = new byte[dCount + 1];
                }
                poss[currKey][pIndex][0] = currIndex;
                            
                if (lens[currKey][pIndex] == 0)
                {
                    lens[currKey][pIndex] = 1;
                }

                pIndex--;
            }
        }

        private static void SearchFromCurrent(
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[][]> poss, 
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[]> lens, 
            IDictionary<Tuple<sbyte, byte>, byte> pIndexes,
            IDictionary<sbyte, byte> dCounts, 
            byte[] number, 
            Tuple<sbyte, byte> startKey,
            byte currIndex,
            byte innerLevel = 0)
        {
            var currD = startKey.Item1;
            var newInnerLevel = (byte) (innerLevel + 1);
            var pIndexStart = pIndexes[startKey];

            for (var i = currIndex + 1; i < number.Length; i++)
            {
                if (number[i] - number[currIndex] != currD)
                {
                    continue;
                }
                
                var newCurrIndex = (byte) i;
                var pIndex = pIndexes[startKey];
                
                if (pIndex > 0 && lens[startKey][pIndex] == 1)
                {
                    lens[startKey][pIndex] = newInnerLevel;
                    Array.Copy(poss[startKey][pIndex - 1], 0, poss[startKey][pIndex], 0, newInnerLevel);
                }
                
                poss[startKey][pIndex][lens[startKey][pIndex]] = newCurrIndex;
                lens[startKey][pIndex]++;

                if (dCounts[currD] > 0)
                {
                    //dCounts[currD]--; TODO: calculate initial count correctly, with multiplication of group counts between each other and multiply for count of groups
                    SearchFromCurrent(poss, lens, pIndexes, dCounts, number, startKey, newCurrIndex, newInnerLevel);
                    pIndexes[startKey]++;
                }
                else
                {
                    break;
                }
            }

            if (pIndexStart != pIndexes[startKey])
            {
                pIndexes[startKey]--;
            }
        }

        private void SetupFoundResults(
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[][]> poss, 
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[]> lens, 
            byte[] number, 
            ICollection<RegularityDetectResultWithPositions> result,
            Tuple<sbyte, byte> currKey)
        {
            var currD = currKey.Item1;
            var currIndex = currKey.Item2;
            
            for (var i = 0; i < lens[currKey].Length; i++)
            {
                var len = lens[currKey][i];
                if (len >= MinLength) // found
                {
                    var resItem = new RegularityDetectResultWithPositions
                    {
                        FirstNumber = number[currIndex],
                        FirstPosition = currIndex,
                        Length = len,
                        RegularityNumber = currD,
                        Positions = new byte[len]
                    };
                    Array.Copy(poss[currKey][i], 0, resItem.Positions, 0, len);
                        
                    result.Add(resItem);
                }
            }
        }
    }
}