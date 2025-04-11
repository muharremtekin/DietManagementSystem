using Bogus;
using DietManagementSystem.Domain.Entities;
using DietManagementSystem.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DietManagementSystem.Persistence.Seed;

public static class DataSeeder
{
    private const string DefaultPassword = "Asd123!";

    private static readonly Faker<ApplicationUser> UserFaker = new Faker<ApplicationUser>()
        .RuleFor(u => u.FullName, f => f.Name.FullName())
        .RuleFor(u => u.UserName, f => f.Internet.UserName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30, DateTime.UtcNow.AddYears(-18)))
        .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.CreatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(u => u.UpdatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(u => u.IsDeleted, f => false);

    private static readonly Faker<DietPlan> DietPlanFaker = new Faker<DietPlan>()
        .RuleFor(d => d.Title, f => f.Lorem.Sentence(3))
        .RuleFor(d => d.StartDate, f => f.Date.Future(1, DateTime.UtcNow))
        .RuleFor(d => d.EndDate, (f, d) => d.StartDate.AddMonths(3))
        .RuleFor(d => d.InitialWeight, f => f.Random.Decimal(50, 120))
        .RuleFor(d => d.CreatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(d => d.UpdatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(d => d.IsDeleted, f => false);

    private static readonly Faker<Meal> MealFaker = new Faker<Meal>()
        .RuleFor(m => m.Title, f => f.Lorem.Sentence(2))
        .RuleFor(m => m.StartTime, f => new TimeSpan(f.Random.Int(6, 20), 0, 0))
        .RuleFor(m => m.EndTime, (f, m) => m.StartTime.Add(TimeSpan.FromHours(1)))
        .RuleFor(m => m.Content, f => f.Lorem.Paragraph())
        .RuleFor(m => m.CreatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(m => m.UpdatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(m => m.IsDeleted, f => false);

    private static readonly Faker<Progress> ProgressFaker = new Faker<Progress>()
        .RuleFor(p => p.Date, f => f.Date.Past(30, DateTime.UtcNow))
        .RuleFor(p => p.Weight, f => f.Random.Decimal(50, 120))
        .RuleFor(p => p.Notes, f => f.Lorem.Sentence())
        .RuleFor(p => p.CreatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(p => p.UpdatedAt, f => f.Date.Past(1, DateTime.UtcNow))
        .RuleFor(p => p.IsDeleted, f => false);

    public static async Task SeedProductionDataAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        // Create roles if they don't exist
        var roles = new[] { "Admin", "Dietitian", "Client" };
        foreach (var role in roles)
        {
            if (!await context.Roles.AnyAsync(r => r.Name == role))
            {
                await context.Roles.AddAsync(new IdentityRole<Guid> { Name = role, NormalizedName = role.ToUpper() });
            }
        }
        await context.SaveChangesAsync();

        // Create admin user if it doesn't exist
        var adminEmail = "admin@diet.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = adminEmail,
                FullName = "System Administrator",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var result = await userManager.CreateAsync(adminUser, DefaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }

    public static async Task SeedDataAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        if (!await context.Users.AnyAsync())
        {
            // Create roles
            var roles = new[] { "Admin", "Dietitian", "Client" };
            foreach (var role in roles)
            {
                if (!await context.Roles.AnyAsync(r => r.Name == role))
                {
                    await context.Roles.AddAsync(new IdentityRole<Guid> { Name = role, NormalizedName = role.ToUpper() });
                }
            }
            await context.SaveChangesAsync();

            // Create admin user
            var adminUser = UserFaker.Generate();
            adminUser.UserName = "admin";
            adminUser.Email = "admin@diet.com";
            adminUser.FullName = "Admin User";
            var result = await userManager.CreateAsync(adminUser, DefaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Create dietitians
            var dietitians = UserFaker.Generate(5);
            foreach (var dietitian in dietitians)
            {
                var createResult = await userManager.CreateAsync(dietitian, DefaultPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(dietitian, "Dietitian");
                }
            }

            // Create clients
            var clients = UserFaker.Generate(10);
            foreach (var client in clients)
            {
                var createResult = await userManager.CreateAsync(client, DefaultPassword);
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(client, "Client");
                }
            }

            await context.SaveChangesAsync();

            // Create diet plans
            var dietPlans = new List<DietPlan>();
            foreach (var dietitian in dietitians)
            {
                var clientDietPlans = DietPlanFaker.Generate(3);
                foreach (var plan in clientDietPlans)
                {
                    plan.DietitianId = dietitian.Id;
                    plan.ClientId = clients[new Random().Next(clients.Count)].Id;
                    dietPlans.Add(plan);
                }
            }
            await context.DietPlans.AddRangeAsync(dietPlans);
            await context.SaveChangesAsync();

            // Create meals for each diet plan
            var meals = new List<Meal>();
            foreach (var dietPlan in dietPlans)
            {
                var dietPlanMeals = MealFaker.Generate(5);
                foreach (var meal in dietPlanMeals)
                {
                    meal.DietPlanId = dietPlan.Id;
                    meals.Add(meal);
                }
            }
            await context.Meals.AddRangeAsync(meals);
            await context.SaveChangesAsync();

            // Create progress entries
            var progressEntries = new List<Progress>();
            foreach (var dietPlan in dietPlans)
            {
                var dietPlanProgress = ProgressFaker.Generate(4);
                foreach (var progress in dietPlanProgress)
                {
                    progress.DietPlanId = dietPlan.Id;
                    progress.ClientId = dietPlan.ClientId;
                    progressEntries.Add(progress);
                }
            }
            await context.Progress.AddRangeAsync(progressEntries);
            await context.SaveChangesAsync();
        }
    }
}