using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.AccountViewModels
{
    public class RecoveryPasswordViewModel
    {
        [EmailAddress]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Email { get; set; }
    }
}
