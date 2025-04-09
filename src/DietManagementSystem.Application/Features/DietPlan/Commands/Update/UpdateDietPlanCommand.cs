using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Update;
public record UpdateDietPlanCommand(string Title, DateTime StartDate, DateTime EndDate, decimal InitialWeight, Guid ClientId) : IRequest
{
    public Guid DietitianId { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public (Guid? RequesterId, string? RequesterRole) RequesterInfo { get; set; } = (Guid.Empty, string.Empty);
    [System.Text.Json.Serialization.JsonIgnore]
    public Guid Id { get; set; }
}
