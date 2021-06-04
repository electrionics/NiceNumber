using System;
using System.Collections.Generic;

namespace NiceNumber.Domain.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        
        public string SessionId { get; set; }
        
        public int NumberId { get; set; }
        
        public DifficultyLevel DifficultyLevel { get; set; }
        
        public int Score { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime? FinishTime { get; set; }
        
        public bool EndInBackground { get; set; }
        
        
        public Number Number { get; set; }
        
        public List<Check> Checks { get; set; }
    }

    public enum DifficultyLevel
    {
        Easy = 1,
        Normal = 2,
        Hard = 3
    }
}