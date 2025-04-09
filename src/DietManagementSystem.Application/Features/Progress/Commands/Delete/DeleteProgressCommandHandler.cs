using MediatR;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using DietManagementSystem.Application.Exceptions;
namespace DietManagementSystem.Application.Features.Progress.Commands.Delete;

public class DeleteProgressCommandHandler : IRequestHandler<DeleteProgressCommand>
{
    private readonly IProgressRepository _progressRepository;
    public DeleteProgressCommandHandler(IProgressRepository progressRepository)
    {
        _progressRepository = progressRepository;
    }
    public async Task Handle(DeleteProgressCommand request, CancellationToken cancellationToken)
    {
        var progress = await _progressRepository.GetSingleAsync(p => p.Id == request.Id);
        if (progress == null) throw new NotFoundException("Progress not found");
        
        progress.IsDeleted = true;
        progress.UpdatedAt = DateTime.UtcNow;

        await _progressRepository.SaveChangesAsync();
    }
}
