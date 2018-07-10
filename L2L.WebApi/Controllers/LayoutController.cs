using L2L.WebApi.Models;
using L2L.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    public class LayoutController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get()
        {
            try
            {
                var model = SvcContainer.LayoutSvc.GetLayoutModel();
                if(model == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }
    }
}
