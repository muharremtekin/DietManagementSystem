using DietManagementSystem.Application.Interfaces.Repositories;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;

namespace DietManagementSystem.Persistence.Repositories;

public sealed class DietPlanRepository : EntityRepository<DietPlan>, IDietPlanRepository
{
    public DietPlanRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}