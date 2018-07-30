using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class ImageViewModel
    {
        public int ImageID { get; set; }
        public string ImagePath { get; set; }
        public Byte[] ImageBytes { get; set; }
        public int ImageForeignID { get; set; }
    }

}