using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class MirrorDigitsAtAnyPosition:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public override RegularityType Type => RegularityType.MirrorDigitsAtAnyPosition;
        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }
    }
}