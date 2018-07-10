using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace L2L.WebApi.Models
{
    public class ToggleModel
    {
        public int Id { get; set; }
        public bool Value { get; set; }
    }

    public class WithAgeModel
    {
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public interface IWithAgeModel
    {
        int Age { get; set; }
        DateTime BirthDate { get; set; }
    }
}