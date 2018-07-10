using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity;

namespace L2L.Data.Repositories
{
    public class AssignmentGroupRepository : GenericRepository<AssignmentGroup>
    {
        public AssignmentGroupRepository(DbContext context) : base(context) { }
    } 

    public class AssignmentRepository : GenericRepository<Assignment>
    {
        public AssignmentRepository(DbContext context) : base(context) { }
    } 
}
