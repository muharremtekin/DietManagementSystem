using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Create;
public record CreateMealCommand(Guid DietPlanId, string Title, TimeSpan StartTime, TimeSpan EndTime, string Content) : IRequest;
