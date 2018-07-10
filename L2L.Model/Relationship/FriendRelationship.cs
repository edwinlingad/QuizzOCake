using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class FriendRelationship
    {
        public int Id { get; set; }

        // Foreign Keys
        public int User1Id { get; set; }
        public int User2Id { get; set; }

        // Navigation Properties
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }

    public class FriendRequest
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsDeleted { get; set; }

        // Foreign Keys
        public int RequestToId { get; set; }
        public int RequestFromId { get; set; }

        // Navigation Properties
        public virtual User RequestTo { get; set; }
        public virtual User RequestFrom { get; set; }
        
    }
}
