using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DietManagementSystem.Persistence.Repositories;
public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationDbContext dbContext;
    protected DbSet<TEntity> entity => dbContext.Set<TEntity>();

    public EntityRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public IQueryable<TEntity> AsQueryable() => entity.AsQueryable();
    public async Task AddAsync(TEntity entity) => await this.entity.AddAsync(entity);
    public void UpdateAsync(TEntity entity)
    {
        this.entity.Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
    }
    public async Task DeleteAsync(Guid entityId)
    {
        var entity = await this.entity.FindAsync(entityId);
        DeleteAsync(entity);
    }

    public void DeleteAsync(TEntity entity)
    {
        if (dbContext.Entry(entity).State == EntityState.Detached)
            this.entity.Attach(entity);

        this.entity.Remove(entity);
    }

    /// <summary>
    /// Retrieves a list of entities that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to filter the entities.</param>
    /// <param name="noTracking">A boolean indicating whether to track the entities or not (default is true).</param>
    /// <param name="orderBy">An optional function to order the results.</param>
    /// <param name="includes">An optional array of expressions to include related entities.</param>
    /// <returns>A task that represents the asynchronous operation, containing a list of entities of type <typeparamref name="TEntity"/>.</returns>
    /// <example>
    /// <code>
    /// var dietPlans = await repository.GetList(dp => dp.ClientId == clientId);
    /// </code>
    /// </example>
    public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (noTracking)
            query = query.AsNoTracking();

        if (includes != null && includes.Length > 0)
            query = ApplyIncludes(query, includes);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves a collection of entities that match the specified predicate.
    /// </summary>
    /// <typeparam name="TCollection">The type of the collection to return.</typeparam>
    /// <param name="predicate">A function to filter the entities.</param>
    /// <param name="collectionSelector">A function that takes an <see cref="IQueryable{TEntity}"/> and returns a <see cref="Task{TCollection}"/>.</param>
    /// <param name="noTracking">A boolean indicating whether to track the entities or not (default is true).</param>
    /// <param name="orderBy">An optional function to order the results.</param>
    /// <param name="includes">An optional array of expressions to include related entities.</param>
    /// <returns>A task that represents the asynchronous operation, containing the collection of entities of type <typeparamref name="TCollection"/>.</returns>
    /// <example>
    /// <code>
    /// var dietPlans = await repository.GetCollection<List<DietPlan>>(
    ///     dp => dp.ClientId == clientId,
    ///     query => query.ToListAsync(), // you can youse select and other LINQ methods here
    ///     noTracking: true,
    ///     orderBy: q => q.OrderBy(dp => dp.StartDate),
    ///     includes: new Expression<Func<DietPlan, object>>[] { dp => dp.Meals });
    /// </code>
    /// </example>
    public async Task<TCollection> GetCollection<TCollection>(Expression<Func<TEntity, bool>> predicate,

                                                                Func<IQueryable<TEntity>, Task<TCollection>> collectionSelector,
                                                                bool noTracking = true,
                                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (noTracking)
            query = query.AsNoTracking();

        if (includes != null && includes.Length > 0)
            query = ApplyIncludes(query, includes);

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        return await collectionSelector(query);
    }

    public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool noTracking = true, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = entity;

        if (predicate != null)
            query = query.Where(predicate);

        query = ApplyIncludes(query, includes);

        if (noTracking)
            query = query.AsNoTracking();

        return await query.SingleOrDefaultAsync();
    }

    public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();


    private static IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
    {
        if (includes != null)
        {
            foreach (var includeItem in includes)
                query = query.Include(includeItem);
        }

        return query;
    }

}
