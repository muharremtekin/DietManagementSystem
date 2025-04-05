namespace DietManagementSystem.Domain.Entities;
public class Dietitian : ApplicationUser
{
    public string Name { get; set; }
    public string Specialization { get; set; }
    public virtual ICollection<Client> Clients { get; set; }
    public virtual ICollection<DietPlan> DietPlans { get; set; }
}
