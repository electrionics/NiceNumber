using System;
using NiceNumber.Core;

namespace NiceNumber.Domain.Entities
{
    public class Check
    {
        public int Id { get; set; }
        
        public Guid GameId { get; set; }
        
        public int RegularityId { get; set; }
        
        public RegularityType CheckType { get; set; }
        
        public int ClosestRegularityId { get; set; }

        public int NeedAddDigits { get; set; }
        
        public int NeedRemoveDigits { get; set; }

        public int NeedChangeDigits => NeedAddDigits + NeedRemoveDigits;

        public int ScoreAdded { get; set; }
        
        public DateTime TimePerformed { get; set; }
        

        public Game Game { get; set; }
        
        public Regularity Regularity { get; set; }
        
        public Regularity ClosestRegularity { get; set; }
    }
}