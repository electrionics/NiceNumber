using System.Collections.Generic;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities.New
{
    public class SameNumbersAtAnyPosition:BaseRegularity<RegularityDetectResult>
    {
        public override RegularityType MainType => RegularityType.SameNumbers;
        
        protected override bool UseSubNumbers => true;
        
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResult> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number, byte[] lengths)
        {
            throw new System.NotImplementedException();
        }
    }
}