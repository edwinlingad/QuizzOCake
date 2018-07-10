using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class TestModel
    {
        public TestModel()
        {
            Questions = new List<QuestionModel>();
        }

        public int Id { get; set; }
        public int QuizzId { get; set; }
        public int DefaultSettingId { get; set; }

        public QuizzModel Quizz { get; set; }
        public IList<QuestionModel> Questions { get; set; }
        public TestSettingModel DefaultSetting { get; set; }
        public IList<MultiChoiceSameChoiceGroupModel> MultiChoiceSameChoiceGroups { get; set; }

    }

    public class TakeTestModel : TestModel
    {
        public TakeTestModel()
        {
            Questions = new List<QuestionModel>();
            QandAQuestions = new List<QAQuestionModel>();
            MultiChoiceQuestions = new List<MCQuestionModel>();
            MultiChoiceSameQuestions = new List<MultiChoiceSameQuestionModel>();
        }

        public IList<QAQuestionModel> QandAQuestions { get; set; }
        public IList<MCQuestionModel> MultiChoiceQuestions { get; set; }
        public IList<MultiChoiceSameQuestionModel> MultiChoiceSameQuestions { get; set; }
    }
}