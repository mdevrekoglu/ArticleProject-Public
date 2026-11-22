using ArticleProject.Domain.Common.Interfaces.Entity;
using System.Linq.Expressions;

namespace ArticleProject.Domain.Common.Interfaces.Repository
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class, IEntity
    {
        Task AddAndSaveAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes);

        Task AddAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes);

        Task UpdateAndSaveAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes);

        Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>?[] includes);

        Task DeleteAndSaveAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteRangeAndSaveAsync(List<TEntity> entities);

        void DeleteRange(List<TEntity> entities);

        Task AddRangeAndSaveAsync(List<TEntity> entities);

        Task AddRangeAsync(List<TEntity> entities);

        Task SaveChangesAsync();

        void SaveChanges();
    }
}