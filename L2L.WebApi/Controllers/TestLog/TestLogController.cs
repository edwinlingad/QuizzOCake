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
    [RoutePrefix("api/TestLog")]
    public class TestLogController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _testLogSvc.GetTestLogById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]TestLogModel model)
        {
            try
            {
                //if (ModelState.IsValid == false || _testLogSvc.CreateTestLog(model) == false)
                if (_testLogSvc.CreateTestLog(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
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
                if (_testLogSvc.DeleteTestLog(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetTestLogsGrouped")]
        public HttpResponseMessage GetTestLogsGrouped(int id)
        {
            try
            {
                var model = _testLogSvc.GetTestLogsGroupedOfUserId(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private TestLogService __testLogSvc;
        private TestLogService _testLogSvc
        {
            get
            {
                if (__testLogSvc == null)
                    __testLogSvc = SvcContainer.TestLogSvc;
                return __testLogSvc;
            }
        }
    }
}