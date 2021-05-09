using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Helpers;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities
{
    public class GeometricProgression:BaseRegularity<RegularityDetectResult>
    {
        public GeometricProgression(byte minLength = 3):base(minLength)
        {
        }
        
        public override RegularityType MainType => RegularityType.GeometricProgression;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            return null;
        }

        protected override List<RegularityDetectResult> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            return null;
        }
        
        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            return null;
        }

        #region DetectAll(byte[] number, byte[] lengths)

        private const byte Accuracy = RegularityConstants.DoubleRegularityNumberAccuracy;
        
        protected override List<RegularityDetectResult> DetectAll(byte[] number, byte[] lengths)
        {
            var subNumbers = GetSubNumbers(number, lengths);
            var subNumberPositions = GetSubNumberPositions(lengths);

            if (lengths.Where((len, i) => subNumbers[i] < Math.Pow(10, len - 1)).Any())
            {
                return new List<RegularityDetectResult>();
            }
            
            var result = new List<RegularityDetectResult>();
            
            var qIndexes = new Dictionary<double, List<Tuple<byte, byte>>>(new DoubleEqualityComparer(Accuracy)); // q - list of position pairs
            
            #region InitializeWorkingCollections
            
            for (byte i = 0; i < subNumbers.Length - 1; i++)
            {
                for (var j = (byte)(i + 1); j < subNumbers.Length ; j++)
                {
                    if (subNumbers[i] == 0) continue; // eliminates divide by zero exception
                    
                    var q = (double)subNumbers[j] / subNumbers[i];
                    if (q.EqualTo(1, Accuracy)) continue; // no same numbers count as geometric progression

                    if (!qIndexes.ContainsKey(q))
                    {
                        qIndexes[q] = new List<Tuple<byte, byte>>();
                    }
                    
                    qIndexes[q].Add(new Tuple<byte, byte>(i, j));
                }
            }
            
            qIndexes = qIndexes
                .Where(x => 
                    x.Value.Count >= MinLength - 1 &&
                    (x.Key.RoundTo(0).EqualTo(x.Key, Accuracy) || 
                    (1 / x.Key).RoundTo(0).EqualTo(1 / x.Key, Accuracy)) // only integer q (multiplier or divider)
                    )
                .ToDictionary(
                    x => x.Key, 
                    x => x.Value
                        .OrderBy(y => y.Item1)
                        .ThenBy(y => y.Item2)
                        .ToList());
            
            #endregion
            
            #region Main Searching

            foreach (var qIndex in qIndexes)
            {
                var searchCollection = new List<Stack<byte>>();
                
                foreach (var tuple in qIndex.Value)
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
                        Type = RegularityType.GeometricProgression,
                        FirstNumber = subNumbers[indexes[0]],
                        FirstPosition = subNumberPositions[indexes[0]],
                        Length = indexes.Count,
                        RegularityNumber = ((double) subNumbers[indexes[1]] / subNumbers[indexes[0]]).RoundTo(Accuracy),
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
        
        protected override bool Include(RegularityDetectResult first, RegularityDetectResult second)
        {
            if (second.Positions.Length > first.Positions.Length)
            {
                return false;
            }
            
            var firstPairs = new List<Tuple<byte, byte>>();
            var secondPairs = new List<Tuple<byte, byte>>();
            for (var i = 0; i < first.Positions.Length; i++)
            {
                firstPairs.Add(new Tuple<byte, byte>(first.Positions[i], first.SubNumberLengths[i]));
                if (i < second.Positions.Length)
                {
                    secondPairs.Add(new Tuple<byte, byte>(second.Positions[i], second.SubNumberLengths[i]));
                }
            }
            
            var res = secondPairs.All(pair => firstPairs.Contains(pair));
            return res;
        }

        protected override IEqualityComparer<RegularityDetectResult> Comparer =>
            RegularityDetectResult.Comparer;
        
        protected override void SetTypes(RegularityDetectResult result)
        {
            base.SetTypes(result);

            if ((int)result.RegularityNumber.RoundTo(0) == 1)
            {
                result.Type = result.SubNumberLengths.All(x => x == 1) 
                    ? RegularityType.SameDigits
                    : RegularityType.SameNumbers;
            }
        }
    }
}