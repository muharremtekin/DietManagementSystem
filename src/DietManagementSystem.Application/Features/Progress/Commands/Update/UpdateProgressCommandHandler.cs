using MediatR;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using DietManagementSystem.Application.Exceptions;

namespace DietManagementSystem.Application.Features.Progress.Commands.Update;

public class UpdateProgressCommandHandler : IRequestHandler<UpdateProgressCommand>
{
    private readonly IProgressRepository _progressRepository;
    public UpdateProgressCommandHandler(IProgressRepository progressRepository) => _progressRepository = progressRepository;
    
    public async Task Handle(UpdateProgressCommand request, CancellationToken cancellationToken)
    {
        var progress = await _progressRepository.GetSingleAsync(p => p.Id == request.Id);
        if (progress == null) throw new NotFoundException("Progress not found");

        progress.Date = request.Date;
        progress.Weight = request.Weight;
        progress.Notes = request.Notes;

        await _progressRepository.SaveChangesAsync();
    }
}   
