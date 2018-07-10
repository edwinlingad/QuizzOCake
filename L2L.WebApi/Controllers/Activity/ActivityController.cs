using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Services;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Activity")]
    public class ActivityController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetActivitiesOfCurrentUser")]
        public HttpResponseMessage GetActivitiesOfCurrentUser(int pageNum, int numPerPage, int skip = 0)
        {
            try
            {
                var model = _activitySvc.GetActivitiesOfCurrentUser(pageNum, numPerPage, skip);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetActivitiesOfUser")]
        public HttpResponseMessage GetActivitiesOfUser(int id, int pageNum, int numPerPage, int skip = 0)
        {
            try
            {
                var model = _activitySvc.GetActivitiesOfUser(id, pageNum, numPerPage, skip);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetActivitiesOfQuizzmates")]
        public HttpResponseMessage GetActivitiesOfQuizzmates(int pageNum, int numPerPage, int skip = 0)
        {
            try
            {
                var model = _activitySvc.GetActivitiesOfQuizzmates(pageNum, numPerPage, skip);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private ActivityService __activitySvc;
        private ActivityService _activitySvc
        {
            get
            {
                if (__activitySvc == null)
                    __activitySvc = SvcContainer.ActivitySvc;
                return __activitySvc;
            }
        }
    }
}
