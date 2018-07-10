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
    [RoutePrefix("api/TextFlashCards")]
    public class TextFlashCardsController : BaseApiController
    {
        [HttpGet]
        [Route("GetTextFlashCards")]
        public HttpResponseMessage GetTextFlashCards(int id)
        {
            try
            {
                var model = _textFlashCardSvc.GetTextFlashCards(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _textFlashCardSvc.GetTextFlashCardById(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]TextFlashCardModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _textFlashCardSvc.CreateTextFlashCard(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {

                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]TextFlashCardModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _textFlashCardSvc.UpdateTextFlashCard(model) == false)
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
                if (_textFlashCardSvc.DeleteTextFlashCard(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private TextFlashCardService __textFlashCardSvc;
        private TextFlashCardService _textFlashCardSvc
        {
            get
            {
                if (__textFlashCardSvc == null)
                    __textFlashCardSvc = SvcContainer.TextFlashCardSvc;
                return __textFlashCardSvc;
            }
        }
    }
}
