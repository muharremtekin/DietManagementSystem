using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Interfaces.Repositories;

namespace DietManagementSystem.Persistence.Repositories;

public sealed class DietPlanRepository : EntityRepository<DietPlan>, IDietPlanRepository
{
    public DietPlanRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}