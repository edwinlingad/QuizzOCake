using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Services
{
    public class LoggingService : BaseService
    {
        public LoggingService(BaseApiController controller)
            : base(controller)
        {
        }

        public void Log(Exception ex)
        {

        }
    }
}