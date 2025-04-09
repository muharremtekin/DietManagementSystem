
using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Queries.GetMealQuery;
public class GetMealQueryHandler : IRequestHandler<GetMealQuery, MealViewModel>
{
    private readonly IMealRepository _mealRepository;

    public GetMealQueryHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task<MealViewModel> Handle(GetMealQuery request, CancellationToken cancellationToken)
    {
        var meal = await _mealRepository.GetSingleAsync(x => x.Id == request.Id);
        if (meal == null) throw new NotFoundException("Meal not found");

        return new MealViewModel(meal.Id, meal.Title, meal.StartTime, meal.EndTime, meal.Content, meal.DietPlanId);
    }
}
