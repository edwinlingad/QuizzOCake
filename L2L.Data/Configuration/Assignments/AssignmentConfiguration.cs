using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using L2L.Entities;

namespace L2L.Data.Configuration
{
    class AssignmentGroupConfiguration : EntityTypeConfiguration<AssignmentGroup>
    {
        public AssignmentGroupConfiguration()
        {
            this.HasRequired(p => p.AssignedBy);
            this.HasRequired(p => p.TestSetting);
            this.HasRequired(p => p.Quizz);
            this.HasMany(p => p.Assignments)
                .WithRequired(q => q.AssignmentGroup);
        }
    } 

    class AssignmentConfiguration : EntityTypeConfiguration<Assignment>
    {
        public AssignmentConfiguration()
        {
            this.HasRequired(p => p.Dependent);
            this.HasRequired(p => p.AssignmentGroup);
            this.HasRequired(p => p.TestResult);
        }
    }
}
