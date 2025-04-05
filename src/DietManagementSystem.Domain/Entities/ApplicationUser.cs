using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public DateTime CreatedAt { get; set; }
}