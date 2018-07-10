using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class LayoutModel
    {
        public LayoutModel()
        {
            TopPanel = new TopPanelModel();
            LeftSideBar = new LeftSideBarModel();
        }

        public TopPanelModel TopPanel { get; set; }
        public LeftSideBarModel LeftSideBar { get; set; }
    }
}