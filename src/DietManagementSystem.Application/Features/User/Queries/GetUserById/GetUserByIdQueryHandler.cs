using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.User.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null || user.IsDeleted) throw new NotFoundException("User not found.");

        return new UserViewModel(user.Id, user.DateOfBirth, user.FullName, user.Email, user.UserName);
    }
}


