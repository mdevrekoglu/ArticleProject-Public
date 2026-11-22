using ArticleProject.Domain.Common.Interfaces.Entity;
using ArticleProject.Domain.Common.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArticleProject.Data.ArticleProjectDbContext.Repositories
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ArticleProjectContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public ReadOnlyRepository(ArticleProjectContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(long id, params Func<IQueryable<TEntity>, IQueryable<TEntity>>?[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            return await query.SingleOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<TEntity> GetAllByFilter(Expression<Func<TEntity, bool>>? predicate = null, params Func<IQueryable<TEntity>, IQueryable<TEntity>>?[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public TEntity GetByFilter(Expression<Func<TEntity, bool>>? predicate = null, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            if (predicate != null)
            {
                return query.FirstOrDefault(predicate);
            }

            return query.FirstOrDefault();
        }

        public IQueryable<TEntity> GetAll(params Func<IQueryable<TEntity>, IQueryable<TEntity>>?[] includes)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = include(query);
                }
            }

            return query;
        }

        public async Task<int> GetCountByFilterAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return await _dbSet.AsNoTracking().CountAsync(predicate);
            }
            return await _dbSet.AsNoTracking().CountAsync();
        }

        public int GetCountByFilter(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return _dbSet.AsNoTracking().Count(predicate);
            }
            return _dbSet.AsNoTracking().Count();
        }
    }
}