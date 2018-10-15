using System.Collections.Generic;
using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.AccountAdminViewModels
{
    public enum AccountStatus
    {
        None = 0,
        Active = 1,
        Disable = 2
    }

    public enum EmailConfirmed
    {
        None = 0,
        Yes = 1,
        No = 2
    }

    public class ListViewModel : PaginatorCore<AppUser>
    {
        public ListViewModel()
        {
            OrderBy = "CreateAt";

            ActiveChoices = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "-- Estado --",
                    Value = AccountStatus.None.ToString()
                },
                new SelectListItem
                {
                    Text = "Activo",
                    Value = AccountStatus.Active.ToString()
                },
                new SelectListItem
                {
                    Text = "Inactivo",
                    Value = AccountStatus.Disable.ToString()
                }
            };

            EmailConfirmedChoices = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "-- Email confirmado --",
                    Value = EmailConfirmed.None.ToString()
                },
                new SelectListItem
                {
                    Text = "Confirmado",
                    Value = EmailConfirmed.Yes.ToString()
                },
                new SelectListItem
                {
                    Text = "Sin confirmar",
                    Value = EmailConfirmed.No.ToString()
                }
            };
        }

        // Filtros de búsqueda
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CreateAt { get; set; }
        public string LastLogin { get; set; }

        public AccountStatus Active { get; set; }
        public List<SelectListItem> ActiveChoices { get; }

        public EmailConfirmed EmailConfirmed { get; set; }
        public List<SelectListItem> EmailConfirmedChoices { get; }
    }
}
