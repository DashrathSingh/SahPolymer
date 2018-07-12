using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class ProductImages : BaseClass
    {
        [ForeignKey("Product")]
        public int ProductID { get; set; }

        public string ImagePath { get; set; }

        public Product Product { get; set; }
    }
}