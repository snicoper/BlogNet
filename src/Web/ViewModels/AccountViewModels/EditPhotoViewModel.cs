using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.AccountViewModels
{
    public class EditPhotoViewModel
    {
        [HiddenInput]
        public string CurrentPhoto { get; set; }

        [Display(Name = "Cambiar imagen")]
        public IFormFile UploadPhoto { get; set; }

        [Display(Name = "Restablecer imagen")]
        public bool Restore { get; set; }
    }
}
