﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QuizzUpvoteConfiguration : EntityTypeConfiguration<QuizzUpvote>
    {
        public QuizzUpvoteConfiguration()
        {
            this.HasRequired(p => p.User);
            this.HasRequired(p => p.QuizRating);
        }
    } 
}
