using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity;

namespace L2L.Data.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>
    {
        public ProfileRepository(DbContext context) : base(context) { }
    }
}
