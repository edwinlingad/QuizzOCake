using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class ResourceModel
    {
        public ResourceTypeEnum rType { get; set; }
        public object model { get; set; }
    }
}