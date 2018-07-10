using L2L.Data;
using L2L.Entities;
using L2L.WebApi.Controllers;
using L2L.WebApi.Interfaces;
using L2L.WebApi.Models;
using L2L.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace L2L.WebApi.Services
{
    public abstract class BaseService
    {
        protected BaseApiController _controller;

        public BaseService(BaseApiController controller)
        {
            _controller = controller;
        }

        public void SetAge(WithAgeModel model)
        {
            if (model.BirthDate != null)
                model.Age = (int)(DateTime.Now.Subtract(model.BirthDate).TotalDays / 365.25);
        }

        public void SetAge(IEnumerable<WithAgeModel> list)
        {
            foreach (var item in list)
            {
                if (item.BirthDate != null)
                    item.Age = (int)(DateTime.Now.Subtract(item.BirthDate).TotalDays / 365.25);
            }
        }

        public void SetAge(IWithAgeModel model)
        {
            if (model.BirthDate != null)
                model.Age = (int)(DateTime.Now.Subtract(model.BirthDate).TotalDays / 365.25);
            model.BirthDate = model.BirthDate.ToLocalTime();
        }

        public void SetAge(IEnumerable<IWithAgeModel> list)
        {
            foreach (var item in list)
            {
                if (item.BirthDate != null)
                    item.Age = (int)(DateTime.Now.Subtract(item.BirthDate).TotalDays / 365.25);
            }
        }

        private ApplicationUnit __uow;
        protected ApplicationUnit _uow
        {
            get
            {
                if (__uow == null)
                    __uow = _controller.Uow;
                return __uow;
            }
        }

        private System.Web.Http.Routing.UrlHelper __urlHelper;
        protected System.Web.Http.Routing.UrlHelper _urlHelper
        {
            get
            {
                if (__urlHelper == null)
                    __urlHelper = _controller.UrlHelper;
                return __urlHelper;
            }
        }

        private SessionHelper __sessionHelper;
        protected SessionHelper _sessionHelper
        {
            get
            {
                if (__sessionHelper == null)
                    __sessionHelper = _controller.SessionHelper;
                return __sessionHelper;
            }
        }

        private ServiceContainer __svcContainer;
        protected ServiceContainer _svcContainer
        {
            get
            {
                if (__svcContainer == null)
                    __svcContainer = _controller.SvcContainer;
                return __svcContainer;
            }
        }

        private User __user;
        protected User _currentUser
        {
            get
            {
                if (__user == null)
                    __user = _svcContainer.UserSvc.GetCurrentUser();
                return __user;
            }
        }
    }
}