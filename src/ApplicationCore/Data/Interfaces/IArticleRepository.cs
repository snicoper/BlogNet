using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;

namespace ApplicationCore.Data.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        IQueryable<Article> GetAllActive();
        IQueryable<Article> GetListArticles();
        Article GetBySlug(string slug);
        bool TitleExists(string title);
        Article GetNextArticle(Article article);
        Article GetPreviousArticle(Article article);
        IQueryable<Article> GetRssArticles(int quantity);
        Task CreateAsync(ITagRepository tagRepository, Article article, IEnumerable<int> tags);
        Task UpdateAsync(ITagRepository tagRepository, Article article, IEnumerable<int> tags);
    }
}
