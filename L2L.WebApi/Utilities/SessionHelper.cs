using L2L.Entities;
using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using L2L.WebApi.Utilities;
using L2L.Data;
using System.Data.Entity;
using L2L.WebApi.Models;
using L2L.WebApi.Interfaces;
using L2L.WebApi.Enums;
using System.Web.SessionState;

namespace L2L.WebApi.Utilities
{
    public class SessionHelper
    {
        private BaseApiController _controller;

        public SessionHelper(BaseApiController controller)
        {
            _controller = controller;
        }

        public void Store(SessionIdxEnum idx, object obj)
        {
            session[idx.ToString()] = obj;
        }

        public object Retrieve(SessionIdxEnum idx)
        {
            string strTmp = idx.ToString();
            return session[idx.ToString()];
        }

        private HttpSessionState _session;
        private HttpSessionState session
        {
            get
            {
                if (_session == null)
                    _session = HttpContext.Current.Session;
                return _session;
            }
        }
    }
}