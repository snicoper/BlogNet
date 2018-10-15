using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class FooterViewModel
    {
        public string SiteName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Requiere un email valido")]
        public string EmailSubscribe { get; set; }

        public bool IsSubscribed { get; set; }

        public string ReturnUrl { get; set; }
    }
}
