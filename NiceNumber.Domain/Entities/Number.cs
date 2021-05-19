using System.Collections.Generic;

namespace NiceNumber.Domain.Entities
{
    public class Number
    {
        public int Id { get; set; }
        
        public long Value { get; set; }
        
        public int Length { get; set; }
        
        public List<Regularity> Regularities { get; set; }
        
        public List<Game> Games { get; set; }
    }
}