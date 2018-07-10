using L2L.Data;
using L2L.WebApi.Services;
using L2L.WebApi.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;

namespace L2L.WebApi.Interfaces
{
    // Passed to Services since they can accept both Controller and ApiController
    public interface IBaseController
    {
        ApplicationUnit Uow { get; }
        SessionHelper SessionHelper { get; }
        System.Web.Http.Routing.UrlHelper ApiUrlHelperInstance { get; }
        ServiceContainer SvcContainer { get; }

        // this is needed because ApiController don't have the Session property
        HttpSessionStateBase GetSession();
    }
}
