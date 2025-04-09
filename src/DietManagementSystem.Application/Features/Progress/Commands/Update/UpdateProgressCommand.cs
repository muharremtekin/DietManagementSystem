using System.Text.Json.Serialization;
using MediatR;

namespace DietManagementSystem.Application.Features.Progress.Commands.Update;

public record UpdateProgressCommand(DateTime Date, decimal Weight, string Notes) : IRequest
{
    [JsonIgnore]
    public Guid Id { get; set; }
}
