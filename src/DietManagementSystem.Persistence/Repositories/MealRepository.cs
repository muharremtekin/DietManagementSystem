using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Interfaces.Repositories;

namespace DietManagementSystem.Persistence.Repositories;
public sealed class MealRepository : EntityRepository<Meal>, IMealRepository
{
    public MealRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
