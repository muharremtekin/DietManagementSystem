
using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace DietManagementSystem.Application.Features.Meal.Queries.GetAllMeals;
public record GetAllMealsQuery(Guid? DietPlanId, DateTime? StartDate, DateTime? EndDate) : IRequest<List<MealViewModel>>;

public class GetAllMealsQueryHandler : IRequestHandler<GetAllMealsQuery, List<MealViewModel>>
{
    private readonly IMealRepository _mealRepository;
    private readonly IDietPlanRepository _dietPlanRepository;

    public GetAllMealsQueryHandler(IMealRepository mealRepository, IDietPlanRepository dietPlanRepository)
    {
        _mealRepository = mealRepository;
        _dietPlanRepository = dietPlanRepository;
    }

    public async Task<List<MealViewModel>> Handle(GetAllMealsQuery request, CancellationToken cancellationToken)
    {
        if (request.DietPlanId.HasValue)
        {
            var dietPlan = await _dietPlanRepository.GetSingleAsync(x => x.Id == request.DietPlanId.Value);
            if (dietPlan == null) throw new NotFoundException("Diet plan not found");
        }

        var query = _mealRepository.AsQueryable();

        if (request.DietPlanId.HasValue)
            query = query.Where(x => x.DietPlanId == request.DietPlanId.Value);

        if (request.StartDate.HasValue)
            query = query.Where(x => x.StartTime >= request.StartDate.Value.TimeOfDay);


        if (request.EndDate.HasValue)
            query = query.Where(x => x.EndTime <= request.EndDate.Value.TimeOfDay);


        var meals = await query.ToListAsync(cancellationToken);

        return meals.Select(x => new MealViewModel(x.Id, x.Title, x.StartTime, x.EndTime, x.Content, x.DietPlanId))
                    .ToList();
    }
}