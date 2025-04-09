using DietManagementSystem.Domain.Entities;
using System.Linq.Expressions;

namespace DietManagementSystem.Persistence.Interfaces.Repositories;

public interface IEntityRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    void UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid entityId);
    void DeleteAsync(TEntity entity);
    IQueryable<TEntity> AsQueryable();
    Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate,
                                bool noTracking = true,
                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                params Expression<Func<TEntity, object>>[] includes);
    Task<TCollection> GetCollection<TCollection>(Expression<Func<TEntity, bool>> predicate,
                                                Func<IQueryable<TEntity>,
                                                Task<TCollection>> collectionSelector,
                                                bool noTracking = true,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate,
                                 bool noTracking = true,
                                 params Expression<Func<TEntity, object>>[] includes);
    Task<int> SaveChangesAsync();
}
