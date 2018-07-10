using L2L.Entities;
using L2L.Entities.Enums;
using L2L.WebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{

    public class TextFlashCardModel : QzEditorModel
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        public string Answer { get; set; }
        public int ReviewerId { get; set; }
        public int OwnerId { get; set; }
    }

    public class QuickNoteModel : QzEditorModel
    {
        public int Id { get; set; }
        //public string Title { get; set; }
        public string Notes { get; set; }
        public int ReviewerId { get; set; }
        public int OwnerId { get; set; }
    }

    public class QuickNoteSimpleModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReviewerId { get; set; }
        public int OwnerId { get; set; }
    }

    public class ReviewerQuestionAnswer
    {
        public string Answer { get; set; }
    }

    public class ReviewerFromQuestionModel: QzEditorModel
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public QuestionViewTypeEnum QuestionViewType { get; set; }
        public IList<ReviewerQuestionAnswer> Answers { get; set; }
    }
}