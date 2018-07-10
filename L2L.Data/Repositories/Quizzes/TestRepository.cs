using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class TestRepository : GenericRepository<Test>
    {
        public TestRepository(DbContext context) : base(context) { }
    }

    public class TestSnapshotRepository : GenericRepository<TestSnapshot>
    {
        public TestSnapshotRepository(DbContext context) : base(context) { }
    } 
}
