namespace NiceNumber.Domain.Entities
{
    public class TutorialTask
    {
        public int Id { get; set; }
        
        public int LevelId { get; set; }
        
        public int Order { get; set; }
        
        public string Name { get; set; }
        
        public string Text { get; set; }
        
        public bool? AnySubtask { get; set; }
        
        public string Subtasks { get; set; }
        
        public string ApplyCondition { get; set; }
        
        public string ConditionParameter { get; set; }
        
        public TutorialLevel Level { get; set; }
    }
}