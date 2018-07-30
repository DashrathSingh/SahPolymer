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

namespace SahPolymer.Controllers
{
    [Authorize]
    public class ContentPagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContentPages
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: ContentPages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPages contentPages = await db.ContentPages.FindAsync(id);
            if (contentPages == null)
            {
                return HttpNotFound();
            }
            return View(contentPages);
        }

        // GET: ContentPages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContentPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,PageName,PageDescription,PageCode,Sort,CreatedDate,UpdatedDate,Description")] ContentPages contentPages)
        {
            if (ModelState.IsValid)
            {
                db.ContentPages.Add(contentPages);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contentPages);
        }

        // GET: ContentPages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPages contentPages = await db.ContentPages.FindAsync(id);
            if (contentPages == null)
            {
                return HttpNotFound();
            }
            return View(contentPages);
        }

        // POST: ContentPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,PageName,PageDescription,PageCode,CreatedDate")] ContentPages contentPages)
        {
            if (ModelState.IsValid)
            {
                contentPages.UpdatedDate = DateTime.Now;

                db.Entry(contentPages).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contentPages);
        }

        // GET: ContentPages/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentPages contentPages = await db.ContentPages.FindAsync(id);
            if (contentPages == null)
            {
                return HttpNotFound();
            }
            return View(contentPages);
        }

        // POST: ContentPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContentPages contentPages = await db.ContentPages.FindAsync(id);
            db.ContentPages.Remove(contentPages);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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

            var v = (from a in db.ContentPages select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.PageCode.Contains(searchitem) || b.PageDescription.Contains(searchitem));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.PageName, x.Sort, x.Description, x.PageDescription,x.PageCode, x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
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
