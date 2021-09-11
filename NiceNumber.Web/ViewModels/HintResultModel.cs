using NiceNumber.Core;

namespace NiceNumber.Web.ViewModels
{
    public class HintResultModel
    {
        public int[] Positions { get; set; }
        
        public RegularityType Type { get; set; }
        
        public double RegularityNumber { get; set; }
    }
}