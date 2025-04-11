using DietManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.WebApi.Extensions;
public static class MigrationExtensions
{
    public static async Task<WebApplication> MigrateDatabaseAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            var configuration = services.GetRequiredService<IConfiguration>();

            await RetryHelper.ExecuteWithRetriesAsync(async () =>
            {
                await context.Database.MigrateAsync();

            }, maxRetries: 10, delayMs: 10000,
            onError: (ex, attempt) => logger.LogWarning(ex, "Migration attempt {Attempt} failed", attempt));
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Database migration failed after all retries");
            throw;
        }

        return app;
    }
}
