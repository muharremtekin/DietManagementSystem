using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Interfaces.Repositories;

namespace DietManagementSystem.Persistence.Repositories;

public sealed class ProgressRepository : EntityRepository<Progress>, IProgressRepository
{
    public ProgressRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}