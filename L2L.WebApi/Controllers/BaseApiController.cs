using L2L.Data;
using L2L.WebApi.Interfaces;
using L2L.WebApi.Services;
using L2L.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace L2L.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        private ApplicationUnit _uow;
        public ApplicationUnit Uow
        {
            get
            {
                if (_uow == null)
                    _uow = new ApplicationUnit();
                return _uow;
            }
        }

        private SessionHelper _sessionHelper;
        public SessionHelper SessionHelper
        {
            get
            {
                if (_sessionHelper == null)
                    _sessionHelper = new SessionHelper(this);
                return _sessionHelper;
            }
        }

        private System.Web.Http.Routing.UrlHelper _urlHelper;
        public System.Web.Http.Routing.UrlHelper UrlHelper
        {
            get
            {
                if (_urlHelper == null)
                    _urlHelper = new UrlHelper(this.Request);
                return _urlHelper;
            }
        }

        private ServiceContainer _serviceContainer;
        public ServiceContainer SvcContainer
        {
            get
            {
                if (_serviceContainer == null)
                    _serviceContainer = new ServiceContainer(this);
                return _serviceContainer;
            }
        }
    }
}