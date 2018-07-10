using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.Entities.Enums;
using L2L.Entities;
using L2L.WebApi.Models;
using Newtonsoft.Json;
using System.Data.Entity;
using L2L.WebApi.Utilities;
using L2L.WebApi.Enums;

namespace L2L.WebApi.Services
{
    public class QuestionTypeService : BaseService
    {
        public QuestionTypeService(BaseApiController controller)
            : base(controller)
        {
        }

        public Test GetTestEntity(int id)
        {
            var entity = _uow.Tests.GetAll()
                .Include(t => t.Questions)
                .Include(t => t.QandAQuestions)
                .Include(t => t.MultiChoiceQuestions)
                .Include(t => t.MultiChoiceSameQuestions)
                .Include(t => t.MultiChoiceSameChoiceGroups.Select(cg => cg.Choices))
                .Include(t => t.DefaultSetting)
                //.Include(t => t.Quizz)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            return entity;
        }

        public Test GetTestEntityWithAnswers(int id)
        {
            var entity = _uow.Tests.GetAll()
                .Include(t => t.Questions)
                .Include(t => t.QandAQuestions.Select(q => q.Answers))
                .Include(t => t.MultiChoiceQuestions.Select(q => q.Choices))
                .Include(t => t.MultiChoiceSameQuestions)
                .Include(t => t.MultiChoiceSameChoiceGroups.Select(cg => cg.Choices))
                .Include(t => t.DefaultSetting)
                .Where(t => t.Id == id)
                .FirstOrDefault();

            return entity;
        }

        #region DeleteQuestion
        //private Dictionary<QuestionTypeEnum, Func<int, bool, bool>> deleteQuestionMap;

        //private void InitializeDeleteQuestionMap()
        //{
        //    deleteQuestionMap = new Dictionary<QuestionTypeEnum, Func<int, bool, bool>>()
        //    {
        //        {QuestionTypeEnum.QandA, DeleteQandAQuestion},
        //        {QuestionTypeEnum.MultipleChoice, DeleteMultipleChoiceQuestion}
        //    };
        //}

        public bool DeleteQuestion(QuestionTypeEnum type, int id, bool callSaveChanges = true)
        {
            IQuestionType qType = _questionTypeFactory.GetQuestion(type);
            if (qType != null)
            {
                qType.DeleteQuestion(id, callSaveChanges);
            }
            return false;
        }

        //private bool DeleteQandAQuestion(int id, bool callSaveChanges = true)
        //{
        //    return _qaQuestionSvc.DeleteQuestion(id, callSaveChanges);
        //}

        //private bool DeleteMultipleChoiceQuestion(int id, bool callSaveChanges = true)
        //{
        //    return _mcQuestionSvc.DeleteQuestion(id, callSaveChanges);
        //}
        #endregion

        #region CreateQuestion
        public QuestionModel CreateQuestion(QuestionTypeEnum type, object model)
        {
            IQuestionType qType = _questionTypeFactory.GetQuestion(type);
            if (qType != null)
            {
                return qType.CreateQuestion(model);
            }
            return null;
        }
        #endregion

        #region GetQuestionString

        // TODO: optimize
        public string GetQuestionString(QuestionTypeEnum type, int id, Test entity)
        {
            string questionStr = String.Empty;
            IEnumerable<IQuestion> list = null;
            switch (type)
            {
                case QuestionTypeEnum.BuiltIn:
                    break;
                case QuestionTypeEnum.QandA:
                    list = entity.QandAQuestions;
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    list = entity.MultiChoiceQuestions;
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    list = entity.MultiChoiceSameQuestions;
                    break;
                default:
                    break;
            }
            questionStr = GetQuestionString(id, list);
            return questionStr;
        }

        private string GetQuestionString(int id, IEnumerable<IQuestion> list)
        {
            var entity = list.SingleOrDefault(q => q.Id == id);
            if (entity != null)
            {
                var qzEditorModel = entity as QzEditor;
                return qzEditorModel.TextContent;
                //return entity.Question;
            }
            return string.Empty;
        }

