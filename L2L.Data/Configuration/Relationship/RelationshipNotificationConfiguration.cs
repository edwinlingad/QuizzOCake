using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class RelationshipNotificationConfiguration : EntityTypeConfiguration<RelationshipNotification>
    {
        public RelationshipNotificationConfiguration()
        {
            this.HasRequired(p => p.ToUser);
            this.HasRequired(p => p.FromUser);
            this.HasOptional(p => p.FriendRequest);
            this.HasOptional(p => p.DependentRequestFromUser);
        }
    } 
}
