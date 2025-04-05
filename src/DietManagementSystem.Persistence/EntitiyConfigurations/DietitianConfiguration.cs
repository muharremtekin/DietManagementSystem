using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.EntitiyConfigurations;

public class DietitianConfiguration : IEntityTypeConfiguration<Dietitian>
{
    public void Configure(EntityTypeBuilder<Dietitian> builder)
    {
        // Tablo adı (TPT)
        builder.ToTable("Dietitians");

        // Alan yapılandırmaları
        builder.Property(d => d.Specialization)
            .HasMaxLength(50);

        // İlişkiler
        builder.HasMany(d => d.Clients)
            .WithOne(c => c.Dietitian)
            .HasForeignKey(c => c.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(d => d.DietPlans)
            .WithOne(dp => dp.Dietitian)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
