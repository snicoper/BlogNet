using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.ArticleViewModels
{
    public class RecommendViewModel
    {
        [Required]
        public string Slug { get; set; }

        public string Title { get; set; }

        [EmailAddress]
        [Display(Name = "De")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string From { get; set; }

        [EmailAddress]
        [Display(Name = "Para")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string To { get; set; }

        [Display(Name = "Mensaje")]
        public string Body { get; set; }
    }
}
