using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities.Enums;

namespace L2L.Entities
{
    public class QuizzFlag
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public QuizzFlagEnum QuizzFlagType { get; set; }

        // Foreign Key
        public int AuthorId { get; set; }
        public int QuizzId { get; set; }

        // Navigation Properties
        public virtual User Author { get; set; }
        public virtual Quizz Quizz { get; set; }
    } 
}
