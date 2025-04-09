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
        await _userManager.UpdateAsync(user);
    }
}
