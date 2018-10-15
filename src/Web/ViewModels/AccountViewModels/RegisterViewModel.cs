using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.AccountViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Requiere de un email valido")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Compare("Password", ErrorMessage = "Las contraseñas han de ser iguales")]
        public string ConfirmPassword { get; set; }
    }
}
