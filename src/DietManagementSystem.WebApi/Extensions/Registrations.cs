using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Interfaces.Repositories;
using DietManagementSystem.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DietManagementSystem.WebApi.Extensions;
public static class Registrations
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
        services.AddScoped<IProgressRepository, ProgressRepository>();

        return services;
    }

    public static IServiceCollection ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var jwtKey = jwtSection["Key"];
        var jwtIssuer = jwtSection["Issuer"];
        var jwtAudience = jwtSection["Audience"];
        var expireDays = jwtSection["ExpireDays"];

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero  // İsteğe bağlı: Token süre uçurumlarını kapatmak için
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("DietitianPolicy", policy =>
                policy.RequireRole("Dietitian", "Admin"));

            options.AddPolicy("ManageClients", policy =>
                policy.RequireRole("Admin", "Dietitian"));

            options.AddPolicy("ManageDietPlans", policy =>
                policy.RequireRole("Admin", "Dietitian"));

            options.AddPolicy("ManageDietitians", policy =>
                policy.RequireRole("Admin"));
        });

        return services;
    }

    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Diet Management System API", Version = "v1" });
            c.CustomSchemaIds(type => type.FullName);
        });
        return services;
    }

    public static IServiceCollection ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });
        return services;
    }

}

