using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzBuiltIn
    {
        public QuizzBuiltIn()
        {
            Flags = new HashSet<QuizzBuiltInQuestionFlag>();
        }

        public int Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string QueryString { get; set; }
        public string Comment { get; set; }

        // Foreign Keys
        public int QuestionId { get; set; }
        public int TestSettingId { get; set; }

        // Navigation Properties
        public virtual Question QuestionContainter { get; set; }
        public virtual ICollection<QuizzBuiltInQuestionFlag> Flags { get; set; }
        public virtual TestSetting TestSetting { get; set; }
    } 
}