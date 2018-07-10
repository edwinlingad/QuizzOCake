using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Utilities;
using L2L.WebApi.Services;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    public class MCQuestionController : BaseApiController
    {
        // POST and DELETE is done thru QuestionController 

        // return MCQuestionModel
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _mcQuestionSvc.GetQuestionById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]MCQuestionModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _mcQuestionSvc.UpdateQuestion(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private MCQuestionService __mcQuestionSvc;
        private MCQuestionService _mcQuestionSvc
        {
            get
            {
                if (__mcQuestionSvc == null)
                    __mcQuestionSvc = SvcContainer.MCQuestionSvc;
                return __mcQuestionSvc;
            }
        }
    }
}
