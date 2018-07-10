using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity.ModelConfiguration;

namespace L2L.Data.Configuration
{
    class PublicGroupConfiguration : EntityTypeConfiguration<PublicGroup>
    {
        public PublicGroupConfiguration()
        {
            this.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Constants.NameMaxLength);

            this.HasRequired(p => p.Creator);
        }
    } 
}
