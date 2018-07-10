using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities.Enums;

namespace L2L.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public bool IsNew { get; set; }
        public int Count { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuizzNotificationId { get; set; }
        public int QuestionNotificationId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual QuizzNotification QuizzNotification { get; set; }
        public virtual QuizzCommentNotification QuizzCommentNotification { get; set; }
        public virtual QuestionNotification QuestionNotification { get; set; }
    }

    public class QuizzNotification
    {
        public QuizzNotification()
        {
            NotificationSources = new List<QuizzNotificationSource>();
        }

        public int Id { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuizzId { get; set; }
        public int NotificationId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Quizz Quizz { get; set; }
        public virtual Notification Notification { get; set; }

        public virtual IList<QuizzNotificationSource> NotificationSources { get; set; }
    }

    public class QuizzNotificationSource
    {
        public int Id { get; set; }
        
        // Foreign Keys
        public int SourceId { get; set; }
        public int QuizzNotificationId { get; set; }

        // Navigation Properties
        public virtual User Source { get; set; }
        public virtual QuizzNotification QuizzNotification { get; set; }
    }

    public class QuizzCommentNotification
    {
        public QuizzCommentNotification()
        {
            NotificationSources = new List<QuizzCommentNotificationSource>();
        }
        public int Id { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuizzCommentId { get; set; }
        public int NotificationId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual QuizzComment QuizzComment { get; set; }
        public virtual Notification Notification { get; set; }

        public virtual IList<QuizzCommentNotificationSource> NotificationSources { get; set; }
    }

    public class QuizzCommentNotificationSource
    {
        public int Id { get; set; }

        // Foreign Keys
        public int SourceId { get; set; }
        public int QuizzCommentNotificationId { get; set; }

        // Navigation Properties
        public virtual User Source { get; set; }
        public virtual QuizzCommentNotification QuizzCommentNotification { get; set; }
    } 

    public class QuestionNotification
    {
        public QuestionNotification()
        {
            NotificationSources = new List<QuestionNotificationSource>();
        }

        public int Id { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public int NotificationId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
        public virtual Notification Notification { get; set; }

        public virtual IList<QuestionNotificationSource> NotificationSources { get; set; }

    }

    public class QuestionNotificationSource
    {
        public int Id { get; set; }

        // Foreign Keys
        public int SourceId { get; set; }
        public int QuestionNotificationId { get; set; }

        // Navigation Properties
        public virtual User Source { get; set; }
        public virtual QuestionNotification QuestionNotification { get; set; }
    } 
}
