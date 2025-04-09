using DietManagementSystem.Application.Features.Meal.Commands.Delete;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.Meal;

public class DeleteMealCommandValidator : AbstractValidator<DeleteMealCommand>
{
    public DeleteMealCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Öğün ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz öğün ID'si.");
    }
} 