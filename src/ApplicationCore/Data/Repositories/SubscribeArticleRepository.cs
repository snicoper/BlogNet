using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;

namespace ApplicationCore.Data.Repositories
{
    public class SubscribeArticleRepository : BaseRepository<SubscribeArticle>, ISubscribeArticleRepository
    {
        public SubscribeArticleRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public bool EmailExists(string email)
        {
            return Entity.Any(s => s.Email == email);
        }

        public SubscribeArticle GetByToken(string token)
        {
            return Entity.FirstOrDefault(s => s.Token.ToString() == token);
        }

        public async Task SubscribeAsync(SubscribeArticle subscribeArticle)
        {
            Guid token;
            while (true)
            {
                token = Guid.NewGuid();
                var exists = Entity.Any(s => s.Token == token);
                if (!exists)
                {
                    break;
                }
            }

            subscribeArticle.SubscribeAt = DateTime.Now;
            subscribeArticle.Token = token;
            await CreateAsync(subscribeArticle);
        }

        public async Task UnSubscribeByTokenAsync(string token)
        {
            var subscribeArticle = Entity.FirstOrDefault(s => s.Token.ToString() == token);
            if (subscribeArticle != null)
            {
                await RemoveAsync(subscribeArticle);
            }
        }
    }
}
