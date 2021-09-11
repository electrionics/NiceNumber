using System.Collections.Generic;
using NiceNumber.Core;

namespace NiceNumber.Domain.Entities
{
    public class HintResult
    {
        public List<byte> StartPositions { get; set; }
        
        public List<byte> SubNumberLengths { get; set; }
        
        public List<byte> AllPositions { get; set; }
        
        public RegularityType Type { get; set; }
        
        public double RegularityNumber { get; set; }
    }
}