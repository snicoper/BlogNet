using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Nombre de usuario requerido")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Contraseña requerida")]
        public string Password { get; set; }

        [Display(Name = "Recuerdame")]
        public bool Remember { get; set; }
    }
}
