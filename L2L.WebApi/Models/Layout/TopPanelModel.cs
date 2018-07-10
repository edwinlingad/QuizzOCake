using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class TopPanelModel
    {
        public int NewQuizzClassNotificationCount { get; set; }
        public int NewFriendRequestCount { get; set; }
        public int NewMessageCount { get; set; }
        public int NewNotificationCount { get; set; }
    }
}