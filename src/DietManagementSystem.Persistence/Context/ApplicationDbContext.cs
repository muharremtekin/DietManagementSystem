using DietManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace DietManagementSystem.Persistence.Context;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<DietPlan> DietPlans { get; set; }
    public DbSet<Meal> Meals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Eğer entity IBaseEntity'yi implement ediyorsa
            if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // entity için bir lambda ifadesi oluşturuyoruz: e => !e.IsDeleted
                var parameter = Expression.Parameter(entityType.ClrType, "e");

                // EF.Property<bool>(e, "IsDeleted")
                var isDeletedProperty = Expression.Call(
                    typeof(EF).GetMethod("Property")!.MakeGenericMethod(typeof(bool)),
                    parameter,
                    Expression.Constant("IsDeleted")
                );

                // !EF.Property<bool>(e, "IsDeleted")
                var filterExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));

                var lambda = Expression.Lambda(filterExpression, parameter);

                // Global query filter olarak ekliyoruz.
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override int SaveChanges()
    {
        OnBeforeSave();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSave();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSave();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSave()
    {
        var now = DateTime.UtcNow;
        var addedEntities = ChangeTracker
                                .Entries<IBaseEntity>()
                                .Where(e => e.State == EntityState.Added)
                                .Select(e => e.Entity)
                                .ToArray();

        foreach (var entity in addedEntities)
        {
            if (entity.CreatedAt == DateTime.MinValue)
                entity.CreatedAt = now;
            entity.UpdatedAt = now;
        }
    }
}