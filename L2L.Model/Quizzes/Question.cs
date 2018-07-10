using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public int Order { get; set; }
        public bool IsFlashCard { get; set; }

        // Foreign Keys
        public int TestId { get; set; }
        public int AuthorId { get; set; }

        // Navigation Properties
        public virtual Test Test { get; set; }
        //public virtual User Author { get; set; }
        public virtual QuickNote QuickNoteRef { get; set; }
    }
}
