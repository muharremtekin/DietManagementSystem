using System;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Commands.Create;

public record CreateProgressCommand(Guid DietPlanId, DateTime Date, decimal Weight, string Notes) : IRequest;
