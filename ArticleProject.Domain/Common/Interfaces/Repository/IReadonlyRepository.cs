using ArticleProject.Domain.Common.Interfaces.Entity;
using System.Linq.Expressions;

namespace ArticleProject.Domain.Common.Interfaces.Repository
{
    public interface IReadOnlyRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity?> GetByIdAsync(long id, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);

        IQueryable<TEntity> GetAllByFilter(Expression<Func<TEntity, bool>>? predicate = null, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);

        TEntity GetByFilter(Expression<Func<TEntity, bool>>? predicate = null, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);

        Task<int> GetCountByFilterAsync(Expression<Func<TEntity, bool>>? predicate = null);

        int GetCountByFilter(Expression<Func<TEntity, bool>>? predicate = null);

        IQueryable<TEntity> GetAll(params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includes);
    }
}