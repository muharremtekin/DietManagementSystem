using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; }

    // Kullanıcının diyetisyen olarak oluşturduğu diyet planları
    public ICollection<DietPlan> DietPlansAsDietitian { get; set; }

    // Danışan olarak sahip olduğu diyet planları
    public ICollection<DietPlan> DietPlansAsClient { get; set; }

    // İlerleme kayıtları (danışan)
    public ICollection<Progress> ProgressEntries { get; set; }
}
