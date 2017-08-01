using System.Collections.Generic;

namespace QuestionScrumVS2015
{
    [System.ComponentModel.DataAnnotations.Schema.Table("question")]
    public partial class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Answer> AllAnswers { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("answer")]
    public partial class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int IdQuestion { get; set; }
        public virtual Question Questions { get; set; }
    }
}
    
