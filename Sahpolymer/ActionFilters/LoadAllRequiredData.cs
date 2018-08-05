using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWellPipe.Models;

namespace SahPolymer.ActionFilters
{

    public class LoggingFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Trace.Write("(Logging Filter)Action Executing: " +
                filterContext.ActionDescriptor.ActionName);
            if (HttpContext.Current.Session["SliderImages"] == null)
            {
                ApplicationDbContext db = new ApplicationDbContext();

                var _sliderImages = db.SliderImages.ToList();
                HttpContext.Current.Session["SliderImages"] = _sliderImages;
                filterContext.Controller.ViewBag.SliderImages = _sliderImages;

            }
            else
            {

                filterContext.Controller.ViewBag.SliderImages = HttpContext.Current.Session["SliderImages"];
            }


            if (HttpContext.Current.Session["ProductsList"] == null)
            {
                ApplicationDbContext db = new ApplicationDbContext();

                var _productsList = db.Products.ToList();
                HttpContext.Current.Session["ProductsList"] = _productsList;
                filterContext.Controller.ViewBag.ProductsList = _productsList;

            }
            else
            {

                filterContext.Controller.ViewBag.ProductsList = HttpContext.Current.Session["ProductsList"];
            }


            base.OnActionExecuting(filterContext);
        }


    }
}