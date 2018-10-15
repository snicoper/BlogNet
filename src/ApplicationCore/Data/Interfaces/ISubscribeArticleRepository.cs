using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;

namespace ApplicationCore.Data.Interfaces
{
    public interface ISubscribeArticleRepository : IRepository<SubscribeArticle>
    {
        bool EmailExists(string email);
        SubscribeArticle GetByToken(string token);
        Task SubscribeAsync(SubscribeArticle subscribeArticle);
        Task UnSubscribeByTokenAsync(string token);
    }
}
