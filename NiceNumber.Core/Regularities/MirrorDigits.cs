using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core.Results;
using NiceNumber.Helpers;

namespace NiceNumber.Core.Regularities
{
    public class MirrorDigits:BaseRegularity<RegularityDetectResultWithPositions>
    {
        public override RegularityType Type => RegularityType.MirrorDigitsAtAnyPosition;
        
        public MirrorDigits():base(2)
        {
        }
        
        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte firstPosition = 0)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> Detect(byte[] number, byte[] lengths, byte firstPosition)
        {
            return null;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number)
        {
            var mirrorCandidates = number
                .GroupBy(x => x)
                .Where(x => x.Count() >= 2)
                .Select(x => x.Key)
                .ToHashSet();

            var sources = new List<List<byte>>();
            var mirrors = new List<List<byte>>();
            
            for (byte i = 0; i < number.Length - MinLength - 1; i++)
            {
                var containsI = mirrorCandidates.Contains(number[i]);
                if (!containsI) continue;

                var newSourceStack = new List<byte> {i};
                sources.Add(newSourceStack);
                
                for (var j = (byte)(i + 1); j < number.Length - MinLength; j++)
                {
                    var containsJ = mirrorCandidates.Contains(number[j]);
                    if (!containsJ) continue;
                    
                    var count = sources.Count;
                    for (var k = 0; k < count; k++)
                    {
                        if (sources[k].Count <= number.Length / 2 - 1 && number.Length - j >= sources[k].Count + 2 && j > sources[k][sources[k].Count - 1])
                        {
                            sources.Add(new List<byte>(sources[k]));
                            sources[k].Add(j);
                        }
                    }
                }
            }
            
            for (var i = (byte)(number.Length - 1); i >= MinLength + 1; i--)
            {
                var containsI = mirrorCandidates.Contains(number[i]);
                if (!containsI) continue;

                var newMirrorStack = new List<byte> {i};
                mirrors.Add(newMirrorStack);

                for (var j = (byte)(i - 1); j >= MinLength; j--)
                {
                    var containsJ = mirrorCandidates.Contains(number[j]);
                    if (!containsJ) continue;
                    
                    var count = mirrors.Count;
                    for (var k = 0; k < count; k++)
                    {
                        if (mirrors[k].Count <= number.Length / 2 - 1 && j >= mirrors[k].Count + 2 && j < mirrors[k][mirrors[k].Count - 1])
                        {
                            mirrors.Add(new List<byte>(mirrors[k]));
                            mirrors[k].Add(j);
                        }
                    }
                }
            }

            var firstArrGroups = sources
                .Where(x => x.Count >= MinLength)
                .Select(x => x.ToArray())
                .GroupBy(x => x.Length)
                .ToArray();

            var secondArrGroups = mirrors
                .Where(x => x.Count >= MinLength)
                .Select(x => x.ToArray())
                .GroupBy(x => x.Length)
                .ToArray();

            var comparer = new ByteArrayEqualityComparer();
            var result = new List<RegularityDetectResultWithPositions>();

            foreach (var firstArrGroup in firstArrGroups)
            {
                var firstNumberArrs = firstArrGroup
                    .Select(x => x
                        .Select(y => number[y])
                        .ToArray())
                    .ToHashSet(comparer);
                
                var secondMatches = new List<byte[]>();
                
                
                foreach (var secondArrGroup in secondArrGroups)
                {
                    if (firstArrGroup.Key != secondArrGroup.Key) continue;

                    foreach (var secondArr in secondArrGroup)
                    {
                        var secondNumberArr = secondArr.Select(x => number[x]).ToArray();
                        if (firstNumberArrs.Contains(secondNumberArr))
                        {
                            secondMatches.Add(secondArr);
                        }
                    }
                }
                
                foreach (var firstArr in firstArrGroup)
                {
                    var firstNumberArr = firstArr
                        .Select(x => number[x])
                        .ToArray();

                    foreach (var secondArr in secondMatches)
                    {
                        if (secondArr[secondArr.Length - 1] <= firstArr[firstArr.Length - 1]) continue;
                        
                        var secondNumberArr = secondArr
                            .Select(x => number[x])
                            .ToArray();

                        if (!comparer.Equals(firstNumberArr, secondNumberArr)) continue;
                        
                        var res = new RegularityDetectResultWithPositions
                        {
                            FirstNumber = number[firstArr[0]],
                            FirstPosition = firstArr[0],
                            Length = firstArr.Length * 2,
                            RegularityNumber = secondArr[0] - firstArr[firstArr.Length - 1] - 1,
                            Positions = firstArr.Concat(secondArr.Reverse()).ToArray()
                        };
                        res.SubNumberLengths = res.Positions.Select(x => (byte)1).ToArray();
                        result.Add(res);
                    }
                }
            }

            return result;
        }

        protected override List<RegularityDetectResultWithPositions> DetectAll(byte[] number, byte[] lengths)
        {
            return null;
        }

        protected override IEqualityComparer<RegularityDetectResultWithPositions> Comparer =>
            RegularityDetectResultWithPositions.Comparer;

        protected override bool Include(RegularityDetectResultWithPositions first, RegularityDetectResultWithPositions second)
        {
            return second.Positions.All(pos => first.Positions.Contains(pos));
        }
    }
}