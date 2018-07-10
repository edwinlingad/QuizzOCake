using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities.Enums;

namespace L2L.WebApi.Models
{
    public class GradeLevelModel
    {
        public string Name { get; set; }
        public QuizzGradeLevelEnum GradeLevel { get; set; }
        public string FgColor { get; set; }
        public string BgColor { get; set; }

        public int NumQuizz { get; set; }
    }
}