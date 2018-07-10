using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzUserRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuizzRatingId { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual QuizzRating QuizzRating { get; set; }
    } 
}
