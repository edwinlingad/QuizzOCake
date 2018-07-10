using L2L.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace L2L.WebApi.Controllers.Notification
{
    [Authorize]
    public class NotificationTypesController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                var list = SvcContainer.NotificationTypesSvc.GetNotificationTypes();
                if(list == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }
    }
}
