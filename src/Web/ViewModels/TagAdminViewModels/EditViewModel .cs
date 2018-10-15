using System.ComponentModel.DataAnnotations;
using ApplicationCore.Data.Entities.Blog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.TagAdminViewModels
{
    public class EditViewModel
    {
        public Tag Tag { get; set; }

        [HiddenInput]
        public string OldImage { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile Image { get; set; }
    }
}
