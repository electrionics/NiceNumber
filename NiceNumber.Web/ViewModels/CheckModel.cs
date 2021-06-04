using System;
using NiceNumber.Core;

namespace NiceNumber.Web.ViewModels
{
    public class CheckModel
    {
        public byte[] Positions { get; set; }
        
        public RegularityType Type { get; set; }
        
        public Guid GameId { get; set; }
    }
}