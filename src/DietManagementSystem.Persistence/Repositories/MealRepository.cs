using DietManagementSystem.Application.Interfaces.Repositories;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;

namespace DietManagementSystem.Persistence.Repositories;
public sealed class MealRepository : EntityRepository<Meal>, IMealRepository
{
    public MealRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
