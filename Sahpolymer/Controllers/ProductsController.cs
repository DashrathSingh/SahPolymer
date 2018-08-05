using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkWellPipe.Models;
using System.Linq.Dynamic;
using System.IO;
using System.Drawing;

namespace SahPolymer.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProductName,ProductDescription,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (Request["contentarea"] != null && !string.IsNullOrEmpty(Request["contentarea"]))
                {

                    product.ProductDescription = Request["contentarea"].ToString();
                }
                product.CreatedDate = DateTime.Now;
                product.UpdatedDate = DateTime.Now;
                db.Products.Add(product);
                await db.SaveChangesAsync();
                Session["ProductsList"] = null;
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProductName,ProductDescription,CategoryID,CreatedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.UpdatedDate = DateTime.Now;
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                Session["ProductsList"] = null;
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Name", product.CategoryID);
            return View(product);
        }

        public ActionResult LoadData()
        {

            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var searchitem = Request["search[value]"];
            int _searchInt = -1;
            if (int.TryParse(searchitem, out _searchInt))
            {
                _searchInt = int.Parse(searchitem);
            }
            else
            {
                _searchInt = -1;
            }
            //Find Order Column
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var v = (from a in db.Products.Include("Category") select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.ProductName.Contains(searchitem) || b.ProductDescription.Contains(searchitem) || b.Description.Contains(searchitem));
            }
            sortColumn = sortColumn == "Category" ? "CategoryID" : sortColumn;
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.Sort, x.Description, x.ProductName, x.ProductDescription, Category = (x.Category != null ? x.Category.Name : ""), x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
        }


        // GET: Services/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {


                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product _product = await db.Products.FindAsync(id);
                if (_product == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Products.Remove(_product);
                    db.SaveChanges();
                }
                return Json(new { Success = true, ex = "", data = "done" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "done" });
            }
        }


        #region Image upload,list and delete region



        public ActionResult GetProductImages(int ProductID)
        {
            try
            {
                var _ProductImages = db.ProductImages.Where(x => x.ProductID == ProductID).ToList();

                return Json(new { Success = true, ex = "", data = _ProductImages.Select(x => new { x.Id, x.ImagePath }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
            try
            {

                HttpFileCollectionBase files = Request.Files;
                string ID = Request["ID"];
                int _ID = -1;
                int.TryParse(ID, out _ID);
                if (_ID != -1) { UploadImages(files, "ProductImages", 2, _ID); }


                return Json(new { Success = true, ex = "", data = "" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "" });
            }

        }

        [HttpPost]
        public ActionResult DeleteImage(int id)
        {
            try
            {


                DeleteImagesByType(2, id);
                return Json(new { Success = true, ex = "", data = "done" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "done" });
            }
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
