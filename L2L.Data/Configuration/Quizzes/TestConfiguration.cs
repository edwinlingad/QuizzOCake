using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class TestConfiguration : EntityTypeConfiguration<Test>
    {
        public TestConfiguration()
        {
            this.HasRequired(p => p.Quizz);
            this.HasRequired(p => p.DefaultSetting);
            this.HasMany(p => p.Questions)
                .WithRequired(q => q.Test);
        }
    }

    class TestSnapshotConfiguration : EntityTypeConfiguration<TestSnapshot>
    {
        public TestSnapshotConfiguration()
        {
            this.HasRequired(p => p.Owner);
        }
    } 
}
