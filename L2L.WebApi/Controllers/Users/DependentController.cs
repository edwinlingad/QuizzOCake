using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.Entities;
using L2L.WebApi.Models;
using L2L.Entities.Enums;
using L2L.WebApi.Enums;
using L2L.WebApi.Services;

namespace L2L.WebApi.Controllers.Users
{
    [Authorize]
    [RoutePrefix("api/Dependent")]
    public class DependentController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetDependentInfo")]
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _dependentSvc.GetDependentInfo(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("UpdatePermissions")]
        public HttpResponseMessage Patch([FromBody]DependentPermissionModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _dependentSvc.UpdatePermissions(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("UpdateNotifications")]
        public HttpResponseMessage Patch([FromBody]DependentNotificationModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _dependentSvc.UpdateNotifications(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private DependentService __dependentSvc;
        private DependentService _dependentSvc
        {
            get
            {
                if (__dependentSvc == null)
                    __dependentSvc = SvcContainer.DependentSvc;
                return __dependentSvc;
            }
        }
    }
}
