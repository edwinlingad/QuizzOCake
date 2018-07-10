using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class DependentRequestFromChild
    {
        public int Id { get; set; }

        // Foreign Keys
        public int ToUserId { get; set; }
        public int FromChildId { get; set; }

        // Navigation Properties
        public virtual User ToUser { get; set; }
        public virtual User FromChild { get; set; }
    } 
}
