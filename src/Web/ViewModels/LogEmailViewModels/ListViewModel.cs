using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities;

namespace Web.ViewModels.LogEmailViewModels
{
    public class ListViewModel : PaginatorCore<LogEmail>
    {
        public ListViewModel()
        {
            OrderBy = "SendAt";
        }

        public string From { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string SendAt { get; set; }
    }
}
