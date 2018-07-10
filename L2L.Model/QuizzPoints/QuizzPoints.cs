using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public enum DailyRewardLevel
    {
        KBelow,
        Grade1To3,
        Grade4To6,
        Grade7to9,
        Grade10To12,
        CollegeAndProf
    }

    public class DailyReward
    {
        public int Id { get; set; }
        public DateTime ForDate { get; set; }
        public DailyRewardLevel Level { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }

        // Navigation Properties
        public virtual Quizz Quizz { get; set; }
        public virtual IList<DailyRewardPerUser> DailyRewardPerUsers { get; set; }
    }

    public class DailyRewardPerUser
    {
        public int Id { get; set; }
        public bool IsTaken { get; set; }
        public int Points { get; set; }

        // Foreign Keys
        public int TestLogId { get; set; }
        public int DailyRewardId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public virtual TestLog TestLog { get; set; }
        public virtual DailyReward DailyReward { get; set; }
        public virtual User User { get; set; }

    } 
}
