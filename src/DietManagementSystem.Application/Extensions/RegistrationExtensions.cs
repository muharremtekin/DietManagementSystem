using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DietManagementSystem.Application.Extensions;
public static class RegistrationExtensions
{
    public static IServiceCollection AddApplicationResgistration(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(assembly);
        });

        return services;
    }
}

