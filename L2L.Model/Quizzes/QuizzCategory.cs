using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryValue { get; set; }
        public string IconStrValue { get; set; }

        public string IconColor { get; set; }
        public string BorderColor { get; set; }
        public string TextColor { get; set; }

        public bool IsIncludedInDailyReward { get; set; }
    } 
}
