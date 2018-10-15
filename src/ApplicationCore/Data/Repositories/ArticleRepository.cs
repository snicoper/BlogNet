using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Data.Repositories
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<Article> GetAllActive()
        {
            return Entity.Where(a => a.Active);
        }

        /// <summary>
        /// Obtener lista de articulos con relaciones TagArticles, Tag y Owner.
        /// Solo se obtiene los articulos activos
        /// </summary>
        public IQueryable<Article> GetListArticles()
        {
            return GetAllActive()
                .OrderByDescending(a => a.CreateAt)
                .Include(a => a.Owner)
                .Include(a => a.DefaultTag)
                .Include(a => a.TagArticles)
                .ThenInclude(ta => ta.Tag);
        }

        public Article GetBySlug(string slug)
        {
            return Entity
                .Include(a => a.Owner)
                .Include(a => a.DefaultTag)
                .Include(a => a.TagArticles)
                    .ThenInclude(ta => ta.Tag)
                .FirstOrDefault(a => a.Slug == slug.ToLower());
        }

        public bool TitleExists(string title)
        {
            return Entity.Any(a => a.Title == title);
        }

        public Article GetNextArticle(Article article)
        {
            return Entity
                .OrderBy(a => a.CreateAt)
                .FirstOrDefault(a => a.CreateAt > article.CreateAt);
        }

        public Article GetPreviousArticle(Article article)
        {
            return Entity
                .OrderByDescending(a => a.CreateAt)
                .FirstOrDefault(a => a.CreateAt < article.CreateAt);
        }

        public IQueryable<Article> GetRssArticles(int quantity)
        {
            return Entity
                .OrderByDescending(a => a.CreateAt)
                .Include(a => a.Owner)
                .Take(quantity);
        }

        public async Task CreateAsync(ITagRepository tagRepository, Article article, IEnumerable<int> tags)
        {
            using (var transaction = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    article.TagArticles = new List<TagArticle>();
                    _createSlug(article);
                    DbContext.Attach(article);

                    foreach (var tagId in tags)
                    {
                        var tag = tagRepository.GetById(tagId);
                        if (tag is null)
                        {
                            throw new NullReferenceException();
                        }

                        article.TagArticles.Add(new TagArticle
                        {
                            ArticleId = article.Id,
                            TagId = tag.Id
                        });
                    }

                    await DbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(ITagRepository tagRepository, Article article, IEnumerable<int> tags)
        {
            using (var transaction = await DbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var removeTagsArticle = article.TagArticles.ToList();
                    DbContext.RemoveRange(removeTagsArticle);
                    DbContext.SaveChanges();

                    foreach (var tagId in tags)
                    {
                        var tag = tagRepository.GetById(tagId);
                        if (tag is null)
                        {
                            throw new NullReferenceException();
                        }

                        article.TagArticles.Add(new TagArticle
                        {
                            ArticleId = article.Id,
                            TagId = tagId
                        });
                    }

                    DbContext.Entry(article).State = EntityState.Modified;
                    await DbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void _createSlug(Article entity)
        {
            entity.Slug = string.IsNullOrEmpty(entity.Slug) ? entity.Title.Slugify() : entity.Slug.Slugify();
        }
    }
}
