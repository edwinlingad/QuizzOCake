using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DbContext context) : base(context) { }

        public User GetUser(int id)
        {
            return DBSet.Where(u => u.Id == id)
                .Include(u => u.Profile)
                .FirstOrDefault();
        }
    }

    public class DependentPermissionRepository : GenericRepository<DependentPermission>
    {
        public DependentPermissionRepository(DbContext context) : base(context) { }
    } 
}
