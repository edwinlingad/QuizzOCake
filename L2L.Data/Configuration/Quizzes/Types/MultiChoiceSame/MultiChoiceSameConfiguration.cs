using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class MultiChoiceSameQuestionConfiguration : EntityTypeConfiguration<MultiChoiceSameQuestion>
    {
        public MultiChoiceSameQuestionConfiguration()
        {
            this.HasRequired(p => p.Test);
            this.HasRequired(p => p.ChoiceGroup);

            this.Property(p => p.Question)
                .IsRequired();
        }
    }

    class MultiChoiceSameChoiceGroupConfiguration : EntityTypeConfiguration<MultiChoiceSameChoiceGroup>
    {
        public MultiChoiceSameChoiceGroupConfiguration()
        {
            this.HasRequired(p => p.Test);
            this.Property(p => p.Name)
                .IsRequired();
            this.HasMany(p => p.Choices)
                .WithRequired(q => q.ChoiceGroup);
            this.HasMany(p => p.Questions)
                .WithRequired(q => q.ChoiceGroup);
        }
    }

    class MultiChoiceSameChoiceConfiguration : EntityTypeConfiguration<MultiChoiceSameChoice>
    {
        public MultiChoiceSameChoiceConfiguration()
        {
            this.HasRequired(p => p.ChoiceGroup);

            this.Property(p => p.Value)
                .IsRequired();
        }
    }
}
