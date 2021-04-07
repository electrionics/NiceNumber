using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class ArithmeticProgressionSequential:BaseRegularity<RegularityDetectResult>
    {
        public ArithmeticProgressionSequential(byte minLength = 3) : base(minLength)
        {
        }
        
        public override RegularityType Type => RegularityType.AriphmeticProgressionSequential;
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            var start = number[0];
            var d = number[1] - number[0];
            for (var i = 1; i < number.Length - 1; i++)
            {
                if (number[i + 1] - number[i] != d)
                {
                    return null;
                }
            }

            return new List<RegularityDetectResult>{
                new RegularityDetectResult
                {
                    Type = RegularityType.AriphmeticProgressionSequential,
                    FirstNumber = start,
                    FirstPosition = firstPosition,
                    Length = number.Length,
                    RegularityNumber = d
                }
            };
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            return null;
        }

        protected override bool Include(RegularityDetectResult first, RegularityDetectResult second)
        {
            return first.RegularityNumber == second.RegularityNumber &&
                   first.FirstPosition <= second.FirstPosition &&
                   first.FirstPosition + first.Length >= second.FirstPosition + second.Length;
        }
    }
}