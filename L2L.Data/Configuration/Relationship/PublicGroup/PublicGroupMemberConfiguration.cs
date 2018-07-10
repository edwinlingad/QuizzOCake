using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class PublicGroupMemberConfiguration : EntityTypeConfiguration<PublicGroupMember>
    {
        public PublicGroupMemberConfiguration()
        {
            this.HasRequired(p => p.Member);
            this.HasRequired(p => p.PublicGroup);
        }
    } 
}
