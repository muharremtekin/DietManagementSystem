using DietManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DietManagementSystem.Persistence.EntitiyConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Tablo adını "Users" olarak ayarlıyoruz.
        builder.ToTable("Users");

        // FullName alanı için maksimum uzunluk belirleniyor.
        builder.Property(u => u.FullName)
            .HasMaxLength(150);

        // DietPlansAsDietitian ilişkisi: Bir diyetisyen, birden fazla diyet planı oluşturabilir.
        builder.HasMany(u => u.DietPlansAsDietitian)
            .WithOne(dp => dp.Dietitian)
            .HasForeignKey(dp => dp.DietitianId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.DietPlansAsClient)
            .WithOne(dp => dp.Client)
            .HasForeignKey(dp => dp.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        // ProgressEntries ilişkisi: Danışanın ilerleme kayıtları.
        builder.HasMany(u => u.ProgressEntries)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}