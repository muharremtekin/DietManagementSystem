namespace DietManagementSystem.Application.DTOs
{
    public record LoginUserDto(string Token, DateTime TokenExpiryTime);
}
