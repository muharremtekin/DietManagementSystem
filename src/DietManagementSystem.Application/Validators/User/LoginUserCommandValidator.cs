using DietManagementSystem.Application.Features.User.Commands.Login;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.User;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.userName)
            .NotEmpty().When(x => string.IsNullOrEmpty(x.email))
            .WithMessage("Kullanıcı adı veya email’den biri zorunludur.");

        RuleFor(x => x.email)
            .NotEmpty().When(x => string.IsNullOrEmpty(x.userName))
            .WithMessage("Kullanıcı adı veya email’den biri zorunludur.")
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.email))
            .WithMessage("Geçersiz email formatı.");

        RuleFor(x => x.password)
            .NotEmpty().WithMessage("Şifre zorunludur.");
    }
}