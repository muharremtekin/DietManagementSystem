using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Delete;

public sealed class DeleteDietPlanCommandHandler : IRequestHandler<DeleteDietPlanCommand>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    public DeleteDietPlanCommandHandler(IDietPlanRepository dietPlanRepository)
    {
        _dietPlanRepository = dietPlanRepository;
    }
    public async Task Handle(DeleteDietPlanCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = await _dietPlanRepository.GetSingleAsync(predicate: d => d.Id == request.DietPlanId, noTracking: false);

        if (dietPlan is null)
            throw new NotFoundException("Diet plan not found.");

        dietPlan.IsDeleted = true;
        dietPlan.UpdatedAt = DateTime.UtcNow;

        await _dietPlanRepository.SaveChangesAsync();

    }
}