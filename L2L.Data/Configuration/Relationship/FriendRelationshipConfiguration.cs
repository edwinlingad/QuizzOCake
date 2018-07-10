using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class FriendRequestConfiguration : EntityTypeConfiguration<FriendRequest>
    {
        public FriendRequestConfiguration()
        {
            this.Property(p => p.Message)
                .HasMaxLength(Constants.MessageMaxLength);

            this.HasRequired(p => p.RequestTo);
            this.HasRequired(p => p.RequestFrom);
        }
    }

    class FriendRelationshipConfiguration : EntityTypeConfiguration<FriendRelationship>
    {
        public FriendRelationshipConfiguration()
        {
            this.HasRequired(p => p.User1);
            this.HasRequired(p => p.User2);
        }
    }
}
