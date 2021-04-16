using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Results;

namespace NiceNumber.Regularities
{
    public class SameDigitsAtAnyPosition:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public SameDigitsAtAnyPosition(byte minLength = 2):base(minLength)
        {
        }
        
        public override RegularityType Type => RegularityType.SameDigitsAtAnyPosition;

        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte firstPosition = 0)
        {
            return null;
        }
        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte[] lengths, byte firstPosition = 0)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number)
        {
            var result = new List<RegularityDetectResultWithPositions>();
            
            var starts = number.ToHashSet(); // TODO: taking into account MinLength

            for (var startIndex = 0; startIndex <= number.Length - MinLength; startIndex++)
            {
                if (!starts.Any())
                {
                    break;
                }
                
                if (starts.Contains(number[startIndex]))
                {
                    var len = 1;
                    var positions = new byte[number.Length];
                    positions[0] = (byte)startIndex;
                    
                    starts.Remove(number[startIndex]);
                    
                    for (var i = startIndex + 1; i < number.Length; i++)
                    {
                        if (number[startIndex] == number[i])
                        {
                            positions[len] = (byte)i;
                            len++;
                        }
                    }

                    if (len >= MinLength) // found
                    {
                        var resItem = new RegularityDetectResultWithPositions
                        {
                            FirstNumber = number[startIndex],
                            FirstPosition = startIndex,
                            Length = len,
                            RegularityNumber = 0,
                            Positions = new byte[len]// TODO: check if work with 0
                        };
                        Array.Copy(positions, 0, resItem.Positions, 0, len);
                        
                        result.Add(resItem);
                    }
                }
            }

            return result;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number, byte[] lengths)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> FilterOut(List<RegularityDetectResultWithPositions> nonPrioritized)
        {
            return nonPrioritized;
        }
    }
}