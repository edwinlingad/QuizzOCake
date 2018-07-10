using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Services;
using L2L.WebApi.Models;
using System.Net.Http;
using System.Net;
using System.Web.Http;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Search")]
    public class SearchController : BaseApiController
    {
        [AllowAnonymous]
        public HttpResponseMessage Get(string search)
        {
            try
            {
                var searchStr = search.Split(' ');
                string search1 = searchStr[0];
                string search2 = "";
                string search3 = "";
                if(searchStr.Length == 3)
                {
                    search2 = searchStr[1];
                    search3 = searchStr[2];
                }
                else if(searchStr.Length == 2)
                {
                    search2 = searchStr[1];
                }
            
                var list = SvcContainer.SearchSvc.Search(search1, search2, search3);
                if (list == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }
    }
}