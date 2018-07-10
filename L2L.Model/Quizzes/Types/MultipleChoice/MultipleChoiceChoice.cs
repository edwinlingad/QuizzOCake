using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class MultipleChoiceChoice
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public bool IsAnswer { get; set; }

        // Foreign Keys
        public int QuestionId { get; set; }

        // Navigation Properties
        public MultipleChoiceQuestion Question { get; set; }
    } 
}
