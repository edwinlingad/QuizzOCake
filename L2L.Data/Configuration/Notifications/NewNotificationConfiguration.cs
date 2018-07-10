using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace L2L.Data.Configuration
{
    class NewNotificationConfiguration : EntityTypeConfiguration<NewNotification>
    {
        public NewNotificationConfiguration()
        {
            this.HasRequired(p => p.ToUser);
            this.HasRequired(p => p.FromUser);
            this.HasRequired(p => p.Quizz);
            this.HasRequired(p => p.QuizzComment);
            this.HasRequired(p => p.Question);
            this.HasRequired(p => p.Assignment);
            this.HasRequired(p => p.AssignmentGroup);
            this.HasOptional(p => p.FriendRequest);
            this.HasOptional(p => p.ToUnQuizzmate);
        }
    } 
}
