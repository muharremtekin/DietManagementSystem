using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.User.Commands.Create;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new BadRequestException("User already exists.");

        var user = new ApplicationUser
        {
            Email = request.Email,
            UserName = request.UserName,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded && !string.IsNullOrEmpty(request.Role))
        {
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(request.Role));
            }

            await _userManager.AddToRoleAsync(user, request.Role);
        }
    }
}