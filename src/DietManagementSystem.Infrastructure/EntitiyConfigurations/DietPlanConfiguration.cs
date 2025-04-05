using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Infrastructure.EntitiyConfigurations;

public class DietPlanConfiguration : BaseEntityConfiguration<DietPlan>
{
    public override void Configure(EntityTypeBuilder<DietPlan> builder)
    {
        // BaseEntity'den gelen yapılandırmaları uygula
        base.Configure(builder);

        // Tablo adı
        builder.ToTable("DietPlans");

        // Alan yapılandırmaları
        builder.Property(dp => dp.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(dp => dp.StartDate)
            .IsRequired();

        builder.Property(dp => dp.EndDate)
            .IsRequired();

        builder.Property(dp => dp.InitialWeight)
            .IsRequired()
            .HasPrecision(5, 2);

        // İlişkiler
        builder.HasOne(dp => dp.Dietitian)
            .WithMany(d => d.DietPlans)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dp => dp.Client)
            .WithMany(c => c.DietPlans)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(dp => dp.Meals)
            .WithOne(m => m.DietPlan)
            .HasForeignKey(m => m.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
