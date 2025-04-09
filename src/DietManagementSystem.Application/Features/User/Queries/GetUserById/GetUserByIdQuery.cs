using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.User.Queries.GetUserById;
public record GetUserByIdQuery(Guid UserId) : IRequest<UserViewModel>;


