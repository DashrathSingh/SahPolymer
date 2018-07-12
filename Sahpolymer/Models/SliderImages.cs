using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class SliderImages : BaseClass
    {
        public string Title { get; set; }

        public string SliderDescription { get; set; }

        public string Type { get; set; }

        public string ImagePath { get; set; }


    }
}