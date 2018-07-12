using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class AlbumImages : BaseClass
    {
        [ForeignKey("album")]
        public int AlbumID { get; set; }

        public string ImagePath { get; set; }

        public Album album { get; set; }
    }
}