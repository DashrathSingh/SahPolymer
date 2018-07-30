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
using SahPolymer.Controllers;

namespace WorkWellPipe.Controllers
{
    [Authorize]
    public class ServicesController : BaseController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Services
        public async Task<ActionResult> Index()
        {
            return View(await db.Services.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,ServiceDescription")] Service service)
        {
            if (ModelState.IsValid)
            {
                if (Request["contentarea"] != null && !string.IsNullOrEmpty(Request["contentarea"]))
                {

                    service.ServiceDescription = Request["contentarea"].ToString();
                }
                service.CreatedDate = DateTime.Now;
                service.UpdatedDate = DateTime.Now;
                db.Services.Add(service);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Service service = await db.Services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,ServiceDescription,Sort,CreatedDate")] Service service)
        {
            if (ModelState.IsValid)
            {

                service.UpdatedDate = DateTime.Now;
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(service);
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
                Service service = await db.Services.FindAsync(id);
                if (service == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Services.Remove(service);
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

            var v = (from a in db.Services select a);
            if (!string.IsNullOrEmpty(searchitem))
            {

                v = v.Where(b => b.Title.Contains(searchitem) || b.ServiceDescription.Contains(searchitem) || b.Description.Contains(searchitem));
            }
            //SORT
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }

            recordsTotal = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data.Select(x => new { x.Id, x.Sort, x.Description, x.Title, x.ServiceDescription, x.CreatedDate, x.UpdatedDate }) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceImages(int id)
        {

            return View();

        }
        [HttpPost]
        public ActionResult PostServiceImages(List<ImageViewModel> Images)
        {
            try
            {
                string path = "";
                foreach (var item in Images)
                {
                    var _serviceImage = new ServiceImages();
                    if (item.ImageBytes != null)
                    {
                        if (string.IsNullOrEmpty(item.ImagePath))
                        {

                            var _fileName = Guid.NewGuid().ToString() + ".jpg";
                            path = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Services/" + _fileName);

                            _serviceImage.ImagePath = _fileName;
                        }
                        else
                        {

                            path = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/Services/" + _serviceImage.ImagePath);
                        }
                        byte[] image = item.ImageBytes;
                        MemoryStream ms = new MemoryStream(image);

                        Image i = Image.FromStream(ms);
                        i.Save(path);
                        _serviceImage.ServiceID = item.ImageForeignID;
                        _serviceImage.ImagePath = _serviceImage.ImagePath;
                    }
                    if (item.ImageID != 0)
                    {
                        var _serviceObj = db.ServiceImages.Where(x => x.Id == item.ImageID).FirstOrDefault();
                        _serviceObj.ImagePath = _serviceImage.ImagePath;

                    }
                    else
                    {
                        db.ServiceImages.Add(_serviceImage);

                    }
                    db.SaveChanges();

                }

                return Json(new { Success = true, ex = "", data = "" });

            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "" });
            }


        }

        public ActionResult GetServiceImages(int ServiceID)
        {
            try
            {
                var _ServiceImages = db.ServiceImages.Where(x => x.ServiceID == ServiceID).ToList();

                return Json(new { Success = true, ex = "", data = _ServiceImages.Select(x => new { x.Id, x.ImagePath }) },JsonRequestBehavior.AllowGet);
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
                if (_ID != -1) { UploadImages(files, "ServiceImages", 1, _ID); }


                return Json(new { Success = true, ex = "", data = "" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = false, ex = ex.InnerException.Message.ToString(), data = "" });
            }

        }
    }
}
