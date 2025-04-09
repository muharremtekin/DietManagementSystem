using DietManagementSystem.Application.Features.DietPlan.Commands.Delete;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.DietPlan;

public class DeleteDietPlanCommandValidator : AbstractValidator<DeleteDietPlanCommand>
{
    public DeleteDietPlanCommandValidator()
    {
        RuleFor(x => x.DietPlanId)
            .NotEmpty().WithMessage("Diyet planı ID'si zorunludur.")
            .NotEqual(Guid.Empty).WithMessage("Geçersiz diyet planı ID'si.");
    }
} 