using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class PublicGroupMembershipRequestConfiguration : EntityTypeConfiguration<PublicGroupMembershipRequest>
    {
        public PublicGroupMembershipRequestConfiguration()
        {
            this.HasRequired(p => p.PublicGroup);
            this.HasRequired(p => p.User);
        }
    } 
}
