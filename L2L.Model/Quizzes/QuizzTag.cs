using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzTag
    {
        public QuizzTag()
        {
            Quizzes = new HashSet<Quizz>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public virtual ICollection<Quizz> Quizzes { get; set; }
    } 
}
