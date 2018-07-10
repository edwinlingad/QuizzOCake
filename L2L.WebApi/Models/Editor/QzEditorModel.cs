using L2L.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class QzEditorModel
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        public AddContentTypeEnum AddContentType { get; set; }
        public string ImageUrl { get; set; }
        public string DrawingContent { get; set; }
        public string ExternalUrl { get; set; }

        public string NewImageFileName { get; set; }
        public string ImageContent { get; set; }
        public bool IsImageChanged { get; set; }
    }

    public class QzImageEditorModel
    {
        public string ImageUrl { get; set; }
        public string ImageContent { get; set; }
        public string NewImageFileName { get; set; }
        public bool IsImageChanged { get; set; }
    }
}