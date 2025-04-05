namespace DietManagementSystem.Domain.Entities;

public class DietPlan : BaseEntity
{
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double InitialWeight { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid DietitianId { get; set; }
    public virtual Dietitian Dietitian { get; set; }

    public Guid ClientId { get; set; }
    public virtual Client Client { get; set; }

    public virtual ICollection<Meal> Meals { get; set; }

}
