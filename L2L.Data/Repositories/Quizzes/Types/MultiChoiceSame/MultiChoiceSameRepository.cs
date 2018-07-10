using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L2L.Entities;
using System.Data.Entity;


namespace L2L.Data.Repositories
{
    public class MultiChoiceSameQuestionRepository : GenericRepository<MultiChoiceSameQuestion>
    {
        public MultiChoiceSameQuestionRepository(DbContext context) : base(context) { }
    }

    public class MultiChoiceSameChoiceGroupRepository : GenericRepository<MultiChoiceSameChoiceGroup>
    {
        public MultiChoiceSameChoiceGroupRepository(DbContext context) : base(context) { }
    }

    public class MultiChoiceSameChoiceRepository : GenericRepository<MultiChoiceSameChoice>
    {
        public MultiChoiceSameChoiceRepository(DbContext context) : base(context) { }
    }
}
