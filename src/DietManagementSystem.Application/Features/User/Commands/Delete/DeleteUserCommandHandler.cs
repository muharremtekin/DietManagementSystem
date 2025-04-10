using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.User.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public DeleteUserCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.userId.ToString());
        if (user == null) throw new NotFoundException("User not found.");
        
        user.IsDeleted = true;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new BadRequestException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
}
