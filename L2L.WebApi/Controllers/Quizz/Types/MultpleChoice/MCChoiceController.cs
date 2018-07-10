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
    public class MCChoiceController : BaseApiController
    {
        // POST: api/MCChoice
        public HttpResponseMessage Post([FromBody]MChoiceModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _mcChoiceSvc.CreateChoice(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {

                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]MChoiceModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _mcChoiceSvc.UpdateChoice(model) == false)
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
                if (_mcChoiceSvc.DeleteChoice(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private MCChoiceService __mcChoiceSvc;
        private MCChoiceService _mcChoiceSvc
        {
            get
            {
                if (__mcChoiceSvc == null)
                    __mcChoiceSvc = SvcContainer.MCChoiceSvc;
                return __mcChoiceSvc;
            }
        }
    }
}
