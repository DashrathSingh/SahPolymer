using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkWellPipe.Models
{
    public class ContentPages : BaseClass
    {
        [Required]
        public string PageName { get; set; }

        [AllowHtml]
        public string PageDescription { get; set; }

        public string PageCode { get; set; }





    }
}