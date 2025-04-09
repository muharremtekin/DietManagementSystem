using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Update;
public record UpdateMealCommand(Guid Id, string Title, TimeSpan StartTime, TimeSpan EndTime, string Content) : IRequest;
