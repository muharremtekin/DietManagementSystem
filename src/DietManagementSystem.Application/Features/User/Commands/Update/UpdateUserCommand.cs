using MediatR;
using System.Text.Json.Serialization;

namespace DietManagementSystem.Application.Features.User.Commands.Update;
public record UpdateUserCommand(string Email, string Password, string FullName, DateTime DateOfBirth) : IRequest
{
    [JsonIgnore]
    public Guid UserId { get; set; } = Guid.Empty;
}
