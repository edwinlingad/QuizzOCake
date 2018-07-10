using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public enum AddContentTypeEnum
    {
        None,
        PictureOnly,
        DrawingOnly,
        PictureAndDrawing
    }

    public class QzEditor
    {
        public string Title { get; set; }
        public string TextContent { get; set; }
        public AddContentTypeEnum AddContentType { get; set; }
        public string ImageUrl { get; set; }
        public string DrawingContent { get; set; }
        public string ExternalUrl { get; set; }
    }
}
