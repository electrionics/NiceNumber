using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities
{
    public class SameNumbers:BaseRegularity<RegularityDetectResult>
    {
        public SameNumbers() : base(2)
        {
        }
        
        public SameNumbers(byte minLength) : base(minLength)
        {
        }
        
        public override RegularityType MainType => RegularityType.SameNumbers;

        public override RegularityType[] PossibleTypes => new[] {RegularityType.SameDigits, RegularityType.SameNumbers};

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

        protected override List<RegularityDetectResult> DetectAll(byte[] number, byte[] lengths)
        {
            var subNumbers = GetSubNumbers(number, lengths);
            var subNumberPositions = GetSubNumberPositions(lengths);
            
            if (lengths.Where((len, i) => len > 1 && subNumbers[i] < Math.Pow(10, len - 1)).Any())
            {
                return new List<RegularityDetectResult>();
            }

            var lengthsNotCount = lengths
                .GroupBy(x => x)
                .Where(x => x.Count() < 2)
                .Select(x => x.Key)
                .ToHashSet();

            var numberIndexes = new Dictionary<int, List<byte>>();
            
            for (byte i = 0; i < subNumbers.Length - MinLength + 1; i++)
            {
                if (lengthsNotCount.Contains(lengths[i])) continue;
                var currNumber = subNumbers[i];
                
                for (var j = (byte)(i + 1); j < subNumbers.Length - MinLength + 2; j++)
                {
                    if (currNumber == subNumbers[j])
                    {
                        if (!numberIndexes.ContainsKey(currNumber))
                        {
                            numberIndexes[currNumber] = new List<byte>{ i };
                        }
                    
                        numberIndexes[currNumber].Add(j);
                    }
                }
            }

            numberIndexes = numberIndexes
                .Where(x => x.Value.Count >= MinLength)
                .ToDictionary(
                    x => x.Key, 
                    x => x.Value
                        .Distinct()
                        .OrderBy(y => y)
                        .ToList());

            var result = new List<RegularityDetectResult>();
            
            foreach (var indexesKey in numberIndexes.Keys)
            {
                var indexes = numberIndexes[indexesKey];
                
                result.Add(new RegularityDetectResult
                {
                    FirstNumber = subNumbers[indexes[0]],
                    FirstPosition = subNumberPositions[indexes[0]],
                    Length = indexes.Count,
                    RegularityNumber = 0,
                    Positions = indexes.Select(index => subNumberPositions[index]).ToArray(),
                    SubNumberLengths = indexes.Select(index => lengths[index]).ToArray()
                });
            }

            return result;
        }

        protected override IEqualityComparer<RegularityDetectResult> Comparer => RegularityDetectResult.Comparer;

        protected override void SetTypes(RegularityDetectResult result)
        {
            base.SetTypes(result);

            if (result.SubNumberLengths.All(x => x == 1))
            {
                result.Type = RegularityType.SameDigits;
            }
        }
    }
}