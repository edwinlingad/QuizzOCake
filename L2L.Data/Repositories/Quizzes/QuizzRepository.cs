using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class QuizzCategoryRepository : GenericRepository<QuizzCategory>
    {
        public QuizzCategoryRepository(DbContext context) : base(context) { }
    } 

    public class QuizzRepository : GenericRepository<Quizz>
    {
        public QuizzRepository(DbContext context) : base(context) { }
    }

    public class QuizzCommentRepository : GenericRepository<QuizzComment>
    {
        public QuizzCommentRepository(DbContext context) : base(context) { }
    }

    public class QuizzCommentLikeRepository : GenericRepository<QuizzCommentLike>
    {
        public QuizzCommentLikeRepository(DbContext context) : base(context) { }
    }

    public class QuizzCommentFlagRepository : GenericRepository<QuizzCommentFlag>
    {
        public QuizzCommentFlagRepository(DbContext context) : base(context) { }
    } 
}
