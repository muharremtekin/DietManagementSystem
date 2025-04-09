using DietManagementSystem.Application.Features.Progress.Commands.Create;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.Progress;

public class CreateProgressCommandValidator : AbstractValidator<CreateProgressCommand>
{
    public CreateProgressCommandValidator()
    {
        RuleFor(x => x.DietPlanId)
            .NotEmpty().WithMessage("Diyet planı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz diyet planı ID'si.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Tarih zorunludur.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Tarih bugünden sonra olamaz.");

        RuleFor(x => x.Weight)
            .NotEmpty().WithMessage("Kilo zorunludur.")
            .GreaterThan(0).WithMessage("Kilo 0'dan büyük olmalıdır.");

        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("Notlar zorunludur.");
    }
} 