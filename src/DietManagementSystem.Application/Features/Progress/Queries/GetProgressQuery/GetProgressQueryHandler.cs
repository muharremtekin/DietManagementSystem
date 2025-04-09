using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetProgressQuery;

public class GetProgressQueryHandler : IRequestHandler<GetProgressQuery, ProgressViewModel>
{
    private readonly IProgressRepository _progressRepository;

    public GetProgressQueryHandler(IProgressRepository progressRepository) => _progressRepository = progressRepository;

    public async Task<ProgressViewModel> Handle(GetProgressQuery request, CancellationToken cancellationToken)
    {
        var progress = await _progressRepository.GetSingleAsync(p => p.Id == request.Id);

        if (progress == null) throw new NotFoundException("Progress not found.");

        return new ProgressViewModel(progress.Id, progress.Date, progress.Weight, progress.Notes);
    }
}

