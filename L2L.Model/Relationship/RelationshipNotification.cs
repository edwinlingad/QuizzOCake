using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class RelationshipNotification
    {
        public int Id { get; set; }
        public RelationshipNotificationTypeEnum RNType { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsNew { get; set; }

        // Foreign Keys
        public int ToUserId { get; set; }
        public int FromUserId { get; set; }
        public int? FriendRequestId { get; set; }
        public int? DependentRequestFromUserId { get; set; }

        // Navigation Properties
        public virtual User ToUser { get; set; }
        public virtual User FromUser { get; set; }
        public virtual FriendRequest FriendRequest { get; set; }
        public virtual DependentRequestFromUser DependentRequestFromUser { get; set; }
    } 
}
