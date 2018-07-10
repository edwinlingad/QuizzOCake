using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class UserGroup
    {
        public UserGroup()
        {
            Members = new HashSet<UserGroupMember>();
        }

        public int Id { get; set; }
        public string GroupName { get; set; }

        // Foreign Keys
        public int OwnerId { get; set; }

        // Navigation Properties
        public virtual User Owner { get; set; }
        public virtual ICollection<UserGroupMember> Members { get; set; }
    } 
}
