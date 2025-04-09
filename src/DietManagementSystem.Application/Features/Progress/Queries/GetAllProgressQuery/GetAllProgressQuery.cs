using System;
using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;

public record GetAllProgressQuery(Guid? DietPlanId) : IRequest<List<ProgressViewModel>>;

