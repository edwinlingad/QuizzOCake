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
    public class QAAnswerController : BaseApiController
    {
        // POST: api/QAAnswer
        public HttpResponseMessage Post([FromBody]QAAnswerModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _qaAnswerSvc.CreateAnswer(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {
                
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]QAAnswerModel model)
        {
            try
            {
               if (ModelState.IsValid == false || _qaAnswerSvc.UpdateAnswer(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK, model);
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
                if (_qaAnswerSvc.DeleteAnswer(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QAAnswerService __qaAnswerSvc;
        private QAAnswerService _qaAnswerSvc
        {
            get
            {
                if (__qaAnswerSvc == null)
                    __qaAnswerSvc = SvcContainer.QAAnswerSvc;
                return __qaAnswerSvc;
            }
        }

    }
}
