﻿using System.Collections.Generic;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class MirrorDigitsWithFixedGap:BaseRegularity<RegularityDetectResultWithGap>
    {
        public override RegularityType Type => RegularityType.MirrorDigitsWithFixedGap;
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