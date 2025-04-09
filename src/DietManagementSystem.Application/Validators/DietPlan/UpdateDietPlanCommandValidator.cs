using DietManagementSystem.Application.Features.DietPlan.Commands.Update;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.DietPlan;

public class UpdateDietPlanCommandValidator : AbstractValidator<UpdateDietPlanCommand>
{
    public UpdateDietPlanCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık zorunludur.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi zorunludur.")
            .LessThan(x => x.EndDate).WithMessage("Başlangıç tarihi bitiş tarihinden önce olmalıdır.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi zorunludur.")
            .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

        RuleFor(x => x.InitialWeight)
            .NotEmpty().WithMessage("Başlangıç kilosu zorunludur.")
            .GreaterThan(0).WithMessage("Başlangıç kilosu 0'dan büyük olmalıdır.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Danışan ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz danışan ID'si.");

        RuleFor(x => x.DietitianId)
            .NotEmpty().WithMessage("Diyetisyen ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz diyetisyen ID'si.");

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Diyet planı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz diyet planı ID'si.");
    }
} 