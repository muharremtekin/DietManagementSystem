using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Infrastructure.EntitiyConfigurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        // Tablo adı (TPT)
        builder.ToTable("Clients");

        // Alan yapılandırmaları
        builder.Property(c => c.InitialWeight)
            .HasPrecision(5, 2); // 5 hane, 2 ondalık

        builder.Property(c => c.BirthDate)
            .IsRequired(false); // Opsiyonel

        // İlişkiler
        builder.HasOne(c => c.Dietitian)
            .WithMany(d => d.Clients)
            .HasForeignKey(c => c.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.DietPlans)
            .WithOne(dp => dp.Client)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
