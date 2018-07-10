using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public ActivityEnum ActivityType { get; set; }
        public int Count { get; set; }

        public int OwnerId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public int QuizzId { get; set; }
        public string QuizzTitle { get; set; }

        public int QuizzCommentId { get; set; }
        public string QuizzComment { get; set; }

        public int TestLogId { get; set; }
        public int Score { get; set; }
        public int Total { get; set; }

        public int QuestionId { get; set; }
        public string ActualQuestion { get; set; }
    }
}