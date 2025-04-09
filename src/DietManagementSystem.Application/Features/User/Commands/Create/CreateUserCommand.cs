using MediatR;
using System.Text.Json.Serialization;

namespace DietManagementSystem.Application.Features.User.Commands.Create;
public sealed record CreateUserCommand : IRequest
{
    public string Email { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public string FullName { get; init; }
    public DateTime DateOfBirth { get; init; }
    [JsonIgnore]
    public string? Role { get; set; }
}
