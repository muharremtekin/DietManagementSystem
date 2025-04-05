using DietManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DietManagementSystem.Persistence.ContextFactory;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=RateWaveLocalDB;Port=5432;Username=admin;Password=password;");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
