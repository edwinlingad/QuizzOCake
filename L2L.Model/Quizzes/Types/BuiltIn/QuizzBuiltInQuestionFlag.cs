using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzBuiltInQuestionFlag
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Comment { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuizBuiltInId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual QuizzBuiltIn QuizBuiltIn { get; set; }
    } 
}
