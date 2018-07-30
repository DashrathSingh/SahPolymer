using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkWellPipe.Models
{
    public class Service : BaseClass
    {
        [Required]
        public string Title { get; set; }
        [AllowHtml]
        public string ServiceDescription { get; set; }


        public List<ServiceImages> Images { get; set; }
    }
}