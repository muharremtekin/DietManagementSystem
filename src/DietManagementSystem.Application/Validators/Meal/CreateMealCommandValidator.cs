using DietManagementSystem.Application.Features.Meal.Commands.Create;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.Meal;

public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
{
    public CreateMealCommandValidator()
    {
        RuleFor(x => x.DietPlanId)
            .NotEmpty().WithMessage("Diyet planı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz diyet planı ID'si.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Başlık zorunludur.")
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Başlangıç saati zorunludur.");

        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("Bitiş saati zorunludur.")
            .GreaterThan(x => x.StartTime).WithMessage("Bitiş saati başlangıç saatinden sonra olmalıdır.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("İçerik zorunludur.");
    }
} 