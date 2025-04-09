using DietManagementSystem.Application.Features.DietPlan.Commands.Create;
using FluentValidation;

namespace DietManagementSystem.Application.Validators.DietPlan;

public class CreateDietPlanCommmandValidator : AbstractValidator<CreateDietPlanCommmand>
{
    public CreateDietPlanCommmandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Diyet planı başlığı boş olamaz.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Başlangıç tarihi belirtilmelidir.")
            .LessThan(x => x.EndDate).WithMessage("Başlangıç tarihi, bitiş tarihinden önce olmalıdır.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("Bitiş tarihi belirtilmelidir.")
            .GreaterThan(x => x.StartDate).WithMessage("Bitiş tarihi, başlangıç tarihinden sonra olmalıdır.");

        RuleFor(x => x.InitialWeight)
            .GreaterThan(0).WithMessage("İlk ağırlık 0'dan büyük olmalıdır.");

        RuleFor(x => x.ClientId)
            .NotEmpty().WithMessage("Müşteri kimliği belirtilmelidir.");
    }
}
