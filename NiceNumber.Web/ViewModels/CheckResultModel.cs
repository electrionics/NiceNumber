namespace NiceNumber.Web.ViewModels
{
    public class CheckResultModel
    {
        public bool Match { get; set; }
        
        public int PointsAdded { get; set; }
        
        public int NewTotalPoints { get; set; }

        public CheckHint AddHint { get; set; }

        public CheckHint RemoveHint { get; set; }
        
        public double RegularityNumber { get; set; }
        
        public bool Hinted { get; set; }
    }

    public enum CheckHint
    {
        No = 0,
        AddOneDigit = 11,
        AddMoreThanOneDigit = 12,
        RemoveOneDigit = 21,
        RemoveMoreThanOneDigit = 22
    }
}