using DietManagementSystem.Application.Extensions;
using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.User.Queries.GetUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserViewModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public GetAllUsersQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<PagedList<UserViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Role))
            return new PagedList<UserViewModel>([], 0, request.PageNumber, request.PageSize);

        // Get role ID first
        var role = await _roleManager.FindByNameAsync(request.Role);
        if (role is null)
            return new PagedList<UserViewModel>([], 0, request.PageNumber, request.PageSize);

        var users = await _userManager.GetUsersInRoleAsync(role.Name);

        return users.Select(u => new UserViewModel(u.Id, u.DateOfBirth, u.FullName, u.Email, u.UserName))
                    .ToPagedList(request.PageNumber, request.PageSize);
    }
}