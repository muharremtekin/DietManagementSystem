using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Delete;
public record DeleteMealCommand(Guid Id) : IRequest;
