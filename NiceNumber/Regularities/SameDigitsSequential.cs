using System.Collections.Generic;
using System.Linq;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class SameDigitsSequential:BaseRegularity<RegularityDetectResult>
    {
        public SameDigitsSequential(byte minLength = 2):base(minLength)
        {
        }
        
        public override RegularityType Type => RegularityType.SameDigitsSequential;

        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            var start = number[0];
            for (var i = 1; i < number.Length; i++)
            {
                if (start != number[i])
                    return null;
            }

            return new List<RegularityDetectResult>
            {
                new RegularityDetectResult
                {
                    Type = RegularityType.SameDigitsSequential,
                    FirstNumber = start,
                    FirstPosition = firstPosition,
                    Length = number.Length,
                    RegularityNumber = 0
                }
            };

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
            return null;
        }

        protected override bool Include(RegularityDetectResult first, RegularityDetectResult second)
        {
            return first.FirstNumber == second.FirstNumber &&
                   first.FirstPosition <= second.FirstPosition &&
                   first.FirstPosition + first.Length >= second.FirstPosition + second.Length;
        }
    }
}