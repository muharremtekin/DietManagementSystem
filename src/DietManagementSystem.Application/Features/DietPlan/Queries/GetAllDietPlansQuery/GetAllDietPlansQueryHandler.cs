using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;

public class GetAllDietPlansQueryHandler : IRequestHandler<GetAllDietPlansQuery, List<DietPlanViewModel>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    public GetAllDietPlansQueryHandler(IDietPlanRepository dietPlanRepository)
    {
        _dietPlanRepository = dietPlanRepository;
    }
    public async Task<List<DietPlanViewModel>> Handle(GetAllDietPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _dietPlanRepository.AsQueryable();

        if (request.QueryParams.ClientId != null)
            query = query.Where(dp => dp.ClientId == request.QueryParams.ClientId);

        if (request.QueryParams.DietitianId != null)
            query = query.Where(dp => dp.DietitianId == request.QueryParams.DietitianId);

        var dietPlans = await query
            .Select(dp => new DietPlanViewModel(dp.Id,
                                                dp.Title,
                                                dp.StartDate,
                                                dp.EndDate,
                                                dp.InitialWeight,
                                                dp.Client.FullName,
                                                null,
                                                null))
            .ToListAsync();

        return dietPlans;
    }
}
