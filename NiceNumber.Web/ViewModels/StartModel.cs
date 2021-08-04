using System;
using System.Collections.Generic;
using NiceNumber.Core;
using NiceNumber.Domain.Entities;

namespace NiceNumber.Web.ViewModels
{
    public class StartModel
    {
        public Guid GameId { get; set; }
        
        public long Number { get; set; }
        
        public int Length { get; set; }
        
        public DifficultyLevel DifficultyLevel { get; set; }
        
        public List<StartRegularityInfo> ExistRegularityInfos { get; set; }
        
        public Dictionary<int, int> ExistRegularityTypeCounts { get; set; }
    }

    public class StartRegularityInfo
    {
        public RegularityType Type { get; set; }
        
        public double RegularityNumber { get; set; }
    }
}