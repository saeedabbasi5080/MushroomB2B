using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasIndex(u => u.PhoneNumber).IsUnique();

        builder.Property(u => u.FullName)
            .HasMaxLength(200);

        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);

        builder.Property(u => u.RefreshTokenExpiresAt);

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
