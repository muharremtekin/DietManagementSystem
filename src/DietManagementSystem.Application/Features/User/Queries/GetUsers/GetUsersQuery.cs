using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.User.Queries.GetUsers;
public record GetUsersQuery(string role) : IRequest<List<UserViewModel>>;
