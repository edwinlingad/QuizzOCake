using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class ReviewerItemConfiguration : EntityTypeConfiguration<ReviewerItem>
    {
        public ReviewerItemConfiguration()
        {
            //this.Property(p => p.ActualReviewerId)
            //    .IsRequired();

            //this.HasRequired(p => p.Reviewer);
        }
    } 
}
