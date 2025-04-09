using DietManagementSystem.Application.DTOs;
using MediatR;

namespace DietManagementSystem.Application.Features.User.Commands.Login;
public record LoginUserCommand(string? userName, string? email, string password) : IRequest<LoginUserDto>;
