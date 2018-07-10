using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QandAQuestion : QzEditor, IQuestion
    {
        public QandAQuestion()
        {
            Answers = new HashSet<QandAAnswer>();
            Flags = new HashSet<QandAQuestionFlag>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public bool AnswersInOrder { get; set; }
        public bool IsMultiplePoints { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int TestId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<QandAAnswer> Answers { get; set; }
        public virtual ICollection<QandAQuestionFlag> Flags { get; set; }
    } 
}
