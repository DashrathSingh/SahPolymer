using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class BrandImages : BaseClass
    {
        [ForeignKey("Brand")]
        public int BrandID { get; set; }

        public string ImagePath { get; set; }

        public Brand Brand { get; set; }
    }
}