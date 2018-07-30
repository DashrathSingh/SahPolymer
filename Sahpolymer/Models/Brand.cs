using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace WorkWellPipe.Models
{
    public class Brand : BaseClass
    {
        [Required]
        public string BrandName { get; set; }

        [AllowHtml]
        public string BrandDescription { get; set; }



    }
}