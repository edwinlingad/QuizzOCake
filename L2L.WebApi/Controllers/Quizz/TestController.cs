using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Services;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Test")]
    public class TestController : BaseApiController
    {
        // return TestModel
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _testSvc.GetTest(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [AllowAnonymous]
        [Route("GetTakeTestModel")]
        public HttpResponseMessage GetTakeTestModel(int id)
        {
            try
            {
                var model = _testSvc.GetTakeTestModel(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private TestService __testSvc;
        private TestService _testSvc
        {
            get
            {
                if (__testSvc == null)
                    __testSvc = SvcContainer.TestSvc;
                return __testSvc;
            }
        }
    }
}
