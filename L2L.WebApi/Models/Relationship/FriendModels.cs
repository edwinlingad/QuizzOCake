using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class FriendRequestModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsNew { get; set; }
        public bool IsAccepted { get; set; }

        // Foreign Keys
        public int RequestToId { get; set; }
        public int RequestFromId { get; set; }
    }

    public class FriendRelationshipModel
    {
        public int Id { get; set; }

        // Foreign Keys
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }
}