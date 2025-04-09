using DietManagementSystem.Application.Validators;
using FluentValidation;
using MediatR;
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

        services.AddValidatorsFromAssemblyContaining<LoginUserCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }


}
