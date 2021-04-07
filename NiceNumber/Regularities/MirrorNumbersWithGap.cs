using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class MirrorNumbersWithGap:BaseRegularity<RegularityDetectResultWithGap>
    {
        public override RegularityType Type => RegularityType.MirrorNumbersWithGap;
        protected override List<RegularityDetectResultWithGap> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithGap> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }
    }
}