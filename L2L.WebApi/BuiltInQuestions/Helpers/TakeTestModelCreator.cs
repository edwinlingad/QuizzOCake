using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Models;
using L2L.Entities.Enums;
using L2L.WebApi.Enums;

namespace L2L.WebApi.BuiltInQuestions.Helpers
{
    public class TakeTestModelCreator
    {
        private int _qIdx = 0;
        private TakeTestModel _model;

        public TakeTestModelCreator()
        {
            _model = new TakeTestModel();
        }

        public TakeTestModel GetTest()
        {
            return _model;
        }

        public void AddQandAQuestion(QAQuestionModel actualQuestion, QuestionViewTypeEnum questionViewType = QuestionViewTypeEnum.Tinymce)
        {
            var question = new QuestionModel()
            {
                Id = _qIdx,
                QuestionType = QuestionTypeEnum.QandA,
                QuestionId = _qIdx,
                QuestionViewType = questionViewType
            };

            actualQuestion.Id = _qIdx;
            _model.Questions.Add(question);
            _model.QandAQuestions.Add(actualQuestion);

            _qIdx++;
        }

        public void AddMultiChoiceQuestion(MCQuestionModel actualQuestion, QuestionViewTypeEnum questionViewType = QuestionViewTypeEnum.Tinymce)
        {
            var question = new QuestionModel()
            {
                Id = _qIdx,
                QuestionType = QuestionTypeEnum.MultipleChoice,
                QuestionId = _qIdx,
                QuestionViewType = questionViewType
            };

            actualQuestion.Id = _qIdx;
            _model.Questions.Add(question);
            _model.MultiChoiceQuestions.Add(actualQuestion);

            _qIdx++;
        }

        public void AddMultiChoiceSameQuestion(MultiChoiceSameQuestionModel actualQuestion, QuestionViewTypeEnum questionViewType = QuestionViewTypeEnum.Tinymce)
        {
            var question = new QuestionModel()
            {
                Id = _qIdx,
                QuestionType = QuestionTypeEnum.MultiChoiceSame,
                QuestionId = _qIdx,
                QuestionViewType = questionViewType
            };

            actualQuestion.Id = _qIdx;
            _model.Questions.Add(question);
            _model.MultiChoiceSameQuestions.Add(actualQuestion);

            _qIdx++;
        }

        
    }
}