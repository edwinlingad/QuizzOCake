using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class CreateQuestionModel
    {
        public QuestionTypeEnum QuestionType { get; set; }
        public object Question { get; set; }
     }
}