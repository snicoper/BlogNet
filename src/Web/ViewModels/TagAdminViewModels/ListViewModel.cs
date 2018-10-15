using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities.Blog;

namespace Web.ViewModels.TagAdminViewModels
{
    public class ListViewModel : PaginatorCore<Tag>
    {
        public ListViewModel()
        {
            OrderBy = "TagArticles.Count";
            SortOrder = SortOrder.Descending;
        }

        public string Name { get; set; }
    }
}
