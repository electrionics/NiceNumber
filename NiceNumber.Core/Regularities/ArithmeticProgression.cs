using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities
{
    public class ArithmeticProgression:BaseRegularity<RegularityDetectResult>
    {
        public ArithmeticProgression(byte minLength = 3):base(minLength)
        {
        }
        
        public override RegularityType MainType => RegularityType.ArithmeticProgression;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            return null;
        }

        protected override List<RegularityDetectResult> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            return null;
        }

        #region DetectAll(byte[] number)

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            var result = new List<RegularityDetectResult>();
            
            var startCounts = new Dictionary<byte, byte>(); // start number - count
            var dCounts = new Dictionary<sbyte, byte>(); // d - count
            
            var poss = new Dictionary<Tuple<sbyte, byte>, List<byte>[]>(); // d, startIndex - positions
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

        private void InitializeWorkingCollections(
            IDictionary<Tuple<sbyte, byte>, List<byte>[]> poss,
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
                    poss[key] = new List<byte>[dCounts[d]];
                    lens[key] = new byte[dCounts[d]];
                    pIndexes[key] = 0;
                }
            }
        }

        private static void InitializeProgressions(
            IReadOnlyDictionary<Tuple<sbyte, byte>, List<byte>[]> poss, 
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
                    poss[currKey][pIndex] = new List<byte>();
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
            IReadOnlyDictionary<Tuple<sbyte, byte>, List<byte>[]> poss, 
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
                    //Array.Copy(poss[startKey][pIndex - 1], 0, poss[startKey][pIndex], 0, newInnerLevel);
                    poss[startKey][pIndex] = poss[startKey][pIndex - 1].ToList();
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
            IReadOnlyDictionary<Tuple<sbyte, byte>, List<byte>[]> poss, 
            IReadOnlyDictionary<Tuple<sbyte, byte>, byte[]> lens, 
            byte[] number, 
            ICollection<RegularityDetectResult> result,
            Tuple<sbyte, byte> currKey)
        {
            var currD = currKey.Item1;
            var currIndex = currKey.Item2;
            
            for (var i = 0; i < lens[currKey].Length; i++)
            {
                var len = lens[currKey][i];
                if (len >= MinLength) // found
                {
                    var resItem = new RegularityDetectResult
                    {
                        FirstNumber = number[currIndex],
                        FirstPosition = currIndex,
                        Length = len,
                        RegularityNumber = currD,
                        Positions = new byte[len]
                    };
                    //Array.Copy(poss[currKey][i], 0, resItem.Positions, 0, len);
                    resItem.Positions = poss[currKey][i].ToArray();
                    
                    result.Add(resItem);
                }
            }
        }

        #endregion

        #region DetectAll(byte[] number, byte[] lengths)
        
        protected override List<RegularityDetectResult> DetectAll(byte[] number, byte[] lengths)
        {
            var subNumbers = GetSubNumbers(number, lengths);
            var subNumberPositions = GetSubNumberPositions(lengths);

            if (lengths.Where((len, i) => len > 1 && subNumbers[i] < Math.Pow(10, len - 1)).Any())
            {
                return new List<RegularityDetectResult>();
            }
            
            var result = new List<RegularityDetectResult>();
            
            var dIndexes = new Dictionary<int, List<Tuple<byte, byte>>>(); // d - list of position pairs
            
            #region InitializeWorkingCollections
            
            for (byte i = 0; i < subNumbers.Length - 1; i++)
            {
                for (var j = (byte)(i + 1); j < subNumbers.Length ; j++)
                {
                    var d = subNumbers[j] - subNumbers[i];
                    if (d == 0) continue; // no same numbers count as arithmetic progression

                    if (!dIndexes.ContainsKey(d)) // && i <= subNumbers.Length - MinLength && j <= subNumbers.Length - MinLength + 1
                    {
                        dIndexes[d] = new List<Tuple<byte, byte>>();
                    }
                    
                    dIndexes[d].Add(new Tuple<byte, byte>(i, j));
                }
            }
            
            dIndexes = dIndexes
                .Where(x => x.Value.Count >= MinLength - 1)
                .ToDictionary(
                    x => x.Key, 
                    x => x.Value
                        .OrderBy(y => y.Item1)
                        .ThenBy(y => y.Item2)
                        .ToList());
            
            #endregion
            
            #region Main Searching

            foreach (var dIndex in dIndexes)
            {
                var searchCollection = new List<Stack<byte>>();
                
                foreach (var tuple in dIndex.Value)
                {
                    var concatenated = false;

                    var i = 0;
                    var newStacks = new List<Stack<byte>>();
                    while (searchCollection.Count > i)
                    {
                        var stack = searchCollection[i];
                        if (stack.Peek() == tuple.Item1 && !newStacks.Contains(stack))
                        {
                            var newStack = new Stack<byte>(new Stack<byte>(stack));
                            newStacks.Add(newStack);
                            searchCollection.Add(newStack);
                            
                            stack.Push(tuple.Item2);
                            concatenated = true;
                        }
                        
                        i++;
                    }

                    if (!concatenated)
                    {
                        var newStack = new Stack<byte>();
                        newStack.Push(tuple.Item1);
                        newStack.Push(tuple.Item2);
                        searchCollection.Add(newStack);
                    }
                }

                foreach (var stack in searchCollection)
                {
                    if (stack.Count < MinLength) continue;
                    
                    var indexes = stack
                        .OrderBy(x => x)
                        .ToList();
                    
                    var res = new RegularityDetectResult
                    {
                        FirstNumber = subNumbers[indexes[0]],
                        FirstPosition = subNumberPositions[indexes[0]],
                        Length = indexes.Count,
                        RegularityNumber = subNumbers[indexes[1]] - subNumbers[indexes[0]],
                        Positions = indexes.Select(x => subNumberPositions[x]).ToArray(),
                        SubNumberLengths = indexes.Select(x => lengths[x]).ToArray()
                    };
                    
                    result.Add(res);
                }
            }
            
            #endregion

            return result;
        }

        #endregion

        protected override IEqualityComparer<RegularityDetectResult> Comparer =>
            RegularityDetectResult.Comparer;

        protected override void SetTypes(RegularityDetectResult result)
        {
            base.SetTypes(result);

            if ((int) result.RegularityNumber == 0)
            {
                result.Type = result.SubNumberLengths.All(x => x == 1) 
                    ? RegularityType.SameDigits
                    : RegularityType.SameNumbers;
            }
        }
    }
}