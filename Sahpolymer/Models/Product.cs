using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Product : BaseClass
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public List<ProductImages> Images { get; set; }

    }
}