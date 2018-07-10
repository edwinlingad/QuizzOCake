using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QuizBuiltInConfiguration : EntityTypeConfiguration<QuizzBuiltIn>
    {
        public QuizBuiltInConfiguration()
        {
            this.Property(p => p.ControllerName)
                .IsRequired()
                .HasMaxLength(Constants.FunctionNameMaxLength);

            this.Property(p => p.ActionName)
                .IsRequired()
                .HasMaxLength(Constants.FunctionNameMaxLength);

            this.Property(p => p.QueryString)
                .IsOptional()
                .HasMaxLength(512);

            this.Property(p => p.Comment)
                .IsRequired()
                .HasMaxLength(Constants.CommentsMaxLength);

            this.HasMany(p => p.Flags)
                .WithRequired(q => q.QuizBuiltIn);

            this.HasRequired(p => p.QuestionContainter);
            this.HasRequired(p => p.TestSetting);
            
        }
    } 
}
