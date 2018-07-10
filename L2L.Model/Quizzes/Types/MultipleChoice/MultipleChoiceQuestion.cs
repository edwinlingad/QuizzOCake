using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class MultipleChoiceQuestion : QzEditor, IQuestion
    {
        public MultipleChoiceQuestion()
        {
            Choices = new HashSet<MultipleChoiceChoice>();
            Flags = new HashSet<MultipleChoiceQuestionFlag>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsMultiplePoints { get; set; }
        public bool ShuffleChoices { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int TestId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<MultipleChoiceChoice> Choices { get; set; }
        public virtual ICollection<MultipleChoiceQuestionFlag> Flags { get; set; }
    } 
}
