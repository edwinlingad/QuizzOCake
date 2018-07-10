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
    public class TestSettingController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _testSettingSvc.GetTestSetting(id);
                if ( model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]TestSettingModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _testSettingSvc.CreateTestSetting(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]TestSettingModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _testSettingSvc.UpdateTestSetting(model) == false)
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
                if (_testSettingSvc.DeleteTestSetting(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private TestSettingService __testSettingSvc;
        private TestSettingService _testSettingSvc
        {
            get
            {
                if (__testSettingSvc == null)
                    __testSettingSvc = SvcContainer.TestSettingSvc;
                return __testSettingSvc;
            }
        }
    }
}
