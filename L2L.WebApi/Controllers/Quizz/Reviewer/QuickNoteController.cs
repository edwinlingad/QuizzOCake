using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Utilities;
using L2L.WebApi.Services;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuickNotes")]
    public class QuickNotesController : BaseApiController
    {
        [HttpGet]
        [Route("GetQuickNotes")]
        [AllowAnonymous]
        public HttpResponseMessage GetQuickNotes(int id)
        {
            try
            {
                var model = _quickNoteSvc.GetQuickNotes(id);
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
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _quickNoteSvc.GetQuickNoteById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]QuickNoteModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quickNoteSvc.CreateQuickNote(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {

                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]QuickNoteModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quickNoteSvc.UpdateQuickNote(model) == false)
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
                if (_quickNoteSvc.DeleteQuickNote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuickNoteService __quickNoteSvc;
        private QuickNoteService _quickNoteSvc
        {
            get
            {
                if (__quickNoteSvc == null)
                    __quickNoteSvc = SvcContainer.QuickNoteSvc;
                return __quickNoteSvc;
            }
        }
    }
}
