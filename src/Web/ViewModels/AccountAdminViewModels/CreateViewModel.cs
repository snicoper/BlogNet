using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Web.ViewModels.AccountAdminViewModels
{
    public class CreateViewModel
    {
        [StringLength(256)]
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UserName { get; set; }

        [StringLength(256)]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Numero de teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "¿Email confirmado?")]
        public bool EmailConfirmed { get; set; } = true;

        public string[] IdRolesToAdd { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}
