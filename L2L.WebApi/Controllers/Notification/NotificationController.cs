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
    [RoutePrefix("api/Notification")]
    public class NotificationController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("GetNewNotificationsCount")]
        public HttpResponseMessage GetNewNotificationsCount()
        {
            try
            {
                var model = _notificationSvc.GetNewNotificationCount();

                return Request.CreateResponse(HttpStatusCode.OK, new { count = model });
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetNewNotifications")]
        public HttpResponseMessage GetNewNotifications()
        {
            try
            {
                var model = _notificationSvc.GetNewNotifications();
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
        [Route("GetAllNotificationsCount")]
        public HttpResponseMessage GetAllNotificationsCount()
        {
            try
            {
                var model = _notificationSvc.GetAllNotificationsCount();

                return Request.CreateResponse(HttpStatusCode.OK, new { count = model });
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetNotifications")]
        public HttpResponseMessage GetNotifications(int pageNum, int numPerPage)
        {
            try
            {
                var model = _notificationSvc.GetNotifications(pageNum, numPerPage);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private NotificationService __notificationSvc;
        private NotificationService _notificationSvc
        {
            get
            {
                if (__notificationSvc == null)
                    __notificationSvc = SvcContainer.NotificationSvc;
                return __notificationSvc;
            }
        }
    }
}
