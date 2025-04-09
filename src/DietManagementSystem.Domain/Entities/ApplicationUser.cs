using Microsoft.AspNetCore.Identity;

namespace DietManagementSystem.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>, IBaseEntity
{
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    // Kullanıcının diyetisyen olarak oluşturduğu diyet planları
    public virtual ICollection<DietPlan> DietPlansAsDietitian { get; set; }

    // Danışan olarak sahip olduğu diyet planları
    public virtual ICollection<DietPlan> DietPlansAsClient { get; set; }

    // İlerleme kayıtları (danışan)
    public virtual ICollection<Progress> ProgressEntries { get; set; }

    // Kullanıcının rollerine erişim (IdentityUserRole ilişkisinden)
    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; }

    // Kullanıcıya ait claim'ler (opsiyonel)
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

    // Kullanıcının login bilgileri (opsiyonel)
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

    // Kullanıcı tokenları (opsiyonel)
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
}
