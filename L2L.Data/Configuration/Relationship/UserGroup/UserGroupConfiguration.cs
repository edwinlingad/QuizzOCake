using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class UserGroupConfiguration : EntityTypeConfiguration<UserGroup>
    {
        public UserGroupConfiguration()
        {
            this.Property(p => p.GroupName)
                .IsRequired()
                .HasMaxLength(Constants.NameMaxLength);

            this.HasRequired(p => p.Owner);
        }
    } 
}
