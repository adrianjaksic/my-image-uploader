using System.Collections.Generic;
using System.Web;
using Web.Models;

namespace Web.Interfaces
{
    public interface IImageService
    {
        List<SavedImage> SaveFiles(string projectCode, string internalPath, string imageSize, HttpPostedFileBase file, bool newFile);
    }
}