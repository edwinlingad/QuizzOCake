using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class PublicGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool AnyoneCanJoin { get; set; }

        // Foreign Keys
        public int CreatorId { get; set; }

        // Navigation Properties
        public virtual User Creator { get; set; }
        public virtual ICollection<PublicGroupMember> Members { get; set; }
    } 
}
