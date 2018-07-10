using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Repositories
{
    public class DailyRewardRepository : GenericRepository<DailyReward>
    {
        public DailyRewardRepository(DbContext context) : base(context) { }
    }

    public class DailyRewardPerUserRepository : GenericRepository<DailyRewardPerUser>
    {
        public DailyRewardPerUserRepository(DbContext context) : base(context) { }
    } 
}
