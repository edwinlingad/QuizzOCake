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
    public class MultiChoiceSameChoiceController : BaseApiController
    {
        public HttpResponseMessage Post([FromBody]MultiChoiceSameChoiceModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _multiChoiceSameChoiceSvc.CreateChoice(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {

                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]MultiChoiceSameChoiceModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _multiChoiceSameChoiceSvc.UpdateChoice(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        // DELETE: api/MultiChoiceSameChoice/5
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (_multiChoiceSameChoiceSvc.DeleteChoice(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private MultiChoiceSameChoiceService __multiChoiceSameChoiceSvc;
        private MultiChoiceSameChoiceService _multiChoiceSameChoiceSvc
        {
            get
            {
                if (__multiChoiceSameChoiceSvc == null)
                    __multiChoiceSameChoiceSvc = SvcContainer.MultiChoiceSameChoiceSvc;
                return __multiChoiceSameChoiceSvc;
            }
        }
    }
}
