using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [HiddenInput]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string UserId { get; set; }

        [HiddenInput]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Token { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ConfirmPassword { get; set; }
    }
}
