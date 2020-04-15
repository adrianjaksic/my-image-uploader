using System.Collections.Generic;
using System.Web;

namespace Web.Interfaces
{
    public interface IImageService
    {
        List<string> SaveFiles(string projectCode, string internalPath, string imageSize, HttpPostedFileBase file, bool newFile);
    }
}