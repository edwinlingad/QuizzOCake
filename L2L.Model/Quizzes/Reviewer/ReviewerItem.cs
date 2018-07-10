using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class ReviewerItem
    {
        public int Id { get; set; }
        public ReviewerTypeEnum ReviewerType { get; set; }
        public int ActualReviewerId { get; set; }
        public int Order { get; set; }

        // Foreign Keys
        public int ReviewerId { get; set; }

        // Navigation Properties        
        public virtual Reviewer Reviewer { get; set; }
    } 
}
