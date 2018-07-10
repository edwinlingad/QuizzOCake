using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class QAAnswerModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public int QuestionId { get; set; }
    }

    public class QAQuestionModel : QzEditorModel, IQuestion
    {
        public QAQuestionModel()
        {
            Answers = new List<QAAnswerModel>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public bool AnswersInOrder { get; set; }
        public bool IsMultiplePoints { get; set; }
        public int AuthorId { get; set; }
        public int TestId { get; set; }

        public IList<QAAnswerModel> Answers;
    }
}