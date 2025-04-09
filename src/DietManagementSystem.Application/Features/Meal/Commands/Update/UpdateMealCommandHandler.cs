using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Update;

public class UpdateMealCommandHandler : IRequestHandler<UpdateMealCommand>
{
    private readonly IMealRepository _mealRepository;

    public UpdateMealCommandHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task Handle(UpdateMealCommand request, CancellationToken cancellationToken)
    {
        var meal = await _mealRepository.GetSingleAsync(m => m.Id == request.Id, noTracking: false);

        if (meal == null) throw new NotFoundException("Meal not found");

        meal.Title = request.Title;
        meal.StartTime = request.StartTime;
        meal.EndTime = request.EndTime;
        meal.Content = request.Content;

        await _mealRepository.SaveChangesAsync();
    }
}