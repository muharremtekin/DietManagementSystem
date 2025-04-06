namespace DietManagementSystem.Domain.Entities;

public class Progress : BaseEntity
{
    public Guid DietPlanId { get; set; }
    public DietPlan DietPlan { get; set; }

    public Guid ClientId { get; set; }
    public ApplicationUser Client { get; set; }

    public DateTime Date { get; set; }

    public decimal Weight { get; set; }

    public string Notes { get; set; }
}