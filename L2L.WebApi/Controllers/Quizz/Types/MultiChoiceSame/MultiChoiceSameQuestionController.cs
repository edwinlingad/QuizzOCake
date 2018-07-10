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
    public class MultiChoiceSameQuestionController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _multiChoiceSameQuestionSvc.GetQuestionById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]MultiChoiceSameQuestionModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _multiChoiceSameQuestionSvc.UpdateQuestion(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private MultiChoiceSameQuestionService __multiChoiceSameQuestionSvc;
        private MultiChoiceSameQuestionService _multiChoiceSameQuestionSvc
        {
            get
            {
                if (__multiChoiceSameQuestionSvc == null)
                    __multiChoiceSameQuestionSvc = SvcContainer.MultiChoiceSameQuestionSvc;
                return __multiChoiceSameQuestionSvc;
            }
        }
    }
}
