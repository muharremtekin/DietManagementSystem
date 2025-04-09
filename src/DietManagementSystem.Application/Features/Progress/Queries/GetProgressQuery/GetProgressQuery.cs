using DietManagementSystem.Application.ViewModels;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Queries.GetProgressQuery;

public record GetProgressQuery(Guid Id) : IRequest<ProgressViewModel>;

