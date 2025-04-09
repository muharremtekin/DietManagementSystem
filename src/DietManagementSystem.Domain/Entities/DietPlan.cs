namespace DietManagementSystem.Domain.Entities;

public class DietPlan : BaseEntity
{
    public string Title { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal InitialWeight { get; set; }

    public Guid ClientId { get; set; }
    public virtual ApplicationUser Client { get; set; }

    public Guid DietitianId { get; set; }
    public virtual ApplicationUser Dietitian { get; set; }

    public virtual ICollection<Meal> Meals { get; set; }

    public virtual ICollection<Progress> ProgressEntries { get; set; }
}
