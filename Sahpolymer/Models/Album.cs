using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkWellPipe.Models
{
    public class Album : BaseClass
    {
        [Required]

        public string AlbumName { get; set; }

        [AllowHtml]
        public string AlbumDescription { get; set; }
        [Required]
        public string Type { get; set; }
    }
}