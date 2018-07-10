using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QandAQuestionConfiguration : EntityTypeConfiguration<QandAQuestion>
    {
        public QandAQuestionConfiguration()
        {
            this.HasMany(p => p.Answers)
                .WithRequired(q => q.Question)
                .WillCascadeOnDelete(false);

            this.HasMany(p => p.Flags)
                .WithRequired(q => q.Question)
                .WillCascadeOnDelete(false);

            this.Property(p => p.Question)
                .IsRequired();

            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.Test);
        }
    } 
}
