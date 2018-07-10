using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzUpvote
    {
        public int Id { get; set; }
        public int UpVote { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int QuizRatingId { get; set; }

        // Navigation Properties
        public virtual QuizzRating QuizRating { get; set; }
        public virtual User User { get; set; }
    } 
}
