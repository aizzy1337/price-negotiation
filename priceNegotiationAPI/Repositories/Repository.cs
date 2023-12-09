using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace priceNegotiationAPI.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext context;
        internal DbSet<TEntity> dbSet;
        protected readonly ILogger _logger;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public Task<TEntity> GetById(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }

        public Task<bool> Add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }

        public Task<bool> Update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }

        public Task<bool> Remove(int id)
        {
            TEntity entityToDelete = context.Set<TEntity>().Find(id);
            context.Set<TEntity>().Remove(entityToDelete);
        }

    }
}
