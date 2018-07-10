using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using L2L.WebApi.Models;
using L2L.WebApi.Services;

namespace L2L.WebApi.Controllers
{
    [RoutePrefix("api/resource")]
    [Authorize]
    public class ResourceController : BaseApiController
    {
        //[Route("GetMany")]
        [AllowAnonymous]
        public HttpResponseMessage Get(ResourceTypeEnum rType, int id, int id2, int id3, int id4, int id5)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.GetMany(id, id2, id3, id4, id5);
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
        [Route("GetAlt")]
        public HttpResponseMessage GetAlt(ResourceTypeEnum rType, int id, int id2, int id3, string str, string str2, string str3)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.GetManyAlt(id, id2, id3, str, str2, str3);
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
        public HttpResponseMessage Get(ResourceTypeEnum rType, int id)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.Get(id);
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
        [Route("GetAlt")]
        public HttpResponseMessage GetAlt(ResourceTypeEnum rType, string str)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.GetAlt(str);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Post(ResourceTypeEnum rType, [FromBody]object modelParam)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.Post(modelParam);
                if (model == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        public HttpResponseMessage Patch(ResourceTypeEnum rType, [FromBody]object modelParam)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                if (resource.Patch(modelParam))
                    return Request.CreateResponse(HttpStatusCode.OK);

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        // DELETE: api/Resource/5
        public HttpResponseMessage Delete(ResourceTypeEnum rType, int id)
        {
            try
            {
                var resource = _resourceFactory.GetResource(rType);
                if (resource == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var model = resource.Delete(id);
                if (model == false)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (ServiceException ex)
            {
                return Request.CreateResponse(ex.HttpStatusCode, ex.Message);
            }
        }

        private ResourceFactory __resourceFactory;
        private ResourceFactory _resourceFactory
        {
            get
            {
                if (__resourceFactory == null)
                    __resourceFactory = new ResourceFactory(this);
                return __resourceFactory;
            }
        }
    }
}
