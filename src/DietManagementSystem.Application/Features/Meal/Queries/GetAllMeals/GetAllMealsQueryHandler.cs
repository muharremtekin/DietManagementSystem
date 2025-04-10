
using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Extensions;
using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;
namespace DietManagementSystem.Application.Features.Meal.Queries.GetAllMeals;

public class GetAllMealsQueryHandler : IRequestHandler<GetAllMealsQuery, PagedList<MealViewModel>>
{
    private readonly IMealRepository _mealRepository;
    private readonly IDietPlanRepository _dietPlanRepository;

    public GetAllMealsQueryHandler(IMealRepository mealRepository, IDietPlanRepository dietPlanRepository)
    {
        _mealRepository = mealRepository;
        _dietPlanRepository = dietPlanRepository;
    }

    public async Task<PagedList<MealViewModel>> Handle(GetAllMealsQuery request, CancellationToken cancellationToken)
    {
        if (request.DietPlanId.HasValue)
        {
            var dietPlan = await _dietPlanRepository.GetSingleAsync(x => x.Id == request.DietPlanId.Value);
            if (dietPlan == null) throw new NotFoundException("Diet plan not found");
        }

        var query = _mealRepository.AsQueryable();

        if (request.UserId.HasValue)
            query = query.Where(x => x.DietPlan.ClientId == request.UserId.Value);

        if (request.DietPlanId.HasValue)
            query = query.Where(x => x.DietPlanId == request.DietPlanId.Value);

        if (request.StartDate.HasValue)
            query = query.Where(x => x.StartTime >= request.StartDate.Value.TimeOfDay);

        if (request.EndDate.HasValue)
            query = query.Where(x => x.EndTime <= request.EndDate.Value.TimeOfDay);

#if DEBUG
        var meals = query.Select(x => new MealViewModel(x.Id, x.Title, x.StartTime, x.EndTime, x.Content, x.DietPlanId))
                        .ToPagedList(request.PageNumber, request.PageSize);
#else
        //var meals = await query.Select(x => new MealViewModel(x.Id, x.Title, x.StartTime, x.EndTime, x.Content, x.DietPlanId))
        //                        .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);

#endif
        return meals;
    }
}