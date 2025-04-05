namespace DietManagementSystem.Domain.Entities;

public class Client : ApplicationUser
{
    public string Name { get; set; }
    public double InitialWeight { get; set; }
    public DateTime BirthDate { get; set; }
    public Guid DietitianId { get; set; }
    public virtual Dietitian Dietitian { get; set; }
    public virtual ICollection<DietPlan> DietPlans { get; set; }
}
