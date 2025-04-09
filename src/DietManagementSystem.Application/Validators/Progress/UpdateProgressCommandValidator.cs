using DietManagementSystem.Application.Features.Progress.Commands.Update;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.Progress;

public class UpdateProgressCommandValidator : AbstractValidator<UpdateProgressCommand>
{
    public UpdateProgressCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("İlerleme kaydı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz ilerleme kaydı ID'si.");

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