using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class BaseClass
    {
        [Key]
        public int Id { get; set; }
        public int Sort { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Description { get; set; }
    }
}