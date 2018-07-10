using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class CategoryConfiguration : EntityTypeConfiguration<QuizzCategory>
    {
        public CategoryConfiguration()
        {
        }
    } 

    class QuizzConfiguration : EntityTypeConfiguration<Quizz>
    {
        public QuizzConfiguration()
        {
            this.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(p => p.Description)
                .IsOptional()
                .HasMaxLength(2048);

            this.HasRequired(p => p.QuizRating)
                .WithRequiredPrincipal(q => q.Quizz);
           
            this.HasRequired(p => p.Owner);

            this.HasMany(p => p.Tests)
                .WithRequired(q => q.Quizz);

            this.HasMany(p => p.Reviewers)
                .WithRequired(q => q.Quizz);

            this.HasMany(p => p.TestLogs)
                .WithRequired(q => q.Quizz);
        }
    }

    class QuizzCommentConfiguration : EntityTypeConfiguration<QuizzComment>
    {
        public QuizzCommentConfiguration()
        {
            this.Property(p => p.Comment)
                .IsRequired();
            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.Quizz);
            this.HasMany(p => p.Likes)
                .WithRequired(q => q.QuizzComment);
            this.HasMany(p => p.Flags)
                .WithRequired(q => q.QuizzComment);
        }
    }

    class QuizzCommentLikeConfiguration : EntityTypeConfiguration<QuizzCommentLike>
    {
        public QuizzCommentLikeConfiguration()
        {
            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.QuizzComment);
        }
    }

    class QuizzCommentFlagConfiguration : EntityTypeConfiguration<QuizzCommentFlag>
    {
        public QuizzCommentFlagConfiguration()
        {
            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.QuizzComment);
        }
    } 
}
