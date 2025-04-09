using MediatR;

namespace DietManagementSystem.Application.Features.User.Commands.Delete;
public record DeleteUserCommand(Guid userId) : IRequest;
