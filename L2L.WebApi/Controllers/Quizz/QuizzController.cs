using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Services;
using L2L.Entities;
using L2L.WebApi.Models;
using L2L.Entities.Enums;
using L2L.WebApi.Enums;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/quizz")]
    public class QuizzController : BaseApiController
    {
        // return QuizzModel
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _quizzSvc.GetQuizzById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }
      
        public HttpResponseMessage Post([FromBody]QuizzModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quizzSvc.CreateQuizz(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        // PATCH: api/Quizz/5
        public HttpResponseMessage Patch([FromBody]QuizzModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quizzSvc.UpdateQuizz(model) == false)
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
                if (_quizzSvc.DeleteQuizz(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        // Makes the quizz available/unavailable to the public
        [Route("live")]
        public HttpResponseMessage Patch([FromBody]ToggleModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quizzSvc.SetLive(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuizzService __quizzSvc;
        public QuizzService _quizzSvc
        {
            get
            {
                if (__quizzSvc == null)
                    __quizzSvc = SvcContainer.QuizzSvc;
                return __quizzSvc;
            }
        }
    }
}
