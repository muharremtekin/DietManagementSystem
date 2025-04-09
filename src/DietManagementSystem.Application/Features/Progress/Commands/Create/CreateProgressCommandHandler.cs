using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Commands.Create;

public class CreateProgressCommandHandler : IRequestHandler<CreateProgressCommand>
{
    private readonly IProgressRepository _progressRepository;
    private readonly IDietPlanRepository _dietPlanRepository;
    public CreateProgressCommandHandler(IProgressRepository progressRepository, IDietPlanRepository dietPlanRepository)
    {
        _progressRepository = progressRepository;
        _dietPlanRepository = dietPlanRepository;
    }
    public async Task Handle(CreateProgressCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = await _dietPlanRepository.GetSingleAsync(dp => dp.Id == request.DietPlanId, includes: [dp => dp.Client]);

        if (dietPlan == null) throw new NotFoundException("Diet plan not found");

        var progress = new Domain.Entities.Progress
        {
            DietPlanId = request.DietPlanId,
            Date = request.Date,
            Weight = request.Weight,
            Notes = request.Notes,
            ClientId = dietPlan.ClientId
        };
        await _progressRepository.AddAsync(progress);

    }
}