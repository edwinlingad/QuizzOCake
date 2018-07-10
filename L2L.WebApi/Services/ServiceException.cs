using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace L2L.WebApi.Services
{
    public class ServiceException : Exception
    {
        public ServiceException(HttpStatusCode httpStatusCode, string message)
        {
            this.HttpStatusCode = httpStatusCode;
            this.message = message;
        }

        public HttpStatusCode HttpStatusCode { get; private set; }
        private string message;
        public override string Message { get { return message; } }
    }

}