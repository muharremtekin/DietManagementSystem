using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.EntitiyConfigurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.Property(c => c.InitialWeight)
            .HasPrecision(5, 2);

        builder.Property(c => c.BirthDate);

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
