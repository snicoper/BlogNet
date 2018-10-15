using System.ComponentModel.DataAnnotations;
using ApplicationCore.Data.Entities.Blog;
using Microsoft.AspNetCore.Http;

namespace Web.ViewModels.TagAdminViewModels
{
    public class CreateViewModel
    {
        public Tag Tag { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public IFormFile Image { get; set; }
    }
}
