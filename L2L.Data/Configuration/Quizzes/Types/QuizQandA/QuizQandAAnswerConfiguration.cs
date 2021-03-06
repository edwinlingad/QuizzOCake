﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QandAAnswerConfiguration : EntityTypeConfiguration<QandAAnswer>
    {
        public QandAAnswerConfiguration()
        {
            this.HasRequired(p => p.Question);
            this.Property(p => p.Answer)
                .IsRequired();
        }
    } 
}
