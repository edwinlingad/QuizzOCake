using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class DailyRewardModel
    {
        public int DailyRewardId { get; set; }
        public int DailyRewardPerUserId { get; set; }
        public bool IsTaken { get; set; }
        public int Points { get; set; }
        public int AvailablePoints { get; set; }
    }
}