using DietManagementSystem.Application.Exceptions;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Create;

public sealed class CreateDietPlanCommmandHandler : IRequestHandler<CreateDietPlanCommmand>
{
    private readonly IDietPlanRepository _dietPlanRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    public CreateDietPlanCommmandHandler(IDietPlanRepository dietPlanRepository, UserManager<ApplicationUser> userManager)
    {
        _dietPlanRepository = dietPlanRepository;
        _userManager = userManager;
    }
    public async Task Handle(CreateDietPlanCommmand request, CancellationToken cancellationToken)
    {
        // Not: Eğer istek atan kişi diyetisyen ise sadece kendisi için diyet planı oluşturabilir fakat adminler herkes için diyet planı oluşturabilir.



        var dietitian = await _userManager.FindByIdAsync(request.DietitianId.ToString());
        if (dietitian == null) throw new NotFoundException("Dietitian not found.");

        var client = await _userManager.FindByIdAsync(request.ClientId.ToString());
        if (client == null) throw new NotFoundException("Client not found.");

        var dietPlan = new Domain.Entities.DietPlan
        {
            Title = request.Title,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            InitialWeight = request.InitialWeight,
            ClientId = request.ClientId,
            DietitianId = request.DietitianId
        };

        await _dietPlanRepository.AddAsync(dietPlan);

        await _dietPlanRepository.SaveChangesAsync();
    }
}