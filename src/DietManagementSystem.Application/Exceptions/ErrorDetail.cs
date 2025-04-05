using System.Text.Json;

namespace DietManagementSystem.Application.Exceptions;

public class ErrorDetail
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    override public string ToString() => JsonSerializer.Serialize(this);
}