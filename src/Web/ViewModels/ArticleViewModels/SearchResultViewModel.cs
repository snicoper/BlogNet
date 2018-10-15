using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.ArticleViewModels
{
    public class SearchResultViewModel : ListViewModel
    {
        [Display(Name = "Buscar")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string q { get; set; }
    }
}
