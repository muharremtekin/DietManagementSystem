using DietManagementSystem.Application.Features.User.Commands.Delete;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.User;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.userId)
            .NotEmpty().WithMessage("Kullanıcı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz kullanıcı ID'si.");
    }
} 