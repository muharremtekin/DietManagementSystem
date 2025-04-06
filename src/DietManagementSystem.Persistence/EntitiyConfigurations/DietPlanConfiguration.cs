using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.EntitiyConfigurations;

public class DietPlanConfiguration : BaseEntityConfiguration<DietPlan>
{
    public override void Configure(EntityTypeBuilder<DietPlan> builder)
    {
        base.Configure(builder);

        builder.Property(dp => dp.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dp => dp.StartDate)
            .IsRequired();

        builder.Property(dp => dp.EndDate)
            .IsRequired();

        builder.Property(dp => dp.InitialWeight)
            .IsRequired();

        builder.HasOne(dp => dp.Client)
            .WithMany(u => u.DietPlansAsClient)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Dietitian)
            .WithMany(u => u.DietPlansAsDietitian)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(dp => dp.Meals)
            .WithOne(m => m.DietPlan)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(dp => dp.ProgressEntries)
            .WithOne(p => p.DietPlan)
            .HasForeignKey(p => p.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}