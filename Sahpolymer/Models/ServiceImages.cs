using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class ServiceImages : BaseClass
    {
        [ForeignKey("Service")]
        public int ServiceID { get; set; }

        public string ImagePath { get; set; }

        public Service Service { get; set; }
    }
}