using System.Collections.Generic;

namespace QuestionScrumVS2015
{
    [System.ComponentModel.DataAnnotations.Schema.Table("question")]
    public class Question
    {
        public Question()
        {
            AllAnswers = new List<Answer>();
        }
        public int Id { get; set; }
        public int IdFromExternalDB { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Answer> AllAnswers { get; set; }
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("answer")]
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }

        public int IdQuestion { get; set; }
        public virtual Question Question { get; set; }
    }
}
    
