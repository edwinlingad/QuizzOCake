using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public enum TimedTypeEnum
    {
        NotTimed,
        WholeQuizTimed,
        TimedPerQuestion
    }

    public class TestSetting
    {
        public TestSetting()
        {
            TimedTypeEnum = TimedTypeEnum.NotTimed;            
        }

        public int Id { get; set; }
        public bool IsOrdered { get; set; }
        public int NumberOfQuestions { get; set; }
        public TimedTypeEnum TimedTypeEnum { get; set; }
        public int SecondsPerQuestion { get; set; }
        public int SecondsForWholeQuiz { get; set; }
        public bool InstantFeedback { get; set; }
    }
}
