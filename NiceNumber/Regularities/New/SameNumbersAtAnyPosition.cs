using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class SameNumbersAtAnyPosition:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public override RegularityType Type => RegularityType.SameNumbersAtAnyPosition;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number, byte[] lengths)
        {
            throw new System.NotImplementedException();
        }
    }
}