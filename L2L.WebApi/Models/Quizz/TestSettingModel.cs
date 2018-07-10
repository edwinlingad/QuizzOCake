using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class TestSettingModel
    {
        public int Id { get; set; }
        public bool IsOrdered { get; set; }
        public int NumberOfQuestions { get; set; }
        public TimedTypeEnum TimedTypeEnum { get; set; }
        public int SecondsPerQuestion { get; set; }
        public int SecondsForWholeQuiz { get; set; }
        public bool InstantFeedback { get; set; }
    }
}