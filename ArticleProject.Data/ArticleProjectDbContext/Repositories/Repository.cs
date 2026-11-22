using ArticleProject.Domain.Common.Interfaces.Entity;
using ArticleProject.Domain.Common.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ArticleProject.Data.ArticleProjectDbContext.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly ArticleProjectContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ArticleProjectContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(long id, params Func<IQueryable<TEntity>, IQueryable<TEntity>>?[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

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
            IQueryable<TEntity> query = _dbSet;

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
            IQueryable<TEntity> query = _dbSet;

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
            IQueryable<TEntity> query = _dbSet;

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
                return await _dbSet.CountAsync(predicate);
            }
            return await _dbSet.CountAsync();
        }

        public int GetCountByFilter(Expression<Func<TEntity, bool>>? predicate = null)
        {
            if (predicate != null)
            {
                return _dbSet.Count(predicate);
            }
            return _dbSet.Count();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task AddAndSaveAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes)
        {
            // Add and save entity
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Load specific navigation properties if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var navigation = _context.Entry(entity).Reference(include);
                    if (!navigation.IsLoaded)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }
        }

        public async Task AddAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes)
        {
            await _dbSet.AddAsync(entity);

            // Load specific navigation properties if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var navigation = _context.Entry(entity).Reference(include);
                    if (!navigation.IsLoaded)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }
        }

        public async Task AddRangeAndSaveAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task UpdateAndSaveAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();

            // Load specific navigation properties if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var navigation = _context.Entry(entity).Reference(include);
                    if (!navigation.IsLoaded)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }
        }

        public async Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes)
        {
            _dbSet.Update(entity);

            // Load specific navigation properties if provided
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    var navigation = _context.Entry(entity).Reference(include);
                    if (!navigation.IsLoaded)
                    {
                        await navigation.LoadAsync();
                    }
                }
            }
        }

        public async Task DeleteAndSaveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task DeleteRangeAndSaveAsync(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public void DeleteRange(List<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}