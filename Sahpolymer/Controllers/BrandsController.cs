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
namespace WorkWellPipe.Controllers
{
    [Authorize]
    public class BrandsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Brands
        public async Task<ActionResult> Index()
        {
            return View(await db.Brands.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,BrandName,BrandDescription")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (Request["contentarea"] != null && !string.IsNullOrEmpty(Request["contentarea"]))
                {

                    brand.BrandDescription = Request["contentarea"].ToString();
                }
                brand.CreatedDate = DateTime.Now;
                brand.UpdatedDate = DateTime.Now;
                db.Brands.Add(brand);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,BrandName,BrandDescription,CreatedDate")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                brand.UpdatedDate = DateTime.Now;
                db.Entry(brand).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(brand);
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

            var v = (from a in db.Brands select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.BrandName.Contains(searchitem) || b.BrandDescription.Contains(searchitem) || b.Description.Contains(searchitem));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.Sort, x.Description, x.BrandName, x.BrandDescription, x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
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
                Brand _brand = await db.Brands.FindAsync(id);
                if (_brand == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Brands.Remove(_brand);
                    db.SaveChanges();
                }
                return Json(new { Success = true, ex = "", data = "done" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "done" });
            }
        }

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
