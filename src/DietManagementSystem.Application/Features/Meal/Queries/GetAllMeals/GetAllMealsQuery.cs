using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using MediatR;
namespace DietManagementSystem.Application.Features.Meal.Queries.GetAllMeals;
public record GetAllMealsQuery(Guid? UserId, Guid? DietPlanId, DateTime? StartDate, DateTime? EndDate)
    : RequestParameters, IRequest<PagedList<MealViewModel>>;
