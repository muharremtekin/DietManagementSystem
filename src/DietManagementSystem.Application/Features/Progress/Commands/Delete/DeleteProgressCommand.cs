using MediatR;
namespace DietManagementSystem.Application.Features.Progress.Commands.Delete;

public record DeleteProgressCommand(Guid Id) : IRequest;
