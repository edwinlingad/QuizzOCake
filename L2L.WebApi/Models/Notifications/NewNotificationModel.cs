using L2L.Entities;
using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class NewNotificationModel
    {
        public int Id { get; set; }
        public DateTime PostedDate { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public bool IsNew { get; set; }
        public string NewFromUser { get; set; }
        public string OldFromUser { get; set; }

        // Foreign Keys
        public int ToUserId { get; set; }
        public int FromUserId { get; set; }
        public int QuizzId { get; set; }
        public int QuizzCommentId { get; set; }
        public int QuestionId { get; set; }
        public int AssignmentId { get; set; }
        public int AssignmentGroupId { get; set; }
        public int? FriendRequestId { get; set; }
        public int? ToUnQuizzmateId { get; set; }

        // generated
        //public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public string FromUserFullName { get; set; }

        public string QuizzTitle { get; set; }
        public string QuizzOwnerUserName { get; set; }
        public string QuizzOwnerFullName { get; set; }

        public string QuizzCommentValue { get; set; }
        public string QuizzCommentOwnerUserName { get; set; }
        public string QuizzCommentOwnerFullName { get; set; }
        
        public int Count { get; set; }
        public int NewCount { get; set; }
        public int? AssignmentScore { get; set; }

        public int? ToQuizzmateUserId { get; set; }
        public string ToQuizzmateUserName { get; set; }
        public string ToQuizzmateUserFullName { get; set; }
        public int? FromQuizzmateUserId { get; set; }
        public string FromQuizzmateUserName { get; set; }
        public string FromQuizzmateUserFullName { get; set; }
        public string QuizzmateRequestMessage { get; set; }
        public string ToUnQuizzmateUserName { get; set; }
        public string ToUnQUizzmateUserFullName { get; set; }

        // Navigation Properties
        //public virtual User ToUser { get; set; }
        //public virtual User FromUser { get; set; }
        //public virtual Quizz Quizz { get; set; }
        //public virtual QuizzComment QuizzComment { get; set; }
        //public virtual Question Question { get; set; }
        //public virtual Assignment Assignment { get; set; }
        //public virtual AssignmentGroup AssignmentGroup { get; set; }
    }
}