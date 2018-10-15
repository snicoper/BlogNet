using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Web.ViewModels.AccountViewModels
{
    public class EditEmailViewModel
    {
        [Display(Name = "Nuevo email")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [EmailAddress(ErrorMessage = "Requiere un email valido")]
        public string NewEmail { get; set; }

        [HiddenInput]
        public string CurrentEmail { get; set; }

        [HiddenInput]
        public bool RequireConfirmedEmail { get; set; }
    }
}
