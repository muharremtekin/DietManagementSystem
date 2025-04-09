using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Update;

public class UpdateDietPlanCommandHandler : IRequestHandler<UpdateDietPlanCommand>
{
    private readonly IDietPlanRepository _dietPlanRepository;

    public UpdateDietPlanCommandHandler(IDietPlanRepository dietPlanRepository)
    {
        _dietPlanRepository = dietPlanRepository;
    }

    public async Task Handle(UpdateDietPlanCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = await _dietPlanRepository.GetSingleAsync(x => x.Id == request.Id, noTracking: false);
        if (dietPlan is null)
            throw new NotFoundException("Diet plan not found.");

        dietPlan.Title = request.Title;
        dietPlan.StartDate = request.StartDate;
        dietPlan.EndDate = request.EndDate;
        dietPlan.InitialWeight = request.InitialWeight;

        await _dietPlanRepository.SaveChangesAsync();
    }
}