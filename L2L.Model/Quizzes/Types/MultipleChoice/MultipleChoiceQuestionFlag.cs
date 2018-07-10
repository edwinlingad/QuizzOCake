using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class MultipleChoiceQuestionFlag
    {
        public int Id { get; set; }
        public string Comment { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuestionId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual MultipleChoiceQuestion Question { get; set; }
    } 
}
