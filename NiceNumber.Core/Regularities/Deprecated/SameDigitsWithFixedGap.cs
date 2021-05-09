﻿using System.Collections.Generic;
using NiceNumber.Core.Results;

namespace NiceNumber.Core.Regularities.Deprecated
{
    public class SameDigitsWithFixedGap:BaseRegularity<RegularityDetectResult>
    {
        public SameDigitsWithFixedGap(byte minLength = 3):base(minLength)
        {
        }
        
        public override RegularityType MainType => RegularityType.SameDigits;

        protected override List<RegularityDetectResult> Detect(byte[] number, byte firstPosition = 0)
        {
            var start = number[0];
            
            var len = 0;
            byte gap = 1;
            
            for (; gap < number.Length - 1; gap++) // TODO: take into account MinLength of regularity
            {
                var found = true;
                
                var j = gap + 1;
                for (; j < number.Length; j += gap + 1)
                {
                    if (start != number[j])
                    {
                        found = false;
                        break;
                    }
                }
                
                if (found)
                {
                    len = j / (gap + 1);
                    break;
                }
            }

            if (len == 0 || len < MinLength)
                return null;

            return new List<RegularityDetectResult>
            {
                new RegularityDetectResult
                {
                    FirstNumber = start,
                    FirstPosition = firstPosition,
                    Length = len,
                    RegularityNumber = gap,
                    Gap = gap
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

        protected override IEqualityComparer<RegularityDetectResult> Comparer =>
            RegularityDetectResult.Comparer;

        protected override bool Include(RegularityDetectResult first, RegularityDetectResult second)
        {
            return first.FirstNumber == second.FirstNumber &&
                   first.FirstPosition <= second.FirstPosition &&
                   (second.Gap + 1) % (first.Gap + 1) == 0 &&
                   first.FirstPosition + (first.Gap + 1) * first.Length >= second.FirstPosition + (second.Gap + 1) * second.Length;
        }
    }
}