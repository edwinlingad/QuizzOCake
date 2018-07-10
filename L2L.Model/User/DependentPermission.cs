using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class DependentPermission
    {
        public int Id { get; set; }
        public bool CanAcceptQuizzmateRequests { get; set; }
        public bool CanUseMessaging { get; set; }

        // Foreign Keys

        // Navigation Properties
        public virtual User User { get; set; }
    } 
}
