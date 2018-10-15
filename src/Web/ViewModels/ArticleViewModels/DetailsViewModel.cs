using ApplicationCore.Data.Entities.Blog;

namespace Web.ViewModels.ArticleViewModels
{
    public class DetailsViewModel
    {
        public Article Article { get; set; }

        public Article PreviousArticle { get; set; }

        public Article NextArticle { get; set; }
    }
}
