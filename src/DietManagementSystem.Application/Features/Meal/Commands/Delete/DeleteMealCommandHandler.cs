using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.Meal.Commands.Delete;

public class DeleteMealCommandHandler : IRequestHandler<DeleteMealCommand>
{
    private readonly IMealRepository _mealRepository;

    public DeleteMealCommandHandler(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public async Task Handle(DeleteMealCommand request, CancellationToken cancellationToken)
    {
        var meal = await _mealRepository.GetSingleAsync(x => x.Id == request.Id, noTracking: false);

        if (meal == null) throw new NotFoundException("Meal not found");

        meal.IsDeleted = true;
        meal.UpdatedAt = DateTime.UtcNow;

        await _mealRepository.SaveChangesAsync();
    }
}