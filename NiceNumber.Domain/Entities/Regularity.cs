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
        
        public List<byte> StartPositions => StartPositionsStr
                .Split(',')
                .Select(byte.Parse)
                .ToList();

        public List<byte> SubNumberLengths => SubNumberLengthsStr
                .Split(',')
                .Select(byte.Parse)
                .ToList();

        public Number Number { get; set; }
        
        public List<Check> Checks { get; set; }
    }
}