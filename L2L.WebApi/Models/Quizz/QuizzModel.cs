using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class QuizzModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsLive { get; set; }
        public QuizzVisibilityEnum Visibility { get; set; }
        public int ModifyPermission { get; set; }

        public bool IsBuiltIn { get; set; }
        public BuiltInTypeEnum BuiltInType { get; set; }
        public int MainType { get; set; }
        public int SubType { get; set; }
        public int SubType2 { get; set; }
        public int SubType3 { get; set; }
        public int SubType4 { get; set; }
        public int SubType5 { get; set; }
        public string QuizzTags { get; set; }

        public int OwnerId { get; set; }
        public int ReviewerId { get; set; }
        public int TestId { get; set; }

        public TestSettingModel DefaultTestSetting { get; set; }

        public int Category { get; set; }
        public QuizzDifficultyEnum Difficulty { get; set; }
        public QuizzGradeLevelEnum GradeLevelMin { get; set; }
        public QuizzGradeLevelEnum GradeLevelMax { get; set; }

        public DailyRewardModel DailyReward { get; set; }
    }
}