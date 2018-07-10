using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class MultipleChoiceQuestionRepository : GenericRepository<MultipleChoiceQuestion>
    {
        public MultipleChoiceQuestionRepository(DbContext context) : base(context) { }
    }
}
