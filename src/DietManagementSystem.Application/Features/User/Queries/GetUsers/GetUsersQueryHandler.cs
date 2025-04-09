using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.User.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserViewModel>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public GetUsersQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<UserViewModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByNameAsync(request.role);

        if (role is null) return [];

        var users = await _userManager.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .Where(u => u.IsDeleted == false && u.UserRoles.Any(r => r.RoleId == role.Id))
            .Select(u => new UserViewModel(u.Id, u.DateOfBirth, u.FullName, u.Email, u.UserName))
            .ToListAsync();

        return users;
    }
}