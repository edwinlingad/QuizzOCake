using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using L2L.WebApi.Models;
using L2L.Entities;

namespace L2L.WebApi.Utilities
{
    public static class UserInfoUtil
    {
        public static string GetLocalId()
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            return currentUserId;
        }

        public static string GetUserName()
        {
            return HttpContext.Current.User.Identity.GetUserName();
        }
    }
}