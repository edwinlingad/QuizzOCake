using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Services;
using AutoMapper.QueryableExtensions;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    [RoutePrefix("api/BuiltInQuestions")]
    public class BuiltInQuestionsController : BaseApiController
    {
        public HttpResponseMessage Get(int id, int numQuestions)
        {
            try
            {
                var quizz = Uow.Quizzes.GetAll()
                    .Where(q => q.Id == id)
                    .ProjectTo<QuizzModel>()
                    .FirstOrDefault();

                if (quizz.IsBuiltIn == true)
                {
                    var builtInQGenerator = BuiltInTypeFactory.GetBuiltIn(quizz.BuiltInType);
                    var test = builtInQGenerator.GetTest(numQuestions, quizz);
                    if (test != null)
                    {
                        test.DefaultSetting = quizz.DefaultTestSetting;

                        return Request.CreateResponse(HttpStatusCode.OK, test);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetFlashCards")]
        public HttpResponseMessage GetFlashCards(int id, int numQuestions)
        {
            try
            {
                var quizz = Uow.Quizzes.GetAll()
                    .Where(q => q.Id == id)
                    .ProjectTo<QuizzModel>()
                    .FirstOrDefault();

                if (quizz.IsBuiltIn == true)
                {
                    var builtInQGenerator = BuiltInTypeFactory.GetBuiltIn(quizz.BuiltInType);
                    var test = builtInQGenerator.GetTest(numQuestions, quizz);
                    var list = _questionTypeSvc.GetReviewerFromTakeTestModel(test);

                    if (list != null)
                        return Request.CreateResponse(HttpStatusCode.OK, list);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuizzService __quizzSvc;
        private QuizzService _quizzSvc
        {
            get
            {
                if (__quizzSvc == null)
                    __quizzSvc = SvcContainer.QuizzSvc;
                return __quizzSvc;
            }
        }

        private QuestionTypeService __questionTypeSvc;
        private QuestionTypeService _questionTypeSvc
        {
            get
            {
                if (__questionTypeSvc == null)
                    __questionTypeSvc = SvcContainer.QuestionTypeSvc;
                return __questionTypeSvc;
            }
        }
    }
}
