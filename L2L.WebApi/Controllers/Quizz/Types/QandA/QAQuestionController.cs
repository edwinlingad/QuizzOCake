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
    public class QAQuestionController : BaseApiController
    {
        // POST is done thru QuestionController 
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _qaQuestionSvc.GetQuestionById(id);
                if(model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]QAQuestionModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _qaQuestionSvc.UpdateQuestion(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QAQuestionService __qaQuestionSvc;
        private QAQuestionService _qaQuestionSvc
        {
            get
            {
                if (__qaQuestionSvc == null)
                    __qaQuestionSvc = SvcContainer.QAQuestionSvc;
                return __qaQuestionSvc;
            }
        }
    }
}
