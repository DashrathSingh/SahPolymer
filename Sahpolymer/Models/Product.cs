using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkWellPipe.Models
{
    public class Product : BaseClass
    {
        [Required]
        public string ProductName { get; set; }

        [AllowHtml]

        public string ProductDescription { get; set; }

        [Required]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public List<ProductImages> Images { get; set; }

    }
}