using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Web.Models
{
    public class FileModel
    {
        [Required(ErrorMessage = "Please enter project code.")]
        [Display(Name = "Project code")]
        public string ProjectCode { get; set; }

        [Required(ErrorMessage = "Please enter internal path of the file.")]
        [Display(Name = "Internal path")]
        public string InternalPath { get; set; }

        [Display(Name = "Image size")]
        public string ImageSize { get; set; }

        [Display(Name = "Create new")]
        public bool CreateNewFile { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

        public string UploadStatus { get; set; }

        public List<SavedImage> DownloadFiles { get; set; }
    }
}