using System;
using System.Collections.Generic;
using NiceNumber.Core;

namespace NiceNumber.Web.ViewModels
{
    public class StartModel
    {
        public Guid GameId { get; set; }
        
        public long Number { get; set; }
        
        public int Length { get; set; }
        
        public List<StartRegularityInfo> RegularityInfos { get; set; }
        
        public Dictionary<RegularityType, int> RegularityTypeCounts { get; set; }
    }

    public class StartRegularityInfo
    {
        public RegularityType Type { get; set; }
        
        public double RegularityNumber { get; set; }
    }
}