using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Models;
using L2L.Entities;
using L2L.WebApi.Utilities;
using L2L.WebApi.Services;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Question")]
    public class QuestionController : BaseApiController
    {
        // For updating order only
        public HttpResponseMessage Patch([FromBody]IEnumerable<QuestionModel> models)
        {
            try
            {
                if (_questionSvc.UpdateQuestions(models) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (_questionSvc.DeleteQuestion(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]CreateQuestionModel model)
        {
            try
            {
                QuestionModel qModel = null;
                if ((qModel = _questionSvc.CreateQuestion(model.QuestionType, model.Question)) == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.Created, qModel);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("IncludeInFlashCards")]
        public HttpResponseMessage IncludeInFlashCards([FromBody]int questionId)
        {
            try
            {
                if (_questionSvc.IncludeInFlashCard(questionId) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("ExcludeInFlashCards")]
        public HttpResponseMessage ExcludeInFlashCards([FromBody]int questionId)
        {
            try
            {
                if (_questionSvc.ExcludeInFlashCard(questionId) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuestionService __questionSvc;
        private QuestionService _questionSvc
        {
            get
            {
                if (__questionSvc == null)
                    __questionSvc = SvcContainer.QuestionSvc;
                return __questionSvc;
            }
        }
    }
}
