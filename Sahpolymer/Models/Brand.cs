using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Brand : BaseClass
    {
        public string BrandName { get; set; }

        public string BrandDescription { get; set; }



    }
}