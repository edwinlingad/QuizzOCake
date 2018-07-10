using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class MChoiceModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Order { get; set; }
        public int QuestionId { get; set; }
        public bool IsAnswer { get; set; }
    }

    public class MCQuestionModel : QzEditorModel, IQuestion
    {
        public MCQuestionModel()
        {
            Choices = new List<MChoiceModel>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public bool IsMultiplePoints { get; set; }
        public bool ShuffleChoices { get; set; }
        public int AuthorId { get; set; }
        public int TestId { get; set; }

        public IList<MChoiceModel> Choices { get; set; }
    }
}