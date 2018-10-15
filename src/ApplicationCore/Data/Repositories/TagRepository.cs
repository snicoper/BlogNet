using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Interfaces;
using ApplicationCore.Extensions;

namespace ApplicationCore.Data.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        public bool NameExists(string name)
        {
            return Entity.Any(t => string.Equals(t.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        public new void Create(Tag entity)
        {
            _createSlug(entity);
            base.Create(entity);
        }

        public new async Task CreateAsync(Tag entity)
        {
            _createSlug(entity);
            await base.CreateAsync(entity);
        }

        public new void Update(Tag entity)
        {
            _createSlug(entity);
            base.Update(entity);
        }

        public new async Task UpdateAsync(Tag entity)
        {
            _createSlug(entity);
            await base.UpdateAsync(entity);
        }

        private void _createSlug(Tag entity)
        {
            entity.Slug = string.IsNullOrEmpty(entity.Slug) ? entity.Name.Slugify() : entity.Slug.Slugify();
        }
    }
}
