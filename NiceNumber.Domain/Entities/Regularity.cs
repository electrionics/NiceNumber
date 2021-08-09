using System;
using System.Collections.Generic;
using System.Linq;
using NiceNumber.Core;

namespace NiceNumber.Domain.Entities
{
    public class Regularity
    {
        public int Id { get; set; }
        
        public int NumberId { get; set; }
        
        public RegularityType Type { get; set; }
        
        public SequenceType SequenceType { get; set; }
        
        public double RegularityNumber { get; set; }
        
        public string StartPositionsStr { get; set; }
        
        public string SubNumberLengthsStr { get; set; }
        
        public bool Deleted { get; set; }

        private List<byte> _startPositions;
        public List<byte> StartPositions => _startPositions ??= StartPositionsStr
                .Split(',')
                .Select(byte.Parse)
                .ToList();

        private List<byte> _subNumberLengths;
        public List<byte> SubNumberLengths => _subNumberLengths ??= SubNumberLengthsStr
                .Split(',')
                .Select(byte.Parse)
                .ToList();

        private List<byte> _allPositions;
        public List<byte> AllPositions => _allPositions ??= StartPositions
            .SelectMany((pos, i) => Enumerable.Range(pos, SubNumberLengths[i]))
            .Select(pos => (byte)pos)
            .ToList();

        public Number Number { get; set; }
        
        public List<Check> Checks { get; set; }
        
        
        private sealed class RegularityEqualityComparer : IEqualityComparer<Regularity>
        {
            public bool Equals(Regularity x, Regularity y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Type == y.Type && 
                       x.SequenceType == y.SequenceType && 
                       x.RegularityNumber.Equals(y.RegularityNumber) && 
                       x.StartPositionsStr == y.StartPositionsStr && 
                       x.SubNumberLengthsStr == y.SubNumberLengthsStr;
            }

            public int GetHashCode(Regularity obj)
            {
                return HashCode.Combine((int) obj.Type, (int) obj.SequenceType, obj.RegularityNumber, obj.StartPositionsStr, obj.SubNumberLengthsStr);
            }
        }

        public static IEqualityComparer<Regularity> RegularityComparer { get; } = new RegularityEqualityComparer();
    }
}