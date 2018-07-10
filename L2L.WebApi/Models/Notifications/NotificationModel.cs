using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;
using L2L.Entities.Enums;

namespace L2L.WebApi.Models
{
    public class NotificationModel
    {
        public DateTime PostedDate { get; set; }
        public bool IsNew { get; set; }
        public int Count { get; set; }
        public int NewCount { get; set; }
        public string Message { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }

        public int DependentId { get; set; }
        public string DependentName { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public int QuizzId { get; set; }
        public string QuizzTitle { get; set; }
        public string QuizzAuthorUserName { get; set; }
        public string QuizzAuthorFullName { get; set; }

        public int QuizzCommentId { get; set; }
        public string QuizzComment { get; set; }
        public string QuizzCommentAuthorUserName { get; set; }
        public string QuizzCommentAuthorFullName { get; set; }

        public int QuestionId { get; set; }
        public string Question { get; set; }
    }

    public class NotificationTypeModel
    {
        public NotificationTypeEnum NotificationType { get; set; }
        public string FAIcon { get; set; }
        public string FgColor { get; set; }
    }
}