using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class ITrackableModel
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
