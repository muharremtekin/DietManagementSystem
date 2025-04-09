using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Delete;
public record DeleteDietPlanCommand(Guid DietPlanId) : IRequest;
