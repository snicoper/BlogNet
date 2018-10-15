using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.Data.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext DbContext;
        protected DbSet<TEntity> Entity { get; }

        protected BaseRepository(DbContext dbContext)
        {
            DbContext = dbContext;
            Entity = DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return Entity.AsNoTracking();
        }

        public TEntity GetById<T>(T id)
        {
            return Entity.Find(id);
        }

        public async Task<TEntity> GetByIdAsync<T>(T id)
        {
            return await Entity.FindAsync(id);
        }

        public void Create(TEntity entity)
        {
            Entity.Add(entity);
            DbContext.SaveChanges();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await Entity.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            Entity.Update(entity);
            DbContext.SaveChanges();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            Entity.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            Entity.UpdateRange(entities);
            DbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            Entity.Remove(entity);
            DbContext.SaveChanges();
        }

        public async Task RemoveAsync(TEntity entity)
        {
            Entity.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entity.RemoveRange(entities);
            DbContext.SaveChanges();
        }
    }
}
