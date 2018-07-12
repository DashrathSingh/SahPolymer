using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Service : BaseClass
    {
        public string Title { get; set; }
        public string ServiceDescription { get; set; }


        public List<ServiceImages> Images { get; set; }
    }
}