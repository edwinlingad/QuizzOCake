using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public interface IQuestion
    {
        int Id { get; set; }
        string Question { get; set; }
    }
}
