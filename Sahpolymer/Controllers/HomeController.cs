using SahPolymer.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWellPipe.Models;

namespace WorkWellPipe.Controllers
{
    [LoggingFilterAttribute()]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Petrochemical()
        {
            return View();
        }


        public ActionResult InfraStructures()
        {
            return View();
        }

        public ActionResult ContentPages(string pageCode)
        {
            var _contentPage = db.ContentPages.Where(x => x.PageCode == pageCode).FirstOrDefault();
            return View(_contentPage);
        }

        public ActionResult ProductDetail(int ID)
        {
            var _ProductDetail = db.Products.Include("Category").Include("Images").Where(x => x.Id == ID).FirstOrDefault();
            return View(_ProductDetail);
        }
    }
}