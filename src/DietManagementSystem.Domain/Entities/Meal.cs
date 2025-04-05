namespace DietManagementSystem.Domain.Entities;

public class Meal : BaseEntity
{
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public Guid DietPlanId { get; set; }
    public virtual DietPlan DietPlan { get; set; }
}