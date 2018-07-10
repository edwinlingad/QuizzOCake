using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class QuizzCategoryModel
    {
        public int Id { get; set; }
        public int QuizzCategoryType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconStrValue { get; set; }

        public string IconColor { get; set; }
        public string BorderColor { get; set; }
        public string TextColor { get; set; }
        public bool IsIncludedInDailyReward { get; set; }

        public int QuizzCount { get; set; }

    }
}