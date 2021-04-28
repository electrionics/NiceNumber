using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Core.Regularities.New
{
    public class MirrorDigitsWithFixedGap:BaseRegularity<RegularityDetectResultWithGap>
    {
        public override RegularityType Type => RegularityType.MirrorDigitsWithFixedGap;
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