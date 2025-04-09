using MediatR;

namespace DietManagementSystem.Application.Features.DietPlan.Commands.Create;
public record CreateDietPlanCommmand(string Title, DateTime StartDate, DateTime EndDate, decimal InitialWeight, Guid ClientId) : IRequest
{
    // istek atan kişi diyetisyen ise sadece kendisi için diyet planı oluşturabilir
    [System.Text.Json.Serialization.JsonIgnore]
    public (Guid? RequesterId, string? RequesterRole) RequesterInfo { get; set; } = (Guid.Empty, string.Empty);
    public Guid DietitianId { get; set; }
}
