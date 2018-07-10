using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class DependentPermissionConfiguration : EntityTypeConfiguration<DependentPermission>
    {
        public DependentPermissionConfiguration()
        {
            this.HasRequired(p => p.User);
        }
    } 
}
