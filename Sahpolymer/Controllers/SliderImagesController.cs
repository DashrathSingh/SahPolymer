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

namespace SahPolymer.Controllers
{
    [Authorize]
    public class SliderImagesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SliderImages
        public ActionResult Index()
        {
            return View();
        }

        // GET: SliderImages/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderImages sliderImages = await db.SliderImages.FindAsync(id);
            if (sliderImages == null)
            {
                return HttpNotFound();
            }
            return View(sliderImages);
        }

        // GET: SliderImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SliderImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,SliderDescription,Type,ImagePath")] SliderImages sliderImages)
        {
            if (ModelState.IsValid)
            {
                string _pathtosave = "~/Images/MainSlider/";
                string path = Server.MapPath(_pathtosave);
                var _fileData = "";

                if (Request.Files != null && Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        _fileData = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        file.SaveAs(path + _fileData);

                    }
                }
                sliderImages.ImagePath = _pathtosave + _fileData;
                sliderImages.CreatedDate = DateTime.Now;
                sliderImages.UpdatedDate = DateTime.Now;
                db.SliderImages.Add(sliderImages);
                await db.SaveChangesAsync();
                Session["SliderImages"] = null;

                return RedirectToAction("Index");
            }

            return View(sliderImages);
        }

        // GET: SliderImages/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderImages sliderImages = await db.SliderImages.FindAsync(id);
            if (sliderImages == null)
            {
                return HttpNotFound();
            }
            return View(sliderImages);
        }

        // POST: SliderImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,SliderDescription,Description,Type,ImagePath,CreatedDate")] SliderImages sliderImages)
        {
            if (ModelState.IsValid)
            {
                string _pathtosave = "~/Images/MainSlider/";
                string path = Server.MapPath(_pathtosave);
                var _fileData = "";
                if (Request.Files != null && Request.Files.Count > 0)
                {

                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        if (!string.IsNullOrEmpty(file.FileName))
                        {

                            _fileData = Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(path + _fileData);
                            sliderImages.ImagePath = _pathtosave + _fileData;

                        }
                        else
                        { sliderImages.ImagePath = sliderImages.Description; }

                    }

                }

                sliderImages.UpdatedDate = DateTime.Now;
                db.Entry(sliderImages).State = EntityState.Modified;

                await db.SaveChangesAsync();
                Session["SliderImages"] = null;
                return RedirectToAction("Index");
            }
            return View(sliderImages);
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

            var v = (from a in db.SliderImages select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.Title.Contains(searchitem) || b.SliderDescription.Contains(searchitem) || b.Description.Contains(searchitem));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.Sort, x.Description, x.ImagePath, x.Title, x.SliderDescription, x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
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
                SliderImages _sliderImage = await db.SliderImages.FindAsync(id);
                if (_sliderImage == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.SliderImages.Remove(_sliderImage);
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
