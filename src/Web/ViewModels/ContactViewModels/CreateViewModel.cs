using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.ContactViewModels
{
    public class CreateViewModel
    {
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string EmailFrom { get; set; }

        [StringLength(256)]
        [Display(Name = "Asunto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Subject { get; set; }

        [Display(Name = "Mensaje")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Message { get; set; }
    }
}
