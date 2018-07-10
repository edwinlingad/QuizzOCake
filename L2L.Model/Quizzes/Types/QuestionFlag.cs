using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuestionFlag
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        //public QuestionTypeEnum QuestionType { get; set; }
        public QuestionFlagEnum QuestionFlagType { get; set; }

        // Foreign Keys
        public int AuthorId { get; set; }
        public int QuestionId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual Question Question { get; set; }
    } 
}
