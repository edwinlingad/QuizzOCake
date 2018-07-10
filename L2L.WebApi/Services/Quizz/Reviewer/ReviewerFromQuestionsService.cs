using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.Entities.Enums;
using L2L.WebApi.Controllers;

namespace L2L.WebApi.Services
{
    public class ReviewerFromQuestionsService : BaseService, IResource
    {
        public ReviewerFromQuestionsService(BaseApiController controller)
            : base(controller)
        {
        }

        public IList<ReviewerFromQuestionModel> GetReviewerFromQuestions(int testId)
        {
            try
            {
                IList<ReviewerFromQuestionModel> list = null;

                var testEntity = _questionTypeSvc.GetTestEntityWithAnswers(testId);
                if (testEntity != null)
                {
                    list = _questionTypeSvc.GetReviewerFromQuestionList(testId);
                }

                var test = _uow.Tests.GetAll()
                    .Where(t => t.Id == testId)
                    .Include(t => t.Quizz.Reviewers)
                    .FirstOrDefault();

                if (test != null)
                {
                    var tfcList = _textFlashCardSvc.GetTextFlashCards(test.Quizz.Reviewers.FirstOrDefault().Id);
                    foreach (var item in tfcList)
                    {
                        var reviewer = new ReviewerFromQuestionModel
                        {
                            Question = item.Title,
                            Title = item.Title,
                            TextContent = item.TextContent,
                            AddContentType = item.AddContentType,
                            ImageUrl = item.ImageUrl,
                            DrawingContent = item.DrawingContent,
                            ExternalUrl = item.ExternalUrl,

                            Answers = new List<ReviewerQuestionAnswer>()
                            {
                                new ReviewerQuestionAnswer
                                {
                                    Answer = item.Answer
                                }
                            }
                        };

                        list.Add(reviewer);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object GetMany(int id, int id2, int id3, int id4, int id5)
        {
            throw new NotImplementedException();
        }

        public object GetManyAlt(int id, int id2, int id3, string str, string str2, string str3)
        {
            throw new NotImplementedException();
        }

        public object GetAlt(string str)
        {
            throw new NotImplementedException();
        }

        public object Get(int id)
        {
            try
            {
                var testId = id;

                var count = _uow.Questions.GetAll()
                    .Where(q => q.TestId == testId && q.IsFlashCard == true)
                    .Count();

                return new { count = count };
            }
            catch (Exception ex)
            {
                _svcContainer.LoggingSvc.Log(ex);
                return null;
            }
        }

        public object Post(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Patch(object modelParam)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private QuestionTypeService __questionTypeSvc;
        private QuestionTypeService _questionTypeSvc
        {
            get
            {
                if (__questionTypeSvc == null)
                    __questionTypeSvc = _svcContainer.QuestionTypeSvc;
                return __questionTypeSvc;
            }
        }

        private TextFlashCardService __textFlashCardSvc;
        private TextFlashCardService _textFlashCardSvc
        {
            get
            {
                if (__textFlashCardSvc == null)
                    __textFlashCardSvc = _svcContainer.TextFlashCardSvc;
                return __textFlashCardSvc;
            }
        }
    }
}