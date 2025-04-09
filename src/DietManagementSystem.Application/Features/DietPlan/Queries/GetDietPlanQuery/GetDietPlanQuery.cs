using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetDietPlanQuery;
public record GetDietPlanQuery(Guid DietPlanId) : IRequest<DietPlanViewModel>;
