﻿using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class MirrorNumbersSequential:BaseRegularity<RegularityDetectResult>
    {
        public override RegularityType Type => RegularityType.MirrorNumbersSequential;
        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override List<RegularityDetectResult> DetectAll(byte[] number)
        {
            throw new System.NotImplementedException();
        }
    }
}