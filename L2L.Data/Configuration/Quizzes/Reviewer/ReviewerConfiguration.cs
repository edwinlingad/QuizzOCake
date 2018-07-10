using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class ReviewerConfiguration : EntityTypeConfiguration<Reviewer>
    {
        public ReviewerConfiguration()
        {
            this.HasRequired(p => p.Quizz);
        }
    }

    class QuickNoteConfiguration : EntityTypeConfiguration<QuickNote>
    {
        public QuickNoteConfiguration()
        {
            this.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(Constants.NameMaxLength);
            this.Property(p => p.Notes)
                .IsRequired();

            this.HasRequired(p => p.Owner);
            this.HasRequired(p => p.Reviewer);

            this.HasMany(p => p.RelatedQuestions)
                .WithOptional(q => q.QuickNoteRef);
        }
    }

    class TextFlashCardConfiguration : EntityTypeConfiguration<TextFlashCard>
    {
        public TextFlashCardConfiguration()
        {
            this.Property(p => p.Title)
                .IsRequired();
            this.Property(p => p.Answer)
                .IsRequired();

            this.HasRequired(p => p.Owner);

            this.HasRequired(p => p.Reviewer);
        }
    } 
}
