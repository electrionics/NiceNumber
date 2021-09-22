using System;
using NiceNumber.Core;

namespace NiceNumber.Web.ViewModels
{
    public class HintModel
    {
        public Guid GameId { get; set; }
        
        public RegularityType? Type { get; set; }
        
        public double? RegularityNumber { get; set; }
    }
}