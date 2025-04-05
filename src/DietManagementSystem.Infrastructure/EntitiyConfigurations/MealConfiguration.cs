using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Infrastructure.EntitiyConfigurations;

public class MealConfiguration : BaseEntityConfiguration<Meal>
{
    public override void Configure(EntityTypeBuilder<Meal> builder)
    {
        base.Configure(builder);

        builder.ToTable("Meals");

        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.StartTime)
            .IsRequired();

        builder.Property(m => m.EndTime)
            .IsRequired();

        builder.Property(m => m.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(m => m.DietPlan)
            .WithMany(dp => dp.Meals)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}