using DietManagementSystem.Application.ViewModels;
using MediatR;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;

public class GetAllProgressQueryHandler : IRequestHandler<GetAllProgressQuery, List<ProgressViewModel>>
{
    private readonly IProgressRepository _progressRepository;
    public GetAllProgressQueryHandler(IProgressRepository progressRepository) => _progressRepository = progressRepository;

    public async Task<List<ProgressViewModel>> Handle(GetAllProgressQuery request, CancellationToken cancellationToken)
    {
        var query = _progressRepository.AsQueryable();

        if (request.DietPlanId.HasValue)
            query = query.Where(p => p.DietPlanId == request.DietPlanId.Value);

        query = query.OrderByDescending(p => p.Date);

        var progress = await query.Select(p => new ProgressViewModel(
             p.Id,
             p.Date,
             p.Weight,
             p.Notes ?? string.Empty))
             .ToListAsync(cancellationToken);

        return progress;
    }
}

