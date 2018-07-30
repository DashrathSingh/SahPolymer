using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Album : BaseClass
    {
        [Required]
        public string AlbumName { get; set; }
        public string AlbumDescription { get; set; }
        public string Type { get; set; }
    }
}