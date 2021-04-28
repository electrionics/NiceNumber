using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Core.Regularities.Deprecated
{
    public class ArithmeticProgressionWithFixedGap:BaseRegularity<RegularityDetectResultWithGap>
    {
        public ArithmeticProgressionWithFixedGap(byte minLength = 3) : base(minLength)
        {
        }
        
        public override RegularityType Type => RegularityType.ArithmeticProgressionWithFixedGap;
        
        protected override bool UseSubNumbers => false;
        
        protected override List<RegularityDetectResultWithGap> Detect(byte[] number, byte firstPosition = 0)
        {
            var start = number[0];

            var res = new List<RegularityDetectResultWithGap>();
            
            for (byte gap = 1; gap < number.Length - 1; gap++) // TODO: take into account MinLength of regularity
            {
                var found = true;
                var d = number[gap + 1] - number[0];
                
                var j = gap + 1;
                for (; j < number.Length; j += gap + 1)
                {
                    if (number[j] - number[j - gap - 1] != d)
                    {
                        found = false;
                        break;
                    }
                }
                
                if (found)
                {
                    var len = j / (gap + 1);

                    if (len >= MinLength && d > 0)
                    {
                        res.Add(new RegularityDetectResultWithGap
                        {
                            FirstNumber = start,
                            FirstPosition = firstPosition,
                            Length = len,
                            RegularityNumber = d,
                            Gap = gap
                        });
                    }
                }
            }

            if (res.Count == 0)
            {
                return null;
            }

            return res;
        }

        protected override List<RegularityDetectResultWithGap> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithGap> DetectAll(byte[] number)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithGap> DetectAll(byte[] number, byte[] lengths)
        {
            return null;
        }

        protected override bool Include(RegularityDetectResultWithGap first, RegularityDetectResultWithGap second)
        {
            return second.RegularityNumber % first.RegularityNumber == 0 &&
                   first.FirstPosition <= second.FirstPosition &&
                   (second.Gap + 1) % (first.Gap + 1) == 0 &&
                   first.FirstPosition + (first.Gap + 1) * first.Length >= second.FirstPosition + (second.Gap + 1) * second.Length;
        }

        protected override IEqualityComparer<RegularityDetectResultWithGap> Comparer => RegularityDetectResultWithGap.Comparer;
    }
}