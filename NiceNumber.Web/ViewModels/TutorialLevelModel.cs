using System.Collections.Generic;

namespace NiceNumber.Web.ViewModels
{
    public class TutorialLevelModel
    {
        public string Title { get; set; }
        
        public string Text { get; set; }
        
        public int Level { get; set; }
        
        public List<TutorialTaskModel> Tasks { get; set; }
    }

    public class TutorialTaskModel
    {
        public int Order { get; set; }
        
        public string Name { get; set; }
        
        public string Text { get; set; }
        
        public bool? AnySubtask { get; set; }
        
        public List<int> Subtasks { get; set; }
        
        public string ApplyCondition { get; set; }
        
        public string ConditionParameter { get; set; }
    }
}