        #endregion

        #region GetReviewerFromQuestionList

        public IList<ReviewerFromQuestionModel> GetReviewerFromQuestionList(int testId)
        {
            var list = new List<ReviewerFromQuestionModel>();
            var test = GetTestEntityWithAnswers(testId);

            foreach (var item in test.Questions)
            {
                if (item.IsFlashCard == false)
                    continue;

                var question = GetQuestionEntity(item, test);
                ReviewerFromQuestionModel reviewer = null;
                if (question != null)
                {
                    reviewer = CreateReviewerFromQuestionModel(item.QuestionType, question, test);
                    if (reviewer != null)
                    {
                        list.Add(reviewer);
                    }
                }
            }

            return list;
        }

        public IList<ReviewerFromQuestionModel> GetReviewerFromTakeTestModel(TakeTestModel tTest)
        {
            try
            {
                var list = new List<ReviewerFromQuestionModel>();
                Test test;
                QuestionViewTypeEnum viewType = tTest.Questions.FirstOrDefault().QuestionViewType;

                tTest.MapToNew<TakeTestModel, Test>(out test);

                foreach (var item in test.Questions)
                {
                    var question = GetQuestionEntity(item, test);
                    ReviewerFromQuestionModel reviewer = null;
                    if (question != null)
                    {
                        reviewer = CreateReviewerFromQuestionModel(item.QuestionType, question, test);
                        if (reviewer != null)
                        {
                            reviewer.QuestionViewType = viewType;
                            list.Add(reviewer);
                        }
                    }
                }

                return list;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private IQuestion GetQuestionEntity(Question question, Test test)
        {
            IEnumerable<IQuestion> list = new List<IQuestion>();
            switch (question.QuestionType)
            {
                case QuestionTypeEnum.BuiltIn:
                    break;
                case QuestionTypeEnum.QandA:
                    list = test.QandAQuestions;
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    list = test.MultiChoiceQuestions;
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    list = test.MultiChoiceSameQuestions;
                    break;
                default:
                    break;
            }

            var actualQuestion = list.Where(q => q.Id == question.QuestionId)
                .FirstOrDefault();

            return actualQuestion;
        }

        private ReviewerFromQuestionModel CreateReviewerFromQuestionModel(QuestionTypeEnum type, IQuestion actualQuestion, Test test)
        {
            ReviewerFromQuestionModel reviewer = null;
            switch (type)
            {
                case QuestionTypeEnum.BuiltIn:
                    break;
                case QuestionTypeEnum.QandA:
                    reviewer = CreateReviewerFromQuestionModelQandA(actualQuestion, test);
                    break;
                case QuestionTypeEnum.MultipleChoice:
                    reviewer = CreateReviewerFromQuestionModelMultiChoice(actualQuestion, test);
                    break;
                case QuestionTypeEnum.MultiChoiceSame:
                    reviewer = CreateReviewerFromQuestionModelMultiChoiceSame(actualQuestion, test);
                    break;
                default:
                    break;
            }

            return reviewer;
        }

        private ReviewerFromQuestionModel CreateReviewerFromQuestionModelQandA(IQuestion actualQuestion, Test test)
        {
            ReviewerFromQuestionModel reviewer = null;
            QandAQuestion qaQuestion = actualQuestion as QandAQuestion;
            if (qaQuestion != null)
            {
                reviewer = new ReviewerFromQuestionModel()
                {
                    QuestionId = actualQuestion.Id,
                    Question = qaQuestion.Question,

                    Title = qaQuestion.Title,
                    TextContent = qaQuestion.TextContent,
                    AddContentType = qaQuestion.AddContentType,
                    ImageUrl = qaQuestion.ImageUrl,
                    DrawingContent = qaQuestion.DrawingContent,
                    ExternalUrl = qaQuestion.ExternalUrl,

                    Answers = new List<ReviewerQuestionAnswer>()
                };

                foreach (var item in qaQuestion.Answers)
                {
                    var answer = new ReviewerQuestionAnswer
                    {
                        Answer = item.Answer
                    };

                    reviewer.Answers.Add(answer);
                }
            }

            return reviewer;
        }

        private ReviewerFromQuestionModel CreateReviewerFromQuestionModelMultiChoice(IQuestion actualQuestion, Test test)
        {
            ReviewerFromQuestionModel reviewer = null;
            MultipleChoiceQuestion mcQuestion = actualQuestion as MultipleChoiceQuestion;
            if (mcQuestion != null)
            {
                reviewer = new ReviewerFromQuestionModel()
                {
                    QuestionId = actualQuestion.Id,
                    Question = mcQuestion.Question,

                    Title = mcQuestion.Title,
                    TextContent = mcQuestion.TextContent,
                    AddContentType = mcQuestion.AddContentType,
                    ImageUrl = mcQuestion.ImageUrl,
                    DrawingContent = mcQuestion.DrawingContent,
                    ExternalUrl = mcQuestion.ExternalUrl,

                    Answers = new List<ReviewerQuestionAnswer>()
                };

                foreach (var item in mcQuestion.Choices)
                {
                    if (item.IsAnswer == true)
                    {
                        var answer = new ReviewerQuestionAnswer
                        {
                            Answer = item.Value
                        };

                        reviewer.Answers.Add(answer);
                    }
                }
            }

            return reviewer;
        }

        private ReviewerFromQuestionModel CreateReviewerFromQuestionModelMultiChoiceSame(IQuestion actualQuestion, Test test)
        {
            ReviewerFromQuestionModel reviewer = null;
            MultiChoiceSameQuestion mcsQuestion = actualQuestion as MultiChoiceSameQuestion;
            if (mcsQuestion != null)
            {
                reviewer = new ReviewerFromQuestionModel()
                {
                    QuestionId = actualQuestion.Id,
                    Question = mcsQuestion.Question,

                    Title = mcsQuestion.Title,
                    TextContent = mcsQuestion.TextContent,
                    AddContentType = mcsQuestion.AddContentType,
                    ImageUrl = mcsQuestion.ImageUrl,
                    DrawingContent = mcsQuestion.DrawingContent,
                    ExternalUrl = mcsQuestion.ExternalUrl,

                    Answers = new List<ReviewerQuestionAnswer>()
                };

                MultiChoiceSameChoiceGroup choiceGroup = test.MultiChoiceSameChoiceGroups.Single(cg => cg.Id == mcsQuestion.ChoiceGroupId);
                var answers = mcsQuestion.Answers.Split(',');

                if (choiceGroup != null)
                {
                    foreach (var item in answers)
                    {
                        int answerId;
                        if (Int32.TryParse(item, out answerId))
                        {
                            var choice = choiceGroup.Choices.SingleOrDefault(c => c.Id == answerId);
                            if (choice != null)
                            {
                                var answer = new ReviewerQuestionAnswer
                                {
                                    Answer = choice.Value
                                };

                                reviewer.Answers.Add(answer);
                            }
                        }
                    }
                }
            }

            return reviewer;
        }
        #endregion

        private QuestionTypeFactory __questionTypeFactory;
        public QuestionTypeFactory _questionTypeFactory
        {
            get
            {
                if (__questionTypeFactory == null)
                    __questionTypeFactory = new QuestionTypeFactory(_controller);
                return __questionTypeFactory;
            }
        }

        private QAQuestionService __qaQuestionSvc;
        private QAQuestionService _qaQuestionSvc
        {
            get
            {
                if (__qaQuestionSvc == null)
                    __qaQuestionSvc = _svcContainer.QAQuestionSvc;
                return __qaQuestionSvc;
            }
        }

        private MCQuestionService __mcQuestionSvc;
        private MCQuestionService _mcQuestionSvc
        {
            get
            {
                if (__mcQuestionSvc == null)
                    __mcQuestionSvc = _svcContainer.MCQuestionSvc;
                return __mcQuestionSvc;
            }
        }
    }
}