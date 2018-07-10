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
    public class MultiChoiceSameChoiceGroupController : BaseApiController
    {
        public HttpResponseMessage Post([FromBody]MultiChoiceSameChoiceGroupModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _multiChoiceSameChoiceGroupSvc.CreateChoiceGroup(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {

                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]MultiChoiceSameChoiceGroupModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _multiChoiceSameChoiceGroupSvc.UpdateChoiceGroup(model) == false)
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
                if (_multiChoiceSameChoiceGroupSvc.DeleteChoiceGroup(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private MultiChoiceSameChoiceGroupService __multiChoiceSameChoiceGroupSvc;
        private MultiChoiceSameChoiceGroupService _multiChoiceSameChoiceGroupSvc
        {
            get
            {
                if (__multiChoiceSameChoiceGroupSvc == null)
                    __multiChoiceSameChoiceGroupSvc = SvcContainer.MultiChoiceSameChoiceGroupSvc;
                return __multiChoiceSameChoiceGroupSvc;
            }
        }
    }
}
