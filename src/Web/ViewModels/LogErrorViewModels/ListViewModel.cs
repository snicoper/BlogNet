using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities;

namespace Web.ViewModels.LogErrorViewModels
{
    public class ListViewModel : PaginatorCore<LogError>
    {
        public ListViewModel()
        {
            OrderBy = "CreateAt";
        }

        public string Message { get; set; }

        public string Username { get; set; }

        public string Path { get; set; }

        public string CreateAt { get; set; }

        public bool? Checked { get; set; }
    }
}
