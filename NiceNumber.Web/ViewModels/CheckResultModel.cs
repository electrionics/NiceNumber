namespace NiceNumber.Web.ViewModels
{
    public class CheckResultModel
    {
        public CheckResultModel()
        {
            Hint = CheckHint.No;
        }
        
        public bool Match { get; set; }
        
        public int PointsAdded { get; set; }
        
        public int NewTotalPoints { get; set; }

        public CheckHint Hint { get; set; }
    }

    public enum CheckHint
    {
        No = 1,
        AddOneDigit = 2
    }
}