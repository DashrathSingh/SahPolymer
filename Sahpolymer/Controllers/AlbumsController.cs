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
    public class AlbumsController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Albums
        public async Task<ActionResult> Index()
        {
            return View(await db.Albums.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,AlbumName,AlbumDescription,Type")] Album album)
        {
            if (ModelState.IsValid)
            {

                if (Request["contentarea"] != null && !string.IsNullOrEmpty(Request["contentarea"]))
                {

                    album.AlbumDescription = Request["contentarea"].ToString();
                }
                album.CreatedDate = DateTime.Now;
                album.UpdatedDate = DateTime.Now;
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,AlbumName,AlbumDescription,Type,CreatedDate")] Album album)
        {
            if (ModelState.IsValid)
            {
                album.UpdatedDate = DateTime.Now;
                db.Entry(album).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(album);
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

            var v = (from a in db.Albums select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.AlbumName.Contains(searchitem) || b.AlbumDescription.Contains(searchitem) || b.Description.Contains(searchitem));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.Sort, x.Description, x.AlbumName, x.AlbumDescription, x.Type, x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
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
                Album _album = await db.Albums.FindAsync(id);
                if (_album == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Albums.Remove(_album);
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


        public ActionResult GetAlbumImages(int AlbumID)
        {
            try
            {
                var _AlbumImages = db.AlbumImages.Where(x => x.AlbumID == AlbumID).ToList();

                return Json(new { Success = true, ex = "", data = _AlbumImages.Select(x => new { x.Id, x.ImagePath }) }, JsonRequestBehavior.AllowGet);
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
                if (_ID != -1) { UploadImages(files, "PhotoGallery", 4, _ID); }


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


                DeleteImagesByType(4, id);
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
