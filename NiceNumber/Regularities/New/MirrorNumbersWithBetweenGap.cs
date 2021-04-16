using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class MirrorNumbersWithBetweenGap:BaseRegularity<RegularityDetectResultWithGap>
    {
        public override RegularityType Type => RegularityType.MirrorNumbersWithBetweenGap;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResultWithGap> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithGap> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithGap> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithGap> DetectAll(byte[] number, byte[] lengths)
        {
            throw new System.NotImplementedException();
        }
    }
}