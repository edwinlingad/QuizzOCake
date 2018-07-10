using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class MultipleChoiceQuestionConfiguration : EntityTypeConfiguration<MultipleChoiceQuestion>
    {
        public MultipleChoiceQuestionConfiguration()
        {
            this.HasMany(p => p.Flags)
                .WithRequired(q => q.Question)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.Test);
        }
    } 
}
