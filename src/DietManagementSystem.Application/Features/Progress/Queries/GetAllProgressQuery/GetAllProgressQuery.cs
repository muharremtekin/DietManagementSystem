using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;

public record GetAllProgressQuery(
    Guid? DietPlanId,
    Guid? UserId,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : RequestParameters, IRequest<PagedList<ProgressViewModel>>;

