using DietManagementSystem.Application.Extensions;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using DietManagementSystem.Persistence.Seed;
using DietManagementSystem.WebApi.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

#region Services Registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerServices();

builder.Services.ConfigureCORS();

builder.Services.AddApplicationResgistration();
builder.Services.AddPersistanceRegistrations(builder.Configuration);

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureAuth(builder.Configuration);
#endregion
var app = builder.Build();

app.ConfigureExceptionHandler();

#region Data Seeding
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerServices(provider);
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await DataSeeder.SeedDataAsync(context, userManager);
    }
}
else
{
    await app.MigrateDatabaseAsync();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        await DataSeeder.SeedProductionDataAsync(context, userManager);
    }
}
#endregion

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
