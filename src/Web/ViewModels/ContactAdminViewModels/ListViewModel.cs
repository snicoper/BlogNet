using System.Collections.Generic;
using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.ContactAdminViewModels
{
    public enum HasReadStatus
    {
        None = 0,
        Unread = 1,
        Read = 2
    }

    public class ListViewModel : PaginatorCore<Contact>
    {
        public ListViewModel()
        {
            OrderBy = "SendAt";

            HasReadChoices = new List<SelectListItem>
            {
                new SelectListItem { Text = "Mostrar todos", Value = HasReadStatus.None.ToString() },
                new SelectListItem { Text = "No leídos", Value = HasReadStatus.Unread.ToString() },
                new SelectListItem { Text = "Leídos", Value = HasReadStatus.Read.ToString() },
            };

        }

        public string EmailFrom { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string SendAt { get; set; }

        public HasReadStatus HasRead { get; set; }

        public List<SelectListItem> HasReadChoices { get; set; }
    }
}
