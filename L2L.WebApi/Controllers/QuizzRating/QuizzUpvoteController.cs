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
    [RoutePrefix("api/QuizzUpvote")]
    public class QuizzUpvoteController : BaseApiController
    {
        [HttpPatch]
        [Route("UpVote")]
        public HttpResponseMessage UpVote([FromBody]int quizzRatingId)
        {
            try
            {
                if (_quizzUpvote.UpVote(quizzRatingId) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpPatch]
        [Route("DownVote")]
        public HttpResponseMessage DownVote([FromBody]int quizzRatingId)
        {
            try
            {
                if (_quizzUpvote.DownVote(quizzRatingId) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private QuizzUpvoteService __quizzUpvote;
        private QuizzUpvoteService _quizzUpvote
        {
            get
            {
                if (__quizzUpvote == null)
                    __quizzUpvote = SvcContainer.QuizzUpvoteSvc;
                return __quizzUpvote;
            }
        }
    }
}
