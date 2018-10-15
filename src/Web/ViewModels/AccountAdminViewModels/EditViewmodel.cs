using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.AccountAdminViewModels
{
    public class Role
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public string Selected { get; set; }
    }

    public class EditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Numero de teléfono")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de creación")]
        public DateTime CreateAt { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Ultimo acceso")]
        public DateTime LastLogin { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; }

        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir contraseña")]
        public string ReNewPassword { get; set; }

        [Display(Name = "Cuenta activa")]
        public bool Active { get; set; }

        [Display(Name = "Email confirmado")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Roles")]
        public ICollection<Role> Roles { get; set; }

        public List<string> IdRolesToAdd { get; set; }
    }
}
