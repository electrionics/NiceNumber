namespace NiceNumber.Domain.Entities
{
    public class TutorialLevel
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string Text { get; set; }
        
        public int Order { get; set; }
        
        public int NumberId { get; set; }
        
        public Number Number { get; set; }
    }
}