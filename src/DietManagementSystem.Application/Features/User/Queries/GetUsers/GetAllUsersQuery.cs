using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DietManagementSystem.Application.Features.User.Queries.GetUsers;
public record GetAllUsersQuery() : RequestParameters, IRequest<PagedList<UserViewModel>>
{
    [BindNever]
    public string Role { get; set; } = string.Empty;
}
