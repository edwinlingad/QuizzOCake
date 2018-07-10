using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QuestionConfiguration : EntityTypeConfiguration<Question>
    {
        public QuestionConfiguration()
        {
            this.HasRequired(p => p.Test);
            this.HasOptional(p => p.QuickNoteRef);
            //this.HasRequired(p => p.Author);
            //this.HasRequired(p => p.Author);
        }
    } 
}
