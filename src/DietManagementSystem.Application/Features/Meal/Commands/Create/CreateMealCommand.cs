using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Create;

public record CreateMealCommand(
    Guid DietPlanId,
    string Title,
    DateTime StartTime,
    DateTime EndTime,
    string Content) : IRequest;
