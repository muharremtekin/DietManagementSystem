using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Create;
public record CreateDietPlanCommmand(string Title, DateTime StartDate, DateTime EndDate, decimal InitialWeight, Guid ClientId) : IRequest
{
    [System.Text.Json.Serialization.JsonIgnore]
    public (Guid? RequesterId, string? RequesterRole) RequesterInfo { get; set; } = (Guid.Empty, string.Empty);
    public Guid DietitianId { get; set; }
}
