using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class PublicGroupMember
    {
        public int Id { get; set; }

        // Foreign Keys
        public int PublicGroupId { get; set; }
        public int MemberId { get; set; }

        // Navigation Properties
        public virtual PublicGroup PublicGroup { get; set; }
        public virtual User Member { get; set; }
    } 
}
