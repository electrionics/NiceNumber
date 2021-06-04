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
    }
}