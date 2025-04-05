using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Infrastructure.EntitiyConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Tablo adı
        builder.ToTable("Users");

        // Alan yapılandırmaları
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);
    }
}
