using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MushroomB2B.Domain.Entities;

namespace MushroomB2B.Infrastructure.Persistence.Configurations;

public sealed class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.FullName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.VehiclePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(d => d.IsActive).IsRequired();

        builder.HasQueryFilter(d => !d.IsDeleted);
    }
}
