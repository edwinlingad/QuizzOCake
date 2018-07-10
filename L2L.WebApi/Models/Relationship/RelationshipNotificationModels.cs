using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public enum RelationshipNotificationResponseEnum
    {
        Accept = 0,
        Reject,
        Resend,
        Cancel
    }

    public class RelationshipNotificationModel
    {
        public int Id { get; set; }
        public RelationshipNotificationTypeEnum RNType { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsNew { get; set; }
        public bool IsAccepted { get; set; }

        // Foreign Keys
        public int ToUserId { get; set; }
        public int FromUserId { get; set; }
        public int? FriendRequestId { get; set; }
        public int? DependentRequestFromUserId { get; set; }

        // generated
        public string ToUserName { get; set; }
        public string ToUserFullName { get; set; }
        public string FromUserName { get; set; }
        public string FromUserFullName { get; set; }
        public string QuizzmateRequestMessage { get; set; }

        // From response
        public RelationshipNotificationResponseEnum Response { get; set; }
    }
}