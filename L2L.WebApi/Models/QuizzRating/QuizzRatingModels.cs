using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities;

namespace L2L.WebApi.Models
{
    public class QuizzCurrentUserRatingModel
    {
        public int QuizzRatingId { get; set; }

        public int Rating { get; set; }
        //public double RatingAvg { get; set; }
        //public int NumRatings { get; set; }
    }

    public class QuizzUserRatingModel
    {
        public int QuizzRatingId { get; set; }

        public double RatingAvg { get; set; }
        public int NumRatings { get; set; }
    }

    public class QuizzUserRatingUpdateRateModel
    {
        public int QuizzRatingId { get; set; }
        public int Rating { get; set; }
    }
}