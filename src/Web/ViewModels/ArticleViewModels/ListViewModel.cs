using ApplicationCore.Core.PaginatorCore;
using ApplicationCore.Data.Entities.Blog;

namespace Web.ViewModels.ArticleViewModels
{
    public class ListViewModel : PaginatorCore<Article>
    {
        public ListViewModel()
        {
            PageSize = 12;
            OrderBy = "CreateAt";
            JustifyContent = "justify-content-end";
        }
    }
}
