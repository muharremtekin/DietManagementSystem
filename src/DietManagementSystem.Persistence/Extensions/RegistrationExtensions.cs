using DietManagementSystem.Application.Interfaces.Repositories;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DietManagementSystem.Persistence.Extensions;
public static class RegistrationExtensions
{
    public static IServiceCollection AddPersistanceRegistrations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(conf =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            conf.UseNpgsql(connectionString, opt =>
            {
                opt.EnableRetryOnFailure();
            });
        });

        services.AddScoped<IDietPlanRepository, DietPlanRepository>();
        services.AddScoped<IMealRepository, MealRepository>();

        return services;
    }
}
