using L2L.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Enums;

namespace L2L.WebApi.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public QuestionTypeEnum QuestionType { get; set; }
        public int Order { get; set; }
        public bool IsFlashCard { get; set; }

        public int TestId { get; set; }

        public string Question { get; set; }

        public QuestionViewTypeEnum QuestionViewType { get; set; }
    }
}