using L2L.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace L2L.WebApi.Controllers
{
    public class ReviewerFromQuestionsController : BaseApiController
    {
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _reviewerFromQuestionsSvc.GetReviewerFromQuestions(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private ReviewerFromQuestionsService __reviewerFromQuestionsSvc;
        private ReviewerFromQuestionsService _reviewerFromQuestionsSvc
        {
            get
            {
                if (__reviewerFromQuestionsSvc == null)
                    __reviewerFromQuestionsSvc = SvcContainer.ReviewerFromQuestionsSvc;
                return __reviewerFromQuestionsSvc;
            }
        }
    }
}
