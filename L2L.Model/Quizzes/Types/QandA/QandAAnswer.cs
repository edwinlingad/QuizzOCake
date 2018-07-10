using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QandAAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }

        // Foreign Keys
        public int QuestionId { get; set; }

        // Navigation Properties
        public virtual QandAQuestion Question { get; set; }
    } 
}
