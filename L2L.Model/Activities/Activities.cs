using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class Activity
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public ActivityEnum ActivityType { get; set; }

        // Foreign Keys
        public int OwnerId { get; set; }
        public int QuizzId { get; set; }
        public int QuizzCommentId { get; set; }
        public int TestLogId { get; set; }
        public int QuestionId { get; set; }

        // Navigation Properties
        public virtual User Owner { get; set; }
        public virtual Quizz Quizz { get; set; }
        public virtual QuizzComment QuizzComment { get; set; }
        public virtual TestLog TestLog { get; set; }
        public virtual Question Question { get; set; }
    } 
}
