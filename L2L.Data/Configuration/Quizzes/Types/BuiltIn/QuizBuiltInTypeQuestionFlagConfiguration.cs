﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QuizBuiltInTypeQuestionFlagConfiguration : EntityTypeConfiguration<QuizzBuiltInQuestionFlag>
    {
        public QuizBuiltInTypeQuestionFlagConfiguration()
        {
            this.Property(p => p.Comment)
                .HasMaxLength(Constants.DescriptionMaxLength);

            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.QuizBuiltIn);
        }
    } 
}
