using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Data.Configuration
{
    class DailyRewardConfiguration : EntityTypeConfiguration<DailyReward>
    {
        public DailyRewardConfiguration()
        {
            this.HasRequired(q => q.Quizz);
            this.HasMany(q => q.DailyRewardPerUsers)
                .WithRequired(p => p.DailyReward);
        }
    }

    class DailyRewardPerUserConfiguration : EntityTypeConfiguration<DailyRewardPerUser>
    {
        public DailyRewardPerUserConfiguration()
        {
            this.HasRequired(p => p.TestLog);
            this.HasRequired(p => p.DailyReward);
            this.HasRequired(p => p.User);
        }
    }
}
