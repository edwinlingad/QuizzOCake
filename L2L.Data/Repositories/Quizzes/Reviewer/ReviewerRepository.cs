using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class ReviewerRepository : GenericRepository<Reviewer>
    {
        public ReviewerRepository(DbContext context) : base(context) { }
    }

    public class QuickNoteRepository : GenericRepository<QuickNote>
    {
        public QuickNoteRepository(DbContext context) : base(context) { }
    }

    public class TextFlashCardRepository : GenericRepository<TextFlashCard>
    {
        public TextFlashCardRepository(DbContext context) : base(context) { }
    } 
}
