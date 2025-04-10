using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.Extensions;
using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;

public class GetAllProgressQueryHandler : IRequestHandler<GetAllProgressQuery, PagedList<ProgressViewModel>>
{
    private readonly IProgressRepository _progressRepository;
    private readonly IDietPlanRepository _dietPlanRepository;

    public GetAllProgressQueryHandler(
        IProgressRepository progressRepository,
        IDietPlanRepository dietPlanRepository)
    {
        _progressRepository = progressRepository;
        _dietPlanRepository = dietPlanRepository;
    }

    public async Task<PagedList<ProgressViewModel>> Handle(GetAllProgressQuery request, CancellationToken cancellationToken)
    {
        if (request.DietPlanId.HasValue)
        {
            var dietPlan = await _dietPlanRepository.GetSingleAsync(x => x.Id == request.DietPlanId.Value);
            if (dietPlan == null) throw new NotFoundException("Diet plan not found");
        }

        var query = _progressRepository.AsQueryable();

        if (request.UserId.HasValue)
            query = query.Where(p => p.ClientId == request.UserId.Value);

        if (request.DietPlanId.HasValue)
            query = query.Where(p => p.DietPlanId == request.DietPlanId.Value);

        if (request.FromDate.HasValue)
            query = query.Where(p => p.Date >= request.FromDate.Value.Date);

        if (request.ToDate.HasValue)
            query = query.Where(p => p.Date <= request.ToDate.Value.Date);

        query = query.OrderByDescending(p => p.Date);

#if DEBUG
        var progress = query
            .Select(p => new ProgressViewModel(
                p.Id,
                p.Date,
                p.Weight,
                p.Notes ?? string.Empty))
            .ToPagedList(request.PageNumber, request.PageSize);
#else
        var progress = await query
            .Select(p => new ProgressViewModel(
                p.Id,
                p.Date,
                p.Weight,
                p.Notes ?? string.Empty))
            .ToPagedListAsync(request.PageNumber, request.PageSize, cancellationToken);
#endif
        return progress;
    }
}

