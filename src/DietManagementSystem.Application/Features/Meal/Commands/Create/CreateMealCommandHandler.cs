using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Create;

public class CreateMealCommandHandler : IRequestHandler<CreateMealCommand>
{
    private readonly IMealRepository _mealRepository;
    private readonly IDietPlanRepository _dietPlanRepository;

    public CreateMealCommandHandler(IMealRepository mealRepository, IDietPlanRepository dietPlanRepository)
    {
        _mealRepository = mealRepository;
        _dietPlanRepository = dietPlanRepository;
    }

    public async Task Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        var dietPlan = _dietPlanRepository.GetSingleAsync(x => x.Id == request.DietPlanId);

        if (dietPlan is null)
            throw new NotFoundException("Diet plan not found.");

        var meal = new Domain.Entities.Meal
        {
            Title = request.Title,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Content = request.Content,
            DietPlanId = request.DietPlanId
        };

        await _mealRepository.AddAsync(meal);
        await _mealRepository.SaveChangesAsync();
    }
}