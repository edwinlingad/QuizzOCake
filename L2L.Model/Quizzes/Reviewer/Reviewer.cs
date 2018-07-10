using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class Reviewer
    {
        public int Id { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }

        // Navigation Properties
        public virtual Quizz Quizz { get; set; }      
        public virtual ICollection<QuickNote> QuickNotes { get; set; }
        public virtual ICollection<TextFlashCard> TextFlashCards { get; set; }
    }

    public class TextFlashCard : QzEditor, IReviewer
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        public string Answer { get; set; }

        // Foreign Keys
        public int ReviewerId { get; set; }
        public int OwnerId { get; set; }

        // Navigation Properties
        public virtual Reviewer Reviewer { get; set; }
        public virtual User Owner { get; set; }
    }

    public class QuickNote : QzEditor, IReviewer
    {
        public QuickNote()
        {
            RelatedQuestions = new HashSet<Question>();
        }

        public int Id { get; set; }
        //public string Title { get; set; }
        public string Notes { get; set; }

        // Foreign Keys
        public int ReviewerId { get; set; }
        public int OwnerId { get; set; }

        // Navigation Properties
        public virtual Reviewer Reviewer { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Question> RelatedQuestions { get; set; }
    } 
}
