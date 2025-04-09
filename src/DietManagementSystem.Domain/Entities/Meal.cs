namespace DietManagementSystem.Domain.Entities;

public class Meal : BaseEntity
{
    public string Title { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Content { get; set; }
    public Guid DietPlanId { get; set; }
    public virtual DietPlan DietPlan { get; set; }
}