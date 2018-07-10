using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class DependentRequestFromUser
    {
        public int Id { get; set; }

        // Foreign Keys
        public int ToChildId { get; set; }
        public int FromUserId { get; set; }

        // Navigation Properties
        public virtual User ToChild { get; set; }
        public virtual User FromUser { get; set; }
    } 
}
