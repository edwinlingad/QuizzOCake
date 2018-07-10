using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class QuizzRating
    {
        public QuizzRating()
        {
            QuizUserRatings = new HashSet<QuizzUserRating>();
            QuizUpvotes = new HashSet<QuizzUpvote>();
        }

        public int Id { get; set; }

        // Foreign Keys
        public int QuizzId { get; set; }

        // Navigation Properties
        public virtual Quizz Quizz { get; set; }
        public virtual ICollection<QuizzUserRating> QuizUserRatings { get; set; }
        public virtual ICollection<QuizzUpvote> QuizUpvotes { get; set; }
    } 
}
