using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class MultiChoiceSameQuestionModel : QzEditorModel, IQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answers { get; set; }
        public bool IsMultiplePoints { get; set; }

        public int TestId { get; set; }
        public int ChoiceGroupId { get; set; }
    }

    public class MultiChoiceSameChoiceGroupModel
    {
        public MultiChoiceSameChoiceGroupModel()
        {
            Choices = new List<MultiChoiceSameChoiceModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShuffleChoices { get; set; }
        public bool IsMultiplePoints { get; set; }
        public int TestId { get; set; }
        public IList<MultiChoiceSameChoiceModel> Choices { get; set; }
    }

    public class MultiChoiceSameChoiceModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ChoiceGroupId { get; set; }
    }
}