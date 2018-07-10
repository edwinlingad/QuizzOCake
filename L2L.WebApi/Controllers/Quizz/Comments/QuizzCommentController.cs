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

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuizzComment")]
    public class QuizzCommentController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get(int id, int pageNum, int numPerPage, int skip, QuizzCommentSortTypeEnum sortType)
        {
            try
            {
                var model = _quizzCommentSvc.GetQuizzComments(id, pageNum, numPerPage, skip, sortType);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post([FromBody]QuizzCommentModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quizzCommentSvc.CreateComment(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.Created, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch([FromBody]QuizzCommentModel model)
        {
            try
            {
                if (ModelState.IsValid == false || _quizzCommentSvc.UpdateComment(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK);
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
                if (_quizzCommentSvc.DeleteComment(id) == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("LikeUpVote")]
        public HttpResponseMessage LikeUpVote([FromBody]int id)
        {
            try
            {
                if (_quizzCommentSvc.UpVote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("LikeDownVote")]
        public HttpResponseMessage LikeDownVote([FromBody]int id)
        {
            try
            {
                if (_quizzCommentSvc.DownVote(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("FlagUpVote")]
        public HttpResponseMessage FlagUpVote([FromBody]int id)
        {
            try
            {
                if (_quizzCommentSvc.FlagComment(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("FlagDownVote")]
        public HttpResponseMessage FlagDownVote([FromBody]int id)
        {
            try
            {
                if (_quizzCommentSvc.UnFlagComment(id) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuizzCommentService __quizzCommentSvc;
        private QuizzCommentService _quizzCommentSvc
        {
            get
            {
                if (__quizzCommentSvc == null)
                    __quizzCommentSvc = SvcContainer.QuizzCommentSvc;
                return __quizzCommentSvc;
            }
        }
    }
}
