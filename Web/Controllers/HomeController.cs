using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;
using Web.Interfaces;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IImageService m_ImageService;

        public HomeController(IImageService imageService)
        {
            m_ImageService = imageService;
        }

        // GET: Home  
        public ActionResult UploadFiles()
        {
            Models.FileModel model = new Models.FileModel();
            model.ProjectCode = "Slike";
            model.InternalPath = AppSettings.DefaultContentFolder;
            model.ImageSize = AppSettings.DefaultSize;
            return View(model);
        }
        [HttpPost]
        public ActionResult UploadFiles(Models.FileModel model)
        {
            if (ModelState.IsValid)
            {
                if (AppSettings.ValidProjectKeys.Contains(model.ProjectCode))
                {
                    model.DownloadFiles = new List<SavedImage>();
                    //iterating through multiple file collection   
                    foreach (HttpPostedFileBase file in model.Files)
                    {
                        if (file != null)
                        {
                            var newFiles = m_ImageService.SaveFiles(model.ProjectCode, model.InternalPath, model.ImageSize, file, model.CreateNewFile);
                            if (newFiles.Count > 0)
                            {
                                model.DownloadFiles.AddRange(newFiles);
                            }
                        }
                    }
                    model.UploadStatus = model.Files.Count().ToString() + " files uploaded successfully.";
                }
                else
                {
                    ModelState.AddModelError("ProjectCode", "Project key is not valid.");
                    model.UploadStatus = "Check Project key.";
                    model.Files = null;
                }
            }
            return View(model);
        }
    }
}