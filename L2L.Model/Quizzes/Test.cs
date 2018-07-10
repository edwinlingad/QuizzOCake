using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class Test
    {
        public int Id { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }
        public int DefaultSettingId { get; set; }

        // Navigation Properties
        public virtual Quizz Quizz { get; set; }
        public virtual TestSetting DefaultSetting { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public virtual ICollection<QandAQuestion> QandAQuestions { get; set; }
        public virtual ICollection<MultipleChoiceQuestion> MultiChoiceQuestions{ get; set; }

        public virtual ICollection<MultiChoiceSameQuestion> MultiChoiceSameQuestions { get; set; }
        public virtual ICollection<MultiChoiceSameChoiceGroup> MultiChoiceSameChoiceGroups { get; set; }

    }

    public class TestSnapshot
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int QuizzId { get; set; }
        public string TakeTestModelSnapshot { get; set; }
        public string TestProgressSnapshot { get; set; }
        public string TakeTestCtrlSnapshot { get; set; }

        // Navigation Properties
        public virtual User Owner { get; set; }
    } 
}
