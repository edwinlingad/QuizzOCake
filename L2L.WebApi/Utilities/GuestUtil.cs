using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Utilities
{
    public static class GuestUtil
    {
        private static List<string> _guestList = new List<string>() {
            "guest", "quizzling1", "quizzling2"
        };

        public static IList<string> GetGuestList()
        {
            return _guestList;
        }
    }
}