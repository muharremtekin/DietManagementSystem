using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;

// admin değilse sadece kendine ait diet planlarını görebilir
public record GetAllDietPlansQuery(Guid? DietitianId, Guid? ClientId)
    : RequestParameters, IRequest<PagedList<DietPlanViewModel>>
{
    [BindNever]
    public (Guid? RequesterId, string? RequesterRole) RequesterInfo { get; set; } = (Guid.Empty, string.Empty);
}
