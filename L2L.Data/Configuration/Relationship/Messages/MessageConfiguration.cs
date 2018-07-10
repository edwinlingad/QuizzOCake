using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class QuizzmateMsgThreadConfiguration : EntityTypeConfiguration<QuizzmateMsgThread>
    {
        public QuizzmateMsgThreadConfiguration()
        {
            this.HasMany(p => p.MsgThreadMembers)
                .WithRequired(q => q.QuizzmateMsgThread);
            this.HasMany(p => p.Messages)
                .WithRequired(q => q.QuizzmateMsgThread);
        }
    }

    class QuizzmateMsgThreadMemberConfiguration : EntityTypeConfiguration<QuizzmateMsgThreadMember>
    {
        public QuizzmateMsgThreadMemberConfiguration()
        {
            this.HasRequired(p => p.User);
            this.HasRequired(p => p.QuizzmateMsgThread);
        }
    }

    class QuizzmateMsg1Configuration : EntityTypeConfiguration<QuizzmateMsg1>
    {
        public QuizzmateMsg1Configuration()
        {
            this.HasRequired(p => p.Author);
            this.HasRequired(p => p.QuizzmateMsgThread);
        }
    } 
}
