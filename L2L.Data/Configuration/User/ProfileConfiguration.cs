using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class ProfileConfiguration : EntityTypeConfiguration<Profile>
    {
        public ProfileConfiguration()
        {
            //this.HasMany(p => p.Users)
            //    .WithRequired(q => q.Profile);

            this.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.NameMaxLength);

            this.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(Constants.NameMaxLength);
        }
    }
}
