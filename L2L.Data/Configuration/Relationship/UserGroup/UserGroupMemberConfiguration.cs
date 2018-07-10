using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class UserGroupMemberConfiguration : EntityTypeConfiguration<UserGroupMember>
    {
        public UserGroupMemberConfiguration()
        {
            this.HasRequired(p => p.UserGroup);
            this.HasRequired(p => p.Member);
        }
    } 
}
