using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.User.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UpdateUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new NotFoundException("User not found.");

        user.FullName = request.FullName;
        user.DateOfBirth = request.DateOfBirth;

        if (!string.IsNullOrEmpty(request.Password))
        {
            var passwordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            user.PasswordHash = passwordHash;
        }

        await _userManager.UpdateAsync(user);
    }
}
