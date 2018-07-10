using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class DependentRequestFromUserModel
    {
        public int Id { get; set; }

        // Foreign Keys
        public int ToChildId { get; set; }
        public int FromUserId { get; set; }

        // generated
        public bool IsAccepted { get; set; }
    }
}