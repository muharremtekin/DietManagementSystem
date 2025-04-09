using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Queries.GetMealQuery;
public record GetMealQuery(Guid Id) : IRequest<MealViewModel>;
