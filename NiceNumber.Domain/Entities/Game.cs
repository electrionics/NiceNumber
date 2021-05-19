using System;
using System.Collections.Generic;

namespace NiceNumber.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        
        public string SessionId { get; set; }
        
        public int NumberId { get; set; }
        
        public Complexity Complexity { get; set; }
        
        public int Score { get; set; }
        
        public DateTime StartTime { get; set; }
        
        
        public Number Number { get; set; }
        
        public List<Check> Checks { get; set; }
    }

    public enum Complexity
    {
        Easy = 1,
        Normal = 2,
        Hard = 3
    }
}