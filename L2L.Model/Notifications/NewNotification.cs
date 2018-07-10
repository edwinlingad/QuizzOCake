using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class NewNotification
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

        // Navigation Properties
        public virtual User ToUser { get; set; }
        public virtual User FromUser { get; set; }
        public virtual Quizz Quizz { get; set; }
        public virtual QuizzComment QuizzComment { get; set; }
        public virtual Question Question { get; set; }
        public virtual Assignment Assignment { get; set; }
        public virtual AssignmentGroup AssignmentGroup { get; set; }
        public virtual FriendRequest FriendRequest { get; set; }
        public virtual User ToUnQuizzmate { get; set; }
    }
}
