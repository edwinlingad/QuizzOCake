using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using L2L.Entities;

namespace L2L.Data.Configuration
{
    class ActivitiesConfiguration : EntityTypeConfiguration<Activity>
    {
        public ActivitiesConfiguration()
        {
            this.Property(q => q.PostedDate).IsRequired();
            this.HasRequired(q => q.Owner);
            this.HasRequired(q => q.Quizz);
            this.HasRequired(q => q.QuizzComment);
            this.HasRequired(q => q.TestLog);
            this.HasRequired(q => q.Question);
        }
    } 
}
