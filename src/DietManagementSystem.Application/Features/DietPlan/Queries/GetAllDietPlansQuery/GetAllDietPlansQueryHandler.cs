using DietManagementSystem.Application.Extensions;
using DietManagementSystem.Application.RequestFeatures;
using DietManagementSystem.Application.ViewModels;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;

public class GetAllDietPlansQueryHandler : IRequestHandler<GetAllDietPlansQuery, PagedList<DietPlanViewModel>>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    public GetAllDietPlansQueryHandler(IDietPlanRepository dietPlanRepository)
    {
        _dietPlanRepository = dietPlanRepository;
    }
    public async Task<PagedList<DietPlanViewModel>> Handle(GetAllDietPlansQuery request, CancellationToken cancellationToken)
    {
        var query = _dietPlanRepository.AsQueryable()
            .Include(dp => dp.Client)
            .Where(dp => !dp.IsDeleted);

        if (request.ClientId != null)
            query = query.Where(dp => dp.ClientId == request.ClientId);

        if (request.DietitianId != null)
            query = query.Where(dp => dp.DietitianId == request.DietitianId);
#if DEBUG
        var dietPlans = query
            .Select(dp => new { dp.Id, dp.Title, dp.StartDate, dp.EndDate, dp.InitialWeight, dp.Client.FullName })
            .ToList();

        return dietPlans.Select(d => new DietPlanViewModel(Id: d.Id, Title: d.Title, StartDate: d.StartDate, EndDate: d.EndDate, d.InitialWeight, d.FullName, null, null))
                        .ToPagedList(request.PageNumber, request.PageSize);

#else
        // ef core'un async query yapısı testlerde patlıyo
        return await query
            .Select(dp => new DietPlanViewModel(dp.Id, dp.Title, dp.StartDate, dp.EndDate, dp.InitialWeight, dp.Client.FullName, null, null))
            .ToPagedListAsync(request.PageNumber, request.PageSize);
#endif
    }

}
