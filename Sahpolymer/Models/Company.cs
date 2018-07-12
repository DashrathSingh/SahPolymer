using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkWellPipe.Models
{
    public class Company : BaseClass
    {
        public string CompanyName { get; set; }

        public string CompanyDescription { get; set; }

        public string CompanyLogo { get; set; }

        public string HeaderColor { get; set; }

        public string BackGroundColor { get; set; }


    }
}