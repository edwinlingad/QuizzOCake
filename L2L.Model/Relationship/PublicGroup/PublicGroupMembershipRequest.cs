using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class PublicGroupMembershipRequest
    {
        public int Id { get; set; }

        // Foreign Keys
        public int PublicGroupId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public virtual PublicGroup PublicGroup { get; set; }
        public virtual User User { get; set; }
    } 
}
