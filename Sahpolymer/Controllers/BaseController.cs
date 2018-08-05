using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkWellPipe.Models;
namespace SahPolymer.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationDbContext _Globalcontext = new ApplicationDbContext();
        // GET: Base


        public void SaveImagesByType(int Type, int ID, string ImagePath)
        {
            switch (Type)
            {
                case 1:
                    var _serviceImage = new ServiceImages();
                    _serviceImage.ServiceID = ID;
                    _serviceImage.ImagePath = ImagePath;
                    _serviceImage.UpdatedDate = DateTime.Now;
                    _serviceImage.CreatedDate = DateTime.Now;
                    _Globalcontext.ServiceImages.Add(_serviceImage);
                    break;
                case 2:
                    var _ProductImage = new ProductImages();
                    _ProductImage.ProductID = ID;
                    _ProductImage.ImagePath = ImagePath;
                    _ProductImage.UpdatedDate = DateTime.Now;
                    _ProductImage.CreatedDate = DateTime.Now;
                    _Globalcontext.ProductImages.Add(_ProductImage);
                    break;
                case 3:
                    var _BrandImage = new BrandImages();
                    _BrandImage.BrandID = ID;
                    _BrandImage.ImagePath = ImagePath;
                    _BrandImage.UpdatedDate = DateTime.Now;
                    _BrandImage.CreatedDate = DateTime.Now;
                    _Globalcontext.BrandImages.Add(_BrandImage);
                    break;
                case 4:
                    var _AlbumImage = new AlbumImages();
                    _AlbumImage.AlbumID = ID;
                    _AlbumImage.ImagePath = ImagePath;
                    _AlbumImage.UpdatedDate = DateTime.Now;
                    _AlbumImage.CreatedDate = DateTime.Now;
                    _Globalcontext.AlbumImages.Add(_AlbumImage);
                    break;

                default:
                    break;
            }

            _Globalcontext.SaveChanges();

        }


        public void DeleteImagesByType(int Type, int ID)
        {
            switch (Type)
            {
                case 1:
                    var _serviceImage = _Globalcontext.ServiceImages.Where(x => x.Id == ID).FirstOrDefault();
                    _Globalcontext.ServiceImages.Remove(_serviceImage);
                    break;
                case 2:
                    var _ProductImage = _Globalcontext.ProductImages.Where(x => x.Id == ID).FirstOrDefault();
                    _Globalcontext.ProductImages.Remove(_ProductImage);
                    break;
                case 3:
                    var _BrandImage = _Globalcontext.BrandImages.Where(x => x.Id == ID).FirstOrDefault();
                    _Globalcontext.BrandImages.Remove(_BrandImage);
                    break;
                case 4:
                    var _AlbumImage = _Globalcontext.AlbumImages.Where(x => x.Id == ID).FirstOrDefault();
                    _Globalcontext.AlbumImages.Remove(_AlbumImage);
                    break;
                default:
                    break;
            }

            _Globalcontext.SaveChanges();

        }


        public void UploadImages(HttpFileCollectionBase Imagefiles, string Folder, int Type, int ID)
        {
            string _pathtosave = "~/Images/" + Folder + "/";
            string path = Server.MapPath(_pathtosave);
            HttpFileCollectionBase files = Imagefiles;
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase file = files[i];
                var _fileData = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(path + _fileData);

                SaveImagesByType(Type, ID, _pathtosave + _fileData);

            }


        }
    }
}