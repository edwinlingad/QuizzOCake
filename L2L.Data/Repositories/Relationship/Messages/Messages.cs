using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class QuizzmateMsgThreadRepository : GenericRepository<QuizzmateMsgThread>
    {
        public QuizzmateMsgThreadRepository(DbContext context) : base(context) { }
    }

    public class QuizzmateMsgThreadMemberRepository : GenericRepository<QuizzmateMsgThreadMember>
    {
        public QuizzmateMsgThreadMemberRepository(DbContext context) : base(context) { }
    }

    public class QuizzmateMsg1Repository : GenericRepository<QuizzmateMsg1>
    {
        public QuizzmateMsg1Repository(DbContext context) : base(context) { }
    } 
}
