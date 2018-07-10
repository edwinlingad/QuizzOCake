using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class UserGroupMember
    {
        public int Id { get; set; }

        // Foreign Keys
        public int UserGroupId { get; set; }
        public int MemberId { get; set; }

        // Navigation Properties
        public virtual UserGroup UserGroup { get; set; }
        public virtual User Member { get; set; }
    } 
}
