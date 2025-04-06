using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.EntitiyConfigurations;

public class ProgressConfiguration : BaseEntityConfiguration<Progress>
{
    public override void Configure(EntityTypeBuilder<Progress> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Date)
            .IsRequired();

        builder.Property(p => p.Weight)
            .IsRequired();

        builder.Property(p => p.Notes)
            .HasMaxLength(3000);

        builder.HasOne(p => p.DietPlan)
            .WithMany(dp => dp.ProgressEntries)
            .HasForeignKey(p => p.DietPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Client)
            .WithMany(u => u.ProgressEntries)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}