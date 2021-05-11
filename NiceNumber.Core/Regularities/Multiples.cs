using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities
{
    public class Multiples:BaseRegularity<RegularityDetectResult>
    {
        public override RegularityType MainType => RegularityType.MultiplesNumbers;
        
        protected override bool UseSubNumbers => true;
        
        public Multiples():base(2)
        {
        }
        
        public Multiples(byte minLength):base(minLength)
        {
        }
        
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

            if (lengths.Where((len, i) => len > 1 && subNumbers[i] < Math.Pow(10, len - 1)).Any()) // not count sub-numbers with leading zero, because sub-numbers without leading zero give same finaL result
            {
                return new List<RegularityDetectResult>();
            }
            
            var result = new List<RegularityDetectResult>();

            for (var i = 0; i < subNumbers.Length; i++)
            {
                if (subNumbers[i] < 2) continue;
                
                for (var j = 0; j < subNumbers.Length; j++)
                {
                    if (i == j || subNumbers[j] < 2 || subNumbers[i] == subNumbers[j]) continue; // not count same numbers as multiples

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

                    if (subNumbers[maxNumberIndex] % subNumbers[minNumberIndex] == 0 && lengths[minNumberIndex] >= MinLength && lengths[maxNumberIndex] >= MinLength)
                    {
                        var firstIndex = Math.Min(minNumberIndex, maxNumberIndex);
                        var lastIndex = Math.Max(minNumberIndex, maxNumberIndex);
                        
                        result.Add(new RegularityDetectResult
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

        protected override bool Include(RegularityDetectResult first, RegularityDetectResult second)
        {
            return false;
        }

        protected override IEqualityComparer<RegularityDetectResult> Comparer =>
            RegularityDetectResult.Comparer;
    }
}