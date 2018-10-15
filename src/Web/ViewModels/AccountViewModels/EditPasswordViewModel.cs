using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.AccountViewModels
{
    public class EditPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ConfirmNewPassword { get; set; }
    }
}
