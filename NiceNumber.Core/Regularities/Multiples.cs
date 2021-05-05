using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities
{
    public class Multiples:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public override RegularityType Type => RegularityType.MultiplesNumbers;
        
        protected override bool UseSubNumbers => true;
        
        public Multiples():base(2)
        {
        }
        
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
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number, byte[] lengths)
        {
            var subNumbers = GetSubNumbers(number, lengths);
            var subNumberPositions = GetSubNumberPositions(lengths);

            if (lengths.Where((len, i) => len > 1 && subNumbers[i] < Math.Pow(10, len - 1)).Any())
            {
                return new List<RegularityDetectResultWithPositions>();
            }
            
            var result = new List<RegularityDetectResultWithPositions>();

            for (var i = 0; i < subNumbers.Length; i++)
            {
                if (subNumbers[i] < 2) continue;
                
                for (var j = 0; j < subNumbers.Length; j++)
                {
                    if (i == j || subNumbers[j] < 2) continue;

                    var minNumberIndex = i;
                    var maxNumberIndex = j;
                    if (subNumbers[i] > subNumbers[j])
                    {
                        minNumberIndex = j;
                        maxNumberIndex = i;
                    }

                    if (subNumbers[minNumberIndex] == 2 && lengths[maxNumberIndex] > 1 && subNumbers[maxNumberIndex] % 10 != 0 ||
                        subNumbers[minNumberIndex] == 4 && lengths[maxNumberIndex] > 2 && subNumbers[maxNumberIndex] % 100 != 0 ||
                        subNumbers[minNumberIndex] == 5 && lengths[maxNumberIndex] > 2 && subNumbers[maxNumberIndex] % 100 != 0 ||
                        subNumbers[minNumberIndex] == 10 && lengths[maxNumberIndex] > 2 && subNumbers[maxNumberIndex] % 100 != 0 ||
                        subNumbers[minNumberIndex] == 20 && lengths[maxNumberIndex] > 2 && subNumbers[maxNumberIndex] % 100 != 0 ||
                        subNumbers[minNumberIndex] == 25 && lengths[maxNumberIndex] > 2 && subNumbers[maxNumberIndex] % 100 != 0)
                    {
                        continue;
                    }

                    if (subNumbers[maxNumberIndex] % subNumbers[minNumberIndex] == 0)
                    {
                        var firstIndex = Math.Min(minNumberIndex, maxNumberIndex);
                        var lastIndex = Math.Max(minNumberIndex, maxNumberIndex);
                        
                        result.Add(new RegularityDetectResultWithPositions
                        {
                            FirstNumber = subNumbers[firstIndex],
                            FirstPosition = subNumberPositions[firstIndex],
                            Length = 2,
                            // ReSharper disable once PossibleLossOfFraction
                            RegularityNumber = subNumbers[maxNumberIndex] / subNumbers[minNumberIndex],
                            Positions = new []{ subNumberPositions[firstIndex], subNumberPositions[lastIndex]},
                            SubNumberLengths = new []{ lengths[firstIndex], lengths[lastIndex]}
                        });
                    }
                }
            }

            return result;
        }
        
        protected override IEqualityComparer<RegularityDetectResultWithPositions> Comparer =>
            RegularityDetectResultWithPositions.Comparer;
    }
}