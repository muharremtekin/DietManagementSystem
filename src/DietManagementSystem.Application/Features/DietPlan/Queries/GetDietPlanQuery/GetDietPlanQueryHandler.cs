using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetDietPlanQuery;

public class GetDietPlanQueryHandler : IRequestHandler<GetDietPlanQuery, DietPlanViewModel>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    public GetDietPlanQueryHandler(IDietPlanRepository dietPlanRepository)
    {
        _dietPlanRepository = dietPlanRepository;
    }
    public async Task<DietPlanViewModel> Handle(GetDietPlanQuery request, CancellationToken cancellationToken)
    {
        var dietPlan = await _dietPlanRepository.GetSingleAsync(
            predicate: d => d.Id == request.DietPlanId,
            includes: [dp => dp.Client,
                        dp => dp.Meals.Where(m => !m.IsDeleted),
                        dp => dp.ProgressEntries.Where(pe => !pe.IsDeleted)
                      ]
            );

        if (dietPlan == null) throw new NotFoundException("Diet Plan not found.");

        var meals = dietPlan.Meals.Select(m => new MealViewModel(m.Id, m.Title, m.StartTime, m.EndTime, m.Content, m.DietPlanId)).ToArray();

        var progresses = dietPlan.ProgressEntries.Select(pe => new ProgressViewModel(pe.Id, pe.Date, pe.Weight, pe.Notes)).ToArray();

        return new DietPlanViewModel(dietPlan.Id,
                                    dietPlan.Title,
                                    dietPlan.StartDate,
                                    dietPlan.EndDate,
                                    dietPlan.InitialWeight,
                                    dietPlan.Client.FullName,
                                    meals,
                                    progresses);
    }
}