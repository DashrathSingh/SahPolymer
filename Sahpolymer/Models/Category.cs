using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Category : BaseClass
    {
        public string Name { get; set; }
        public string Type { get; set; }
        [ForeignKey("ParentCategory")]
        public int? ParentID { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }

        public Category ParentCategory { get; set; }
    }
}