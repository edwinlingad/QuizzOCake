using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using L2L.Entities;
using L2L.Constants;

namespace L2L.Data
{
    class CustomDatabaseInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        private DataContext _context;

        protected override void Seed(DataContext context)
        {
            _context = context;
        }
    }
}
