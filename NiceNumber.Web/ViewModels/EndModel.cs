using System.Collections.Generic;
using NiceNumber.Core;

namespace NiceNumber.Web.ViewModels
{
    public class EndModel
    {
        public int TotalScore { get; set; }
        
        public int SpentMinutes { get; set; }
        
        public int SpentSeconds { get; set; }
        
        public List<EndRegularityInfo> FoundRegularityInfos { get; set; }
    }

    public class EndRegularityInfo
    {
        public RegularityType Type { get; set; }
        
        public int Count { get; set; }
    }
}