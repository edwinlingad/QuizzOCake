using L2L.WebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Services;
using L2L.WebApi.Models;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/QuizzOverview")]
    public class QuizzOverviewController : BaseApiController
    {
        ////[Route("Search")]
        //public HttpResponseMessage Get(
        //    string category = "",
        //    string levelMin = "",
        //    string levelMax = "",
        //    string searchString = "",
        //    string availOnly = "",
        //    int pageNum = 0,
        //    int numPerpage = 10,
        //    SortByEnum sortBy = SortByEnum.DateModified,
        //    SortTypeEnum sortType = SortTypeEnum.Descending)
        //{
        //    try
        //    {
        //        var list = _quizzOverviewSvc.GetQuizzOverviewModelsx(
        //            category, levelMin, levelMax, pageNum, numPerpage, sortBy, sortType);

        //        if (list == null)
        //            return Request.CreateResponse(HttpStatusCode.NotFound);

        //        return Request.CreateResponse(HttpStatusCode.OK, list);
        //    }
        //    catch (ServiceException ex)
        //    {
        //        return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
        //    }
        //}

        [AllowAnonymous]
        // return QuizzOverviewModel
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var model = _quizzOverviewSvc.GetQuizzOverviewModel(id);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("GetOverviews")]
        public HttpResponseMessage GetOverviews(
            string category = "",
            string levelMin = "",
            string levelMax = "",
            string searchString = "",
            int availOnly = 1,
            int dailyRewardsOnly = 0,
            int pageNum = 0,
            int numPerpage = 10,
            int userId = 0,
            SortByEnum sortBy = SortByEnum.DateModified,
            SortTypeEnum sortType = SortTypeEnum.Descending
            )
        {
            try
            {
                QuizzOverviewQueryParam qSetting = new QuizzOverviewQueryParam
                {
                    Category = category,
                    GradeLevelMin = levelMin,
                    GradeLevelMax = levelMax,
                    SearchString = searchString,
                    PageNum = pageNum,
                    NumPerPage = numPerpage,
                    SortBy = sortBy,
                    SortType = sortType,
                    UserId = userId,
                    AvailOnly = availOnly,
                    DailyRewardsOnly = dailyRewardsOnly,
                };

                var result = _quizzOverviewSvc.GetQuizzOverviewModels(qSetting);
                if (result == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }

        }

        private QuizzOverviewService __quizzOverviewSvc;
        private QuizzOverviewService _quizzOverviewSvc
        {
            get
            {
                if (__quizzOverviewSvc == null)
                    __quizzOverviewSvc = SvcContainer.QuizzOverviewSvc;
                return __quizzOverviewSvc;
            }
        }

    }
}
