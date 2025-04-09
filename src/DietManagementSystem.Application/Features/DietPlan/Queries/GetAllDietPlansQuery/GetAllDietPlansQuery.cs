using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;

// admin değilse sadece kendine ait diet planlarını görebilir
public record GetAllDietPlansQuery(GetAllDietPlansQueryParams QueryParams, (Guid? RequesterId, string? RequesterRole) RequesterInfo) : IRequest<List<DietPlanViewModel>>
{
    //[System.Text.Json.Serialization.JsonIgnore]
    //public (Guid? RequesterId, string? RequesterRole) RequesterInfo { get; set; } = (Guid.Empty, string.Empty);
}

public record GetAllDietPlansQueryParams(Guid? DietitianId, Guid? ClientId);